using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOKE.Entity.Notification.DTO
{
    public class NotificationLog : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public long? NotificationId { get; set; }
        [ForeignKey("NotificationId")]
        public virtual Notification Notification { get; set; }
        public string FcmToken { get; set; }
        public bool FirebaseSuccess { get; set; }
        public string? FirebaseError { get; set; }
        public string? MemberCivilId { get; set; }
        public bool IsRead { get; set; }
        public long? DeviceId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}
