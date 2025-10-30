using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.OperationManagement.ViewModel
{
    public class FileViewModel : BaseEntityViewModel
    {
        public long? FileId { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string? FileName { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public long? FolderId { get; set; }
        public string? FolderName { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "REQUIRED")]
        public string? FileRefNo { get; set; }
        public string? FileLink { get; set; }
        public List<IFormFile>? Attachment { get; set; }
        public long? AttachmentTypeId { get; set; }
        public long? FileStorageId { get; set; }
        public string? UploadFileName { get; set; }
        public bool AttachmentAny { get; set; }
        public long? ContentLength { get; set; }
        public string? ContentType { get; set; }
        public DateTime? UpdatedOrCreatedDate { get; set; }
        public string? FileExtension { get; set; }
        public string? UpdatedOrCreatedDateformatted => UpdatedOrCreatedDate?.ToString("d-MMM-yyyy");
        public string? FilePath { get; set; }
        public long? FileCount { get; set; }
        public long? loggedinUserId { get; set; }
    }
}
