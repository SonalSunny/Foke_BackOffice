using FOKE.Entity;
using FOKE.Entity.UnitData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.Unit
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly IUnitRepository _unitRepository;

        private readonly ISharedLocalizer _sharedLocalizer;
        public IPagedList<UnitViewModel> pagedListData { get; private set; }
        [BindProperty]
        public long? unitId { get; set; }
        [BindProperty]
        public string? UnitName { get; set; }
        [BindProperty]
        public long? Statusid { get; set; }

        public IndexModel(IUnitRepository unitRepository, ISharedLocalizer sharedLocalizer)
        {
            _unitRepository = unitRepository;
            _sharedLocalizer = sharedLocalizer;

        }
        public void OnGet(string isGoBack)
        {
            setPagedListColumns();

            if (isGoBack?.ToLower() != "y")
            {

                TempData["PRO_FILTER_Unit"] = null;

                TempData["PRO_FILTER_STATUS"] = null;
            }
        }
        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();
            //  objList.Add(new PageListFilterColumns { ColumName = "All", ColumnDescription = _sharedLocalizer.Localize("DD_ALL_TABLE_SEARCH").Value });
            objList.Add(new PageListFilterColumns { ColumName = "UnitName", ColumnDescription = _sharedLocalizer.Localize("Unit Name").Value });
            //objList.Add(new PageListFilterColumns { ColumName = "CivilId", ColumnDescription = _sharedLocalizer.Localize("CivilID").Value });
            //objList.Add(new PageListFilterColumns { ColumName = "PassportNo", ColumnDescription = _sharedLocalizer.Localize("PassportNumber").Value });
            //objList.Add(new PageListFilterColumns { ColumName = "ContactNo", ColumnDescription = _sharedLocalizer.Localize("ContactNumber").Value });


            pageListFilterColumns = objList;
        }
        public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc, string nm, string showProject)
        {

            setPagedListColumns();
            pageNo = pn ?? 1;  // Default to page 1
            pageSize = ps ?? 10;  // Default page size
            sortOrder = so;
            sortColumn = sc;
            globalSearch = gs;
            searchField = gsc;
            var Unit = GenericUtilities.Convert<string>(TempData.Peek("PRO_FILTER_Unit"));
            var Status = TempData.Peek("PRO_FILTER_STATUS");
            Statusid = GenericUtilities.Convert<long?>(Status);
            if (Statusid == null)
            {
                Statusid = 1;
            }
            var objResponse = _unitRepository.GetAllUnits(Statusid, Unit);

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

        public JsonResult OnPostApplyFilter()
        {

            TempData["PRO_FILTER_Unit"] = UnitName;
            TempData["PRO_FILTER_STATUS"] = Statusid.ToString();

            return new JsonResult(true);
        }
        public JsonResult OnPostDeleteUnit(int? keyid)
        {
            var retData = new ResponseEntity<bool>();
            var objModel = new UnitViewModel();
            objModel.UnitId = Convert.ToInt32(keyid);
            retData = _unitRepository.DeleteUnit(objModel);
            return new JsonResult(retData);
        }



    }
}
