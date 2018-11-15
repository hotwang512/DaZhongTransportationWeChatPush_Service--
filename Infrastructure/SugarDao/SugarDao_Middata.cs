using SqlSugar;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SugarDao
{
    public class SugarDao_Middata
    {
        public static SqlSugarClient GetInstance()
        {
            string connection = ConfigSugar.GetAppString("MidDataDataBaseConn");
            return new SqlSugarClient(connection);
        }
    }
}
