﻿using System;
using Newtonsoft.Json.Linq;

namespace weixin.PublicAccount
{
    /// <summary>
    /// 解析统计数据
    /// </summary>
    public interface IParsable
    {
        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        void Parse(JObject jo);
    }
}
