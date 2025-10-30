using System.ComponentModel.DataAnnotations;

namespace FOKE.Entity.FileUpload.DTO
{
    public class FileStorage : BaseEntity
    {
        [Key]
        public long FileStorageId { get; set; }
        public long? ContentLength { get; set; }
        public string ContentType { get; set; }
        public string OrgFileName { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string StorageMode { get; set; }
        public string FilePath { get; set; }
    }
}
