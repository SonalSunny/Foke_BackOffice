using FOKE.Entity;
using FOKE.Entity.Common;
using FOKE.Entity.OperationManagement.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.FileMaster
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly IFileMasterRepository _fileMasterRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        private readonly IDropDownRepository _dropDownRepository;
        public List<DropDownViewModel> FolderList { get; set; }
        public IPagedList<FileViewModel> pagedListData { get; private set; }
        #region FilterDeclaration
        [BindProperty]
        public long? Statusid { get; set; }
        [BindProperty]
        public long? FolderId { get; set; }


        #endregion
        public IndexModel(ISharedLocalizer sharedLocalizer, IFileMasterRepository fileMasterRepository, IDropDownRepository dropDownRepository)
        {

            _sharedLocalizer = sharedLocalizer;
            _fileMasterRepository = fileMasterRepository;
            _dropDownRepository = dropDownRepository;
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
            sortColumn = "FileName";
            BindDropdowns();
        }
        public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc,
            string nm, string showProject)
        {
            setPagedListColumns();
            pageNo = pn;
            pageSize = ps;
            sortOrder = so;
            sortColumn = sc;
            globalSearch = gs;
            searchField = gsc;
            var Status = TempData.Peek("PRO_FILTER_STATUS");
            var Folder = TempData.Peek("PRO_FILTER_FOLDER");

            Statusid = GenericUtilities.Convert<long?>(Status);
            FolderId = GenericUtilities.Convert<long?>(Folder);
            if (Statusid == null)
            {
                Statusid = 1;
            }
            if (FolderId == null)
            {
                FolderId = 0;
            }
            var objResponce = _fileMasterRepository.GetAllFiles(Statusid);
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
            objList.Add(new PageListFilterColumns { ColumName = "FolderName", ColumnDescription = _sharedLocalizer.Localize("FOLDER_NAME").Value });
            objList.Add(new PageListFilterColumns { ColumName = "FileRefNo", ColumnDescription = _sharedLocalizer.Localize("FILE_REF_NUMBER").Value });
            objList.Add(new PageListFilterColumns { ColumName = "FileName", ColumnDescription = _sharedLocalizer.Localize("FILE_NAME").Value });
            pageListFilterColumns = objList;
        }
        public JsonResult OnPostDeleteFile(int? keyid, int? Id)
        {
            var retData = new ResponseEntity<bool>();
            var objModel = new FileViewModel();
            objModel.FileId = Convert.ToInt32(keyid);
            objModel.DiffId = Convert.ToInt32(Id);
            retData = _fileMasterRepository.DeleteFile(objModel);
            return new JsonResult(retData);
        }
        private void BindDropdowns()
        {

            FolderList = _dropDownRepository.GetFolder();
        }
        public JsonResult OnPostApplyFilter()
        {

            // Store filter values in TempData
            TempData["PRO_FILTER_STATUS"] = Statusid.ToString();
            TempData["PRO_FILTER_FOLDER"] = FolderId.ToString();
            return new JsonResult(true);
        }
        public IActionResult OnPostExportData()
        {

            var status = TempData.Peek("PRO_FILTER_STATUS");
            var folder = TempData.Peek("PRO_FILTER_FOLDER");
            Statusid = GenericUtilities.Convert<long?>(status);
            FolderId = GenericUtilities.Convert<long?>(folder);
            var empData = _fileMasterRepository.ExporttoExcel("", Statusid);
            var tempFileName = empData.returnData;
            return new JsonResult(new { tFileName = tempFileName, fileName = "FileMaster.xlsx" });
        }
        public IActionResult OnGetAttachmentByIds(string encryptedID)
        {
            var document = _fileMasterRepository.GetAttachment(encryptedID);
            return new JsonResult(document);
        }
    }
}
