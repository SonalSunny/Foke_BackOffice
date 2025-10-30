using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.Common;
using FOKE.Entity.MembershipRegistration.ViewModel;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages
{
    public class RegistrationModel : BasePageModel
    {

        [BindProperty]
        public MembershipViewModel inputModel { get; set; }
        private readonly FOKEDBContext _dbContext;


        private readonly IMembershipFormRepository _membershipFormRepository;
        private readonly IDropDownRepository _dropDownRepository;


        [BindProperty]
        public List<DropDownViewModel> GenderList { get; set; }
        public List<DropDownViewModel> BloodGroupList { get; set; }
        public List<DropDownViewModel> ProffessionList { get; set; }
        public List<DropDownViewModel> WorkPlaceList { get; set; }
        public List<DropDownViewModel> DistrictList { get; set; }
        public List<DropDownViewModel> AreaList { get; set; }
        public List<DropDownViewModel> HearaboutUsList { get; set; }
        public List<DropDownViewModel> YearList { get; set; }
        public List<DropDownViewModel> DepartmentList { get; set; }


        public RegistrationModel(IMembershipFormRepository membershipFormRepository, IDropDownRepository dropDownRepository)
        {
            _membershipFormRepository = membershipFormRepository;
            _dropDownRepository = dropDownRepository;
        }
        public void OnGet()
        {
            BindDropdowns();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var retData = new ResponseEntity<MembershipViewModel>();

            var civilIdStatus = _membershipFormRepository.IsValidKuwaitCivilID(inputModel.CivilId);

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
            if (inputModel.DOB != null)
            {
                if (ModelState.ContainsKey("inputModel.DOB"))
                {
                    ModelState["inputModel.DOB"].Errors.Clear(); // ✅ Clear the DOB error manually
                }
            }



            // 1. Validate Passport No
            var passportStatus = _membershipFormRepository.IsValidatePassportNo(inputModel.PassportNo);
            if (passportStatus == 1)
                ModelState.AddModelError("inputModel.PassportNo", "Invalid Passport No.");
            else if (passportStatus == 2)
                ModelState.AddModelError("inputModel.PassportNo", "Passport No already exists.");
            

            if (!string.IsNullOrWhiteSpace(inputModel.ContactNo.ToString()))
            {
                bool isContactValid = _membershipFormRepository.IsValidContactNo(inputModel.ContactNo.Value);
                if (!isContactValid)
                {
                    ModelState.AddModelError("inputModel.ContactNo", "Contact number already exists.");
                    
                }
            }


            if (ModelState.IsValid)
            {

                retData = await _membershipFormRepository.RegisterMember(inputModel);
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
                    BindDropdowns();
                    return Page();
                }
            }
            else
            {
                retData.transactionStatus = HttpStatusCode.BadRequest;
                pageErrorMessage = "Fill all Required fields";
                IsSuccessReturn = false;
            }
            BindDropdowns();
            return Page();
        }
        public IActionResult OnGetValidateCivilId(string CivilId)
        {
            long Status = _membershipFormRepository.IsValidKuwaitCivilID(CivilId);
            return new JsonResult(new { Status });
        }

        public IActionResult OnGetValidatePassportNo(string PassportNo)
        {
            long Status = _membershipFormRepository.IsValidatePassportNo(PassportNo);
            return new JsonResult(new { Status });
        }

        public IActionResult OnGetValidateContactNo(long ContactNo)
        {
            bool isValid = _membershipFormRepository.IsValidContactNo(ContactNo);
            return new JsonResult(new { isValid });
        }
        private void BindDropdowns()
        {
            GenderList = _dropDownRepository.GetGender();
            WorkPlaceList = _dropDownRepository.GetWorkPlace();
            BloodGroupList = _dropDownRepository.GetBloodGroup();
            DistrictList = _dropDownRepository.GetDistrict();
            ProffessionList = _dropDownRepository.GetProffession();
            AreaList = _dropDownRepository.GetArea();
            HearaboutUsList = _dropDownRepository.GetHearAboutUs();
            YearList = _dropDownRepository.GetYearList();
            DepartmentList = _dropDownRepository.GetDepartmentList();
        }


    }
}
