using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.OperationManagement.ViewModel
{
    public class FolderViewModel : BaseEntityViewModel
    {
        public long? FolderId { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string? FolderName { get; set; }
        public string? Description { get; set; }
        public DateTime? UpdatedOrCreatedDate { get; set; }
        public string? FirstLetter { get; set; }
        public long? FileId { get; set; }
        public List<IFormFile>? Attachment { get; set; }
        public long? AttachmentTypeId { get; set; }
        public long? FileStorageId { get; set; }
        public string? UploadFileName { get; set; }
        public bool AttachmentAny { get; set; }
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public long? loggedinUserId { get; set; }

    }
}
