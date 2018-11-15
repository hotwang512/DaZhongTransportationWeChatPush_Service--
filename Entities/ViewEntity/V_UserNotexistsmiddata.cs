using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewEntity
{
    public class V_UserNotexistsmiddata
    {
        public string UserID { get; set; }

        public string Name { get; set; }

        public string ID { get; set; }

        public string Age { get; set; }

        public string Sex { get; set; }

        public string JobNumber { get; set; }

        public string ServiceNumber { get; set; }

        public string OwnedCompany { get; set; }

        public Guid OwnedFleet { get; set; }

        public string LicensePlate { get; set; }

        public string PhoneNumber { get; set; }

        public string WeChatNumber { get; set; }

        public int ApprovalStatus { get; set; }

        public int ApprovalType { get; set; }

        public string ApprovalUser { get; set; }

        public int DepartmenManager { get; set; }

        public int ReviewPush { get; set; }

        public string Role { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedUser { get; set; }

        public DateTime ChangeDate { get; set; }

        public string ChangeUser { get; set; }

        public Guid Vguid { get; set; }
    }
}
