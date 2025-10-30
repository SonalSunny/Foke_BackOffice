using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.AreaMaster.ViewModel;
using FOKE.Entity.AreaMember.DTO;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class AreaRepository : IAreaRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;
        public AreaRepository(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor)
        {
            this._dbContext = FOKEDBContext;
            this._httpContextAccessor = httpContextAccessor;
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



        public async Task<ResponseEntity<AreaDataViewModel>> AddArea(AreaDataViewModel model)
        {
            var retModel = new ResponseEntity<AreaDataViewModel>();

            try
            {

                var AreaExists = _dbContext.AreaDatas
                       .Any(u => u.AreaName == model.AreaName);
                if (AreaExists)
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                    retModel.returnMessage = "Area Already Exists";
                }
                else
                {


                    var area = new Entity.AreaMaster.DTO.AreaData
                    {
                        AreaName = model.AreaName,
                        Description = model.Description,
                        Active = true,
                        CreatedBy = model.loggedinUserId

                    };
                    await _dbContext.AreaDatas.AddAsync(area);
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


        public async Task<ResponseEntity<AreaDataViewModel>> UpdateArea(AreaDataViewModel model)
        {
            var retModel = new ResponseEntity<AreaDataViewModel>();

            try
            {
                var area = await _dbContext.AreaDatas
                    .Where(r => r.AreaId == model.AreaId && r.Active)
                    .SingleOrDefaultAsync();

                if (area != null)
                {
                    // Checking if the profession already exists
                    var areaExists = await _dbContext.AreaDatas
                        .AnyAsync(r => r.AreaId != model.AreaId && r.AreaName == model.AreaName && r.Active);

                    if (areaExists)
                    {
                        retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                        retModel.returnMessage = "Area already exists with the same name";
                        return retModel;
                    }

                    area.AreaName = model.AreaName;
                    area.Description = model.Description;
                    area.UpdatedBy = model.loggedinUserId;
                    area.UpdatedDate = DateTime.UtcNow;

                    await _dbContext.SaveChangesAsync();

                    // Return success
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Area updated successfully";
                    retModel.returnData = model;
                }
                else
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Area not found or inactive";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "An internal server error occurred";
            }

            return retModel;
        }




        public ResponseEntity<AreaDataViewModel> GetAreabyId(long areaId)
        {
            var retModel = new ResponseEntity<AreaDataViewModel>();
            try
            {
                var objRole = _dbContext.AreaDatas
                     .SingleOrDefault(u => u.AreaId == areaId);
                var objModel = new AreaDataViewModel();
                objModel.AreaId = objRole.AreaId;
                objModel.AreaName = objRole.AreaName;
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

        public ResponseEntity<List<AreaDataViewModel>> GetAllAreas(long? Status, string? Area)
        {
            var retModel = new ResponseEntity<List<AreaDataViewModel>>();
            try
            {
                var objModel = new List<AreaDataViewModel>();

                IQueryable<Entity.AreaMaster.DTO.AreaData> retData = _dbContext.AreaDatas.Where(c => c.Active == true);
                if (Status.HasValue)
                {
                    if (Status == 2)
                    {
                        retData = _dbContext.AreaDatas.Where(c => c.Active == true || c.Active == false);
                    }
                    else if (Status == 1)
                    {
                        retData = _dbContext.AreaDatas.Where(c => c.Active == true);
                    }
                    else if (Status == 0)
                    {
                        retData = _dbContext.AreaDatas.Where(c => c.Active == false);
                    }
                }


                if (!string.IsNullOrEmpty(Area))
                {
                    retData = retData.Where(c => (c.AreaName ?? "").ToLower().Contains(Area.ToLower()));
                }

                objModel = retData.Select(c => new AreaDataViewModel()
                {
                    AreaId = c.AreaId,
                    AreaName = c.AreaName,

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

        public ResponseEntity<bool> DeleteArea(AreaDataViewModel objModel)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var areaDetails = _dbContext.AreaDatas.Find(objModel.AreaId);



                if (areaDetails.Active)
                {
                    areaDetails.Active = false;

                    _dbContext.Entry(areaDetails).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    retModel.returnMessage = "Deactivated";
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;

                }
                else
                {
                    areaDetails.Active = true;
                    _dbContext.Entry(areaDetails).State = EntityState.Modified;
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

        public async Task AssignMembersToAreaAsync(long areaId, List<long> memberIds)
        {

            try
            {
                // Fetch already assigned member IDs for this area
                var existingMemberIds = await _dbContext.AreaMembers
                    .Where(am => am.AreaId == areaId && memberIds.Contains(am.UserMemberId))
                    .Select(am => am.UserMemberId)
                    .ToListAsync();

                // Get only new member IDs that are not already assigned
                var newMemberIds = memberIds.Except(existingMemberIds).ToList();

                // Create AreaMember entries for new members
                var newAreaMembers = newMemberIds.Select(memberId => new AreaMember
                {
                    AreaId = areaId,
                    UserMemberId = memberId,
                    Active = true
                });

                // Add and save only if there's anything new to insert
                if (newAreaMembers.Any())
                {
                    await _dbContext.AreaMembers.AddRangeAsync(newAreaMembers);
                    await _dbContext.SaveChangesAsync();
                }

            }


            catch (Exception ex)
            {



            }
        }

        public async Task<List<long>> GetAssignedMemberIdsAsync(long areaId)
        {
            return await _dbContext.AreaMembers
                .Where(am => am.AreaId == areaId)
                .Select(am => am.UserMemberId)
                .ToListAsync();
        }
        //public List<MembershipAccepted> GetMembersAssignedToArea(long areaId, string? keyword = null, string? column = null)
        //{
        //    var query = _dbContext.AreaMembers
        //        .Where(am => am.AreaId == areaId)
        //        .Join(
        //            _dbContext.Users,
        //            am => am.UserMemberId,
        //            ma => ma.UserId,
        //            (am, ma) => ma
        //        )
        //        .Distinct(); // Optional: Ensures distinct members


        //    if (!string.IsNullOrWhiteSpace(keyword) && !string.IsNullOrWhiteSpace(column))
        //    {
        //        string lowerKeyword = keyword.ToLower().Trim();

        //        switch (column.ToLower())
        //        {
        //            case "fullname":
        //                query = query.Where(m => m.Name != null && m.Name.ToLower().Contains(lowerKeyword));
        //                break;

        //            case "civilid":
        //                query = query.Where(m => m.CivilId != null && m.CivilId.ToLower().Contains(lowerKeyword));
        //                break;

        //            case "passport number":
        //                query = query.Where(m => m.PassportNo != null && m.PassportNo.ToLower().Contains(lowerKeyword));
        //                break;

        //            case "contact number":
        //                query = query.Where(m => m.ContactNo != null && m.ContactNo.ToString().Contains(lowerKeyword));
        //                break;

        //            default:
        //                // Optional: implement "all fields" fallback
        //                query = query.Where(m =>
        //                    (m.Name != null && m.Name.ToLower().Contains(lowerKeyword)) ||
        //                    (m.CivilId != null && m.CivilId.ToLower().Contains(lowerKeyword)) ||
        //                    (m.ReferanceNo != null && m.ReferanceNo.ToLower().Contains(lowerKeyword)) ||
        //                    (m.ContactNo != null && m.ContactNo.ToString().Contains(lowerKeyword))
        //                );
        //                break;
        //        }
        //    }

        //    return query.ToList();
        //}

        public List<UserViewModel> GetMembersAssignedToArea(long areaId, string? keyword = null, string? column = null)
        {
            var query = _dbContext.AreaMembers
                .Where(am => am.AreaId == areaId)
                .Join(
                    _dbContext.Users,
                    am => am.UserMemberId,
                    u => u.UserId,
                    (am, u) => u
                )
                .Distinct(); // Optional: ensures no duplicates

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
        public async Task<bool> UpdateAssignedUsers(long areaId, List<long>? assignedUserIds, long updatedBy)
        {
            try
            {
                // Remove existing members
                var existing = _dbContext.AreaMembers.Where(x => x.AreaId == areaId);
                _dbContext.AreaMembers.RemoveRange(existing);

                // Add new members
                if (assignedUserIds != null && assignedUserIds.Any())
                {
                    var newMembers = assignedUserIds.Select(userId => new AreaMember
                    {
                        AreaId = areaId,
                        UserMemberId = userId,
                        CreatedBy = updatedBy,
                        CreatedDate = DateTime.UtcNow,
                        Active = true,
                        UpdatedBy = updatedBy,
                        UpdatedDate = DateTime.UtcNow
                    }).ToList();

                    await _dbContext.AreaMembers.AddRangeAsync(newMembers);
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
