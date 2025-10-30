using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.ContactUs.DTO
{
    public class ClientEnquieryData : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ContactNo { get; set; }
        public string? DeviceId { get; set; }
        public string? CivilId { get; set; }
        public long? DevicePrimaryId { get; set; }
        public long? Type { get; set; }
        public string? Comment { get; set; }
        public bool IsNotificationSent { get; set; }
        public bool IsResolved { get; set; }
        public long? ResolvedBy { get; set; }
        public DateTime? ResolvedDate { get; set; }

    }
}
