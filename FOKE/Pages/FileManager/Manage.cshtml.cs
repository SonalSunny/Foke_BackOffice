//using FOKE.Entity.OperationManagement.ViewModel;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FOKE.Pages.FileManager
{
    public class ManageModel : PagedListBasePageModel
    {
        //public readonly IFileMasterRepository _fileMasterRepository;
        public readonly IDropDownRepository _dropdownRepository;
        //public IPagedList<FileViewModel> pagedListData { get; private set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 9;
        public int TotalPages { get; set; }
        public string? FolderName { get; set; }
        [BindProperty]
        public string? SearchText { get; set; }
        [BindProperty]
        public long? Folderid { get; set; }
        [BindProperty]
        public string? pagecode { get; set; }
        public ManageModel(IDropDownRepository dropdownrepository)
        {
            _dropdownRepository = dropdownrepository;
        }
        public void OnGet(string pageCode, string? searchText, long? id, string? Foldername, int pageIndex = 1)
        {
            pagecode = pageCode;
            SearchText = searchText;
            Folderid = id;
            FolderName = Foldername;
            PageIndex = pageIndex;
            //var objResponce = _fileMasterRepository.GetLibraryFiles(Folderid, SearchText);
            //if (objResponce.transactionStatus == System.Net.HttpStatusCode.OK)
            //{
            //    var allFiles = objResponce.returnData.ToList(); // Convert to List

            //    int totalItems = allFiles.Count;
            //    TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            //    // Ensure PageIndex is at least 1
            //    if (PageIndex < 1) PageIndex = 1;

            //    // Apply pagination using ToPagedList()
            //    pagedListData = allFiles.ToPagedList(PageIndex, PageSize);
            //}
        }
        //public IActionResult OnGetAttachmentByIds(string encryptedID)
        //{
        //    var document = _fileMasterRepository.GetAttachment(encryptedID);
        //    return new JsonResult(document);
        //}
    }
}
