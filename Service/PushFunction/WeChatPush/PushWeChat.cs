using Common;
using Entities.UserDefinedEntity;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Common.WxPay;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceLog;

namespace Service.PushFunction.WeChatPush
{
    public abstract class PushWeChat : PushFunction
    {
        #region 属性（accessToken,OAuth验证企业获取code Url,根据code获取成员信息Url,获取TokenUrl,上传Url,发送Url）
        protected string _accessToken;
        /// <summary>
        /// 授权Token
        /// </summary>
        public string AccessToken1
        {
            get { return _accessToken; }
        }

        /// <summary>
        /// 企业号公司agentID
        /// </summary>
        protected string _weChatAgentID = ConfigSugar.GetAppString("WeChatAgentID");

        protected string _oAuthUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base#wechat_redirect";
        /// <summary>
        /// OAuth验证企业获取code Url
        /// </summary>
        public string OAuthUrl
        {
            get { return _oAuthUrl; }
        }

        protected string _getUserInfoUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={0}&code={1}";
        /// <summary>
        /// 根据code获取成员信息Url
        /// </summary>
        public string GetUserInfoUrl
        {
            get { return _getUserInfoUrl; }
        }

        private string _tokenUrl = "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}";
        /// <summary>
        /// 获取Token地址
        /// </summary>
        public string TokenUrl
        {
            get { return _tokenUrl; }
        }

        protected string _getUserUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/list?access_token={0}&department_id={1}&fetch_child={2}&status={3}";
        /// <summary>
        /// 获取微信用户详细信息Url
        /// </summary>
        public string GetUserUrl
        {
            get { return _getUserUrl; }
        }

        protected string _uploadUrl = "https://qyapi.weixin.qq.com/cgi-bin/material/add_mpnews?access_token={0}";
        //https://qyapi.weixin.qq.com/cgi-bin/material/add_material?type={0}&access_token={1}
        /// <summary>
        /// 获取上传路径地址
        /// </summary>
        public string UploadUrl
        {
            get { return _uploadUrl; }
        }

        protected string _sendUrl = "https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}";
        /// <summary>
        /// 发送推送接口地址
        /// </summary>
        public string SendUrl
        {
            get { return _sendUrl; }
        }

        private string _deleteUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/delete?access_token={0}&userid={1}";


        /// <summary>
        /// 删除通讯录人员接口地址
        /// </summary>
        protected string DeleteUrl
        {
            get { return _deleteUrl; }
        }

        /// <summary>
        /// 禁用通讯录人员接口地址
        /// </summary>
        private string _updateUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/update?access_token={0}";

        protected string UpdateUrl
        {
            get { return _updateUrl; }
        }

        /// <summary>
        /// 发送企业红包接口地址
        /// </summary>

        private string _sendRedPacketUrl = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendworkwxredpack";

        protected string SendRedPacketUrl
        {
            get { return _sendRedPacketUrl; }
        }
        #endregion

        /// <summary>
        /// 上传素材
        /// </summary>
        /// <returns></returns>
        protected abstract U_UploadResult UpLoadSource(U_AccessToken accessTokenModel, U_Content contentModel);

        /// <summary>
        /// 获取公司账号信息
        /// (获取企业号AccessToken)
        /// </summary>
        /// <returns></returns>
        protected bool SetCorpAccount(U_AccessToken accessToken)
        {
            string respText = "";
            bool result = false;

            //获取josn数据
            string url = string.Format(_tokenUrl, accessToken.UserKey, accessToken.UserPassword);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream resStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(resStream, Encoding.Default);
                respText = reader.ReadToEnd();
                resStream.Close();
            }

            try
            {
                JavaScriptSerializer Jss = new JavaScriptSerializer();
                Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);

