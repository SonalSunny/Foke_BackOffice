using DocumentFormat.OpenXml.Bibliography;
using FOKE.Entity;
using FOKE.Entity.Common;
using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.PaymentReports.SummaryReport
{
    public class IndexModel : PagedListBasePageModel
    {
        private readonly IDropDownRepository _dropDownRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        private readonly IReportRepository _reportRepository;
        [BindProperty]
        public long? UserId { get; set; }
        public string? Year { get; set; }

        public List<DropDownViewModel> UserList { get; set; }
        public List<DropDownViewModel> YearList { get; set; }

        public IPagedList<PostMembershipViewModel> pagedListData { get; private set; }
        public IndexModel(ISharedLocalizer sharedLocalizer, IDropDownRepository dropDownRepository, IReportRepository reportRepository)
        {
            _sharedLocalizer = sharedLocalizer;
            _dropDownRepository = dropDownRepository;
            _reportRepository = reportRepository;
        }

        public void OnGet()
        {
            BindDropdowns();
        }

        private void BindDropdowns()
        {
            var currentYear = DateTime.Now.Year.ToString();
            UserList = _dropDownRepository.GetUsers();
            YearList = _dropDownRepository.GetallCampaignYears();
        }

        public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc, string nm, long? searchfield, string? Year)
        {
            BindDropdowns();
            pageNo = pn ?? 1;  // Default to page 1
            pageSize = ps ?? 10;  // Default page size
            sortOrder = so;
            sortColumn = sc;
            globalSearch = gs;
            searchField = gsc;
            if (!searchfield.HasValue)
            {
                var areaTemp = TempData.Peek("PRO_FILTER_AREA");
                searchfield = GenericUtilities.Convert<long?>(areaTemp);
            }
            var objResponse = _reportRepository.GetPaymentSummaryReport(searchfield,Year);

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
    }
}
