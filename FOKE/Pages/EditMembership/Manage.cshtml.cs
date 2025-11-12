using FOKE.Entity;
using FOKE.Entity.CampaignData.ViewModel;
using FOKE.Entity.Common;
using FOKE.Entity.FileUpload.ViewModel;
using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.EditMembership
{
    public class ManageModel : PagedListBasePageModel
    {
        public List<AttachmentViewModel> ProfileImages { get; set; } = new();


        [BindProperty]
        public PostMembershipViewModel inputModel { get; set; }
        private readonly ISharedLocalizer _sharedLocalizer;
        private readonly IMembershipFormRepository _membershipFormRepository;
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IDropDownRepository _dropDownRepository;
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

        public ManageModel(IDropDownRepository dropDownRepository, IMembershipFormRepository membershipFormRepository, ISharedLocalizer sharedLocalizer, IAttachmentRepository attachmentRepository)
        {
            _membershipFormRepository = membershipFormRepository;
            _dropDownRepository = dropDownRepository;
            _sharedLocalizer = sharedLocalizer;
            _attachmentRepository = attachmentRepository;
        }
        public void OnGet(long? Id, string mode)
        {

            inputModel = new PostMembershipViewModel();
            if (Id > 0)
            {
                var retData = _membershipFormRepository.GetAcceptedMemberById(Convert.ToInt64(Id));
                if (retData.transactionStatus == HttpStatusCode.OK)
                {
                    isValidRequest = true;
                    inputModel = retData.returnData;

                    var attachmentResult = _attachmentRepository.GetAttachmentById(Id.Value.ToString());
                    if (attachmentResult != null && attachmentResult.returnData != null)
                    {
                        ProfileImages = attachmentResult.returnData;
                    }
                }
                else
                {
                    isValidRequest = false;
                    pageErrorMessage = retData.returnMessage;
                }


            }
            setPagedListColumns();
            BindDropdowns();

        }
        public async Task<IActionResult> OnPostAsync()
        {
            var retData = new ResponseEntity<PostMembershipViewModel>();

            // Remove fields from ModelState that are not relevant for this edit form, if needed
            var fieldsToSkip = new[]
            {
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
                    ModelState.Remove(field);
            }

            var civilIdStatus = _membershipFormRepository.IsValidKuwaitCivilIDforEdit(inputModel.CivilId, inputModel.IssueId);

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
                // Clear existing errors (especially useful on Edit)
                if (ModelState.ContainsKey("inputModel.CivilId"))
                {
                    ModelState["inputModel.CivilId"].Errors.Clear();
                }
            }



            // 1. Validate Passport No
            var passportStatus = _membershipFormRepository.IsValidatePassportNoforEdit(inputModel.PassportNo, inputModel.IssueId);
            if (passportStatus == 1)
                ModelState.AddModelError("inputModel.PassportNo", "Invalid Passport No.");
            else if (passportStatus == 2)
                ModelState.AddModelError("inputModel.PassportNo", "Passport No already exists.");

            if (!string.IsNullOrWhiteSpace(inputModel.ContactNo.ToString()))
            {
                bool isContactValid = _membershipFormRepository.IsValidContactNoforEdit(inputModel.ContactNo.Value, inputModel.IssueId);
                if (!isContactValid)
                {
                    ModelState.AddModelError("inputModel.ContactNo", "Contact number already exists.");
                }
            }


            if (inputModel.DateofBirth.HasValue)
            {
                inputModel.DateofBirthString = inputModel.DateofBirth.Value.ToString("dd-MM-yyyy");
            }

            if (ModelState.IsValid)
            {
                retData = await _membershipFormRepository.EditMembershipAsync(inputModel);

                if (retData.transactionStatus != System.Net.HttpStatusCode.OK)
                {
                    pageErrorMessage = retData.returnMessage;
                    IsSuccessReturn = false;
                }
                else
                {
                    ModelState.Clear();
                    IsSuccessReturn = true;
                    sucessMessage = retData.returnMessage;

                    // Optionally reload inputModel from DB or reset, depending on your UX
                    // inputModel = new PostMembershipViewModel();
                }
            }
            else
            {
                retData.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                pageErrorMessage = "Fill all required fields";
                IsSuccessReturn = false;
            }

            setPagedListColumns();
            BindDropdowns();
            return Page();
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


        public IActionResult OnGetValidateCivilId(string CivilId, long? issueId)
        {
            long Status = _membershipFormRepository.IsValidKuwaitCivilIDforEdit(CivilId, issueId);
            return new JsonResult(new { Status });
        }

        public IActionResult OnGetValidateContactNo(long ContactNo, long? issueId)
        {
            bool isValid = _membershipFormRepository.IsValidContactNoforEdit(ContactNo, issueId);
            return new JsonResult(new { isValid });
        }

        public IActionResult OnGetValidatePassportNo(string PassportNo, long? issueId)
        {
            long status = _membershipFormRepository.IsValidatePassportNoforEdit(PassportNo, issueId);
            return new JsonResult(new { status });
        }
        public IActionResult OnGetGetDetails(string keyword, string searchText)
        {
            var response = _membershipFormRepository.GetMemberDetails(keyword, searchText);

            // Return entire response as JSON
            return new JsonResult(response);
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
    }
}
