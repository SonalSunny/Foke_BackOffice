using FOKE.Entity.FileUpload.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOKE.Entity.OperationManagement.DTO
{
    public class FileMaster : BaseEntity
    {
        [Key]
        public long? FileId { get; set; }
        public string? FileName { get; set; }
        public long? FolderId { get; set; }
        [ForeignKey("FolderId")]
        public virtual FolderMaster FolderMaster { get; set; }
        public string? Description { get; set; }
        public string? FileRefNo { get; set; }
        public string? FileLink { get; set; }
        public string? UploadFileName { get; set; }
        public long? FileStorageId { get; set; }
        [ForeignKey("FileStorageId")]
        public virtual FileStorage FileStorage { get; set; }
    }
}
