using Entities.UserDefinedEntity;
using Service.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Service.PushFunction.WeChatPush
{
    public class VedioPush : PushWeChat
    {
        public VedioPush()
        {

        }

        protected override U_UploadResult UpLoadSource(U_AccessToken accessTokenModel, U_Content contentModel)
        {
            U_UploadResult uploadResult = new U_UploadResult();
            return uploadResult;
        }

        /// <summary>
        /// 重写推送方法
        /// </summary>
        /// <param name="accessTokenModel"></param>
        /// <param name="contentModel"></param>
        /// <returns></returns>
        public override string Push(U_AccessToken accessTokenModel, U_Content contentModel)
        {

            SetCorpAccount(accessTokenModel);
            U_UploadResult uploadResult = UpLoadSource(accessTokenModel, contentModel);
            string postUrl = string.Format(_sendUrl, _accessToken);
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

        protected override string GetPushJson(U_UploadResult uploadResult, U_Content contentModel)
        {
            throw new NotImplementedException();
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
