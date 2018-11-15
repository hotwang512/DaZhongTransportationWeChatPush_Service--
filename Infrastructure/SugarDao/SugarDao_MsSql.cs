using SqlSugar;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.SugarDao
{
    public class SugarDao_MsSql
    {
        public static SqlSugarClient GetInstance()
        {
            string connection = ConfigSugar.GetAppString("DaZhongDataBaseConn");
            return new SqlSugarClient(connection);
        }
    }
}
