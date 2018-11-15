using Common;
using Entities.UserDefinedEntity;
using Service.PushFunction.ShortMessagePush;
using Service.PushFunction.WeChatPush;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DaZhongManagementSystem.Entities.TableEntity;
using Entities.TableEntity;
using Infrastructure.DataServer;

namespace Service.PushFunction
{
    /// <summary>
    /// 相当于接口
    /// </summary>
    public abstract class PushFunction
    {
        /// <summary>
        /// 微信推送或者短信推送
        /// </summary>
        /// <param name="accessTokenModel">授权对象</param>
        /// <param name="contentModel">消息</param>
        /// <returns></returns>
        public abstract string Push(U_AccessToken accessTokenModel, U_Content contentModel);
        /// <summary>
        /// 微信推送或者短信推送
        /// </summary>
        /// <param name="accessTokenModel">授权对象</param>
        /// <param name="contentModels">消息</param>
        /// <returns></returns>
        public abstract List<string> Push(U_AccessToken accessTokenModel, List<U_Content> contentModels);

        /// <summary>
        /// 更新推送消息为已发送
        /// </summary>
        /// <param name="contentModel"></param>
        /// <returns></returns>
        public bool UpdatePushStatus(U_Content contentModel)
        {
            PushServer _ps = new PushServer();
            return _ps.UpdatePushStatus(contentModel);
        }

        /// <summary>
        /// 插入红包信息
        /// </summary>
        /// <param name="redpacketPushInfos"></param>
        /// <returns></returns>
        public bool InsertRedPacketInfo(List<Business_Redpacket_Push_Information> redpacketPushInfos)
        {
            PushServer _ps = new PushServer();
            return _ps.InsertRedPacketInfo(redpacketPushInfos);
        }
        /// <summary>
        /// 插入企业付款信息
        /// </summary>
        /// <param name="paymentInfos"></param>
        /// <returns></returns>
        public bool InsertPaymentInfos(List<Business_Enterprisepayment_Information> paymentInfos)
        {
            PushServer _ps = new PushServer();
            return _ps.InsertPaymentInfos(paymentInfos);
        }
        /// <summary>
        /// 更新是否查看推送状态
        /// </summary>
        /// <param name="contentModel"></param>
        /// <returns></returns>
        public bool UpdateIsReadStatus(U_Content contentModel, string invalidusers = "")
        {
            PushServer _ps = new PushServer();
            return _ps.UpdateIsReadStatus(contentModel, invalidusers);
        }

    }


    public class PushFunctionFactory
    {
        public static PushFunction PushFunc(PushMode pushMode, MsgType msgType)
        {
            PushFunction pf = null;
            switch (pushMode)
            {
                case PushMode.WeChat:
                    pf = PushWeChatFactory.PushMsg(pushMode, msgType);
                    break;
                case PushMode.ShortMsg:
                    //pf = PushShortMsg
                    pf = PushShortMsgFactory.PushMsg(pushMode, msgType);
                    break;
            }
            return pf;
        }
    }
}
