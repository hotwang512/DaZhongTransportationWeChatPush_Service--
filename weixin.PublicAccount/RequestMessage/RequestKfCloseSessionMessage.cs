﻿using System;
using System.Collections.Generic;

namespace weixin.PublicAccount
{
    /// <summary>
    /// 客服关闭会话事件消息
    /// </summary>
    public class RequestKfCloseSessionMessage : RequestEventMessage
    {
        /// <summary>
        /// 获取客服账号
        /// </summary>
        public string KfAccount { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="createTime"></param>
        /// <param name="kfAccount">客服账号</param>
        public RequestKfCloseSessionMessage(string toUserName, string fromUserName, DateTime createTime,
            string kfAccount)
            : base(toUserName, fromUserName, createTime, RequestEventTypeEnum.kf_close_session)
        {
            KfAccount = kfAccount;
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n客服账号：{1}",
                base.ToString(), KfAccount);
        }
    }
}