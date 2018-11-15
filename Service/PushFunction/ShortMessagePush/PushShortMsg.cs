using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Entities.UserDefinedEntity;
using Common;

namespace Service.PushFunction.ShortMessagePush
{
    public abstract class PushShortMsg : PushFunction
    {

        protected string _noticeUrl = "https://api.sms.jpush.cn/v1/messages";
        /// <summary>
        /// 短信消息接口地址
        /// </summary>
        public string NoticeUrl
        {
            get { return _noticeUrl; }
        }

        protected string _verificationCodeUrl = "https://api.sms.jpush.cn/v1/codes";
        /// <summary>
        /// 短信发送验证码接口地址
        /// </summary>
        protected string VerificationCode
        {
            get { return _verificationCodeUrl; }
        }

        protected string _validVerificationCodeUrl = "https://api.sms.jpush.cn/v1/codes/{0}/valid";
        /// <summary>
        /// 验证短信验证码接口地址
        /// </summary>
        protected string ValidVerificationCodeUrl
        {
            get { return _validVerificationCodeUrl; }
        }
        /// <summary>
        /// 短信发送
        /// </summary>
        /// <param name="accessTokenModel">短信凭据对象</param>
        /// <param name="contentModel">消息对象</param>
        /// <returns></returns>
        public override string Push(U_AccessToken accessTokenModel, U_Content contentModel)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 微信推送或者短信推送
        /// </summary>
        /// <param name="accessTokenModel">授权对象</param>
        /// <param name="contentModel">消息</param>
        /// <returns></returns>
        public override List<string> Push(U_AccessToken accessTokenModel, List<U_Content> contentModels)
        {
            throw new NotImplementedException();

        }
    }

    /// <summary>
    /// 简单工厂类（负责发送微信推送信息）
    /// </summary>
    public class PushShortMsgFactory
    {
        public static PushFunction PushMsg(PushMode pushMode, MsgType msgType)
        {
            PushFunction pt = null;
            switch (msgType)
            {
                case MsgType.NoticeSMS:
                    pt = new NoticeSMS();                   //通知短信
                    break;
                case MsgType.RevenueSMS:
                    pt = new RevenueSMS();                  //营收短信
                    break;
                case MsgType.VerificationCodeSMS:
                    pt = new VerificationCodeSMS();         //验证码短信
                    break;

                case MsgType.ValidVerificationCodeSMS:
                    pt = new ValidVerificationCodeSMS();    //验证验证码短信
                    break;
            }
            return pt;
        }
    }
}
