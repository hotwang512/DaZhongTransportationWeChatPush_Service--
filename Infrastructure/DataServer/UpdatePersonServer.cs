using DaZhongManagementSystem.Entities.TableEntity;
using Entities.ViewEntity;
using Infrastructure.SugarDao;
using SqlSugar;
using System;
using System.Collections.Generic;
using Entities.UserDefinedEntity;
using ServiceLog;

namespace Infrastructure.DataServer
{
    public class UpdatePersonServer
    {
        /// <summary>
        /// 获取离职人员列表（存在于Person表而不存在与middata表的人员列表）
        /// </summary>
        /// <returns></returns>
        public List<V_UserNotexistsmiddata> GetPeosonList()
        {
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                List<V_UserNotexistsmiddata> personList = new List<V_UserNotexistsmiddata>();
                try
                {
                    personList = _dbMsSql.Queryable<V_UserNotexistsmiddata>().Where(c => c.ApprovalStatus != 4).Where(i => i.ApprovalType == 1).ToList();
                }
                catch (Exception exp)
                {
                    LogManager.WriteLog(LogFile.Error, "获取离职人员列表：" + exp.Message);
                }
                return personList;
            }
        }

        /// <summary>
        /// 删除Person列表数据
        /// </summary>
        /// <param name="personModel"></param>
        /// <returns></returns>
        public bool DeletePersonData(V_UserNotexistsmiddata personModel)
        {
            bool result = false;
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                try
                {
                    result = _dbMsSql.Delete<Business_Personnel_Information>(i => i.Vguid == personModel.Vguid);
                }
                catch (Exception exp)
                {
                    LogManager.WriteLog(LogFile.Error, "删除Person表：" + exp.Message);
                }
            }
            return result;
        }


        /// <summary>
        /// 更改Person列表的人员状态
        /// </summary>
        /// <param name="personModel"></param>
        ///  <param name="status">更改状态为:3-“未匹配”,4-"已离职"</param>
        /// <returns></returns>
        public bool UpdatePersonStatus(V_UserNotexistsmiddata personModel, int status)
        {
            bool result = false;
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                try
                {
                    result = _dbMsSql.Update<Business_Personnel_Information>(new { ApprovalStatus = status }, i => i.Vguid == personModel.Vguid);  //
                }
                catch (Exception exp)
                {
                    LogManager.WriteLog(LogFile.Error, "更改Person表人员状态：" + exp.Message);
                }
            }
            return result;
        }
        /// <summary>
        /// 将人员状态由未匹配改为已关注
        /// </summary>
        /// <param name="personModel"></param>
        /// <returns></returns>
        public bool UpdateStatus2Focus(V_UserNotexistsmiddata personModel)
        {
            bool result = false;
            using (SqlSugarClient _dbMsSql = SugarDao_MsSql.GetInstance())
            {
                try
                {
                    result = _dbMsSql.Update<Business_Personnel_Information>(new { ApprovalStatus = 2 }, i => i.ApprovalStatus == 3);  //
                }
                catch (Exception exp)
                {
                    LogManager.WriteLog(LogFile.Error, "更改Person表人员状态：" + exp.Message);
                }
            }
            return result;
        }


        /// <summary>
        /// 根据微信后台人员状态，更改人员表的状态
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool UpdatePersonStatus(UserList user)
        {
            bool result = false;
            using (var db = SugarDao_MsSql.GetInstance())
            {
                try
                {
                    switch (user.status)
                    {
                        case "1":  //已激活
                            result = db.Update<Business_Personnel_Information>(new { ApprovalStatus = 2 }, i => i.UserID == user.userid); //已关注
                            break;
                        case "2":  //已禁用
                            result = db.Update<Business_Personnel_Information>(new { ApprovalStatus = 5 }, i => i.UserID == user.userid); //已禁用
                            break;
                        case "4":  //未激活
                            result = db.Update<Business_Personnel_Information>(new { ApprovalStatus = 1 }, i => i.UserID == user.userid); //未关注
                            break;
                    }
                }
                catch (Exception exp)
                {
                    LogManager.WriteLog(LogFile.Error, "同步微信后台，更改Person表人员状态：" + exp.Message);
                }
            }
            return result;
        }
    }
}