                //通过键access_token获取值
                _accessToken = respDic["access_token"].ToString();
                if (!string.IsNullOrEmpty(_accessToken))
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogFile.Error, "获取AccessToken PushWeChat.cs：" + ex.ToString() + "/n" + ex.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// 获取企业号下人员详细信息
        /// </summary>
        /// <returns></returns>
        protected List<U_WeChatUser> SearchWeChatUserList()
        {
            U_WeChatUserResult weChatUserResult = new U_WeChatUserResult();
            try
            {
                string url = string.Format(_getUserUrl, _accessToken, 1, 1, 1);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (Stream resStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(resStream, Encoding.UTF8);
                    string respText = reader.ReadToEnd();
                    resStream.Close();
                    //  weChatUserResult = JsonConvert.DeserializeObject<U_WeChatUserResult>(respText);
                    weChatUserResult = JsonHelper.JsonToModel<U_WeChatUserResult>(respText);
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogFile.Error, "获取企业号下人员详细信息PushWeChat.cs：" + ex.Message + "/n" + ex.StackTrace);
            }
            return weChatUserResult.userlist;
        }

        /// <summary>
        /// 通过OAuth验证企业获取code
        /// </summary>
        /// <returns></returns>
        public string GetCorpCode()
        {
            string url = string.Format(_oAuthUrl, ConfigSugar.GetAppString("WeChatCorpID"), ConfigSugar.GetAppString("OpenHttpAddress") + "/BasicDataManagement/WeChatExercise/Index");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream resStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(resStream, Encoding.UTF8);
                string respText = reader.ReadToEnd();
                resStream.Close();
                return respText;
            }

        }

        /// <summary>
        /// 删除微信通讯录人员
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string DeleteWeChatPerson(string userID)
        {
            string respText = "";
            try
            {
                string url = string.Format(_deleteUrl, _accessToken, userID);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (Stream resStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(resStream, Encoding.UTF8);
                    respText = reader.ReadToEnd();
                    resStream.Close();
                    reader.Close();
                }
            }
            catch (Exception exp)
            {
                LogManager.WriteLog(LogFile.Error, "删除微信通讯录人员PushWeChat.cs：" + exp.ToString() + "/n" + exp.StackTrace);
            }
            return respText;
        }


        /// <summary>
        /// 禁用微信通讯录人员
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string DisableWeChatData(string userID)
        {
            string respText = "";
            try
            {
                string responeJsonStr = "";
                responeJsonStr = "{";
                responeJsonStr += "\"userid\": \"" + userID + "\",";
                responeJsonStr += "\"enable\": 0,";
                responeJsonStr += "}";
                string url = string.Format(_updateUrl, _accessToken);
                respText = PostWebRequest(url, responeJsonStr, Encoding.UTF8);
            }
            catch (Exception exp)
            {
                LogManager.WriteLog(LogFile.Error, "禁用微信通讯录人员PushWeChat.cs：" + exp.ToString() + "/n" + exp.StackTrace);
            }
            return respText;
        }


        /// <summary>
        /// 根据userid获取openid
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string ConvertToOpenidByUserId(string accessToken, string userId)
        {
            string respText = "";
            string convertToOpenidUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/convert_to_openid?access_token={0}";
            string responeJsonStr = "";
            responeJsonStr = "{";
            responeJsonStr += "\"userid\": \"" + userId + "\"";
            responeJsonStr += "}";
            string url = string.Format(convertToOpenidUrl, accessToken);
            respText = PostWebRequest(url, responeJsonStr, Encoding.UTF8);
            return respText;
        }
        /// <summary>
        /// 上传临时素材
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string UploadTempResource(string filePath)
        {
            const string url = "https://qyapi.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type=image";
            var uploadUrl = string.Format(url, _accessToken);
            var mediaId = "1G6nrLmr5EC3MMb_-zK1dDdzmd0p7cNliYu9V5w7o8K0";
            using (var client = new WebClient())
            {
                var cm = CacheManager<string>.GetInstance();
                if (cm.Get("media_id") == null)
                {
                    byte[] resource = client.UploadFile(new Uri(uploadUrl), filePath);
                    string retdata = Encoding.UTF8.GetString(resource);
                    var data = JsonConvert.DeserializeObject(retdata) as JObject;
                    if (data != null)
                    {
                        mediaId = data["media_id"].ToString();
                        cm.Add("media_id", mediaId, 3 * 24 * 3600);
                    }
                }
                return mediaId;
            }
        }

