using FOKE.Entity;
using FOKE.Entity.Common;
using FOKE.Entity.MembershipData.ViewModel;
using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Entity.WorkPlaceData.DTO;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using FOKE.Services.Repository;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.PaymentReports.Membership_Report
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly IMembershipFormRepository _membershipFormRepository;
        private readonly IDropDownRepository _dropDownRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        public IPagedList<PostMembershipViewModel> pagedListData { get; private set; }
        #region FilterDeclaration

        [BindProperty]
        public long? Campaign { get; set; }
        [BindProperty]
        public long? UserId { get; set; }
        #endregion
        public List<DropDownViewModel> CampignList { get; set; }

        public IndexModel(ISharedLocalizer sharedLocalizer, IDropDownRepository dropDownRepository, IMembershipFormRepository membershipFormRepository)
        {
            _sharedLocalizer = sharedLocalizer;
            _dropDownRepository = dropDownRepository;
            _membershipFormRepository = membershipFormRepository;
        }
        public void OnGet(string isGoBack, int id)
        {
            UserId = id;
            setPagedListColumns();
            if (isGoBack?.ToLower() != "y")
            {
                TempData["PRO_FILTER_CAMPAIGN"] = null;
            }
            BindDropdowns();
        }
        public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc, string nm, long? id)
        {
            setPagedListColumns();
            BindDropdowns();
            pageNo = pn ?? 1;  // Default to page 1
            pageSize = ps ?? 10;  // Default page size
            sortOrder = so;
            sortColumn = sc;
            globalSearch = gs;
            searchField = gsc;

            var CampaignId = GenericUtilities.Convert<long>(TempData.Peek("PRO_FILTER_CAMPAIGN"));
            Campaign = GenericUtilities.Convert<long?>(CampaignId);


            var inputData = new MemberListFilter
            {
                CampaignId = Campaign,
                Type = 0,
                Pagenumber = pn,
                Pagesize = ps,
                SearchString = gs,
                SearchColumn = gsc
            };

            var objResponse = _membershipFormRepository.GetAllAcceptedMembers(inputData);
            if (objResponse != null && objResponse.transactionStatus == System.Net.HttpStatusCode.OK
                && objResponse.returnData != null)
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
            pageListFilterColumns = objList;
        }
        private void BindDropdowns()
        {
            CampignList = _dropDownRepository.GetAllCampaignList();
        }
        public JsonResult OnPostApplyFilter()
        {
            // Store filter values in TempData
            TempData["PRO_FILTER_CAMPAIGN"] = Campaign.ToString();
            return new JsonResult(true);
        }
        public IActionResult OnPostExportData()
        {
            var campaign = TempData.Peek("PRO_FILTER_CAMPAIGN");

            Campaign = GenericUtilities.Convert<long?>(campaign);

            var empData = _membershipFormRepository.ExportMembershipDatatoExcel("", Campaign);
            var tempFileName = empData.returnData;
            return new JsonResult(new { tFileName = tempFileName, fileName = "MemberShipReport.xlsx" });
        }
    }
}
