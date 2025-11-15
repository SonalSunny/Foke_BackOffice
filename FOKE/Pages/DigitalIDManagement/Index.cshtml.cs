using FOKE.Entity;
using FOKE.Entity.Common;
using FOKE.Entity.MembershipData.ViewModel;
using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.DigitalIDManagement
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly IMembershipFormRepository _membershipFormRepository;
        private readonly IDropDownRepository _dropDownRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        public IPagedList<PostMembershipViewModel> pagedListData { get; private set; }
        [BindProperty]
        public long? Areaid { get; set; }
        [BindProperty]
        public long? ProffessionID { get; set; }
        [BindProperty]
        public long? WorkPlaceId { get; set; }
        [BindProperty]
        public List<DropDownViewModel> ProffessionList { get; set; }
        public List<DropDownViewModel> WorkPlaceList { get; set; }
        public List<DropDownViewModel> AreaList { get; set; }
        [BindProperty]
        public string? pageCode { get; set; }

        public class ExportRequestModel
        {
            public string GlobalSearch { get; set; }
            public string GlobalSearchColumn { get; set; }
        }

        public IndexModel(IMembershipFormRepository membershipFormRepository, ISharedLocalizer sharedLocalizer, IDropDownRepository dropDownRepository)
        {
            _membershipFormRepository = membershipFormRepository;
            _sharedLocalizer = sharedLocalizer;
            _dropDownRepository = dropDownRepository;
        }
        public void OnGet(string isGoBack, string PageCode)
        {
            pageCode = PageCode;
            setPagedListColumns();
            BindDropdowns();
            if (isGoBack?.ToLower() != "y")
            {
                TempData["PRO_FILTER_AREA"] = null;
                TempData["PRO_FILTER_PROFESSION"] = null;
                TempData["PRO_FILTER_WORKPLACE"] = null;
            }
        }


        public JsonResult OnPostApplyFilter()
        {

            TempData["PRO_FILTER_PROFESSION"] = ProffessionID.ToString();
            TempData["PRO_FILTER_AREA"] = Areaid.ToString();
            TempData["PRO_FILTER_WORKPLACE"] = WorkPlaceId.ToString(); ;

            return new JsonResult(true);
        }


        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();
            objList.Add(new PageListFilterColumns { ColumName = "Name", ColumnDescription = _sharedLocalizer.Localize("Full Name").Value });
            objList.Add(new PageListFilterColumns { ColumName = "ReferenceNumber", ColumnDescription = _sharedLocalizer.Localize("Member ID").Value });
            objList.Add(new PageListFilterColumns { ColumName = "CivilId", ColumnDescription = _sharedLocalizer.Localize("Civil ID").Value });
            objList.Add(new PageListFilterColumns { ColumName = "Email", ColumnDescription = _sharedLocalizer.Localize("Email").Value });
            objList.Add(new PageListFilterColumns { ColumName = "PhoneNo", ColumnDescription = _sharedLocalizer.Localize("Contact Number").Value });

               
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
            var Area = TempData.Peek("PRO_FILTER_AREA");
            Areaid = GenericUtilities.Convert<long?>(Area);
            var Proffesion = TempData.Peek("PRO_FILTER_PROFESSION");
            ProffessionID = GenericUtilities.Convert<long?>(Proffesion);
            var Workplace = TempData.Peek("PRO_FILTER_WORKPLACE");
            WorkPlaceId = GenericUtilities.Convert<long?>(Workplace);


            var inputData = new MemberListFilter
            {
                AreaId = Areaid,
                ProffesionId = ProffessionID,
                WorkplaceId = WorkPlaceId,
                Type = 1,
                Pagenumber = pn,
                Pagesize = ps,
                SearchString = gs,
                SearchColumn = gsc
            };


            var response = _membershipFormRepository.GetAllAcceptedMembers(inputData);
            if (response.transactionStatus == System.Net.HttpStatusCode.OK)
            {
                pagedListData = PagedList(response.returnData, response.TotalCount);
            }

            return new PartialViewResult
            {
                ViewName = "_IndexPartial",
                ViewData = ViewData
            };
        }
        private void BindDropdowns()
        {
            WorkPlaceList = _dropDownRepository.GetWorkPlace();
            ProffessionList = _dropDownRepository.GetProffession();
            AreaList = _dropDownRepository.GetArea();
        }

        public async Task<JsonResult> OnGetOtpSettingsAsync(long issueId)
        {
            var membershipAccepted = await _membershipFormRepository.GetOtpSettingsRawAsync(issueId);

            if (membershipAccepted == null)
            {
                return new JsonResult(new { success = false, message = "Not found" }) { StatusCode = 404 };
            }

            return new JsonResult(new
            {
                email = membershipAccepted.EmailOtp ?? false,
                mobile = membershipAccepted.MobileOtp ?? false
            });
        }

        public IActionResult OnPostUpdateOtpPreference(long id, bool mobileOtp, bool emailOtp)
        {
            try
            {
                _membershipFormRepository.UpdateOtpPreferences(id, mobileOtp, emailOtp);
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                // Log error if needed
                return StatusCode(500, "Failed to update OTP preferences.");
            }
        }

        //public IActionResult OnPostExportData([FromBody] ExportRequestModel data)
        //{
        //    var Area = TempData.Peek("PRO_FILTER_AREA");
        //    var Areaid = GenericUtilities.Convert<long?>(Area);

        //    // Use the received data
        //    var empData = _membershipFormRepository.ExportMembersDatatoExcel(
        //        Areaid, 0, 0, data.GlobalSearchColumn, data.GlobalSearch
        //    );

        //    var tempFileName = empData.returnData;

        //    return new JsonResult(new { tFileName = tempFileName, fileName = "AllMembersList.xlsx" });
        //}

        //public IActionResult OnPostExportPdf([FromBody] ExportRequestModel request)
        //{
        //    var Area = TempData.Peek("PRO_FILTER_AREA");
        //    var Areaid = GenericUtilities.Convert<long?>(Area);
        //    var data = _membershipFormRepository.ExportAllMemberstoPdf(Areaid, 0, 0, request.GlobalSearchColumn, request.GlobalSearch);
        //    return new JsonResult(new { tFileName = data.returnData, fileName = "AllMembersList.pdf" });
        //}
    }
}
