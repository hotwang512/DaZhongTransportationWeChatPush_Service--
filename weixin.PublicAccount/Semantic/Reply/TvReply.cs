﻿using System;
using Newtonsoft.Json.Linq;

namespace weixin.PublicAccount.Semantic
{
    /// <summary>
    /// 电视节目预告语义应答
    /// </summary>
    public class TvReply : BaseReply
    {
        /// <summary>
        /// 电视节目预告语义
        /// </summary>
        public TvSemantic semantic { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            semantic = Utility.Parse<TvSemantic>((JObject)jo["semantic"]);
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n语义应答：{1}",
                base.ToString(), semantic);
        }
    }
}