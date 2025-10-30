namespace FOKE.Entity.FileUpload.ViewModel
{
    public class AttachmentViewModel : BaseEntityViewModel
    {
        public int AttachmentId { get; set; }
        public string AttachmentModule { get; set; }
        public long? AttachmentMasterId { get; set; }
        public string ActualFileName { get; set; }
        public string FileName { get; set; }
        public long? ContentLength { get; set; }
        public string FileExtension { get; set; }
        public long FileStorageId { get; set; }
        public string ContentType { get; set; }

        public byte[] FileData { get; set; }

        public string? RetFileData { get; set; }
        public string? Message { get; set; }
        public string FilePath { get; set; }
    }
}
