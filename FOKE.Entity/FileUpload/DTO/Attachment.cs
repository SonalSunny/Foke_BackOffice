using FOKE.Entity.FileUpload.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOKE.Entity.Task_Manager.DTO
{
    public class AttachmentData : BaseEntity
    {
        [Key]
        public int AttachmentId { get; set; }
        public string AttachmentModule { get; set; }
        public long? AttachmentMasterId { get; set; }
        public string FileName { get; set; }
        public string OrgFileName { get; set; }
        public long? FileStorageId { get; set; }

        [ForeignKey("FileStorageId")]
        public virtual FileStorage FileStorage { get; set; }
    }
}
