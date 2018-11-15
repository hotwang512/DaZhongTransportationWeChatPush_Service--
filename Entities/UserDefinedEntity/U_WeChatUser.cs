using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities.UserDefinedEntity
{
    public class U_WeChatUser
    {
        public string userid { get; set; }

        public string name { get; set; }

        public List<int> department { get; set; }

        public string mobile { get; set; }

        public string gender { get; set; }

        public string avatar { get; set; }

        public int status { get; set; }


    }
}
