using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities.UserDefinedEntity
{
    public class U_UploadResult : U_WeChatResult
    {
        /// <summary>
        /// 素材ID
        /// </summary>
        public string media_id { get; set; }

        public string type { get; set; }

        public string created_at { get; set; }
    }
}
