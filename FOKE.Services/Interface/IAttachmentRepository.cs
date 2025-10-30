using FOKE.Entity;
using FOKE.Entity.FileUpload.DTO;
using FOKE.Entity.FileUpload.ViewModel;

namespace FOKE.Services.Interface
{
    public interface IAttachmentRepository
    {
        Task<ResponseEntity<FileStorage>> SaveAttachment(FileStorageViewModel objModel);
        ResponseEntity<bool> DeleteAttachment(long Id);
        ResponseEntity<List<AttachmentViewModel>> GetAttachmentById(string encryptedID);
    }
}
