using FOKE.Entity;
using FOKE.Entity.WorkPlaceData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.WorkPlace
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly IWorkPlaceRepository _workplaceRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        public IPagedList<WorkPlaceViewModel> pagedListData { get; private set; }
        [BindProperty]
        public string? WorkPlaceName { get; set; }
        [BindProperty]
        public string? Description { get; set; }
        [BindProperty]
        public long? Statusid { get; set; }
        public IndexModel(IWorkPlaceRepository workplaceRepository, ISharedLocalizer sharedLocalizer)
        {
            _workplaceRepository = workplaceRepository;
            _sharedLocalizer = sharedLocalizer;
        }

        public void OnGet(string isGoBack)
        {
            setPagedListColumns();
            if (isGoBack?.ToLower() != "y")
            {

                TempData["PRO_FILTER_STATUS"] = null;
                TempData["PRO_FILTER_WORKPLACE"] = null;
            }
        }

        public JsonResult OnPostApplyFilter()
        {

            TempData["PRO_FILTER_WORKPLACE"] = WorkPlaceName;
            TempData["PRO_FILTER_STATUS"] = Statusid.ToString();

            return new JsonResult(true);
        }
        public JsonResult OnPostDeleteWorkPlace(int? keyid)
        {
            var retData = new ResponseEntity<bool>();
            var objModel = new WorkPlaceViewModel();
            objModel.WorkPlaceId = Convert.ToInt32(keyid);
            retData = _workplaceRepository.DeleteWorkPlace(objModel);
            return new JsonResult(retData);
        }
        public IActionResult OnPostExportData()
        {
            var Status = TempData.Peek("PRO_FILTER_STATUS");
            var ProfessionName = TempData.Peek("PRO_FILTER_WORKPLACE");
            Statusid = GenericUtilities.Convert<long?>(Status);

            if (Statusid == null)
            {
                Statusid = 1;
            }

            var empData = _workplaceRepository.ExportWorkPlaceToExcel(Statusid, "");
            var tempFileName = empData.returnData;
            return new JsonResult(new { tFileName = tempFileName, fileName = "WorkplaceMaster.xlsx" });
        }

        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();
            objList.Add(new PageListFilterColumns { ColumName = "All", ColumnDescription = _sharedLocalizer.Localize("DD_ALL_TABLE_SEARCH").Value });
            objList.Add(new PageListFilterColumns { ColumName = "WorkPlace", ColumnDescription = _sharedLocalizer.Localize("WORKPLACE").Value });
            pageListFilterColumns = objList;
        }
        public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc, string nm, string showProject)
        {
            setPagedListColumns();
            pageNo = pn ?? 1;
            pageSize = ps ?? 10;
            sortOrder = so;
            sortColumn = sc;
            globalSearch = gs;
            searchField = gsc;
            var Status = TempData.Peek("PRO_FILTER_STATUS");
            Statusid = GenericUtilities.Convert<long?>(Status);
            var objList = _workplaceRepository.GetAllWorkPlace(Statusid, gs);

            if (objList != null && objList.transactionStatus == System.Net.HttpStatusCode.OK && objList.returnData != null)
            {
                pagedListData = PagedList(objList.returnData);
            }

            return new PartialViewResult
            {
                ViewName = "_IndexPartial",
                ViewData = ViewData
            };
        }
    }
}





