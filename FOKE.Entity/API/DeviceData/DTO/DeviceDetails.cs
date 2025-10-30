using FOKE.Entity.FileUpload.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOKE.Entity.API.DeviceData.DTO
{
    public class DeviceDetails : BaseEntity
    {
        [Key]
        public long? DeviceDetailId { get; set; }
        public string? DeviceId { get; set; }
        public string? CivilId { get; set; }
        public string? FCMToken { get; set; }
        public string? DeviceName { get; set; }
        public string? DeviceModel { get; set; }
        public string? DeviceType { get; set; }
        public string? OrgFileName { get; set; }
        public string? FilePath { get; set; }
        public long? FileStorageId { get; set; }
        [ForeignKey("FileStorageId")]
        public virtual FileStorage FileStorage { get; set; }
        public DateTime? LastOpenDateTime { get; set; }
        public DateTime? LastClosedDateTime { get; set; }
        public DateTime? LogOutDateTime { get; set; }
        public bool IsForceLogout { get; set; }
        public bool IsUninstalled { get; set; }

    }
}
