using FOKE.Entity.MembershipData.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOKE.Entity.MembershipData.ViewModel
{
    public class PaymentQueryResult
    {
        public MembershipAccepted m { get; set; }
        public string AreaName { get; set; }
        public string UnitName { get; set; }
        public string ZoneName { get; set; }
        public long? PaymentReceivedBy { get; set; }
        public string PaymentReceivedByUser { get; set; }
        public string CreatedByUser { get; set; }
        public string CampaignName { get; set; }
        public DateTime? PaidDate { get; set; }
        public decimal? AmountReceived { get; set; }
    }
}
