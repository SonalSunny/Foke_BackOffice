using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.Notification.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IfireBaseNotificationService _notificationRepo;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;

        public NotificationRepository(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor, IfireBaseNotificationService notificationRepo)
        {
            this._dbContext = FOKEDBContext;
            this._notificationRepo = notificationRepo;
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
        public async Task<ResponseEntity<NotificationViewModel>> SaveNotification(NotificationViewModel model)
        {
            var retModel = new ResponseEntity<NotificationViewModel>();

            try
            {
                var notification = new Entity.Notification.DTO.Notification
                {
                    NotificationType = model.NotificationType,
                    SendTo = model.SendTo,
                    SendToNumbers = model.SendToNumbers,
                    Header = model.Header,
                    RemovedNumbers = model.RemovedNumbers,
                    Content = model.NotificationContent,
                    AreaId = model.Area,
                    ZoneId = model.Zone,
                    UnitId = model.Unit,
                    Active = true,
                    CreatedBy = loggedInUser,
                    CreatedDate = DateTime.UtcNow,
                    Status = false,
                    LogGeneratedStatus = false,
                };

                await _dbContext.NotificationDatas.AddAsync(notification);
                await _dbContext.SaveChangesAsync();
                var Type = "Push notifications";
                if (model.NotificationType.Trim().ToLower() == Type.Trim().ToLower())
                {
                    var data = await _notificationRepo.SetNotificationContentByMembersAsync();
                }
                retModel.transactionStatus = HttpStatusCode.OK;
                retModel.returnMessage = "Notification Send successfully.";
                retModel.returnData = model; // optional: return back input model or mapped saved entity
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error: " + ex.Message;
            }

            return retModel;
        }

        public ReciepientCountDto AllMemberCount()
        {
            var ReturnData = new ReciepientCountDto();
            var reciepientData = GetMemberDataByType(0, 0);
            var devicedata = reciepientData.DistinctBy(i => i.Name).ToList();
            ReturnData.ReciepientData = devicedata;
            ReturnData.Count = devicedata.Count();
            return ReturnData;
        }

        public ReciepientCountDto AllCommitteeMemberCount()
        {
            var ReturnData = new ReciepientCountDto();
            var reciepientData = GetMemberDataByType(1, 0);
            var devicedata = reciepientData.DistinctBy(i => i.Name).ToList();
            ReturnData.ReciepientData = devicedata;
            ReturnData.Count = devicedata.Count();
            return ReturnData;
        }

        public ReciepientCountDto AllActiveMemberCount()
        {
            var ReturnData = new ReciepientCountDto();
            var reciepientData = GetMemberDataByType(2, 0);
            if (reciepientData != null)
            {
                var devicedata = reciepientData.DistinctBy(i => i.Name).ToList();
                ReturnData.ReciepientData = devicedata;
                ReturnData.Count = devicedata.Count();
            }
            return ReturnData;
        }

        public ReciepientCountDto AllInActiveMemberCount()
        {
            var ReturnData = new ReciepientCountDto();
            var reciepientData = GetMemberDataByType(3, 0);
            if (reciepientData != null)
            {
                var devicedata = reciepientData.DistinctBy(i => i.Name).ToList();
                ReturnData.ReciepientData = devicedata;
                ReturnData.Count = devicedata.Count();
            }
            return ReturnData;
        }

        public ReciepientCountDto MembersByArea(long? AreaId)
        {
            var ReturnData = new ReciepientCountDto();
            var reciepientData = GetMemberDataByType(4, (long)AreaId);
            var devicedata = reciepientData.DistinctBy(i => i.Name).ToList();
            ReturnData.ReciepientData = devicedata;
            ReturnData.Count = devicedata.Count();
            return ReturnData;
        }

        public ReciepientCountDto MembersByZone(long? ZoneId)
        {

            var ReturnData = new ReciepientCountDto();
            var reciepientData = GetMemberDataByType(5, (long)ZoneId);
            var devicedata = reciepientData.DistinctBy(i => i.Name).ToList();
            ReturnData.ReciepientData = devicedata;
            ReturnData.Count = devicedata.Count();
            return ReturnData;
        }

        public ReciepientCountDto MembersByUnit(long? UnitID)
        {
            var ReturnData = new ReciepientCountDto();
            var reciepientData = GetMemberDataByType(6, (long)UnitID);
            var devicedata = reciepientData.DistinctBy(i => i.Name).ToList();
            ReturnData.ReciepientData = devicedata;
            ReturnData.Count = devicedata.Count();
            return ReturnData;
        }

        public ResponseEntity<List<NotificationViewModel>> GetAllNotifications(long? Status)
        {
            var retModel = new ResponseEntity<List<NotificationViewModel>>();
            try
            {
                var objModel = new List<NotificationViewModel>();

                // Base query: Active users
                IQueryable<Entity.Notification.DTO.Notification> retData = _dbContext.NotificationDatas.Where(c => c.Active == true);
                if (Status.HasValue)
                {
                    if (Status == 2)
                    {
                        // Include both Active = true and Active = false
                        retData = _dbContext.NotificationDatas.Where(c => c.Active == true || c.Active == false);
                    }
                    else if (Status == 1)
                    {
                        // Status = 1 maps to Active = true
                        retData = _dbContext.NotificationDatas.Where(c => c.Active == true);
                    }
                    else if (Status == 0)
                    {
                        // Status = 0 maps to Active = false
                        retData = _dbContext.NotificationDatas.Where(c => c.Active == false);
                    }
                }

                // Map to ViewModel
                objModel = retData.Select(c => new NotificationViewModel()
                {
                    NotificationId = c.NotificationId,
                    Header = c.Header,
                    NotificationContent = c.Content,
                    NotificationType = c.NotificationType,
                    Active = c.Active,
                    SendToTime = c.CreatedDate != null ? c.CreatedDate.Value.AddHours(3) : null, // ✅ Add 3 hours


                }).ToList();
                // Set response
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel.OrderByDescending(i => i.NotificationId).ToList();
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message; // Include error message for debugging
            }

            return retModel;
        }

        public ResponseEntity<List<NotificationViewModel>> GetAllNotificationlogs(long? NotificationId)
        {
            var retModel = new ResponseEntity<List<NotificationViewModel>>();
            try
            {
                var objModel = _dbContext.MembershipAcceptedDatas
                              .Join(_dbContext.NotificationLogs,
                                    ma => ma.CivilId,
                                    n => n.MemberCivilId,
                                    (ma, n) => new { ma, n })
                              .Join(_dbContext.DeviceDetails,
                                    combined => combined.n.DeviceId,         // join on NotificationLog.DeviceId
                                    d => d.DeviceDetailId,                                // join with DeviceDetails.Id
                                    (combined, d) => new { combined.ma, combined.n, Device = d })
                              .Where(joined => NotificationId == null || joined.n.NotificationId == NotificationId)
                              .Select(joined => new NotificationViewModel
                              {
                                  NotificationId = (long)joined.n.NotificationId,
                                  Name = joined.ma.Name,
                                  ContactNo = joined.ma.ContactNo,
                                  FirebaseSuccess = joined.n.FirebaseSuccess,
                                  SendToTime = joined.n.CreatedDate.HasValue ? joined.n.CreatedDate.Value.AddHours(3) : (DateTime?)null,
                                  DeviceModel = joined.Device.DeviceModel // assuming you have this field
                              })
                      .ToList();

                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message;
            }

            return retModel;
        }

        public ResponseEntity<List<ReciepientData>> GetMemberDetails(string? Keyword, string? SearchText)
        {
            var retModel = new ResponseEntity<List<ReciepientData>>();

            try
            {
                var objModel = GetMemberDataByType(7, 0, Keyword.ToLower().Trim());
                retModel.returnData = objModel;
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message;
            }

            return retModel;
        }

        public List<ReciepientData> GetMemberDataByType(long Type, long Id, string Keyword = null)
        {

            var departmentData = _dbContext.Departments.Where(x => x.Active).ToList();
            var profileDatas = _dbContext.MemberProfileDatas.Where(i => i.Active).Include(p => p.FileStorage).ToList();

            var reciepientData = _dbContext.MembershipAcceptedDatas
                            .Include(x => x.WorkPlace)
                            .Include(x => x.Profession)
                            .Include(x => x.AreaData)
                            .Include(x => x.Zone)
                            .Include(x => x.Unit)
                             .Where(m => m.Active)
                             .Join(_dbContext.DeviceDetails.Where(d => d.Active),
                                   m => m.CivilId,
                                   d => d.CivilId,
                                   (m, d) => new { Member = m, Device = d }) // keep both objects
                             .Distinct().AsEnumerable()
                             .Select(x => new ReciepientData
                             {
                                 RefNo = x.Member.ReferanceNo,
                                 IssueId = x.Member.IssueId,
                                 Name = x.Member.Name,
                                 PhoneNo = x.Member.ContactNo.ToString(),
                                 AmountRecieved = x.Member.AmountRecieved,
                                 AreaId = x.Member.AreaId,
                                 ZoneId = x.Member.ZoneId,
                                 UnitId = x.Member.UnitId,
                                 CivilId = x.Member.CivilId,
                                 PassportNo = x.Member.PassportNo,
                                 Email = x.Member.Email,
                                 Area = x.Member.AreaData.AreaName,
                                 Zone = x.Member.Zone.ZoneName,
                                 Unit = x.Member.Unit.UnitName,
                                 Department = x.Member.DepartmentId != null ? departmentData.FirstOrDefault(c => c.DepartmentId == x.Member.DepartmentId).DepartmentName : null,
                                 Workplace = x.Member.WorkPlace.WorkPlaceName,
                                 Proffession = x.Member.Profession.ProffessionName,
                                 ImagePath = profileDatas.Any(i => i.MemberId == x.Member.IssueId) ? profileDatas.FirstOrDefault(i => i.MemberId == x.Member.IssueId).FileStorage.FilePath : "/images/Default.jpg",
                             }).DistinctBy(i => i.IssueId).ToList();

            if (Type == 1)
            {
                var MemberIDs = _dbContext.CommitteMembers.Where(m => m.Active == true).Select(i => i.IssueId).ToList();
                reciepientData = reciepientData.Where(i => MemberIDs.Contains(i.IssueId)).ToList();
            }
            if (Type == 2)
            {
                reciepientData = reciepientData.Where(i => i.AmountRecieved != 0).ToList();
            }
            if (Type == 3)
            {
                reciepientData = reciepientData.Where(i => (i.AmountRecieved ?? 0) == 0).ToList();
            }
            if (Type == 4)
            {
                reciepientData = reciepientData.Where(i => i.AreaId == Id).ToList();
            }
            if (Type == 5)
            {
                reciepientData = reciepientData.Where(i => i.ZoneId == Id).ToList();
            }
            if (Type == 6)
            {
                reciepientData = reciepientData.Where(i => i.UnitId == Id).ToList();
            }
            if (Type == 7)
            {
                reciepientData = reciepientData.Where(m =>
                     m.Name.ToLower().Trim().Contains(Keyword) ||
                     m.CivilId.Contains(Keyword) ||
                     m.PassportNo.Contains(Keyword) ||
                     m.Email.ToString().Contains(Keyword) ||
                     m.PhoneNo.ToString().Contains(Keyword))
                .ToList();
            }
            return reciepientData;
        }
    }
}