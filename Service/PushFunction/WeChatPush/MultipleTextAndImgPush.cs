using Entities.UserDefinedEntity;
using Service.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using SyntacticSugar;

namespace Service.PushFunction.WeChatPush
{
    /// <summary>
    /// 
    /// </summary>
    public class MultipleTextAndImgPush : PushWeChat
    {
        public MultipleTextAndImgPush()
        {
            //_pl = new PushLogic();
        }

        protected override U_UploadResult UpLoadSource(U_AccessToken accessTokenModel, U_Content contentModel)
        {
            U_UploadResult uploadResult = new U_UploadResult();
            return uploadResult;
        }

        protected override string GetPushJson(U_UploadResult uploadResult, U_Content contentModel)
        {
            //List<U_WeChatUser> weChatUserList = SearchWeChatUserList();
            //string pushObject = GetPushObject(contentModel, weChatUserList);
            string pushObject = GetPushObject(contentModel);
            string responeJsonStr = "";
            responeJsonStr = "{";
            responeJsonStr += "\"touser\": \"" + pushObject + "\",";
            responeJsonStr += "\"toparty\": \"\",";
            responeJsonStr += "\"totag\": \"\",";
            responeJsonStr += "\"msgtype\": \"news\",";
            responeJsonStr += "\"agentid\":\"" + _weChatAgentID + "\",";
            responeJsonStr += "\"news\": {";
            responeJsonStr += "\"articles\":[";
            responeJsonStr += " {";
            responeJsonStr += "\"title\": \"" + contentModel.Title + "\",";
            responeJsonStr += "\"description\": \"" + contentModel.CoverDescption + "\",";
            responeJsonStr += "\"url\": \"" + ConfigSugar.GetAppString("OpenHttpAddress") + "/BasicDataManagement/WeChatExercise/Index\",";
            responeJsonStr += "\"picurl\": \"http://wx.qlogo.cn/mmhead/Q3auHgzwzM5eGibkia18N1q7icAdkgOIS3gvWZYaXTqbq3Tr8iaZM6S5jw/0\",";
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
            //获取微信用户详细信息
            List<U_WeChatUser> weChatUserList = SearchWeChatUserList();
            //比较微信用户列表与要推送人员列表
            List<U_WeChatUser> pushPersonListOk = weChatUserList.Join(contentModel.PushObject, c => c.mobile, b => b.PhoneNumber, (c, b) => c).ToList();
            if (result)
            {
                string postUrl = string.Format(_sendUrl, _accessToken);
                //获取推送内容Json
                string json = GetPushJson(uploadResult, contentModel);
                //推送
                string pushResult = PostWebRequest(postUrl, json, Encoding.UTF8);
                U_WeChatResult resultModel = JsonHelper.JsonToModel<U_WeChatResult>(pushResult);
                if (resultModel.errcode == 0)
                {
                    UpdatePushStatus(contentModel);
                }

                return pushResult;
            }
            else
            {
                return "推送失败！";
            }
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
