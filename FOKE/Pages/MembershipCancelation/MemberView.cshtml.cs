using FOKE.DataAccess;
using FOKE.Entity.Common;
using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Entity.MembershipRegistration.ViewModel;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.MembershipCancelation
{
    public class MemberViewModel : BasePageModel
    {
        [BindProperty]
        public PostMembershipViewModel inputModel { get; set; }

        public List<MembershipViewModel> Comments { get; set; }

        [BindProperty]
        public int TaskId { get; set; }

        [BindProperty]
        public int CommentType { get; set; }

        [BindProperty]
        public string CommentHtml { get; set; }

        [BindProperty]
        public IFormFile Attachment { get; set; }

        public long? Status { get; set; }
        public List<DropDownViewModel> StatusList { get; set; }

        private readonly FOKEDBContext _dbContext;
        private readonly IDropDownRepository _dropDownRepository;
        public List<DropDownViewModel> RejectedReasonList { get; set; }

        public string? pageErrorMessage { get; set; }
        private readonly IMembershipFormRepository _membershipFormRepository;
        public MemberViewModel(IDropDownRepository dropDownRepository, IMembershipFormRepository membershipFormRepository)
        {
            _dropDownRepository = dropDownRepository;
            _membershipFormRepository = membershipFormRepository;
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
                }
                else
                {
                    isValidRequest = false;
                    pageErrorMessage = retData.returnMessage;
                }
            }
            BindDropdowns();

        }


        private void BindDropdowns()
        {
            RejectedReasonList = _dropDownRepository.GetCancelledReasonList();

        }
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnPostCancelMemberAsync([FromBody] CancelMemberRequest request)
        {
            var response = await _membershipFormRepository.CancelMembershipAsync(request.MemberId, request.Reason, request.Description);

            if (response.transactionStatus == HttpStatusCode.OK && response.returnData)
            {
                return new JsonResult(new { success = true });
            }
            else
            {
                return new JsonResult(new { success = false, message = response.returnMessage ?? "Cancellation failed" });
            }
        }



        public class CancelMemberRequest
        {
            public long MemberId { get; set; }  // or long if you want
            public string Reason { get; set; }
            public string Description { get; set; }
        }
    }
}
