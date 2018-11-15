using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Entities.UserDefinedEntity;
using ServiceLog;
using SyntacticSugar;

namespace Service.PushFunction.WeChatPush
{
    public class AgreementPush : PushWeChat
    {
        protected override U_UploadResult UpLoadSource(U_AccessToken accessTokenModel, U_Content contentModel)
        {
            return null;
        }

        protected override string GetPushJson(U_UploadResult uploadResult, U_Content contentModel)
        {
            string knowledgeBaseUrl = ConfigSugar.GetAppString("OpenHttpAddress") + "/WeChatPush/PushDetail/AgreementDetail?Vguid=" + contentModel.VGUID;//跳转推送内容界面Url

            string url = string.Format(_oAuthUrl, ConfigSugar.GetAppString("WeChatCorpID"), knowledgeBaseUrl);
            ////企业获取Code
            //string code = GetCorpCode();
            //List<U_WeChatUser> weChatUserList = SearchWeChatUserList();
            //string pushObject = GetPushObject(contentModel, weChatUserList);
            string pushObject = GetPushObject(contentModel);
            string responeJsonStr = "";
            responeJsonStr = "{";
            responeJsonStr += "\"touser\": \"" + pushObject + "\",";
            responeJsonStr += "\"toparty\": \"\",";
            responeJsonStr += "\"totag\": \"\",";
            responeJsonStr += "\"msgtype\": \"news\",";
            responeJsonStr += "\"agentid\":\"" + ConfigSugar.GetAppString("WeChatAgentID") + "\",";
            responeJsonStr += "\"news\": {";
            responeJsonStr += "\"articles\":[";
            responeJsonStr += " {";
            responeJsonStr += "\"title\": \"" + contentModel.Title + "\",";
            responeJsonStr += "\"description\": \"" + contentModel.CoverDescption + "\",";
            responeJsonStr += "\"url\": \"" + url + "\",";
            responeJsonStr += "\"picurl\": \"" + ConfigSugar.GetAppString("OpenHttpAddress") + contentModel.CoverImg + "\",";
            responeJsonStr += "}";
            responeJsonStr += "]";
            responeJsonStr += "}";
            responeJsonStr += "}";
            return responeJsonStr;
        }

        /// <summary>
        /// 重写推送方法
        /// </summary>
        /// <param name="accessTokenModel"></param>
        /// <param name="contentModel"></param>
        /// <returns></returns>
        public override string Push(U_AccessToken accessTokenModel, U_Content contentModel)
        {
            //获取AccessToken
            bool result = SetCorpAccount(accessTokenModel);
            //如果有素材需要上传，则上传素材，否则返回null
            U_UploadResult uploadResult = UpLoadSource(accessTokenModel, contentModel);
            if (!result) return "推送失败！";
            string pushResult;
            try
            {
                string postUrl = string.Format(_sendUrl, _accessToken);
                //获取推送内容Json
                string json = GetPushJson(uploadResult, contentModel);
                //推送
                pushResult = PostWebRequest(postUrl, json, Encoding.UTF8);
                U_WeChatResult resultModel = JsonHelper.JsonToModel<U_WeChatResult>(pushResult);
                if (resultModel.errcode != 0)
                {
                    LogManager.WriteLog(LogFile.Error, "协议推送:" + resultModel.errcode + resultModel.errmsg);
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogFile.Error, ex.ToString());
                pushResult = "推送失败！";
            }
            UpdatePushStatus(contentModel);
            UpdateIsReadStatus(contentModel);
            return pushResult;
        }

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
