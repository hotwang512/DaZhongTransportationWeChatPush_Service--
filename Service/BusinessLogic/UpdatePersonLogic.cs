using Common;
using DaZhongManagementSystem.Entities.TableEntity;
using Entities.UserDefinedEntity;
using Entities.ViewEntity;
using Infrastructure.DataServer;
using Service.PushFunction.WeChatPush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLog;

namespace Service.BusinessLogic
{
    public class UpdatePersonLogic : PushWeChat
    {
        public UpdatePersonServer _updatePersonServer;
        public UpdatePersonLogic()
        {
            _updatePersonServer = new UpdatePersonServer();
        }

        /// <summary>
        /// 获取离职人员列表（存在于Person表而不存在与middata表的人员列表）
        /// </summary>
        /// <returns></returns>
        public List<V_UserNotexistsmiddata> GetPeosonList()
        {
            return _updatePersonServer.GetPeosonList();
        }

        /// <summary>
        /// 删除Person列表数据
        /// </summary>
        /// <param name="personList"></param>
        /// <returns></returns>
        public bool DeletePersonData(List<V_UserNotexistsmiddata> personList)
        {
            bool result = false;
            foreach (var item in personList)
            {
                result = _updatePersonServer.DeletePersonData(item);
            }
            return result;
        }
        /// <summary>
        /// 更改Person列表的人员状态
        /// </summary>
        /// <param name="personList"></param>
        ///  <param name="status"></param>
        /// <returns></returns>
        public bool UpdatePersonStatus(List<V_UserNotexistsmiddata> personList, int status)
        {
            bool result = false;
            foreach (var item in personList)
            {
                result = _updatePersonServer.UpdatePersonStatus(item, status);
            }
            return result;
        }
        /// <summary>
        /// 更改Person列表的人员状态
        /// </summary>
        /// <param name="personList"></param>
        /// <returns></returns>
        public bool UpdateStatus2Focus(List<V_UserNotexistsmiddata> personList)
        {
            bool result = false;
            foreach (var item in personList)
            {
                result = _updatePersonServer.UpdateStatus2Focus(item);
            }
            return result;
        }
        /// <summary>
        /// 删除微信通讯录人员
        /// </summary>
        /// <returns></returns>
        public bool DeleteWeChatData(U_AccessToken accessTokenModel, List<V_UserNotexistsmiddata> personList)
        {
            bool result = false;
            string isSuccessDelete = string.Empty;
            bool isGetAccessToken = SetCorpAccount(accessTokenModel);
            if (isGetAccessToken)
            {
                foreach (var item in personList)
                {
                    isSuccessDelete = DeleteWeChatPerson(item.UserID);
                    U_WeChatResult resultModel = JsonHelper.JsonToModel<U_WeChatResult>(isSuccessDelete);
                    if (resultModel.errcode == 0)
                    {
                        result = true;
                    }
                    else
                    {
                        LogManager.WriteLog(LogFile.Error, "删除微信通讯录人员Logic：" + resultModel.errcode + resultModel.errmsg);
                        result = false;
                        return result;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 禁用微信通讯录人员
        /// </summary>
        /// <param name="accessTokenModel"></param>
        /// <param name="personList"></param>
        /// <returns></returns>
        public bool DisableWeChatData(U_AccessToken accessTokenModel, List<V_UserNotexistsmiddata> personList)
        {
            bool result = false;
            string isSuccess = string.Empty;
            bool isGetAccessToken = SetCorpAccount(accessTokenModel);
            if (isGetAccessToken)
            {
                foreach (var item in personList)
                {
                    isSuccess = DisableWeChatData(item.UserID);
                    U_WeChatResult resultModel = JsonHelper.JsonToModel<U_WeChatResult>(isSuccess);
                    if (resultModel.errcode == 0)
                    {
                        result = true;
                    }
                    else
                    {
                        LogManager.WriteLog(LogFile.Error, "禁用微信通讯录人员Logic：" + resultModel.errcode + resultModel.errmsg);
                        result = false;
                        return result;
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///  根据微信后台人员状态，更改人员表的状态
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public bool UpdatePersonStatus(List<UserList> users)
        {
            bool result = false;
            foreach (var user in users)
            {
                result = _updatePersonServer.UpdatePersonStatus(user);
            }
            return result;
        }
        protected override U_UploadResult UpLoadSource(U_AccessToken accessTokenModel, U_Content contentModel)
        {
            return null;
        }

        protected override string GetPushJson(U_UploadResult uploadResult, U_Content contentModel)
        {
            return null;
        }
    }
}
