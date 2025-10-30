using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Entity.UnitData.ViewModel;
using FOKE.Entity.UnitMember;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class UnitRepository : IUnitRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;
        public UnitRepository(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor)
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



        public async Task<ResponseEntity<UnitViewModel>> AddUnit(UnitViewModel model)
        {
            var retModel = new ResponseEntity<UnitViewModel>();

            try
            {

                var UnitExists = _dbContext.Units
                       .Any(u => u.UnitName == model.UnitName);
                if (UnitExists)
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                    retModel.returnMessage = "Unit Already Exists";
                }
                else
                {


                    var unit = new Entity.UnitData.DTO.Unit
                    {
                        UnitName = model.UnitName,
                        Description = model.Description,
                        Active = true,
                        CreatedBy = model.loggedinUserId

                    };
                    await _dbContext.Units.AddAsync(unit);
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


        public async Task<ResponseEntity<UnitViewModel>> UpdateUnit(UnitViewModel model)
        {
            var retModel = new ResponseEntity<UnitViewModel>();

            try
            {
                var unit = await _dbContext.Units
                    .Where(r => r.UnitId == model.UnitId && r.Active)
                    .SingleOrDefaultAsync();

                if (unit != null)
                {

                    var unitExists = await _dbContext.Units
                        .AnyAsync(r => r.UnitId != model.UnitId && r.UnitName == model.UnitName && r.Active);

                    if (unitExists)
                    {
                        retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                        retModel.returnMessage = "Unit already exists with the same name";
                        return retModel;
                    }

                    unit.UnitName = model.UnitName;
                    unit.Description = model.Description;
                    unit.UpdatedBy = model.loggedinUserId;
                    unit.UpdatedDate = DateTime.UtcNow;

                    await _dbContext.SaveChangesAsync();

                    // Return success
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Unit updated successfully";
                    retModel.returnData = model;
                }
                else
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Unit not found or inactive";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "An internal server error occurred";
            }

            return retModel;
        }




        public ResponseEntity<UnitViewModel> GetUnitbyId(long UnitId)
        {
            var retModel = new ResponseEntity<UnitViewModel>();
            try
            {
                var objRole = _dbContext.Units
                     .SingleOrDefault(u => u.UnitId == UnitId);
                var objModel = new UnitViewModel();
                objModel.UnitId = objRole.UnitId;
                objModel.UnitName = objRole.UnitName;
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

        public ResponseEntity<List<UnitViewModel>> GetAllUnits(long? Status, string? unit)
        {
            var retModel = new ResponseEntity<List<UnitViewModel>>();
            try
            {
                var objModel = new List<UnitViewModel>();

                IQueryable<Entity.UnitData.DTO.Unit> retData = _dbContext.Units.Where(c => c.Active == true);
                if (Status.HasValue)
                {
                    if (Status == 2)
                    {
                        retData = _dbContext.Units.Where(c => c.Active == true || c.Active == false);
                    }
                    else if (Status == 1)
                    {
                        retData = _dbContext.Units.Where(c => c.Active == true);
                    }
                    else if (Status == 0)
                    {
                        retData = _dbContext.Units.Where(c => c.Active == false);
                    }
                }


                if (!string.IsNullOrEmpty(unit))
                {
                    retData = retData.Where(c => (c.UnitName ?? "").ToLower().Contains(unit.ToLower()));
                }

                objModel = retData.Select(c => new UnitViewModel()
                {
                    UnitId = c.UnitId,
                    UnitName = c.UnitName,

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

        public ResponseEntity<bool> DeleteUnit(UnitViewModel objModel)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var UnitDetails = _dbContext.Units.Find(objModel.UnitId);



                if (UnitDetails.Active)
                {
                    UnitDetails.Active = false;

                    _dbContext.Entry(UnitDetails).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    retModel.returnMessage = "Deactivated";
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;

                }
                else
                {
                    UnitDetails.Active = true;
                    _dbContext.Entry(UnitDetails).State = EntityState.Modified;
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

        public async Task AssignMembersToUnitAsync(long unitId, List<long> memberIds)
        {

            try
            {
                // Fetch already assigned member IDs for this area
                var existingMemberIds = await _dbContext.UnitMembers
                    .Where(am => am.UnitId == unitId && memberIds.Contains(am.UserMemberId))
                    .Select(am => am.UserMemberId)
                    .ToListAsync();

                // Get only new member IDs that are not already assigned
                var newMemberIds = memberIds.Except(existingMemberIds).ToList();

                // Create AreaMember entries for new members
                var newUnitMembers = newMemberIds.Select(memberId => new UnitMember
                {
                    UnitId = unitId,
                    UserMemberId = memberId,
                    Active = true
                });

                // Add and save only if there's anything new to insert
                if (newUnitMembers.Any())
                {
                    await _dbContext.UnitMembers.AddRangeAsync(newUnitMembers);
                    await _dbContext.SaveChangesAsync();
                }

            }


            catch (Exception ex)
            {



            }
        }

        public async Task<List<long>> GetAssignedMemberIdsAsync(long unitId)
        {
            return await _dbContext.UnitMembers
                .Where(am => am.UnitId == unitId)
                .Select(am => am.UserMemberId)
                .ToListAsync();
        }
        public List<UserViewModel> GetMembersAssignedToUnit(long unitId, string? keyword = null, string? column = null)
        {
            var query = _dbContext.UnitMembers
                .Where(am => am.UnitId == unitId)
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
        public async Task<bool> UpdateAssignedUsers(long unitId, List<long>? assignedUserIds, long updatedBy)
        {
            try
            {
                // Remove existing members
                var existing = _dbContext.UnitMembers.Where(x => x.UnitId == unitId);
                _dbContext.UnitMembers.RemoveRange(existing);

                // Add new members
                if (assignedUserIds != null && assignedUserIds.Any())
                {
                    var newMembers = assignedUserIds.Select(userId => new UnitMember
                    {
                        UnitId = unitId,
                        UserMemberId = userId,
                        CreatedBy = updatedBy,
                        CreatedDate = DateTime.UtcNow,
                        Active = true,
                        UpdatedBy = updatedBy,
                        UpdatedDate = DateTime.UtcNow
                    }).ToList();

                    await _dbContext.UnitMembers.AddRangeAsync(newMembers);
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
