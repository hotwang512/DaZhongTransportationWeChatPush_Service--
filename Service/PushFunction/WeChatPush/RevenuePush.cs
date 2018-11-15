using System.Collections.Generic;
using System.Text;
using Common;
using Entities.UserDefinedEntity;
using SyntacticSugar;

namespace Service.PushFunction.WeChatPush
{
    public class RevenuePush : PushWeChat
    {

        protected override U_UploadResult UpLoadSource(U_AccessToken accessTokenModel, U_Content contentModel)
        {
            return null;
        }

        protected override string GetPushJson(U_UploadResult uploadResult, U_Content contentModel)
        {
            string revenueUrl = ConfigSugar.GetAppString("OpenHttpAddress") + "/WeChatPush/WeChatRevenue/RevenuePay?Vguid="+contentModel.VGUID;  //营收支付页面
            string fixedAmountUrl = ConfigSugar.GetAppString("OpenHttpAddress") + "/WeChatPush/WeChatRevenue/FixedPay?Vguid=" + contentModel.VGUID;  //营收支付页面
            string url = string.Format(_oAuthUrl, ConfigSugar.GetAppString("WeChatCorpID"), contentModel.RevenueType == 1 ? fixedAmountUrl : revenueUrl);

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
                    UpdateIsReadStatus(contentModel, resultModel.invaliduser);
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
