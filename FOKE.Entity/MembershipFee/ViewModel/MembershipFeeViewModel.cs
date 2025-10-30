using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.MembershipFee.ViewModel
{
    public class MembershipFeeViewModel : BaseEntityViewModel
    {
        public long MemberID { get; set; }
        public long? Campaign { get; set; }
        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal PaidAmount { get; set; }
        public DateTime? PaidDate { get; set; }
        [Required(ErrorMessage = "Select payment type")]
        public long? PaymentType { get; set; }
        public long? PaymentReceivedBy { get; set; }
        public string? CollectionRemark { get; set; }
        public long? loggedinUserId { get; set; }

    }
}
