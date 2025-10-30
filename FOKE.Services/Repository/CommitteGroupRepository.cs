using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.CommitteeManagement.DTO;
using FOKE.Entity.CommitteeManagement.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class CommitteGroupRepository : ICommitteGroupRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;

        public CommitteGroupRepository(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor)
        {
            this._dbContext = FOKEDBContext;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<ResponseEntity<CommitteGroupViewModel>> AddCommitteeGroup(CommitteGroupViewModel model)
        {
            var retModel = new ResponseEntity<CommitteGroupViewModel>();

            try
            {
                bool groupExists = await _dbContext.CommitteeGroups
                    .AnyAsync(u => u.GroupName == model.GroupName && u.CommitteeId == model.CommitteeId && u.Active);

                if (groupExists)
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Group already exists under the selected committee.";
                    return retModel;
                }

                var newGroup = new Committegroup
                {
                    GroupName = model.GroupName,
                    CommitteeId = model.CommitteeId,
                    SortOrder = model.SortOrder,
                    Active = true,
                    CreatedBy = loggedInUser,
                    CreatedDate = DateTime.UtcNow
                };

                await _dbContext.CommitteeGroups.AddAsync(newGroup);
                await _dbContext.SaveChangesAsync();

                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnMessage = "Group saved successfully.";
                retModel.returnData = model;
            }
            catch (Exception ex)
            {
                // Log ex if you have a logger
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "A server error occurred.";
            }

            return retModel;
        }

        public async Task<ResponseEntity<CommitteGroupViewModel>> UpdateCommitteeGroup(CommitteGroupViewModel model)
        {
            var retModel = new ResponseEntity<CommitteGroupViewModel>();

            try
            {
                var group = await _dbContext.CommitteeGroups
                    .FirstOrDefaultAsync(g => g.GroupId == model.GroupId);

                if (group == null)
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Group not found.";
                    return retModel;
                }

                bool exists = await _dbContext.CommitteeGroups
                    .AnyAsync(g => g.GroupId != model.GroupId &&
                                   g.GroupName == model.GroupName &&
                                   g.CommitteeId == model.CommitteeId &&
                                   g.Active);

                if (exists)
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Another group with the same name exists in this committee.";
                    return retModel;
                }

                // Update fields
                group.GroupName = model.GroupName;
                group.SortOrder = model.SortOrder;
                group.CommitteeId = model.CommitteeId;
                group.UpdatedBy = loggedInUser;
                group.UpdatedDate = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();

                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnMessage = "Group updated successfully.";
                retModel.returnData = model;
            }
            catch (Exception)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "An internal server error occurred.";
            }

            return retModel;
        }

        public ResponseEntity<CommitteGroupViewModel> GetCommitteeGroupById(long groupId)
        {
            var retModel = new ResponseEntity<CommitteGroupViewModel>();

            try
            {
                var group = _dbContext.CommitteeGroups
                .Include(g => g.Committee)
                     .FirstOrDefault(g => g.GroupId == groupId);


                if (group == null)
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.NotFound;
                    retModel.returnMessage = "Group not found.";
                    return retModel;
                }

                var model = new CommitteGroupViewModel
                {
                    GroupId = group.GroupId,
                    GroupName = group.GroupName,
                    SortOrder = group.SortOrder,
                    CommitteeId = group.CommitteeId ?? 0,
                    CommitteeName = group.Committee?.CommitteeName ?? "N/A",
                    Active = group.Active
                };

                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = model;
            }
            catch (Exception)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Error retrieving group data.";
            }

            return retModel;
        }

        public ResponseEntity<List<CommitteGroupViewModel>> GetAllCommitteeGroups(long? status)
        {
            var retModel = new ResponseEntity<List<CommitteGroupViewModel>>();

            try
            {
                var query = _dbContext.CommitteeGroups.Include(g => g.Committee).AsQueryable();
                if (status.HasValue)
                {
                    switch (status.Value)
                    {
                        case 0:
                            query = query.Where(g => g.Active == false);
                            break;
                        case 1:
                            query = query.Where(g => g.Active == true);
                            break;
                        case 2:
                            // All groups (active + inactive)
                            break;
                    }
                }

                var result = query
                    .OrderBy(g => g.SortOrder)
                    .ThenBy(g => g.GroupName)
                    .Select(g => new CommitteGroupViewModel
                    {
                        GroupId = g.GroupId,
                        GroupName = g.GroupName,
                        SortOrder = g.SortOrder,
                        CommitteeId = g.CommitteeId ?? 0,
                        CommitteeName = g.Committee.CommitteeName,

                        Active = g.Active
                    })
                    .ToList();

                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = result;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message;
            }

            return retModel;
        }

        public ResponseEntity<bool> DeleteGroup(CommitteGroupViewModel objModel)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var Details = _dbContext.CommitteeGroups.Find(objModel.GroupId);

                if (Details.Active)
                {
                    Details.Active = false;

                    _dbContext.Entry(Details).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    retModel.returnMessage = "Deactivated";
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;

                }
                else
                {
                    Details.Active = true;
                    _dbContext.Entry(Details).State = EntityState.Modified;
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

    }
}









