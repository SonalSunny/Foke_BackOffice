using FOKE.Entity;
using FOKE.Entity.OperationManagement.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.FolderMaster
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly IFolderMasterRepository _folderMasterRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        private readonly IDropDownRepository _dropDownRepository;
        public IPagedList<FolderViewModel> pagedListData { get; private set; }

        #region FilterDeclaration

        [BindProperty]
        public long? Statusid { get; set; }
        [BindProperty]
        public DateTime? FromDate { get; set; }
        [BindProperty]
        public DateTime? ToDate { get; set; }


        #endregion
        public IndexModel(IDropDownRepository dropDownRepository, ISharedLocalizer sharedLocalizer, IFolderMasterRepository folderMasterRepository)
        {
            _dropDownRepository = dropDownRepository;
            _sharedLocalizer = sharedLocalizer;
            _folderMasterRepository = folderMasterRepository;
        }
        public void OnGet(string isGoBack)
        {
            setPagedListColumns();
            if (isGoBack?.ToLower() != "y")
            {
                TempData["FILTER_DATE_FROM"] = "";
                TempData["FILTER_DATE_TO"] = "";
                TempData["PRO_FILTER_STATUS"] = null;
            }
            sortColumn = "FolderName";
        }
        public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc,
            string nm)
        {
            setPagedListColumns();
            pageNo = pn;
            pageSize = ps;
            sortOrder = so;
            sortColumn = sc;
            globalSearch = gs;
            searchField = gsc;
            var fromDate = TempData.Peek("FILTER_DATE_FROM");
            var toDate = TempData.Peek("FILTER_DATE_TO");
            var Status = TempData.Peek("PRO_FILTER_STATUS");
            FromDate = GenericUtilities.Convert<DateTime?>(fromDate);
            ToDate = GenericUtilities.Convert<DateTime?>(toDate);
            Statusid = GenericUtilities.Convert<long?>(Status);

            var objResponce = _folderMasterRepository.GetAllFolders(Statusid, FromDate, ToDate);
            if (objResponce.transactionStatus == System.Net.HttpStatusCode.OK)
            {

                pagedListData = PagedList(objResponce.returnData);
            }

            return new PartialViewResult
            {
                ViewName = "_IndexPartial",
                ViewData = ViewData
            };
        }
        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();
            objList.Add(new PageListFilterColumns { ColumName = "All", ColumnDescription = _sharedLocalizer.Localize("DD_ALL_TABLE_SEARCH").Value });
            objList.Add(new PageListFilterColumns { ColumName = "FolderName", ColumnDescription = _sharedLocalizer.Localize("Folder Name").Value });
            pageListFilterColumns = objList;
        }
        public JsonResult OnPostApplyFilter()
        {

            // Store filter values in TempData
            TempData["PRO_FILTER_STATUS"] = Statusid.ToString();
            TempData["FILTER_DATE_FROM"] = FromDate?.ToString("yyyy-MM-dd"); // Format the DateTime if it's not null
            TempData["FILTER_DATE_TO"] = ToDate?.ToString("yyyy-MM-dd"); // Format the DateTime if it's not null
            return new JsonResult(true);
        }
        public IActionResult OnPostExportData()
        {
            var fromDate = TempData.Peek("FILTER_DATE_FROM");
            var toDate = TempData.Peek("FILTER_DATE_TO");
            var status = TempData.Peek("PRO_FILTER_STATUS");
            FromDate = GenericUtilities.Convert<DateTime?>(fromDate);
            ToDate = GenericUtilities.Convert<DateTime?>(toDate);
            Statusid = GenericUtilities.Convert<long?>(status);

            var empData = _folderMasterRepository.ExporttoExcel("", Statusid, FromDate, ToDate);
            var tempFileName = empData.returnData;
            return new JsonResult(new { tFileName = tempFileName, fileName = "FolderMaster.xlsx" });
        }
        public JsonResult OnPostDeleteFolder(int? keyid, int? Id)
        {
            var retData = new ResponseEntity<bool>();
            var objModel = new FolderViewModel();
            objModel.FolderId = Convert.ToInt32(keyid);
            objModel.DiffId = Convert.ToInt32(Id);
            retData = _folderMasterRepository.DeleteFolder(objModel);
            return new JsonResult(retData);
        }
    }
}
