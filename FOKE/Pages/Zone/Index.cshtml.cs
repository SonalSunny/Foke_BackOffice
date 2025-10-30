using FOKE.Entity;
using FOKE.Entity.ZoneMaster.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.Zone
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly IZoneRepository _zoneRepository;

        private readonly ISharedLocalizer _sharedLocalizer;
        public IPagedList<ZoneViewModel> pagedListData { get; private set; }
        [BindProperty]
        public long? Zoneid { get; set; }


        [BindProperty]
        public string? ZoneName { get; set; }

        [BindProperty]
        public long? Statusid { get; set; }

        public IndexModel(IZoneRepository zoneRepository, ISharedLocalizer sharedLocalizer)
        {
            _zoneRepository = zoneRepository;
            _sharedLocalizer = sharedLocalizer;

        }
        public void OnGet(string isGoBack)
        {
            setPagedListColumns();

            if (isGoBack?.ToLower() != "y")
            {


                TempData["PRO_FILTER_STATUS"] = null;
                TempData["PRO_FILTER_Zone"] = null;

            }
        }
        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();
            //  objList.Add(new PageListFilterColumns { ColumName = "All", ColumnDescription = _sharedLocalizer.Localize("DD_ALL_TABLE_SEARCH").Value });
            objList.Add(new PageListFilterColumns { ColumName = "ZoneName", ColumnDescription = _sharedLocalizer.Localize("Zone Name").Value });



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
            var Zone = GenericUtilities.Convert<string>(TempData.Peek("PRO_FILTER_Zone"));
            var Status = TempData.Peek("PRO_FILTER_STATUS");
            Statusid = GenericUtilities.Convert<long?>(Status);
            if (Statusid == null)
            {
                Statusid = 1;
            }
            var objResponse = _zoneRepository.GetAllZones(Statusid, Zone);

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

            TempData["PRO_FILTER_AREA"] = ZoneName;
            TempData["PRO_FILTER_STATUS"] = Statusid.ToString();

            return new JsonResult(true);
        }
        public JsonResult OnPostDeleteZone(int? keyid)
        {
            var retData = new ResponseEntity<bool>();
            var objModel = new ZoneViewModel();
            objModel.ZoneId = Convert.ToInt32(keyid);
            retData = _zoneRepository.DeleteZone(objModel);
            return new JsonResult(retData);
        }
    }
}
