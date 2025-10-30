using FOKE.Entity;
using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.DigitalIDManagement
{
    public class MemberDetailsViewModel : BasePageModel
    {
        public PostMembershipViewModel inputModel { get; set; }

        private readonly ISharedLocalizer _sharedLocalizer;
        private readonly IMembershipFormRepository _membershipFormRepository;

        public MemberDetailsViewModel(IDropDownRepository dropDownRepository, IMembershipFormRepository membershipFormRepository)
        {
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
        }

        public JsonResult OnPostForceLogout(long deviceId)
        {
            var retData = new ResponseEntity<bool>();
            retData = _membershipFormRepository.ForceLogOutDevice(deviceId);
            return new JsonResult(retData);
        }
    }
}
