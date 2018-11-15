using System.Collections.Generic;

namespace Entities.UserDefinedEntity
{
    public class U_User
    {
        public string errcode { get; set; }

        public string errmsg { get; set; }

        public List<UserList> userlist { get; set; }

    }

    public class UserList
    {

        public string userid { get; set; }

        public string name { get; set; }

    //    public string department { get; set; }

      //  public string order { get; set; }

        public string position { get; set; }

        public string mobile { get; set; }

        public string gender { get; set; }

        public string email { get; set; }

        public string isleader { get; set; }

        public string avatar { get; set; }

        public string telephone { get; set; }

        public string english_name { get; set; }

        public string status { get; set; }

    }
}