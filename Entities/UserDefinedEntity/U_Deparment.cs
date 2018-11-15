using System.Collections.Generic;

namespace Entities.UserDefinedEntity
{
    public class U_Deparment
    {
        public string errcode { get; set; }

        public string errmsg { get; set; }

        public List<Deparment> department { get; set; }

    }



    public class Deparment
    {
        public int id { get; set; }

        public string name { get; set; }

        public string parentid { get; set; }

        public string order { get; set; }
    }
}