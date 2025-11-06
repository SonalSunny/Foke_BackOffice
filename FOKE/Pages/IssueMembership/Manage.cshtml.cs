using FOKE.Entity;
using FOKE.Entity.CampaignData.ViewModel;
using FOKE.Entity.Common;
using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.IssueMembership
{
    public class ManageModel : PagedListBasePageModel
    {
        [BindProperty]
        public PostMembershipViewModel inputModel { get; set; }

        [BindProperty]
        public string? ActionStatus { get; set; }

        [BindProperty]
        public string? RejectionReason { get; set; }

        [BindProperty]
        public string? RejectionRemarks { get; set; }
        [BindProperty]
        public long? Issueid { get; set; }


        public List<DropDownViewModel> GenderList { get; set; }
        public List<DropDownViewModel> BloodGroupList { get; set; }
        public List<DropDownViewModel> ProfesssionList { get; set; }
        public List<DropDownViewModel> WorkPlaceList { get; set; }
        public List<DropDownViewModel> DistrictList { get; set; }
        public List<DropDownViewModel> AreaList { get; set; }
        public List<DropDownViewModel> ZoneList { get; set; }
        public List<DropDownViewModel> UnitList { get; set; }
        public List<CampaignDropDownList> CampaignList { get; set; }
        public List<DropDownViewModel> PaymentTypeList { get; set; }
        public List<DropDownViewModel> CountryCodeList { get; set; }
        public List<DropDownViewModel> HearAboutUsList { get; set; }
        public List<DropDownViewModel> DepartmentList { get; set; }
        public List<DropDownViewModel> YearList { get; set; }
        public List<DropDownViewModel> RejectedReasonList { get; set; }
        public List<DropDownViewModel> RelationTypeList { get; set; }


        public string? pageErrorMessage { get; set; }

        private readonly IMembershipFormRepository _membershipFormRepository;
        private readonly IDropDownRepository _dropDownRepository;
        private readonly ISharedLocalizer _sharedLocalizer;

        public ManageModel(IMembershipFormRepository membershipFormRepository, IDropDownRepository dropDownRepository, ISharedLocalizer sharedLocalizer)
        {
            _membershipFormRepository = membershipFormRepository;
            _dropDownRepository = dropDownRepository;
            _sharedLocalizer = sharedLocalizer;
        }

        public void OnGet(long? id, string mode)
        {
            _formMode = mode;
            isValidRequest = true;
            inputModel = new PostMembershipViewModel();
            if (id > 0)
            {
                var retData = _membershipFormRepository.GetMemberById(Convert.ToInt64(id));
                if (retData.transactionStatus == HttpStatusCode.OK)
                {
                    isValidRequest = true;
                    inputModel = retData.returnData;
                    BindDropdowns();
                }
                else
                {
                    isValidRequest = false;
                    pageErrorMessage = retData.returnMessage;
                }
            }
            else
            {
                _formMode = FormModeEnum.add.ToString();
            }
            setPagedListColumns();
            BindDropdowns();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var retData = new ResponseEntity<PostMembershipViewModel>();

            if (formMode.Equals(FormModeEnum.edit))
            {
                var fieldsToSkip = new[]
                {
                    "inputModel.ZoneId",
                    "inputModel.UnitId",
                    "inputModel.CampaignId",
                    "inputModel.CampaignAmount",
                    "inputModel.AmountRecieved",
                    "inputModel.PaymentTypeId",
                    "inputModel.PaymentRemarks",
                    "inputModel.Attachment",
                    "sortColumn",
                    "sortOrder",
                    "searchField",
                    "globalSearch",
                    "globalSearchColumn"
                };

                foreach (var field in fieldsToSkip)
                {
                    if (ModelState.ContainsKey(field))
                    {
                        ModelState.Remove(field);
                    }
                }
            }
            else
            {
                var fieldsToSkip = new[]
                {
                    "inputModel.PaymentRemarks",
                    "inputModel.Attachment",
                    "sortColumn",
                    "sortOrder",
                    "searchField",
                    "globalSearch",
                    "globalSearchColumn"
                };

                foreach (var field in fieldsToSkip)
                {
                    if (ModelState.ContainsKey(field))
                    {
                        ModelState.Remove(field);
                    }
                }
            }
            if (Issueid == null)
            {
                var civilIdStatus = _membershipFormRepository.IsValidateKuwaitCivilID(inputModel.CivilId, inputModel.MembershipId);

                if (civilIdStatus == 1)
                {
                    ModelState.AddModelError("inputModel.CivilId", "Invalid Civil ID.");
                }
                else if (civilIdStatus == 2)
                {
                    ModelState.AddModelError("inputModel.CivilId", "Civil ID already exists.");
                }
                else if (civilIdStatus == 3)
                {
                    if (ModelState.ContainsKey("inputModel.CivilId"))
                    {
                        ModelState["inputModel.CivilId"].Errors.Clear();
                    }
                }

                var passportStatus = _membershipFormRepository.IsValidateEditIssuePassportNo(inputModel.PassportNo, inputModel.MembershipId);
                if (passportStatus == 1)
                    ModelState.AddModelError("inputModel.PassportNo", "Invalid Passport No.");
                else if (passportStatus == 2)
                    ModelState.AddModelError("inputModel.PassportNo", "Passport No already exists.");

                if (!string.IsNullOrWhiteSpace(inputModel.ContactNo.ToString()))
                {
                    bool isContactValid = _membershipFormRepository.IsValidEditIssueContactNo(inputModel.ContactNo.Value, inputModel.MembershipId);
                    if (!isContactValid)
                    {
                        ModelState.AddModelError("inputModel.ContactNo", "Contact number already exists.");
                    }
                }
            }


            if (ModelState.IsValid)
            {
                if (formMode.Equals(FormModeEnum.add))
                {
                    inputModel.MembershipStatus = ActionStatus == "accept" ? 1 : 0;
                    inputModel.RejectionReason = RejectionReason;
                    inputModel.RejectionRemarks = RejectionRemarks;
                    if (Issueid > 0)
                    {
                        inputModel.IssueId = Issueid;
                    }

                    retData = await _membershipFormRepository.IssueMember(inputModel);
                    if (retData.transactionStatus != HttpStatusCode.OK)
                    {
                        ViewData["SelectedCampaign"] = inputModel.CampaignId;
                        Issueid = retData.returnData?.IssueId;
                        pageErrorMessage = retData.returnMessage;
                        IsSuccessReturn = false;
                    }
                    else
                    {
                        ModelState.Clear();
                        IsSuccessReturn = true;
                        sucessMessage = retData.returnMessage;
                        BindDropdowns();
                        setPagedListColumns();
                        return Page();
                    }
                }
                else
                {
                    retData = await _membershipFormRepository.UpdateRegisterdMember(inputModel);
                    if (retData.transactionStatus != HttpStatusCode.OK)
                    {
                        pageErrorMessage = retData.returnMessage;
                        IsSuccessReturn = false;
                    }
                    else
                    {
                        ModelState.Clear();
                        IsSuccessReturn = true;
                        sucessMessage = retData.returnMessage;
                        inputModel = new PostMembershipViewModel();
                    }
                }
            }
            else
            {

                retData.transactionStatus = HttpStatusCode.BadRequest;
                pageErrorMessage = "Fill all Required fields";
                IsSuccessReturn = false;
            }

            if (inputModel.DateofBirth.HasValue)
            {
                inputModel.DateofBirthString = inputModel.DateofBirth.Value.ToString("dd-MM-yyyy");
            }

            setPagedListColumns();
            BindDropdowns();
            return Page();
        }


        public IActionResult OnGetValidateCivilId(string civilId, long? membershipId)
        {
            var status = _membershipFormRepository.IsValidateKuwaitCivilID(civilId, membershipId);
            return new JsonResult(new { status });
        }
        public IActionResult OnGetValidateContactNo(long ContactNo, long? membershipId)
        {
            bool isValid = _membershipFormRepository.IsValidEditIssueContactNo(ContactNo, membershipId);
            return new JsonResult(new { isValid });
        }

        public IActionResult OnGetValidatePassportNo(string PassportNo, long? membershipId)
        {
            long status = _membershipFormRepository.IsValidateEditIssuePassportNo(PassportNo, membershipId);
            return new JsonResult(new { status });
        }

        private void BindDropdowns()
        {
            GenderList = _dropDownRepository.GetGender();
            WorkPlaceList = _dropDownRepository.GetWorkPlace();
            BloodGroupList = _dropDownRepository.GetBloodGroup();
            DistrictList = _dropDownRepository.GetDistrict();
            ProfesssionList = _dropDownRepository.GetProffession();
            AreaList = _dropDownRepository.GetArea();
            ZoneList = _dropDownRepository.GetZone();
            UnitList = _dropDownRepository.GetUnit();
            PaymentTypeList = _dropDownRepository.GetPaymentTypes();
            CampaignList = _dropDownRepository.GetCampaignList();
            HearAboutUsList = _dropDownRepository.GetHearAboutUs();
            DepartmentList = _dropDownRepository.GetDepartmentList();
            YearList = _dropDownRepository.GetYearList();
            RejectedReasonList = _dropDownRepository.GetRejectedReasonList();
            RelationTypeList = _dropDownRepository.GetRelationTypes();
        }

        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();
            objList.Add(new PageListFilterColumns { ColumName = "All", ColumnDescription = _sharedLocalizer.Localize("DD_ALL_TABLE_SEARCH").Value });
            objList.Add(new PageListFilterColumns { ColumName = "FullName", ColumnDescription = _sharedLocalizer.Localize("Full Name").Value });
            objList.Add(new PageListFilterColumns { ColumName = "CivilID", ColumnDescription = _sharedLocalizer.Localize("Civil ID").Value });
            objList.Add(new PageListFilterColumns { ColumName = "Passport Number", ColumnDescription = _sharedLocalizer.Localize("Passport Number").Value });
            objList.Add(new PageListFilterColumns { ColumName = "Contact Number", ColumnDescription = _sharedLocalizer.Localize("Contact Number").Value });

            pageListFilterColumns = objList;
        }

        public IActionResult OnGetGetDetails(string keyword, string searchText)
        {
            var response = _membershipFormRepository.GetMemberDetails(keyword, searchText);
            return new JsonResult(response);
        }

    }
}
