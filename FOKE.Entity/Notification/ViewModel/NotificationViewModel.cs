using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.Notification.ViewModel
{
    public class NotificationViewModel
    {
        public long NotificationId { get; set; }
        public string? NotificationType { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public long? SendTo { get; set; }
        public string? SendToNumbers { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string? Header { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string? NotificationContent { get; set; }
        public long? Area { get; set; }
        public long? Zone { get; set; }
        public long? Unit { get; set; }
        public string? Displaycontent { get; set; }
        public bool? Active { get; set; }
        public string? Name { get; set; }
        public long? ContactNo { get; set; }
        public bool FirebaseSuccess { get; set; }
        public DateTime? SendToTime { get; set; }
        public string? DeviceModel { get; set; }
        public string? RemovedNumbers { get; set; }

    }
}
