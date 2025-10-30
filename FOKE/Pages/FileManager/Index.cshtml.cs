//using FOKE.Entity.OperationManagement.ViewModel;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FOKE.Pages.FileManager
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly IDropDownRepository _dropdownRepository;
        //public IPagedList<FolderViewModel> pagedListData { get; private set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 9;
        public int TotalPages { get; set; }
        [BindProperty]
        public string? SearchText { get; set; }
        [BindProperty]
        public string? pagecode { get; set; }
        public IndexModel(IDropDownRepository dropdownRepository)
        {
            _dropdownRepository = dropdownRepository;
        }
        public void OnGet(string pageCode, string? searchText, int pageIndex = 1)
        {
            pagecode = pageCode;
            SearchText = searchText;
            PageIndex = pageIndex;
            //var objResponce = _folderMasterRepository.GetLibraryFolders(SearchText, pagecode);
            //if (objResponce.transactionStatus == System.Net.HttpStatusCode.OK)
            //{
            //    var allFolders = objResponce.returnData.ToList(); // Convert to List

            //    int totalItems = allFolders.Count;
            //    TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            //    // Ensure PageIndex is at least 1
            //    if (PageIndex < 1) PageIndex = 1;

            //    // Apply pagination using ToPagedList()
            //    pagedListData = allFolders.ToPagedList(PageIndex, PageSize);
            //}
        }
    }
}
