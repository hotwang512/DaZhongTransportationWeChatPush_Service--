using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.UserDefinedEntity
{
    public class U_Revenue_Temp_Para : U_Temp_Para
    {
        /// <summary>
        /// 月份
        /// </summary>
        public string Month { get; set; }
        /// <summary>
        /// 历史欠款
        /// </summary>
        public string HistoricalArrears { get; set; }
        /// <summary>
        /// 上期结余
        /// </summary>
        public string TheBalance { get; set; }
        /// <summary>
        /// 应缴金额
        /// </summary>
        public string AmountDue { get; set; }
        /// <summary>
        /// 本期缴款
        /// </summary>
        public string CurrentPayment { get; set; }
        /// <summary>
        /// 本期账户结余
        /// </summary>
        public string CurrentAccountBalance { get; set; }
    }
}
