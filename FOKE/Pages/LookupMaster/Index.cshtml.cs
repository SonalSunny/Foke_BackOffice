using FOKE.Entity;
using FOKE.Entity.Common;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.LookupMaster
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly ILookupRepository _lookupRepository;
        private readonly IDropDownRepository _dropDownRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        public IPagedList<LookupViewModel> pagedListData { get; private set; }
        #region FilterDeclaration

        [BindProperty]
        public long? LookUpType { get; set; }
        [BindProperty]
        public string? LookUpName { get; set; }
        [BindProperty]
        public long? Statusid { get; set; }
        #endregion
        public List<DropDownViewModel> LookUpTypeList { get; set; }
        public IndexModel(ILookupRepository lookupRepository, IDropDownRepository dropDownRepository, ISharedLocalizer sharedLocalizer)
        {
            _lookupRepository = lookupRepository;
            _dropDownRepository = dropDownRepository;
            _sharedLocalizer = sharedLocalizer;
        }
        public void OnGet(string isGoBack)
        {
            setPagedListColumns();
            if (isGoBack?.ToLower() != "y")
            {
                TempData["FILTER_LOOKUPNAME"] = "";
                TempData["FILTER_LOOKUPTYPE_ID"] = null;
            }
            //sortColumn = "LookUpName";
            BindDropdowns();
        }
        public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc, string nm)
        {
            setPagedListColumns();
            BindDropdowns();
            pageNo = pn ?? 1;  // Default to page 1
            pageSize = ps ?? 10;  // Default page size
            sortOrder = so;
            sortColumn = sc;
            globalSearch = gs;
            searchField = gsc;

            var LookupName = GenericUtilities.Convert<string>(TempData.Peek("FILTER_LOOKUPNAME"));
            var Lookuptypeid = GenericUtilities.Convert<long>(TempData.Peek("FILTER_LOOKUPTYPE_ID"));
            var Status = TempData.Peek("PRO_FILTER_STATUS");
            Statusid = GenericUtilities.Convert<long?>(Status);
            if (Statusid == null)
            {
                Statusid = 1;
            }

            var objResponse = _lookupRepository.GetAllLookups(Statusid, Lookuptypeid, LookupName);

            if (objResponse != null && objResponse.transactionStatus == System.Net.HttpStatusCode.OK
                && objResponse.returnData != null)
            {
                pagedListData = PagedList(objResponse.returnData);
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
            objList.Add(new PageListFilterColumns { ColumName = "LookUpName", ColumnDescription = _sharedLocalizer.Localize("LOOKUP_NAME").Value });
            objList.Add(new PageListFilterColumns { ColumName = "LookUpTypeName", ColumnDescription = _sharedLocalizer.Localize("LOOKUP_TYPE_NAME").Value });
            pageListFilterColumns = objList;
        }
        private void BindDropdowns()
        {
            LookUpTypeList = _dropDownRepository.GetLookupTypeList();

        }
        public JsonResult OnPostDeleteTeam(int? keyid, int? Id)
        {
            var retData = new ResponseEntity<bool>();
            var objModel = new LookupViewModel();
            objModel.LookUpId = Convert.ToInt32(keyid);
            objModel.DiffId = Convert.ToInt32(Id);
            retData = _lookupRepository.DeleteTeam(objModel);
            return new JsonResult(retData);
        }
        public JsonResult OnPostApplyFilter()
        {
            // Store filter values in TempData
            TempData["FILTER_LOOKUPTYPE_ID"] = LookUpType.ToString();
            TempData["PRO_FILTER_STATUS"] = Statusid.ToString();
            return new JsonResult(true);
        }

        public IActionResult OnPostExportData()
        {
            var status = TempData.Peek("PRO_FILTER_STATUS");
            LookUpName = GenericUtilities.Convert<string>(TempData.Peek("FILTER_LOOKUPNAME"));
            LookUpType = GenericUtilities.Convert<long>(TempData.Peek("FILTER_LOOKUPTYPE_ID"));
            Statusid = GenericUtilities.Convert<long?>(status);
            var empData = _lookupRepository.ExportTeamDatatoExcel("", Statusid, LookUpType, LookUpName);
            var tempFileName = empData.returnData;
            return new JsonResult(new { tFileName = tempFileName, fileName = "LookUp.xlsx" });
        }
    }
}
