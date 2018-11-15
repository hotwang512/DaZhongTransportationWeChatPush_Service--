using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities.UserDefinedEntity
{
    public class U_WeChatResult
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int errcode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }

        /// <summary>
        /// 无效用户
        /// </summary>
        public string invaliduser { get; set; } = string.Empty;
    }
}
