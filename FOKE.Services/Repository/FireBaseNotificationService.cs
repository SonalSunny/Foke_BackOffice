using FirebaseAdmin.Messaging;
using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.API.APIModel.ViewModel;
using FOKE.Entity.Notification.DTO;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class FireBaseNotificationService : IfireBaseNotificationService
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDropDownRepository _dropDownRepository;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;

        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IWebHostEnvironment _env;

        public FireBaseNotificationService(FOKEDBContext dbContext, IHttpContextAccessor httpContextAccessor, IAttachmentRepository attachRepository, IWebHostEnvironment env, IDropDownRepository dropDownRepository)
        {
            this._dbContext = dbContext;
            this._httpContextAccessor = httpContextAccessor;
            this._attachmentRepository = attachRepository;
            this._dropDownRepository = dropDownRepository;
            _env = env ?? throw new ArgumentNullException(nameof(env));

            try
            {
                claimsPrincipal = _httpContextAccessor?.HttpContext?.User;
                var isAuthenticated = claimsPrincipal?.Identity?.IsAuthenticated ?? false;
                if (isAuthenticated)
                {
                    var userIdentity = claimsPrincipal?.Identity?.Name;
                    if (userIdentity != null)
                    {
                        long userid = 0;
                        Int64.TryParse(userIdentity, out userid);
                        if (userid > 0)
                        {
                            loggedInUser = userid;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<BatchResponse> SendPushToMultipleAsync(string title, string body, List<string> fcmTokens)
        {
            if (fcmTokens == null || fcmTokens.Count == 0)
                throw new ArgumentException("No FCM tokens provided.");

            var message = new MulticastMessage()
            {
                Tokens = fcmTokens,
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = title,
                    Body = body
                },
                Android = new AndroidConfig
                {
                    Priority = Priority.High
                },
                Apns = new ApnsConfig
                {
                    Aps = new Aps
                    {
                        Alert = new ApsAlert
                        {
                            Title = title,
                            Body = body
                        },
                        Sound = "default"
                    }
                }
            };

            var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
            return response; // Contains success/failure counts and error details
        }

        public ResponseEntity<bool> SendNotificationToMembers()
        {
            var response = new ResponseEntity<bool>();
            try
            {
                var NotificationData = _dbContext.NotificationDatas.Where(i => i.LogGeneratedStatus == false).ToList();
                if (NotificationData != null)
                {

                }
            }
            catch (Exception ex)
            {
                response.transactionStatus = HttpStatusCode.InternalServerError;
                response.returnMessage = "Server Error: " + ex.Message;
            }
            return response;
        }

        public async Task<ResponseEntity<bool>> SetNotificationContentByMembersAsync()
        {
            var response = new ResponseEntity<bool>();
            try
            {
                var Type = "Push notifications";
                var NotificationData = _dbContext.NotificationDatas.Where(i => i.LogGeneratedStatus == false && i.NotificationType.Trim().ToLower() == Type.Trim().ToLower()).ToList();
                if (NotificationData != null)
                {
                    var Memberdetails = _dbContext.MembershipAcceptedDatas.Where(i => i.Active).ToList();
                    if (Memberdetails != null)
                    {
                        var LogList = new List<NotificationLog>();
                        var NotificationList = new List<Entity.Notification.DTO.Notification>();

                        var MemberCivilIds = Memberdetails.Select(i => i.CivilId).ToList();

                        var DeviceData = _dbContext.DeviceDetails.Where(i => MemberCivilIds.Contains(i.CivilId) && i.FCMToken != null && i.Active).ToList();
                        var CommitteMemberIds = _dbContext.CommitteMembers.Where(i => i.Active).Select(i => i.IssueId).ToList();
                        var CommitteMemberCivilIds = Memberdetails.Where(i => CommitteMemberIds.Contains(i.IssueId)).Select(i => i.CivilId).ToList();

                        var ActiveMemberCivilIds = Memberdetails.Where(i => i.AmountRecieved != 0 && i.AmountRecieved != null).Select(i => i.CivilId).ToList();

                        foreach (var item in NotificationData)
                        {
                            if (item.SendTo == 43) //All Committe Members
                            {
                                DeviceData = DeviceData.Where(i => CommitteMemberCivilIds.Contains(i.CivilId)).ToList();
                            }
                            else if (item.SendTo == 44) //All Active Members
                            {
                                DeviceData = DeviceData.Where(i => ActiveMemberCivilIds.Contains(i.CivilId)).ToList();
                            }
                            else if (item.SendTo == 45) //All InActive Members
                            {
                                DeviceData = DeviceData.Where(i => !ActiveMemberCivilIds.Contains(i.CivilId)).ToList();
                            }
                            else if (item.SendTo == 46) //Members by Area
                            {
                                var IdByArea = Memberdetails.Where(i => i.AreaId == item.AreaId).Select(i => i.CivilId).ToList();
                                DeviceData = DeviceData.Where(i => IdByArea.Contains(i.CivilId)).ToList();
                            }
                            else if (item.SendTo == 47) //Members by Zone
                            {
                                var IdByZone = Memberdetails.Where(i => i.ZoneId == item.ZoneId).Select(i => i.CivilId).ToList();
                                DeviceData = DeviceData.Where(i => IdByZone.Contains(i.CivilId)).ToList();
                            }
                            else if (item.SendTo == 48) //Members by Unit
                            {
                                var IdByUnit = Memberdetails.Where(i => i.UnitId == item.UnitId).Select(i => i.CivilId).ToList();
                                DeviceData = DeviceData.Where(i => IdByUnit.Contains(i.CivilId)).ToList();
                            }
                            else if (item.SendTo == 49) //SpecificIds
                            {
                                var PhoneNumbers = item.SendToNumbers.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(i => long.Parse(i.Trim())).ToList();
                                var CivilIDs = Memberdetails.Where(i => PhoneNumbers.Contains((long)i.ContactNo)).Select(i => i.CivilId).ToList();
                                DeviceData = DeviceData.Where(i => CivilIDs.Contains(i.CivilId)).ToList();
                            }

                            if (item.RemovedNumbers != null)
                            {
                                var removedNumbers = item.RemovedNumbers.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(i => long.Parse(i.Trim())).ToList();
                                var RemovedCivilIDs = Memberdetails.Where(i => removedNumbers.Contains((long)i.ContactNo)).Select(i => i.CivilId).ToList();
                                DeviceData = DeviceData.Where(i => !RemovedCivilIDs.Contains(i.CivilId)).ToList();
                            }

                            foreach (var token in DeviceData)
                            {
                                var log = new NotificationLog
                                {
                                    FcmToken = token.FCMToken,
                                    NotificationId = item.NotificationId,
                                    MemberCivilId = token.CivilId,
                                    CreatedDate = DateTime.UtcNow,
                                    CreatedBy = loggedInUser,
                                    Active = true,
                                    DeviceId = token.DeviceDetailId,
                                    Title = item.Header,
                                    Content = item.Content,
                                };

                                try
                                {
                                    var message = new Message
                                    {
                                        Token = token.FCMToken,
                                        Notification = new FirebaseAdmin.Messaging.Notification
                                        {
                                            Title = item.Header,
                                            Body = item.Content
                                        }
                                    };
                                    var firebaseResponse = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                                    log.FirebaseSuccess = true;
                                }
                                catch (Exception ex)
                                {
                                    log.FirebaseSuccess = false;
                                    log.FirebaseError = ex.Message;
                                }
                                LogList.Add(log);
                            }
                            item.LogGeneratedStatus = true;
                            item.Status = true;
                        }
                        await _dbContext.NotificationLogs.AddRangeAsync(LogList);
                        await _dbContext.SaveChangesAsync();
                        response.transactionStatus = HttpStatusCode.OK;
                        response.returnMessage = "Success";
                    }
                    else
                    {
                        response.transactionStatus = HttpStatusCode.NoContent;
                        response.returnMessage = "No Data To Send Notification";
                    }
                }
            }
            catch (Exception ex)
            {
                response.transactionStatus = HttpStatusCode.InternalServerError;
                response.returnMessage = "Server Error: " + ex.Message;
            }
            return response;
        }

        public async Task<ResponseEntity<NotificationByMemberDataList>> GetNotificationByMember(string CivilID, long DeviceId)
        {
            var response = new ResponseEntity<NotificationByMemberDataList>();
            try
            {
                var Return = new NotificationByMemberDataList();
                var NotificationData = _dbContext.NotificationLogs.Where(i => i.MemberCivilId == CivilID && i.Active && i.DeviceId == DeviceId)
                    .Include(c => c.Notification).ToList();
                if (NotificationData.Any())
                {
                    var NotificatonData = NotificationData.Select(
                        i => new NotificationByMemberData
                        {
                            Id = i.Id,
                            Header = i.Notification.Header,
                            Message = i.Notification.Content,
                            IsRead = i.IsRead,
                            CreatedDateUtc = i.CreatedDate,
                            CreayedDateStringKuwait = GenericUtilities.ConvertAndFormatToKuwaitDateTime(i.CreatedDate, GenericUtilities.dateFormat),
                        }).ToList();

                    foreach (var item in NotificationData)
                    {
                        item.IsRead = true;
                    }
                    _dbContext.SaveChanges();
                    Return.NotificationList = NotificatonData.OrderByDescending(i => i.Id).ToList();
                    Return.IsUnRead = NotificatonData.Any(i => !i.IsRead) ? true : false;
                    response.returnData = Return;
                    response.returnMessage = "Success";
                    response.transactionStatus = HttpStatusCode.OK;
                }
                else
                {
                    response.transactionStatus = HttpStatusCode.NoContent;
                    response.returnMessage = "No Data To Send Notification";
                }
            }
            catch (Exception ex)
            {
                response.transactionStatus = HttpStatusCode.InternalServerError;
                response.returnMessage = "Server Error: " + ex.Message;
            }
            return response;
        }

        public async Task<ResponseEntity<bool>> UpdateMessageStatus(long MessageId)
        {
            var response = new ResponseEntity<bool>();
            try
            {
                var NotificationData = _dbContext.NotificationLogs.FirstOrDefault(i => i.Id == MessageId);
                if (NotificationData != null)
                {
                    NotificationData.Active = false;
                    _dbContext.Entry(NotificationData).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    response.returnData = true;
                    response.returnMessage = "Deactivated";
                    response.transactionStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    response.returnData = false;
                    response.transactionStatus = HttpStatusCode.NoContent;
                    response.returnMessage = "No Data Found";
                }
            }
            catch (Exception ex)
            {
                response.returnData = false;
                response.transactionStatus = HttpStatusCode.InternalServerError;
                response.returnMessage = "Server Error: " + ex.Message;
            }
            return response;
        }

        public async Task<ResponseEntity<bool>> LogOut(long DeviceId)
        {
            var response = new ResponseEntity<bool>();
            try
            {
                var DeviceData = _dbContext.DeviceDetails.FirstOrDefault(i => i.DeviceDetailId == DeviceId);
                if (DeviceData != null)
                {
                    DeviceData.Active = false;
                    DeviceData.LogOutDateTime = DateTime.UtcNow;
                    _dbContext.Entry(DeviceData).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    response.returnData = true;
                    response.returnMessage = "Deactivated";
                    response.transactionStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    response.returnData = false;
                    response.transactionStatus = HttpStatusCode.NoContent;
                    response.returnMessage = "No Device Found";
                }
            }
            catch (Exception ex)
            {
                response.returnData = false;
                response.transactionStatus = HttpStatusCode.InternalServerError;
                response.returnMessage = "Server Error: " + ex.Message;
            }
            return response;
        }
    }
}
