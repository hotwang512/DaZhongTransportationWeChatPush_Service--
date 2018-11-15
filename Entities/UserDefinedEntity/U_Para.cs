using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.UserDefinedEntity
{
    public class U_Para
    {
        public string mobile { get; set; }
        public int temp_id { get; set; }
        public U_Temp_Para temp_para { get; set; }
    }
}
