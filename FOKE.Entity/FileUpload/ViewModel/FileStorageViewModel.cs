namespace FOKE.Entity.FileUpload.ViewModel
{
    public class FileStorageViewModel : BaseEntityViewModel
    {
        public long FileStorageId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string FileExtension { get; set; }
        public long? ContentLength { get; set; }
        public string StorageMode { get; set; }
        public byte[] FileData { get; set; }
        public string FilePath { get; set; }
        public string? RetFileData { get; set; }
        public string? Message { get; set; }
        public long? FolderId { get; set; }
    }
}
