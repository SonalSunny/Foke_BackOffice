using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.CancelReasonMaster.DTO
{
    public class CancelReasonData : BaseEntity
    {
        [Key]
        public long ReasonId { get; set; }
        public string? CancelReason { get; set; }
    }
}
