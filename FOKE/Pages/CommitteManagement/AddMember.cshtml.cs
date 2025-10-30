using FOKE.Entity;
using FOKE.Entity.CommitteeManagement.ViewModel;
using FOKE.Entity.Common;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.CommitteManagement
{
    public class AddMemberModel : BasePageModel
    {
        public string? pageErrorMessage { get; set; }
        [BindProperty]
        public string? MemberId { get; set; }
        public string MemberName { get; set; }

        [BindProperty]
        public CommitteMemberViewModel inputModel { get; set; }
        public List<DropDownViewModel> GroupList { get; set; }
        private readonly ICommitteeMemberRepository _committeeMemberRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        private readonly IDropDownRepository _dropDownRepository;
        private readonly IMembershipFormRepository _membershipFormRepository;
        private readonly IAttachmentRepository _attachmentRepository;
        public AddMemberModel(ICommitteeMemberRepository committeeMemberRepository, IDropDownRepository dropDownRepository, ISharedLocalizer sharedLocalizer, IMembershipFormRepository membershipFormRepository, IAttachmentRepository attachmentRepository)
        {
            _committeeMemberRepository = committeeMemberRepository;
            _dropDownRepository = dropDownRepository;
            _sharedLocalizer = sharedLocalizer;
            _membershipFormRepository = membershipFormRepository;
            _attachmentRepository = attachmentRepository;
        }
        public async Task<IActionResult> OnGet(long? id, string? mode)
        {
            _formMode = mode;
            inputModel = new CommitteMemberViewModel();

            if (_formMode == "edit")
            {
                if (id == null)
                    return NotFound();

                // Get CommitteeMember
                var retData = _committeeMemberRepository.GetCommitteeMemberById(id.Value);
                if (retData.transactionStatus == HttpStatusCode.OK && retData.returnData != null)
                {
                    isValidRequest = true;
                    inputModel = retData.returnData;

                    // Fetch MemberName from MembershipForm using IssueId
                    if (inputModel.IssueId.HasValue)
                    {
                        var memberData = _membershipFormRepository.GetAcceptedMemberById(inputModel.IssueId.Value);
                        if (memberData.transactionStatus == HttpStatusCode.OK && memberData.returnData != null)
                        {
                            MemberName = memberData.returnData.Name;
                            MemberId = memberData.returnData.ReferenceNumber;
                        }
                    }

                    // MemberId = inputModel.IssueId; // Still use IssueId as ID
                }
                else
                {
                    isValidRequest = false;
                    pageErrorMessage = retData.returnMessage;
                }
            }
            else
            {
                if (id == null)
                    return NotFound();

                // This ID is IssueId directly
                var retData = _membershipFormRepository.GetAcceptedMemberById(id.Value);
                if (retData.transactionStatus == HttpStatusCode.OK && retData.returnData != null)
                {
                    isValidRequest = true;
                    MemberId = retData.returnData.ReferenceNumber;
                    MemberName = retData.returnData.Name;

                    inputModel = new CommitteMemberViewModel
                    {
                        IssueId = id.Value
                        //Name = retData.returnData.Name,
                        //   ContactNo = retData.returnData.ContactNo,
                        //   CountryCodeId = retData.returnData.CountryCodeId
                    };
                }
                else
                {
                    isValidRequest = false;
                    pageErrorMessage = retData.returnMessage;
                }
            }

            BindDropdowns();
            return Page();
        }


        private void BindDropdowns()
        {
            GroupList = _dropDownRepository.GetGroupList();

        }
        public async Task<IActionResult> OnPostAsync()
        {

            ResponseEntity<CommitteMemberViewModel> addRetData = null;
            ResponseEntity<bool> updateRetData = null;

            if (ModelState.IsValid)
            {
                if (inputModel.CommitteMemberId > 0)
                {

                    updateRetData = await _committeeMemberRepository.UpdateCommitteeMember(inputModel);

                    if (updateRetData.transactionStatus == HttpStatusCode.OK && updateRetData.returnData)
                    {
                        ModelState.Clear();
                        IsSuccessReturn = true;
                        sucessMessage = "Committee member updated successfully.";


                        inputModel = new CommitteMemberViewModel();
                        BindDropdowns();
                        return Page();
                    }
                    else
                    {
                        pageErrorMessage = updateRetData.returnMessage ?? "Error updating committee member.";
                        IsSuccessReturn = false;
                    }
                }
                else
                {

                    addRetData = await _committeeMemberRepository.AddCommitteeMember(inputModel);

                    if (addRetData.transactionStatus == HttpStatusCode.OK && addRetData.returnData != null)
                    {
                        ModelState.Clear();
                        IsSuccessReturn = true;
                        sucessMessage = "Committee member added successfully.";


                        inputModel = new CommitteMemberViewModel();
                        BindDropdowns();
                        return Page();
                    }
                    else
                    {
                        pageErrorMessage = addRetData.returnMessage ?? "Error adding committee member.";
                        IsSuccessReturn = false;
                    }
                }
            }
            else
            {
                pageErrorMessage = "Fill all required fields.";
                IsSuccessReturn = false;

                // Remove overwritten fields so assigned values persist
                ModelState.Remove(nameof(MemberId));
                ModelState.Remove(nameof(MemberName));

                // Reassign based on IssueId
                if (inputModel.IssueId.HasValue)
                {
                    var mem = _membershipFormRepository.GetAcceptedMemberById(inputModel.IssueId.Value);
                    if (mem.transactionStatus == HttpStatusCode.OK && mem.returnData != null)
                    {
                        MemberId = mem.returnData.ReferenceNumber;
                        MemberName = mem.returnData.Name;
                    }
                }
            }

            BindDropdowns();
            return Page();
        }

    }
}
