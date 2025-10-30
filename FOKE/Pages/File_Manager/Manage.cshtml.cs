using FOKE.Entity.OperationManagement.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;
using X.PagedList.Extensions;

namespace FOKE.Pages.File_Manager
{
    public class ManageModel : PageModel
    {
        public readonly IFileMasterRepository _fileMasterRepository;
        public IPagedList<FileViewModel> pagedListData { get; private set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 9;
        public int TotalPages { get; set; }
        [BindProperty]
        public string? SearchText { get; set; }
        [BindProperty]
        public string? pagecode { get; set; }
        [BindProperty]
        public long? Folderid { get; set; }
        [BindProperty]
        public string? FolderName { get; set; }
        public ManageModel(IFileMasterRepository fileMasterRepository)
        {
            _fileMasterRepository = fileMasterRepository;
        }
        public void OnGet(string pageCode, string? searchText, long? id, string? Foldername, int pageIndex = 1)
        {
            pagecode = pageCode;
            SearchText = searchText;
            PageIndex = pageIndex;
            Folderid = id;
            FolderName = Foldername;
            var objResponce = _fileMasterRepository.GetLibraryFiles(Folderid, SearchText);
            if (objResponce.transactionStatus == System.Net.HttpStatusCode.OK)
            {
                var allFolders = objResponce.returnData.ToList(); // Convert to List

                int totalItems = allFolders.Count;
                TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

                // Ensure PageIndex is at least 1
                if (PageIndex < 1) PageIndex = 1;

                // Apply pagination using ToPagedList()
                pagedListData = allFolders.ToPagedList(PageIndex, PageSize);
            }
        }
        public IActionResult OnGetAttachmentByIds(string encryptedID)
        {
            var document = _fileMasterRepository.GetAttachment(encryptedID);
            return new JsonResult(document);
        }
    }
}
