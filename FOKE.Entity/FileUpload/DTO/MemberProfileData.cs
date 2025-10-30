using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOKE.Entity.FileUpload.DTO
{
    public class MemberProfileData : BaseEntity
    {
        [Key]
        public int AttachmentId { get; set; }
        public long? MemberId { get; set; }
        public string? FileName { get; set; }
        public string? OrgFileName { get; set; }
        public long? FileStorageId { get; set; }

        [ForeignKey("FileStorageId")]
        public virtual FileStorage FileStorage { get; set; }
    }
}
