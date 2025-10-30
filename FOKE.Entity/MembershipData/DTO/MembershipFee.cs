using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOKE.Entity.MembershipData.DTO
{
    [Index(nameof(MemberID), nameof(Campaign), IsUnique = true)]
    public class MembershipFee : BaseEntity
    {
        [Key]
        public long MembershipFeeId { get; set; }
        public long? MemberID { get; set; }
        public long? Campaign { get; set; }
        [Column(TypeName = "decimal(18,3)")]
        public decimal? AmountToPay { get; set; }
        [Column(TypeName = "decimal(18,3)")]
        public decimal? PaidAmount { get; set; }
        public DateTime? PaidDate { get; set; }
        public long? PaymentType { get; set; }
        public long? PaymentReceivedBy { get; set; }
        public string? CollectionRemark { get; set; }


    }
}
