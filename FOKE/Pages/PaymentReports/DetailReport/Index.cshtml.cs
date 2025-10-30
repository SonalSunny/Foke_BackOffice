using Azure;
using FOKE.Entity;
using FOKE.Entity.Common;
using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.PaymentReports.DetailReport
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly IReportRepository _reportRepository;
        private readonly IDropDownRepository _dropDownRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        public IPagedList<PostMembershipViewModel> pagedListData { get; private set; }
        #region FilterDeclaration

        [BindProperty]
        public long? Area { get; set; }
        [BindProperty]
        public long? Unit { get; set; }
        [BindProperty]
        public long? Zone { get; set; }
        [BindProperty]
        public long? UserId { get; set; }
        [BindProperty]
        public long? CampaignId { get; set; }
        [BindProperty]
        public DateTime? Fromdate { get; set; }
        [BindProperty]
        public DateTime? Todate { get; set; }
        #endregion
        public List<DropDownViewModel> AreaList { get; set; }
        public List<DropDownViewModel> UnitList { get; set; }
        public List<DropDownViewModel> zoneList { get; set; }

        public IndexModel(ISharedLocalizer sharedLocalizer, IDropDownRepository dropDownRepository, IReportRepository reportRepository)
        {
            _sharedLocalizer = sharedLocalizer;
            _dropDownRepository = dropDownRepository;
            _reportRepository = reportRepository;
        }

        public void OnGet(string isGoBack, int id, long? campaignId)
        {
            UserId = id;
            CampaignId = campaignId;
            setPagedListColumns();
            if (isGoBack?.ToLower() != "y")
            {
                TempData["PRO_FILTER_AREA"] = null;
                TempData["PRO_FILTER_UNIT"] = null;
                TempData["PRO_FILTER_ZONE"] = null;
                TempData["FILTER_DATE_FROM"] = "";
                TempData["FILTER_DATE_TO"] = "";
            }
            //sortColumn = "LookUpName";
            BindDropdowns();
        }

        public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc, string nm, long? id, long? campaignId)
        {
            setPagedListColumns();
            BindDropdowns();
            pageNo = pn ?? 1;  // Default to page 1
            pageSize = ps ?? 10;  // Default page size
            sortOrder = so;
            sortColumn = sc;
            globalSearch = gs;
            searchField = gsc;
            var SearchString = gs;
            var SearchColumn = gsc;

            var AreaId = GenericUtilities.Convert<string>(TempData.Peek("PRO_FILTER_AREA"));
            var UnitId = GenericUtilities.Convert<long>(TempData.Peek("PRO_FILTER_UNIT"));
            var ZoneId = GenericUtilities.Convert<long>(TempData.Peek("PRO_FILTER_ZONE"));
            var FromDate = TempData.Peek("FILTER_DATE_FROM");
            var ToDate = TempData.Peek("FILTER_DATE_TO");
            Area = GenericUtilities.Convert<long?>(AreaId);
            Unit = GenericUtilities.Convert<long?>(UnitId);
            Zone = GenericUtilities.Convert<long?>(ZoneId);
            Fromdate = GenericUtilities.Convert<DateTime?>(FromDate);
            Todate = GenericUtilities.Convert<DateTime?>(ToDate);
            var objResponse = _reportRepository.GetPaymentDetailReport(Area, Unit, Zone, id, Fromdate, Todate,campaignId, pn, ps, SearchString, SearchColumn);

            if (objResponse != null && objResponse.transactionStatus == System.Net.HttpStatusCode.OK && objResponse.returnData != null)
            {
                pagedListData = PagedList(objResponse.returnData, objResponse.TotalCount);
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
            objList.Add(new PageListFilterColumns { ColumName = "ReferenceNumber", ColumnDescription = _sharedLocalizer.Localize("Member Id").Value });
            objList.Add(new PageListFilterColumns { ColumName = "Name", ColumnDescription = _sharedLocalizer.Localize("Name").Value });
            objList.Add(new PageListFilterColumns { ColumName = "CivilId", ColumnDescription = _sharedLocalizer.Localize("CivilId").Value });
            objList.Add(new PageListFilterColumns { ColumName = "PaymentRecievedBy", ColumnDescription = _sharedLocalizer.Localize("Collected By").Value });

            pageListFilterColumns = objList;
        }

        private void BindDropdowns()
        {
            AreaList = _dropDownRepository.GetArea();
            zoneList = _dropDownRepository.GetZone();
            UnitList = _dropDownRepository.GetUnit();
        }

        public JsonResult OnPostApplyFilter()
        {
            // ? Store the selected filters in TempData
            TempData["PRO_FILTER_AREA"] = Area?.ToString();
            TempData["PRO_FILTER_UNIT"] = Unit?.ToString();
            TempData["PRO_FILTER_ZONE"] = Zone?.ToString();
            TempData["FILTER_DATE_FROM"] = Fromdate?.ToString("yyyy-MM-dd");
            TempData["FILTER_DATE_TO"] = Todate?.ToString("yyyy-MM-dd");
            return new JsonResult(true);
        }
    }
}
