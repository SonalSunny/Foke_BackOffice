using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using System.Net;

namespace FOKE.Pages.RejectedMembersList
{
    public class MemberViewModel : BasePageModel
    {
        public PostMembershipViewModel inputModel { get; set; }

        private readonly IDropDownRepository _dropDownRepository;
        private readonly IMembershipFormRepository _membershipFormRepository;
        public string? pageErrorMessage { get; set; }

        public MemberViewModel(IDropDownRepository dropDownRepository, IMembershipFormRepository membershipFormRepository)
        {
            _dropDownRepository = dropDownRepository;
            _membershipFormRepository = membershipFormRepository;
        }


        public void OnGet(long? Id, string mode)
        {
            _formMode = mode;
            isValidRequest = true;
            inputModel = new PostMembershipViewModel();
            if (Id > 0)
            {
                var retData = _membershipFormRepository.GetRejectedMemberById(Convert.ToInt64(Id));
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
        }
    }
}
