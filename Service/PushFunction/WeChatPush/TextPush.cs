using Entities.UserDefinedEntity;
using Service.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using ServiceLog;
using SyntacticSugar;

namespace Service.PushFunction.WeChatPush
{
    public class TextPush : PushWeChat
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TextPush()
        {
            //_uploadUrl = "";
            //_pl = new PushLogic();
        }

        protected override U_UploadResult UpLoadSource(U_AccessToken accessTokenModel, U_Content contentModel)
        {
            return null;
        }

        /// <summary>
        /// 生成推送的Json数据格式
        /// </summary>
        /// <param name="uploadResult"></param>
        /// <param name="contentModel"></param>
        /// <returns></returns>
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
            responeJsonStr += "\"msgtype\": \"text\",";
            responeJsonStr += "\"agentid\":\"" + ConfigSugar.GetAppString("WeChatAgentID") + "\",";
            responeJsonStr += "\"text\": {";
            responeJsonStr += "\"content\":\"" + contentModel.Message + "\",";
            responeJsonStr += "},";
            responeJsonStr += "\"safe\":\"0\"";
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
            U_UploadResult uploadResult = new U_UploadResult();
            //比较微信用户列表与要推送人员列表
            //List<U_WeChatUser> pushPersonListOk = weChatUserList.Join(contentModel.PushObject, c => c.mobile, b => b.PhoneNumber, (c, b) => c).ToList();
            if (result)
            {
                string postUrl = string.Format(_sendUrl, _accessToken);
                //获取推送内容Json
                string json = GetPushJson(uploadResult, contentModel);
                //推送
                string pushResult = PostWebRequest(postUrl, json, Encoding.UTF8);
                U_WeChatResult resultModel = JsonHelper.JsonToModel<U_WeChatResult>(pushResult);
                UpdatePushStatus(contentModel);
                if (resultModel.errcode == 0)
                {
                    UpdateIsReadStatus(contentModel, resultModel.invaliduser);
                }
                else
                {
                    LogManager.WriteLog(LogFile.Error, "文本推送" + resultModel.errcode + resultModel.errmsg);
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
