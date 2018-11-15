using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Entities.UserDefinedEntity;
using SyntacticSugar;
using Infrastructure.DataServer;

namespace Service.PushFunction.ShortMessagePush
{
    public class NoticeSMS : PushShortMsg
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
            U_Notification_Teamp_Para ntp = new U_Notification_Teamp_Para();
            //将短信营收实体转换微信
            ntp.Notification = contentModel.Message;

            /*U_Temp_Para tp = new U_Temp_Para();*/

            U_Para para = new U_Para();
            para.mobile = contentModel.PushObject[0].PhoneNumber.Trim();
            para.temp_id = Convert.ToInt32(ConfigSugar.GetAppString("Notification_Temp_ID"));
            para.temp_para = ntp;

            string json = Common.JsonHelper.ModelToJson<U_Para>(para);

            System.Net.Http.StringContent sc = new System.Net.Http.StringContent(json);
            sc.Headers.Remove("Content-Type");
            sc.Headers.Add("Content-Type", "application/json");
            Task<HttpResponseMessage> taskHrm = httpClient.PostAsync(_noticeUrl, sc);
            Task.WaitAll(taskHrm);
            Task<string> taskStr = taskHrm.Result.Content.ReadAsStringAsync();
            Task.WaitAll(taskStr);
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
