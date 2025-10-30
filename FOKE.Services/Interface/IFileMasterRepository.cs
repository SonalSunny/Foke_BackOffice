using FOKE.Entity;
using FOKE.Entity.FileUpload.ViewModel;
using FOKE.Entity.OperationManagement.ViewModel;

namespace FOKE.Services.Interface
{
    public interface IFileMasterRepository
    {
        Task<ResponseEntity<FileViewModel>> CreateFile(FileViewModel model);
        ResponseEntity<FileViewModel> GetFileById(long FileId);
        ResponseEntity<List<FileViewModel>> GetAllFiles(long? Status);
        ResponseEntity<string> ExporttoExcel(string search, long? Statusid);
        ResponseEntity<bool> DeleteFile(FileViewModel objModel);
        FileStorageViewModel GetAttachment(string encryptid);
        Task<ResponseEntity<bool>> DeleteAttachment(long attachmentId);
        Task<ResponseEntity<FileViewModel>> UpdateFile(FileViewModel model);
        ResponseEntity<List<FileViewModel>> GetLibraryFiles(long? id, string? SearchText);
        ResponseEntity<List<FileViewModel>> Getfilemanagerfiles(string? SearchText, string pageCode);
        // Task<ResponseEntity<FileStorage>> SaveAttachment(List<IFormFile> fileInputs, long? CreatedBy);
    }
}