        /// <summary>
        /// Post数据接口
        /// </summary>
        /// <param name="postUrl">接口地址</param>
        /// <param name="jsonData">提交json数据</param>
        /// <param name="dataEncode">编码方式</param>
        /// <param name="isUseCert">是否使用证书</param>
        /// <returns></returns>
        protected string PostWebRequest(string postUrl, string jsonData, Encoding dataEncode, bool isUseCert = false)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(jsonData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                if (isUseCert)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory;
                    X509Certificate2 cert = new X509Certificate2(path + WxPayConfig.SSLCERT_PATH, WxPayConfig.SSLCERT_PASSWORD);
                    webReq.ClientCertificates.Add(cert);
                }
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogFile.Error, ex.ToString());
                return ex.Message;
            }
            return ret;
        }

        public static bool ValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return sslPolicyErrors == SslPolicyErrors.None;
        }

        /// <summary>
        /// 通过微信推送类型确定推送的json数据
        /// </summary>
        /// <param name="contentModel"></param>
        protected abstract string GetPushJson(U_UploadResult uploadResult, U_Content contentModel);

        /// <summary>
        /// 将数据库中要推送人员列表与微信获取的人员列表进行对比
        /// </summary>
        /// <param name="contentModel"></param>
        /// <param name="weChatUserList"></param>
        /// <returns></returns>
        protected string GetPushObject(U_Content contentModel, List<U_WeChatUser> weChatUserList)
        {
            List<U_WeChatUser> pushUserList = weChatUserList.Join(contentModel.PushObject, c => c.userid, b => b.UserID, (c, b) => c).ToList();
            string pushObject = "";
            foreach (var i in pushUserList)
            {
                if (i.userid != "")
                {
                    pushObject += i.userid + "|";
                }
            }
            pushObject = pushObject.TrimEnd('|');
            return pushObject;
        }
        /// <summary>
        /// 将数据库中要推送人员列表与微信获取的人员列表进行对比
        /// </summary>
        /// <param name="contentModel"></param>
        /// <param name="weChatUserList"></param>
        /// <returns></returns>
        protected string GetPushObject(U_Content contentModel)
        {
            string pushObject = "";
            foreach (var i in contentModel.PushObject)
            {
                if (i.UserID != "")
                {
                    pushObject += i.UserID + "|";
                }
            }
            pushObject = pushObject.TrimEnd('|');
            return pushObject;
        }
        /// <summary>
        /// 重写Push方法
        /// </summary>
        /// <param name="accessTokenModel"></param>
        /// <param name="contentModel"></param>
        /// <returns></returns>
        public override string Push(U_AccessToken accessTokenModel, U_Content contentModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 重写Push方法
        /// </summary>
        /// <param name="accessTokenModel"></param>
        /// <param name="contentModel"></param>
        /// <returns></returns>
        public override List<string> Push(U_AccessToken accessTokenModel, List<U_Content> contentModels)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 简单工厂类（负责发送微信推送信息）
    /// </summary>
    public class PushWeChatFactory
    {
        public static PushFunction PushMsg(PushMode pushMode, MsgType msgType)
        {
            PushFunction pt = null;
            switch (msgType)
            {
                case MsgType.Text:
                    pt = new TextPush();                    //文本推送
                    break;
                case MsgType.Img:
                    pt = new ImagePush();                   //图片推送
                    break;
                case MsgType.SingleTextAndImg:
                    pt = new SingleTextAndImgPush();        //图文推送
                    break;
                case MsgType.Exercise:
                    pt = new ExercisePush();                //习题推送
                    break;
                case MsgType.Trained:
                    pt = new SingleTextAndImgPush();        //培训推送
                    break;
                case MsgType.KnowledgeBase:                 //知识库推送
                    pt = new KnowledgeBasePush();
                    break;
                case MsgType.Revenue:                       //营收推送
                    pt = new RevenuePush();
                    break;
                case MsgType.Agreement:
                    pt = new AgreementPush();               //协议推送
                    break;
                case MsgType.RedPacket:                     //红包推送
                    pt = new RedPacketPush();
                    break;
                case MsgType.Payment:                       //企业付款推送
                    pt = new PaymentPush();
                    break;
                case MsgType.Salary:
                    pt = new SalaryPush();                //工资条推送
                    break;
            }
            return pt;
        }


    }
}
