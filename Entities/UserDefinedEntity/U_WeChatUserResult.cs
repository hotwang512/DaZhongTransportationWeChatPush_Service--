using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities.UserDefinedEntity
{
    public class U_WeChatUserResult : U_WeChatResult
    {
        public List<U_WeChatUser> userlist { get; set; }
    }
}
