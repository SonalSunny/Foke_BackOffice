using FOKE.Entity;
using FOKE.Entity.OperationManagement.ViewModel;

namespace FOKE.Services.Interface
{
    public interface IFolderMasterRepository
    {
        Task<ResponseEntity<FolderViewModel>> CreateFolder(FolderViewModel model);
        Task<ResponseEntity<FolderViewModel>> UpdateFolder(FolderViewModel model);
        ResponseEntity<FolderViewModel> GetFolderById(long FolderId);
        ResponseEntity<List<FolderViewModel>> GetAllFolders(long? Statusid, DateTime? FromDate, DateTime? Todate);
        ResponseEntity<string> ExporttoExcel(string search, long? Statusid, DateTime? FromDate, DateTime? Todate);
        ResponseEntity<bool> DeleteFolder(FolderViewModel objModel);
        ResponseEntity<List<FolderViewModel>> GetLibraryFolders(string? SearchText, string pageCode);
        ResponseEntity<List<FolderViewModel>> GetActivityFolders(string? SearchText, string? pageCode);
        ResponseEntity<List<FolderViewModel>> GetKnowlegebaseFolders(string? SearchText, string? pageCode);
    }
}
