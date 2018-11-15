using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.UserDefinedEntity
{
    /// <summary>
    /// 极光短信通知模板
    /// </summary>
    public class U_Notification_Teamp_Para : U_Temp_Para
    {
        /// <summary>
        /// Notification 参数
        /// </summary>
        public string Notification { get; set; }
    }
}
