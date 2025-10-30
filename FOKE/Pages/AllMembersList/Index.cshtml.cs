using FOKE.Entity;
using FOKE.Entity.Common;
using FOKE.Entity.MembershipData.ViewModel;
using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.AllMembersList
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
        public long? Gender { get; set; }
        [BindProperty]
        public long? BloodGroup { get; set; }
        [BindProperty]
        public long? District { get; set; }
        [BindProperty]
        public long? Department { get; set; }
        [BindProperty]
        public long? Workingsince { get; set; }
        [BindProperty]
        public long? Zone { get; set; }
        [BindProperty]
        public long? Unit { get; set; }
        [BindProperty]
        public long? AgeGroupId { get; set; }
        [BindProperty]
        public long? PaymentStatus { get; set; }
        [BindProperty]
        public List<DropDownViewModel> ProffessionList { get; set; }
        [BindProperty]
        public List<DropDownViewModel> WorkPlaceList { get; set; }
        [BindProperty]
        public List<DropDownViewModel> AreaList { get; set; }



        [BindProperty]
        public List<DropDownViewModel> GenderList { get; set; }
        [BindProperty]
        public List<DropDownViewModel> BloodGroupList { get; set; }
        [BindProperty]
        public List<DropDownViewModel> DistrictList { get; set; }
        [BindProperty]
        public List<DropDownViewModel> DepartmentList { get; set; }
        [BindProperty]
        public List<DropDownViewModel> YearList { get; set; }
        [BindProperty]
        public List<DropDownViewModel> ZoneList { get; set; }
        [BindProperty]

        public List<DropDownViewModel> UnitList { get; set; }

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
                TempData["PRO_FILTER_GENDER"] = null;
                TempData["PRO_FILTER_BLOODGROUP"] = null;
                TempData["PRO_FILTER_DISTRICT"] = null;
                TempData["PRO_FILTER_DEPT"] = null;
                TempData["PRO_FILTER_WORKYEAR"] = null;
                TempData["PRO_FILTER_ZONE"] = null;
                TempData["PRO_FILTER_UNIT"] = null;
                TempData["PRO_FILTER_AGE"] = null;
                TempData["PRO_FILTER_PAYMENT"] = null;
            }
        }


        public JsonResult OnPostApplyFilter()
        {

            TempData["PRO_FILTER_PROFESSION"] = ProffessionID.ToString();
            TempData["PRO_FILTER_AREA"] = Areaid.ToString();
            TempData["PRO_FILTER_WORKPLACE"] = WorkPlaceId.ToString();
            TempData["PRO_FILTER_GENDER"] = Gender.ToString();
            TempData["PRO_FILTER_BLOODGROUP"] = BloodGroup.ToString();
            TempData["PRO_FILTER_DISTRICT"] = District.ToString();
            TempData["PRO_FILTER_DEPT"] = Department.ToString();
            TempData["PRO_FILTER_WORKYEAR"] = Workingsince.ToString();
            TempData["PRO_FILTER_ZONE"] = Zone.ToString();
            TempData["PRO_FILTER_UNIT"] = Unit.ToString();
            TempData["PRO_FILTER_AGE"] = AgeGroupId.ToString();
            TempData["PRO_FILTER_PAYMENT"] = PaymentStatus.ToString();
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
            objList.Add(new PageListFilterColumns { ColumName = "PassportNo", ColumnDescription = _sharedLocalizer.Localize("Passport No").Value });


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
            var Genderid = TempData.Peek("PRO_FILTER_GENDER");
            Gender = GenericUtilities.Convert<long?>(Genderid);
            var Blood = TempData.Peek("PRO_FILTER_BLOODGROUP");
            BloodGroup = GenericUtilities.Convert<long?>(Blood);
            var Distr = TempData.Peek("PRO_FILTER_DISTRICT");
            District = GenericUtilities.Convert<long?>(Distr);
            var Dept = TempData.Peek("PRO_FILTER_DEPT");
            Department = GenericUtilities.Convert<long?>(Dept);
            var WorkYear = TempData.Peek("PRO_FILTER_WORKYEAR");
            Workingsince = GenericUtilities.Convert<long?>(WorkYear);
            var Zoneid = TempData.Peek("PRO_FILTER_ZONE");
            Zone = GenericUtilities.Convert<long?>(Zoneid);
            var UnitID = TempData.Peek("PRO_FILTER_UNIT");
            Unit = GenericUtilities.Convert<long?>(UnitID);
            var Age = TempData.Peek("PRO_FILTER_AGE");

            AgeGroupId = GenericUtilities.Convert<long?>(Age);
            var PayStatus = TempData.Peek("PRO_FILTER_PAYMENT");

            PaymentStatus = GenericUtilities.Convert<long?>(PayStatus);

            var inputData = new MemberListFilter
            {
                AreaId = Areaid,
                ProffesionId = ProffessionID,
                WorkplaceId = WorkPlaceId,
                Type = 0,
                Gender = Gender,
                BloodGroup = BloodGroup,
                District = District,
                Department = Department,
                Workingsince = Workingsince,
                Zone = Zone,
                Unit = Unit,
                AgeGroupId = AgeGroupId,
                PaymentSatus = PaymentStatus,
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
            GenderList = _dropDownRepository.GetGender();
            WorkPlaceList = _dropDownRepository.GetWorkPlace();
            BloodGroupList = _dropDownRepository.GetBloodGroup();
            DistrictList = _dropDownRepository.GetDistrict();
            AreaList = _dropDownRepository.GetArea();
            ZoneList = _dropDownRepository.GetZone();
            UnitList = _dropDownRepository.GetUnit();
            DepartmentList = _dropDownRepository.GetDepartmentList();
            YearList = _dropDownRepository.GetYearList();
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

        public IActionResult OnPostExportData([FromBody] ExportRequestModel data)
        {
            var Area = TempData.Peek("PRO_FILTER_AREA");
            var Areaid = GenericUtilities.Convert<long?>(Area);
            var Proffesion = TempData.Peek("PRO_FILTER_PROFESSION");
            var ProfessionID = GenericUtilities.Convert<long?>(Proffesion);
            var Workplace = TempData.Peek("PRO_FILTER_WORKPLACE");
            var WorkPlaceID = GenericUtilities.Convert<long?>(Workplace);
            var Genderid = TempData.Peek("PRO_FILTER_GENDER");
            var Genders = GenericUtilities.Convert<long?>(Genderid);
            var Blood = TempData.Peek("PRO_FILTER_BLOODGROUP");
            var BloodGroups = GenericUtilities.Convert<long?>(Blood);
            var Distr = TempData.Peek("PRO_FILTER_DISTRICT");
            var Dist = GenericUtilities.Convert<long?>(Distr);
            var Dept = TempData.Peek("PRO_FILTER_DEPT");
            var Departments = GenericUtilities.Convert<long?>(Dept);
            var WorkYear = TempData.Peek("PRO_FILTER_WORKYEAR");
            var WorkYr = GenericUtilities.Convert<long?>(WorkYear);
            var Zoneid = TempData.Peek("PRO_FILTER_ZONE");
            var Zones = GenericUtilities.Convert<long?>(Zoneid);
            var UnitID = TempData.Peek("PRO_FILTER_UNIT");
            var Units = GenericUtilities.Convert<long?>(UnitID);
            var Age = TempData.Peek("PRO_FILTER_AGE");
            var AgeGroup = GenericUtilities.Convert<long?>(Age);
            var PayStatus = TempData.Peek("PRO_FILTER_PAYMENT");
            var PaidStatus = GenericUtilities.Convert<long?>(PayStatus);
            var empData = _membershipFormRepository.ExportMembersDatatoExcel(
                Areaid, ProfessionID, WorkPlaceID, Genders, BloodGroups, Dist, Departments, WorkYr, Zones, Units, AgeGroup, PaidStatus, data.GlobalSearchColumn, data.GlobalSearch
            );

            var tempFileName = empData.returnData;

            return new JsonResult(new { tFileName = tempFileName, fileName = "AllMembersList.xlsx" });
        }

        public IActionResult OnPostExportPdf([FromBody] ExportRequestModel request)
        {
            var Area = TempData.Peek("PRO_FILTER_AREA");
            var Areaid = GenericUtilities.Convert<long?>(Area);
            var Proffesion = TempData.Peek("PRO_FILTER_PROFESSION");
            var ProfessionID = GenericUtilities.Convert<long?>(Proffesion);
            var Workplace = TempData.Peek("PRO_FILTER_WORKPLACE");
            var WorkPlaceID = GenericUtilities.Convert<long?>(Workplace);
            var Genderid = TempData.Peek("PRO_FILTER_GENDER");
            var Genders = GenericUtilities.Convert<long?>(Genderid);
            var Blood = TempData.Peek("PRO_FILTER_BLOODGROUP");
            var BloodGroups = GenericUtilities.Convert<long?>(Blood);
            var Distr = TempData.Peek("PRO_FILTER_DISTRICT");
            var Dist = GenericUtilities.Convert<long?>(Distr);
            var Dept = TempData.Peek("PRO_FILTER_DEPT");
            var Departments = GenericUtilities.Convert<long?>(Dept);
            var WorkYear = TempData.Peek("PRO_FILTER_WORKYEAR");
            var WorkYr = GenericUtilities.Convert<long?>(WorkYear);
            var Zoneid = TempData.Peek("PRO_FILTER_ZONE");
            var Zones = GenericUtilities.Convert<long?>(Zoneid);
            var UnitID = TempData.Peek("PRO_FILTER_UNIT");
            var Units = GenericUtilities.Convert<long?>(UnitID);
            var Age = TempData.Peek("PRO_FILTER_AGE");
            var AgeGroup = GenericUtilities.Convert<long?>(Age);
            var PayStatus = TempData.Peek("PRO_FILTER_PAYMENT");
            var PaidStatus = GenericUtilities.Convert<long?>(PayStatus);
            var data = _membershipFormRepository.ExportAllMemberstoPdf(Areaid, ProfessionID, WorkPlaceID, Genders, BloodGroups, Dist, Departments, WorkYr, Zones, Units, AgeGroup, PaidStatus, request.GlobalSearchColumn, request.GlobalSearch);
            return new JsonResult(new { tFileName = data.returnData, fileName = "AllMembersList.pdf" });
        }
    }
}
