using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public enum PushMode
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,

        /// <summary>
        /// 微信推送
        /// </summary>
        WeChat = 1,

        /// <summary>
        /// 短信推送
        /// </summary>
        ShortMsg = 2
    }
}
