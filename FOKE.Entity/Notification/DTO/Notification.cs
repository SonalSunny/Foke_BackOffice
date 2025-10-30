using FOKE.Entity.AreaMaster.DTO;
using FOKE.Entity.UnitData.DTO;
using FOKE.Entity.ZoneMaster.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOKE.Entity.Notification.DTO
{
    public class Notification : BaseEntity
    {
        [Key]
        public long NotificationId { get; set; }
        public string? NotificationType { get; set; }
        public long? SendTo { get; set; }
        public string? SendToNumbers { get; set; }
        public string? Header { get; set; }
        public string? Content { get; set; }
        public bool? Status { get; set; }
        public long? AreaId { get; set; }
        [ForeignKey("AreaId")]
        public virtual AreaData AreaData { get; set; }
        public long? UnitId { get; set; }
        [ForeignKey("UnitId")]
        public virtual Unit Unit { get; set; }
        public long? ZoneId { get; set; }
        [ForeignKey("ZoneId")]
        public virtual Zone Zone { get; set; }
        public bool LogGeneratedStatus { get; set; }
        public string? RemovedNumbers { get; set; }

    }
}
