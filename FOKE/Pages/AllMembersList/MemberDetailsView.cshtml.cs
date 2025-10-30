using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.AllMembersList
{
    public class MemberDetailsViewModel : BasePageModel
    {
        public PostMembershipViewModel inputModel { get; set; } = new PostMembershipViewModel();

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


        public async Task<IActionResult> OnPostUploadProfileImage(IFormFile ProfileImage)
        {
            try
            {
                var profileImage = Request.Form.Files["ProfileImage"];
                var memberId = Request.Form["MemberId"].ToString();
                var ImageList = new List<IFormFile>();
                ImageList.Add(profileImage);
                var Result = await _membershipFormRepository.SaveAttachment(ImageList, Convert.ToInt64(memberId), null);

                if (Result.returnData)
                {
                    return new JsonResult(new
                    {
                        success = true,
                        message = "Profile image uploaded successfully.",
                    });
                }
                else
                {
                    return new JsonResult(new { success = false, message = "Failed to update profile image in database." });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "An error occurred while uploading the image." });
            }
        }
    }
}
