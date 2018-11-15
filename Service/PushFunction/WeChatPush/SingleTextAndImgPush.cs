using Common;
using Entities.UserDefinedEntity;
using Infrastructure.DataServer;
using Service.BusinessLogic;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.PushFunction.WeChatPush
{
    public class SingleTextAndImgPush : PushWeChat
    {
        public SingleTextAndImgPush()
        {
            //_pl = new PushLogic();
        }

        protected override U_UploadResult UpLoadSource(U_AccessToken accessTokenModel, U_Content contentModel)
        {
            return null;
        }

        protected override string GetPushJson(U_UploadResult uploadResult, U_Content contentModel)
        {
            string pushUrl = ConfigSugar.GetAppString("OpenHttpAddress") + "/WeChatPush/PushDetail/PushDetail?Vguid=" + contentModel.VGUID;//跳转推送内容界面Url
            string url = string.Format(_oAuthUrl, ConfigSugar.GetAppString("WeChatCorpID"), pushUrl);

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
            responeJsonStr += "\"picurl\": \"" + ConfigSugar.GetAppString("OpenHttpAddress") + contentModel.CoverImg + "\"";
            responeJsonStr += "}";
            responeJsonStr += "]";
            responeJsonStr += "}";
            responeJsonStr += "}";
            return responeJsonStr;
        }

        protected string GetPushJson(U_UploadResult uploadResult, List<U_Content> contentModels)
        {
            string pushUrl = string.Empty;
            string url = string.Empty;
            string responeJsonStr = "";
            List<U_WeChatUser> weChatUserList = SearchWeChatUserList();
            string pushObject = GetPushObject(contentModels[0], weChatUserList);
            responeJsonStr = "{";
            responeJsonStr += "\"touser\": \"" + pushObject + "\",";
            responeJsonStr += "\"toparty\": \"\",";
            responeJsonStr += "\"totag\": \"\",";
            responeJsonStr += "\"msgtype\": \"news\",";
            responeJsonStr += "\"agentid\":\"" + ConfigSugar.GetAppString("WeChatAgentID") + "\",";
            responeJsonStr += "\"news\": {";
            responeJsonStr += "\"articles\":[";
            foreach (var content in contentModels)
            {
                pushUrl = ConfigSugar.GetAppString("OpenHttpAddress") + "/WeChatPush/PushDetail/PushDetail?Vguid=" + content.VGUID;//跳转推送内容界面Url
                url = string.Format(_oAuthUrl, ConfigSugar.GetAppString("WeChatCorpID"), pushUrl);
                responeJsonStr += " {";
                responeJsonStr += "\"title\": \"" + content.Title + "\",";
                responeJsonStr += "\"description\": \"" + content.CoverDescption + "\",";
                responeJsonStr += "\"url\": \"" + url + "\",";
                responeJsonStr += "\"picurl\": \"" + ConfigSugar.GetAppString("OpenHttpAddress") + content.CoverImg + "\"";
                responeJsonStr += "}";
                responeJsonStr += ",";
            }
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
            //比较微信用户列表与要推送人员列表
            //List<U_WeChatUser> pushPersonListOk = weChatUserList.Join(contentModel.PushObject, c => c.mobile, b => b.PhoneNumber, (c, b) => c).ToList();
            if (result)
            {
                string postUrl = string.Format(_sendUrl, _accessToken);
                //获取推送内容Json
                string json = GetPushJson(uploadResult, contentModel);
                //推送
                string pushResult = PostWebRequest(postUrl, json, Encoding.UTF8);
                //   U_WeChatResult resultModel = JsonHelper.JsonToModel<U_WeChatResult>(pushResult);

                UpdatePushStatus(contentModel);

                return pushResult;
            }
            else
            {
                return "推送失败！";
            }
        }

        public override List<string> Push(U_AccessToken accessTokenModel, List<U_Content> contentModels)
        {
            //获取AccessToken
            List<string> rtnList = new List<string>();
            bool result = SetCorpAccount(accessTokenModel);
            if (result)
            {
                string postUrl = string.Format(_sendUrl, _accessToken);
                //获取推送内容Json
                string json = GetPushJson(null, contentModels);
                //推送
                string pushResult = PostWebRequest(postUrl, json, Encoding.UTF8);
                rtnList.Add(pushResult);
                foreach (var contentModel in contentModels)
                {
                    UpdatePushStatus(contentModel);
                }
            }
            else
            {
                rtnList.Add("推送失败!");

            }
            return rtnList;
        }
    }
}
