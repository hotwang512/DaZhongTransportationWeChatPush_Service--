using DaZhongManagementSystem.Entities.TableEntity;
using System;
using System.Collections.Generic;
using SqlSugar;
using Entities.TableEntity;
using Entities.UserDefinedEntity;
using ServiceLog;

namespace Infrastructure.DataServer
{
    public class PushServer
    {
        /// <summary>
        /// 获取推送数据
        /// </summary>
        /// <returns></returns>
        public string GetPushMsg()
        {
            using (SqlSugar.SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                string responseMsg = "";
                bool result = false;
                List<Business_WeChatPush_Information> pushList = GetPushList();//获取要推送的数据
                foreach (var item in pushList)
                {
                    List<Business_Personnel_Information> pushPersonList = new List<Business_Personnel_Information>();
                    pushPersonList = GetPushUserWeChat(item);//获取推送信息接收者信息列表
                    result = PushMsg(item, pushPersonList);//返回推送结果
                    if (!result)
                    {
                        responseMsg = item.Title + "推送失败";
                        return responseMsg;
                    }
                }

                return responseMsg;
            }
        }

        /// <summary>
        /// 获取推送列表（获取要推送的数据）
        /// </summary>
        /// <returns></returns>
        public List<Business_WeChatPush_Information> GetPushList()
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                List<Business_WeChatPush_Information> pushList = _dbMsSql.Queryable<Business_WeChatPush_Information>().Where(i => i.Status == 3 && i.MessageType != 3).ToList();//获取已审核数据进行推送
                return pushList;
            }
        }
        /// <summary>
        /// 获取推送列表（获取要推送的数据）
        /// </summary>
        /// <returns></returns>
        public List<Business_WeChatPush_Information> GetImgPushList()
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                List<Business_WeChatPush_Information> pushList = _dbMsSql.Queryable<Business_WeChatPush_Information>().Where(i => i.Status == 3 && i.MessageType == 3).ToList();//获取已审核数据进行推送
                return pushList;
            }
        }

        /// <summary>
        /// 获取多图文子表信息
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public List<Business_WeChatPush_MoreGraphic_Information> GetMoreGraphicList(Guid vguid)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                var moreGraphicList = _dbMsSql.Queryable<Business_WeChatPush_MoreGraphic_Information>().Where(i => i.WeChatPushVguid == vguid).OrderBy(i => i.Ranks).ToList();//获取已审核数据进行推送
                return moreGraphicList;
            }
        }

        /// <summary>
        /// 获取推送用户信息列表（获取推送接收者信息列表）微信
        /// </summary>
        /// <param name="vguid"></param>
        /// <returns></returns>
        public List<Business_Personnel_Information> GetPushUserWeChat(Business_WeChatPush_Information pushModel)
        {
            using (SqlSugar.SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                var queryable = _dbMsSql.Queryable<Business_Personnel_Information>()
               .JoinTable<Business_Personnel_Information, Business_WeChatPushDetail_Information>((c1, c2) => c1.UserID == c2.PushObject && c2.Business_WeChatPushVguid == pushModel.VGUID, JoinType.INNER);
                return queryable.ToList();
            }
        }

        /// <summary>
        /// 获取推送用户信息列表（获取推送接收者信息列表）短信
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Business_Personnel_Information> GetPushUserSms(Business_WeChatPush_Information pushModel)
        {
            using (SqlSugar.SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                var queryable = _dbMsSql.Queryable<Business_Personnel_Information>()
               .JoinTable<Business_Personnel_Information, Business_WeChatPushDetail_Information>((c1, c2) => c1.UserID == c2.PushObject && c2.Business_WeChatPushVguid == pushModel.VGUID, JoinType.INNER);
                return queryable.ToList();
            }
        }

        /// <summary>
        /// 推送消息(定时/不定时)
        /// </summary>
        /// <param name="pushPersonList"></param>
        /// <returns></returns>
        public bool PushMsg(Business_WeChatPush_Information pushModel, List<Business_Personnel_Information> pushPersonList)
        {
            bool result = false;
            //PushFunction pf = new PushFunction();
            return result;
        }

        /// <summary>
        /// 更新推送状态
        /// </summary>
        /// <param name="contentModel"></param>
        /// <returns></returns>
        public bool UpdatePushStatus(U_Content contentModel)
        {
            using (SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                result = _dbMsSql.Update<Business_WeChatPush_Information>(new { Status = 4, PushDate = DateTime.Now }, i => i.VGUID == contentModel.VGUID);

                return result;
            }
        }

        /// <summary>
        /// 插入红包信息
        /// </summary>
        /// <param name="redpacketPushInfos"></param>
        /// <returns></returns>
        public bool InsertRedPacketInfo(List<Business_Redpacket_Push_Information> redpacketPushInfos)
        {
            using (SqlSugarClient db = SugarDao.SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                result = db.SqlBulkCopy(redpacketPushInfos);
                return result;
            }
        }
        /// <summary>
        /// 插入企业付款信息
        /// </summary>
        /// <param name="paymentInfos"></param>
        /// <returns></returns>
        public bool InsertPaymentInfos(List<Business_Enterprisepayment_Information> paymentInfos)
        {
            using (SqlSugarClient db = SugarDao.SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                result = db.SqlBulkCopy(paymentInfos);
                return result;
            }
        }
        /// <summary>
        /// 更新是否查看推送状态
        /// </summary>
        /// <param name="contentModel"></param>
        /// <returns></returns>
        public bool UpdateIsReadStatus(U_Content contentModel, string invalidusers = "")
        {
            using (SqlSugar.SqlSugarClient _dbMsSql = SugarDao.SugarDao_MsSql.GetInstance())
            {
                bool result = false;
                try
                {
                    foreach (var item in contentModel.PushObject)
                    {
                        if (!invalidusers.Contains(item.UserID))
                        {
                            result = _dbMsSql.Update<Business_WeChatPushDetail_Information>(new { ISRead = "1" }, i => i.Business_WeChatPushVguid == contentModel.VGUID && i.PushObject == item.UserID);
                        }
                    }
                }
                catch (Exception exp)
                {
                    LogManager.WriteLog(LogFile.Error, "更新推送是否查看：" + exp.ToString());
                }

                return result;
            }
        }



    }
}
