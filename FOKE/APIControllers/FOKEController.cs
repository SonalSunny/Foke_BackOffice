using FOKE.Entity.API.APIModel.ViewModel;
using FOKE.Entity.API.DeviceLogin.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FOKEController : ControllerBase
    {
        private readonly IMembershipFormRepository _MembershipForm;
        private readonly IOfferRepository _offerRepo;
        private readonly INewsAndEventsRepository _eventsRepo;
        private readonly IcontactUsRepository _contactUsRepo;
        private readonly IappInfoSectionRepository _appInfoRepo;
        private readonly ICommitteeMemberRepository _committeRepo;
        private readonly IfireBaseNotificationService _fireBaseRepo;

        public FOKEController(IMembershipFormRepository MembershipForm, IDropDownRepository dropDownRepository, IOfferRepository offerRepo, INewsAndEventsRepository eventsRepo, IcontactUsRepository contactUsRepo, IappInfoSectionRepository appInfoRepo, ICommitteeMemberRepository committeRepo, IfireBaseNotificationService fireBaseRepo)
        {
            _MembershipForm = MembershipForm;
            _offerRepo = offerRepo;
            _eventsRepo = eventsRepo;
            _contactUsRepo = contactUsRepo;
            _appInfoRepo = appInfoRepo;
            _committeRepo = committeRepo;
            _fireBaseRepo = fireBaseRepo;
        }

        [AllowAnonymous]
        [HttpPost("VerifyMemberByCivilID")]
        public async Task<ActionResult> VerifyMemberByCivilID(DeviceLoginViewModel model)
        {
            try
            {
                var Data = await _MembershipForm.GetMemberDataByCivilID(model);
                if (Data.transactionStatus == HttpStatusCode.OK)
                {
                    return Ok(Data.returnData);
                }
                else
                {
                    return BadRequest(Data.returnMessage ?? "An error occurred while fetching Member data.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("UploadProfilePicture")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> UploadProfilePicture(IFormFile ImageData)
        {
            try
            {
                var deviceId = HttpContext.User.FindFirst("deviceId")?.Value;
                var civilId = HttpContext.User.FindFirst("civilId")?.Value;
                var devicePrimaryid = HttpContext.User.FindFirst("devicePrimaryId")?.Value;

                var devicePrimaryID = int.Parse(devicePrimaryid);
                var Data = await _MembershipForm.UploadProfilePicture(ImageData, civilId, deviceId, devicePrimaryID);
                if (Data.transactionStatus == HttpStatusCode.OK)
                {
                    return Ok(Data.returnData);
                }
                else
                {
                    return BadRequest(Data.returnMessage ?? "An error occurred while uploading image.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetMemberDataPostLogin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GetMemberDataPostLogin()
        {
            try
            {
                var deviceId = HttpContext.User.FindFirst("deviceId")?.Value;
                var civilId = HttpContext.User.FindFirst("civilId")?.Value;
                var devicePrimaryid = HttpContext.User.FindFirst("devicePrimaryId")?.Value;

                var devicePrimaryID = int.Parse(devicePrimaryid);

                var Data = await _MembershipForm.GetMemberDataPostLogin(civilId, deviceId, devicePrimaryID);
                if (Data.transactionStatus == HttpStatusCode.OK)
                {
                    return Ok(Data.returnData);
                }
                else
                {
                    return BadRequest(Data.returnMessage ?? "An error occurred while Getting MemberData.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetAllOfferData")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GetAllOfferData()
        {
            try
            {
                var deviceId = HttpContext.User.FindFirst("deviceId")?.Value;
                var civilId = HttpContext.User.FindFirst("civilId")?.Value;
                var devicePrimaryid = HttpContext.User.FindFirst("devicePrimaryId")?.Value;

                var devicePrimaryID = int.Parse(devicePrimaryid);

                var Data = await _offerRepo.GetAllOffers();
                if (Data.transactionStatus == HttpStatusCode.OK)
                {
                    return Ok(Data.returnData);
                }
                else
                {
                    return BadRequest(Data.returnMessage ?? "An error occurred while Getting OfferData.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetAllNewsAndEventsData")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GetAllNewsAndEventsData(long Type, int PageNumber)
        {
            try
            {
                var deviceId = HttpContext.User.FindFirst("deviceId")?.Value;
                var civilId = HttpContext.User.FindFirst("civilId")?.Value;
                var devicePrimaryid = HttpContext.User.FindFirst("devicePrimaryId")?.Value;

                var devicePrimaryID = int.Parse(devicePrimaryid);

                var Data = await _eventsRepo.GetAllNewsEventsData(Type, PageNumber, civilId, devicePrimaryID);
                if (Data.transactionStatus == HttpStatusCode.OK)
                {
                    return Ok(Data.returnData);
                }
                else if (Data.transactionStatus == HttpStatusCode.Locked)
                {
                    return UnprocessableEntity(Data.returnData);
                }
                else
                {
                    return BadRequest(Data.returnMessage ?? "An error occurred while Getting OfferData.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("SaveClientEnquiery")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> SaveClientEnquiery(ContactUsDto Model)
        {
            try
            {
                Model.DeviceId = HttpContext.User.FindFirst("deviceId")?.Value;
                Model.CivilId = HttpContext.User.FindFirst("civilId")?.Value;
                var devicePrimaryID = HttpContext.User.FindFirst("devicePrimaryId")?.Value;

                Model.DevicePrimaryId = int.Parse(devicePrimaryID);

                var Data = await _contactUsRepo.AddClientEnquiery(Model);
                if (Data.transactionStatus == HttpStatusCode.OK)
                {
                    return Ok(Data.returnData);
                }
                else
                {
                    return BadRequest(Data.returnMessage ?? "An error occurred while Getting OfferData.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetAllEventTypes")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GetEventTypes()
        {
            try
            {
                var Data = await _eventsRepo.GetAllEventTypes();
                if (Data.transactionStatus == HttpStatusCode.OK)
                {
                    return Ok(Data.returnData);
                }
                else
                {
                    return BadRequest(Data.returnMessage ?? "An error occurred while Getting OfferData.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetAllSectionTypes")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GetAllSectionTypes()
        {
            try
            {
                var Data = await _appInfoRepo.GetAllSectionTypes();
                if (Data.transactionStatus == HttpStatusCode.OK)
                {
                    return Ok(Data.returnData);
                }
                else if (Data.transactionStatus == HttpStatusCode.NoContent)
                {
                    return BadRequest(Data.returnMessage ?? "No Data Found.");
                }
                else
                {
                    return BadRequest(Data.returnMessage ?? "An error occurred while Getting OfferData.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetSectionDataByType")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GetSectionDataByType(long Type)
        {
            try
            {
                var Data = await _appInfoRepo.GetSectionDataByType(Type);
                if (Data.transactionStatus == HttpStatusCode.OK)
                {
                    return Ok(Data.returnData);
                }
                else if (Data.transactionStatus == HttpStatusCode.NoContent)
                {
                    return BadRequest(Data.returnMessage ?? "No Data Found.");
                }
                else
                {
                    return BadRequest(Data.returnMessage ?? "An error occurred while Getting OfferData.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetCommitteDetailsByGroup")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GetCommitteDetailsByGroup(long? GroupId)
        {
            try
            {
                var Data = await _committeRepo.GetCommitteDetailsByGroup(GroupId);
                if (Data.transactionStatus == HttpStatusCode.OK)
                {
                    return Ok(Data.returnData);
                }
                else if (Data.transactionStatus == HttpStatusCode.NoContent)
                {
                    return BadRequest(Data.returnMessage ?? "No Data Found.");
                }
                else
                {
                    return BadRequest(Data.returnMessage ?? "An error occurred while Getting OfferData.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllGroups")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GetAllGroups()
        {
            try
            {
                var Data = await _committeRepo.GetAllGroupData();
                if (Data.transactionStatus == HttpStatusCode.OK)
                {
                    return Ok(Data.returnData);
                }
                else if (Data.transactionStatus == HttpStatusCode.NoContent)
                {
                    return BadRequest(Data.returnMessage ?? "No Data Found.");
                }
                else
                {
                    return BadRequest(Data.returnMessage ?? "An error occurred while Getting OfferData.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("UpdateLoginLogOutTime")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> UpdateLoginLogOutTime(long Type)
        {
            try
            {
                var deviceId = HttpContext.User.FindFirst("deviceId")?.Value;
                var civilId = HttpContext.User.FindFirst("civilId")?.Value;
                var devicePrimaryid = HttpContext.User.FindFirst("devicePrimaryId")?.Value;

                var Data = await _appInfoRepo.UpdateLoginLogOutTime(Type, int.Parse(devicePrimaryid));
                if (Data.transactionStatus == HttpStatusCode.OK)
                {
                    return Ok(Data.returnData);
                }
                else if (Data.transactionStatus == HttpStatusCode.NoContent)
                {
                    return BadRequest(Data.returnMessage ?? "No Data Found.");
                }
                else
                {
                    return BadRequest(Data.returnMessage ?? "An error occurred while Getting OfferData.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("NotificationSendAPI")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> NotificationSendAPI(PushNotificationDto Model)
        {
            try
            {
                var result = await _fireBaseRepo.SendPushToMultipleAsync(Model.Title, Model.Body, Model.FcmToken);
                return Ok(result.SuccessCount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetNotificationByMember")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GetNotificationByMember()
        {
            try
            {
                var deviceId = HttpContext.User.FindFirst("deviceId")?.Value;
                var civilId = HttpContext.User.FindFirst("civilId")?.Value;
                var devicePrimaryid = HttpContext.User.FindFirst("devicePrimaryId")?.Value;
                var DeviceId = long.Parse(devicePrimaryid);

                var Data = await _fireBaseRepo.GetNotificationByMember(civilId, DeviceId);
                if (Data.transactionStatus == HttpStatusCode.OK)
                {
                    return Ok(Data.returnData);
                }
                else if (Data.transactionStatus == HttpStatusCode.NoContent)
                {
                    return BadRequest(Data.returnMessage ?? "No Data Found.");
                }
                else
                {
                    return BadRequest(Data.returnMessage ?? "An error occurred while Getting Data.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("UpdateMessageStatus")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> UpdateMessageStatus(long MessageID)
        {
            try
            {
                var Data = await _fireBaseRepo.UpdateMessageStatus(MessageID);
                if (Data.transactionStatus == HttpStatusCode.OK)
                {
                    return Ok(Data.returnData);
                }
                else if (Data.transactionStatus == HttpStatusCode.NoContent)
                {
                    return BadRequest(Data.returnMessage ?? "No Data Found To Remove.");
                }
                else
                {
                    return BadRequest(Data.returnMessage ?? "An error occurred while Getting Data.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("LogOut")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> LogOut()
        {
            try
            {
                var deviceId = HttpContext.User.FindFirst("deviceId")?.Value;
                var civilId = HttpContext.User.FindFirst("civilId")?.Value;
                var devicePrimaryid = HttpContext.User.FindFirst("devicePrimaryId")?.Value;

                var DeviceId = long.Parse(devicePrimaryid);
                var Data = await _fireBaseRepo.LogOut(DeviceId);
                if (Data.transactionStatus == HttpStatusCode.OK)
                {
                    return Ok(Data.returnData);
                }
                else if (Data.transactionStatus == HttpStatusCode.NoContent)
                {
                    return BadRequest(Data.returnMessage ?? "No Devices Were Found.");
                }
                else
                {
                    return BadRequest(Data.returnMessage ?? "An error occurred while Getting Data.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}