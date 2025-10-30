using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Entity.ZoneMaster.ViewModel;
using FOKE.Entity.ZoneMember;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class ZoneRepository : IZoneRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;
        public ZoneRepository(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor)
        {
            this._dbContext = FOKEDBContext;

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
            catch (Exception)
            {
            }
        }



        public async Task<ResponseEntity<ZoneViewModel>> AddZone(ZoneViewModel model)
        {
            var retModel = new ResponseEntity<ZoneViewModel>();

            try
            {

                var ZoneExists = _dbContext.Zones
                       .Any(u => u.ZoneName == model.ZoneName);
                if (ZoneExists)
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                    retModel.returnMessage = "Zone Already Exists";
                }
                else
                {


                    var zone = new Entity.ZoneMaster.DTO.Zone
                    {
                        ZoneName = model.ZoneName,
                        Description = model.Description,
                        Active = true,
                        CreatedBy = model.loggedinUserId

                    };
                    await _dbContext.Zones.AddAsync(zone);
                    await _dbContext.SaveChangesAsync();

                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Saved Successfully";
                    retModel.returnData = model;

                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error Occured";
            }
            return retModel;
        }


        public async Task<ResponseEntity<ZoneViewModel>> UpdateZone(ZoneViewModel model)
        {
            var retModel = new ResponseEntity<ZoneViewModel>();

            try
            {
                var zone = await _dbContext.Zones
                    .Where(r => r.ZoneId == model.ZoneId && r.Active)
                    .SingleOrDefaultAsync();

                if (zone != null)
                {

                    var zoneExists = await _dbContext.Zones
                        .AnyAsync(r => r.ZoneId != model.ZoneId && r.ZoneName == model.ZoneName && r.Active);

                    if (zoneExists)
                    {
                        retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                        retModel.returnMessage = "Zone already exists with the same name";
                        return retModel;
                    }

                    zone.ZoneName = model.ZoneName;
                    zone.Description = model.Description;
                    zone.UpdatedBy = model.loggedinUserId;
                    zone.UpdatedDate = DateTime.UtcNow;

                    await _dbContext.SaveChangesAsync();

                    // Return success
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Zone updated successfully";
                    retModel.returnData = model;
                }
                else
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Zone not found or inactive";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "An internal server error occurred";
            }

            return retModel;
        }




        public ResponseEntity<ZoneViewModel> GetZonebyId(long zoneId)
        {
            var retModel = new ResponseEntity<ZoneViewModel>();
            try
            {
                var objRole = _dbContext.Zones
                     .SingleOrDefault(u => u.ZoneId == zoneId);
                var objModel = new ZoneViewModel();
                objModel.ZoneId = objRole.ZoneId;
                objModel.ZoneName = objRole.ZoneName;
                objModel.Description = objRole.Description;


                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel;

            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }

        public ResponseEntity<List<ZoneViewModel>> GetAllZones(long? Status, string? zone)
        {
            var retModel = new ResponseEntity<List<ZoneViewModel>>();
            try
            {
                var objModel = new List<ZoneViewModel>();

                IQueryable<Entity.ZoneMaster.DTO.Zone> retData = _dbContext.Zones.Where(c => c.Active == true);
                if (Status.HasValue)
                {
                    if (Status == 2)
                    {
                        retData = _dbContext.Zones.Where(c => c.Active == true || c.Active == false);
                    }
                    else if (Status == 1)
                    {
                        retData = _dbContext.Zones.Where(c => c.Active == true);
                    }
                    else if (Status == 0)
                    {
                        retData = _dbContext.Zones.Where(c => c.Active == false);
                    }
                }


                if (!string.IsNullOrEmpty(zone))
                {
                    retData = retData.Where(c => (c.ZoneName ?? "").ToLower().Contains(zone.ToLower()));
                }

                objModel = retData.Select(c => new ZoneViewModel()
                {
                    ZoneId = c.ZoneId,
                    ZoneName = c.ZoneName,

                    Description = c.Description.Length > 75 ? c.Description.Substring(0, 75) + " See more..." : c.Description,
                    Active = c.Active,
                    CreatedUsername = _dbContext.Users.FirstOrDefault(e => e.UserId == c.CreatedBy).UserName
                }).ToList();

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

        public ResponseEntity<bool> DeleteZone(ZoneViewModel objModel)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var zoneDetails = _dbContext.Zones.Find(objModel.ZoneId);



                if (zoneDetails.Active)
                {
                    zoneDetails.Active = false;

                    _dbContext.Entry(zoneDetails).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    retModel.returnMessage = "Deactivated";
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;

                }
                else
                {
                    zoneDetails.Active = true;
                    _dbContext.Entry(zoneDetails).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    retModel.returnMessage = "Activated";
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                }







            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }
        public async Task<List<long>> GetAssignedMemberIdsAsync(long zoneId)
        {
            return await _dbContext.ZoneMembers
                .Where(am => am.ZoneId == zoneId)
                .Select(am => am.UserMemberId)
                .ToListAsync();
        }

        public async Task AssignMembersToZoneAsync(long zoneId, List<long> memberIds)
        {

            try
            {
                // Fetch already assigned member IDs for this area
                var existingMemberIds = await _dbContext.ZoneMembers
                    .Where(am => am.ZoneId == zoneId && memberIds.Contains(am.UserMemberId))
                    .Select(am => am.UserMemberId)
                    .ToListAsync();

                // Get only new member IDs that are not already assigned
                var newMemberIds = memberIds.Except(existingMemberIds).ToList();

                // Create AreaMember entries for new members
                var newzoneMembers = newMemberIds.Select(memberId => new ZoneMember
                {
                    ZoneId = zoneId,
                    UserMemberId = memberId,
                    Active = true
                });

                // Add and save only if there's anything new to insert
                if (newzoneMembers.Any())
                {
                    await _dbContext.ZoneMembers.AddRangeAsync(newzoneMembers);
                    await _dbContext.SaveChangesAsync();
                }

            }


            catch (Exception ex)
            {



            }
        }

        public List<UserViewModel> GetMembersAssignedToZone(long zoneId, string? keyword = null, string? column = null)
        {
            var query = _dbContext.ZoneMembers
                .Where(am => am.ZoneId == zoneId)
                .Join(
                    _dbContext.Users,
                    am => am.UserMemberId,
                    ma => ma.UserId,
                    (am, ma) => ma
                )
                .Distinct(); // Optional: Ensures distinct members

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string searchColumn = column?.Trim().ToLower();
                string lowerKeyword = keyword.Trim().ToLower();

                if (searchColumn == "username")
                {
                    query = query.Where(u => u.UserName != null && u.UserName.ToLower().Contains(lowerKeyword));
                }
                else if (searchColumn == "all" || string.IsNullOrEmpty(searchColumn))
                {
                    query = query.Where(u =>
                        (u.UserName != null && u.UserName.ToLower().Contains(lowerKeyword))

                    );
                }
            }

            return query.Select(u => new UserViewModel
            {
                UserId = u.UserId,
                Username = u.UserName
            }).ToList();

        }
        public async Task<bool> UpdateAssignedUsers(long zoneId, List<long>? assignedUserIds, long updatedBy)
        {
            try
            {
                // Remove existing members
                var existing = _dbContext.ZoneMembers.Where(x => x.ZoneId == zoneId);
                _dbContext.ZoneMembers.RemoveRange(existing);

                // Add new members
                if (assignedUserIds != null && assignedUserIds.Any())
                {
                    var newMembers = assignedUserIds.Select(userId => new ZoneMember
                    {
                        ZoneId = zoneId,
                        UserMemberId = userId,
                        CreatedBy = updatedBy,
                        CreatedDate = DateTime.UtcNow,
                        Active = true,
                        UpdatedBy = updatedBy,
                        UpdatedDate = DateTime.UtcNow
                    }).ToList();

                    await _dbContext.ZoneMembers.AddRangeAsync(newMembers);
                }

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }





    }
}
