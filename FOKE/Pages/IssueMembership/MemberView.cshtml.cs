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
    public class MemberViewModel : PagedListBasePageModel
    {
        [BindProperty]
        public PostMembershipViewModel inputModel { get; set; }
        public List<DropDownViewModel> ZoneList { get; set; }
        public List<DropDownViewModel> UnitList { get; set; }
        public List<CampaignDropDownList> CampaignList { get; set; }
        public List<DropDownViewModel> PaymentTypeList { get; set; }
        public List<DropDownViewModel> RejectedReasonList { get; set; }


        [BindProperty]
        public string? ActionStatus { get; set; }

        [BindProperty]
        public string? RejectionReason { get; set; }

        [BindProperty]
        public string? RejectionRemarks { get; set; }
        [BindProperty]
        public long? Issueid { get; set; }

        private readonly IDropDownRepository _dropDownRepository;
        private readonly IMembershipFormRepository _membershipFormRepository;
        private readonly ISharedLocalizer _sharedLocalizer;

        public string? pageErrorMessage { get; set; }

        public MemberViewModel(IDropDownRepository dropDownRepository, IMembershipFormRepository membershipFormRepository, ISharedLocalizer sharedLocalizer)
        {
            _dropDownRepository = dropDownRepository;
            _membershipFormRepository = membershipFormRepository;
            _sharedLocalizer = sharedLocalizer;
        }

        public void OnGet(long? Id, string mode)
        {
            _formMode = mode;
            isValidRequest = true;
            inputModel = new PostMembershipViewModel();
            if (Id > 0)
            {
                var retData = _membershipFormRepository.GetMemberById(Convert.ToInt64(Id));
                if (retData.transactionStatus == HttpStatusCode.OK)
                {
                    isValidRequest = true;
                    inputModel = retData.returnData;
                }
                else
                {
                    isValidRequest = false;
                    pageErrorMessage = retData.returnMessage;
                }
            }
            BindDropdowns();
            setPagedListColumns();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var retData = new ResponseEntity<bool>();
            var ValidForApproval = false;
            if (ActionStatus == "accept")
            {
                if ( inputModel.CampaignId != null)
                {
                    ValidForApproval = true;
                }
                else
                {
                    retData.transactionStatus = HttpStatusCode.BadRequest;
                    pageErrorMessage = "Fill all Required fields";
                    IsSuccessReturn = false;
                    setPagedListColumns();
                }
            }
            else
            {
                ValidForApproval = true;
            }

            if (ValidForApproval)
            {
                inputModel.MembershipStatus = ActionStatus == "accept" ? 1 : 0;
                inputModel.RejectionReason = RejectionReason;
                inputModel.RejectionRemarks = RejectionRemarks;
                if (Issueid != null)
                {
                    inputModel.IssueId = Issueid;
                }
                retData = await _membershipFormRepository.IssueMemberFromRegister(inputModel);
                if (retData.transactionStatus != HttpStatusCode.OK)
                {
                    if (inputModel.IssueId > 0)
                    {
                        ViewData["SelectedCampaign"] = inputModel.CampaignId;
                        Issueid = inputModel.IssueId;
                    }
                    var Data = _membershipFormRepository.GetMemberById(Convert.ToInt64(inputModel.MembershipId));
                    if (Data.transactionStatus == HttpStatusCode.OK)
                    {
                        isValidRequest = true;
                        inputModel = Data.returnData;
                    }
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
            setPagedListColumns();
            BindDropdowns();
            return Page();
        }

        private void BindDropdowns()
        {
            ZoneList = _dropDownRepository.GetZone();
            UnitList = _dropDownRepository.GetUnit();
            PaymentTypeList = _dropDownRepository.GetPaymentTypes();
            CampaignList = _dropDownRepository.GetCampaignList();
            RejectedReasonList = _dropDownRepository.GetRejectedReasonList();
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
    }
}
