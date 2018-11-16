using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public enum MsgType
    {
        /// <summary>
        /// 转换报错
        /// </summary>
        None = 0,

        /// <summary>
        /// 文本推送
        /// </summary>
        Text = 1,

        /// <summary>
        /// 图片推送
        /// </summary>
        Img = 2,

        /// <summary>
        /// 单图文推送
        /// </summary>
        SingleTextAndImg = 3,

        /// <summary>
        /// 习题推送
        /// </summary>
        Exercise = 4,

        /// <summary>
        /// 培训推送
        /// </summary>
        Trained = 5,

        /// <summary>
        /// 预留推送
        /// </summary>
    //    Vidio = 6,
        /// <summary>
        /// 知识库推送
        /// </summary>
        KnowledgeBase = 6,

        /// <summary>
        /// 多图文推送
        /// </summary>
        //MultipleTextAndImg = 5,

        /// <summary>
        /// 短信通知
        /// </summary>
        NoticeSMS = 7,

        /// <summary>
        /// 营收短信
        /// </summary>
        RevenueSMS = 8,
        /// <summary>
        /// 验证码短信
        /// </summary>
        VerificationCodeSMS = 9,
        /// <summary>
        /// 验证验证码短信
        /// </summary>
        ValidVerificationCodeSMS = 10,

        /// <summary>
        /// 营收消息推送
        /// </summary>
        Revenue = 11,

        /// <summary>
        /// 协议推送
        /// </summary>
        Agreement = 12,

        /// <summary>
        /// 红包
        /// </summary>
        RedPacket = 13,

        /// <summary>
        /// 向员工付款
        /// </summary>
        Payment = 14,

        /// <summary>
        /// 工资条推送
        /// </summary>
        Salary = 15,
        /// <summary>
        /// 问卷推送
        /// </summary>
        Question = 16

    }
}
