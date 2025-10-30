using FOKE.Entity;
using FOKE.Entity.AreaMaster.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.Area
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly IAreaRepository _areaRepository;

        private readonly ISharedLocalizer _sharedLocalizer;
        public IPagedList<AreaDataViewModel> pagedListData { get; private set; }
        [BindProperty]
        public long? Areaid { get; set; }
        #region FilterDeclaration

        [BindProperty]
        public string? AreaName { get; set; }

        [BindProperty]
        public long? Statusid { get; set; }
        #endregion



        public IndexModel(IAreaRepository areaRepository, ISharedLocalizer sharedLocalizer)
        {
            _areaRepository = areaRepository;
            _sharedLocalizer = sharedLocalizer;

        }
        public void OnGet(string isGoBack)
        {
            setPagedListColumns();
            //  BindDropdowns();
            if (isGoBack?.ToLower() != "y")
            {

                TempData["PRO_FILTER_AREA"] = null;

                TempData["PRO_FILTER_STATUS"] = null;
            }
        }
        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();
            //  objList.Add(new PageListFilterColumns { ColumName = "All", ColumnDescription = _sharedLocalizer.Localize("DD_ALL_TABLE_SEARCH").Value });
            objList.Add(new PageListFilterColumns { ColumName = "AreaName", ColumnDescription = _sharedLocalizer.Localize("Area Name").Value });


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
            var Area = GenericUtilities.Convert<string>(TempData.Peek("PRO_FILTER_AREA"));
            var Status = TempData.Peek("PRO_FILTER_STATUS");
            Statusid = GenericUtilities.Convert<long?>(Status);
            if (Statusid == null)
            {
                Statusid = 1;
            }
            var objResponse = _areaRepository.GetAllAreas(Statusid, Area);

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
        private void BindDropdowns()
        {
            //WorkPlaceList = _dropDownRepository.GetWorkPlace();
            //ProffessionList = _dropDownRepository.GetProffession();
            //AreaList = _dropDownRepository.GetArea();
        }
        public JsonResult OnPostApplyFilter()
        {

            TempData["PRO_FILTER_AREA"] = AreaName;
            TempData["PRO_FILTER_STATUS"] = Statusid.ToString();

            return new JsonResult(true);
        }
        public JsonResult OnPostDeleteArea(int? keyid)
        {
            var retData = new ResponseEntity<bool>();
            var objModel = new AreaDataViewModel();
            objModel.AreaId = Convert.ToInt32(keyid);
            retData = _areaRepository.DeleteArea(objModel);
            return new JsonResult(retData);
        }

    }
}
