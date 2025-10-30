using FOKE.Entity.FileUpload.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FOKE.Entity.OperationManagement.DTO
{
    public class FolderMaster : BaseEntity
    {
        [Key]
        public long? FolderId { get; set; }
        public string? FolderName { get; set; }
        public string? Description { get; set; }
        public string? UploadFileName { get; set; }
        public long? FileStorageId { get; set; }
        [ForeignKey("FileStorageId")]
        public virtual FileStorage? FileStorage { get; set; }

    }

}
