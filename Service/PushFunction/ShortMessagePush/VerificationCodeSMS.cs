using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Entities.UserDefinedEntity;
using SyntacticSugar;

namespace Service.PushFunction.ShortMessagePush
{
    public class VerificationCodeSMS : PushShortMsg
    {
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="accessTokenModel">授权对象</param>
        /// <param name="contentModel">消息模板</param>
        /// <returns></returns>
        public override string Push(U_AccessToken accessTokenModel, U_Content contentModel)
        {
            HttpClient httpClient = new HttpClient();
            //将服务凭证转换为Base64编码格式  
            byte[] auth = Encoding.UTF8.GetBytes(string.Format("{0}:{1}", ConfigSugar.GetAppString("SMSAppKey"), ConfigSugar.GetAppString("MasterSecret")));
            String auth64 = Convert.ToBase64String(auth);
            //创建并指定服务凭证，认证方案为Basic  
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", auth64);

            ///发送短信验证码参数
            U_Para para = new U_Para();
            para.mobile = contentModel.PushObject[0].PhoneNumber.Trim();
            para.temp_id = 1;
            //将参数转换为Json字符串
            string json = para.ModelToJson();
            //构造HTTP 字符内容对象
            System.Net.Http.StringContent sc = new System.Net.Http.StringContent(json);
            //移除Content-Type内容
            sc.Headers.Remove("Content-Type");
            //Content-Type内容设置为JSON类型
            sc.Headers.Add("Content-Type", "application/json");
            //发送短信
            Task<HttpResponseMessage> taskHrm = httpClient.PostAsync(_verificationCodeUrl, sc);
            Task.WaitAll(taskHrm);
            //获取返回值
            Task<string> taskStr = taskHrm.Result.Content.ReadAsStringAsync();
            Task.WaitAll(taskStr);
            httpClient.Dispose();
            UpdatePushStatus(contentModel);
            return taskStr.Result;
        }
        /// <summary>
        /// 微信推送或者短信推送
        /// </summary>
        /// <param name="accessTokenModel">授权对象</param>
        /// <param name="contentModel">消息</param>
        /// <returns></returns>
        public override List<string> Push(U_AccessToken accessTokenModel, List<U_Content> contentModels)
        {
            List<string> rtnList = new List<string>();
            foreach (var contentModel in contentModels)
            {
                string rtn = Push(accessTokenModel, contentModel);
                rtnList.Add(rtn);
            }
            return rtnList;
        }
    }
}
