using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.UserDefinedEntity
{
    /// <summary>
    /// 短信验证码模板参数
    /// </summary>
    public class U_VerificationCode_Temp_Para : U_Temp_Para
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string code { get; set; }
    }
}
