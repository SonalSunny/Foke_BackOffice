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
    public class CommitteeRepository : ICommitteeRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;

        public CommitteeRepository(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor)
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

        public async Task<ResponseEntity<CommitteViewModel>> AddCommittee(CommitteViewModel model)
        {
            var retModel = new ResponseEntity<CommitteViewModel>();

            try
            {
                var CommitteeExists = _dbContext.Committees
                       .Any(u => u.CommitteeName == model.CommitteeName);
                if (CommitteeExists)
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                    retModel.returnMessage = "Committee Already Exists";
                }
                else
                {


                    await _dbContext.SaveChangesAsync();
                    var Committe = new Committee
                    {
                        CommitteeName = model.CommitteeName,
                        FromDate = model.FromDate,
                        ToDate = model.ToDate,
                        SortOrder = model.SortOrder,
                        Active = true,
                        CreatedBy = loggedInUser,
                        CreatedDate = DateTime.UtcNow,


                    };
                    await _dbContext.Committees.AddAsync(Committe);
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

        public async Task<ResponseEntity<CommitteViewModel>> UpdateCommittee(CommitteViewModel model)
        {
            var retModel = new ResponseEntity<CommitteViewModel>();
            try
            {

                var Committee = await _dbContext.Committees
                .FirstOrDefaultAsync(r => r.CommitteeId == model.CommitteeId);
                if (Committee != null)
                {

                    var CommitteeExists = await _dbContext.Committees
                        .AnyAsync(r => r.CommitteeId != model.CommitteeId && r.CommitteeName == model.CommitteeName && r.Active);

                    if (CommitteeExists)
                    {
                        retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                        retModel.returnMessage = "Committee already exists with the same name";
                        return retModel;
                    }

                    Committee.CommitteeName = model.CommitteeName;
                    Committee.FromDate = model.FromDate;
                    Committee.ToDate = model.ToDate;
                    Committee.SortOrder = model.SortOrder;

                    Committee.UpdatedDate = DateTime.UtcNow;
                    Committee.UpdatedBy = loggedInUser;
                    await _dbContext.SaveChangesAsync();
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Committee updated successfully";
                    retModel.returnData = model;
                }
                else
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Committee not found or inactive";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "An internal server error occurred";
            }

            return retModel;
        }

        public ResponseEntity<CommitteViewModel> GetCommitteeById(long CommitteeId)
        {
            var retModel = new ResponseEntity<CommitteViewModel>();
            try
            {
                var objRole = _dbContext.Committees
                     .SingleOrDefault(u => u.CommitteeId == CommitteeId);
                var objModel = new CommitteViewModel();
                objModel.CommitteeId = objRole.CommitteeId;
                objModel.CommitteeName = objRole.CommitteeName;
                objModel.FromDate = objRole.FromDate;
                objModel.ToDate = objRole.ToDate;
                objModel.SortOrder = objRole.SortOrder.HasValue ? (int)objRole.SortOrder.Value : 0;
                objModel.Active = objRole.Active;
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel;

            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }

        public ResponseEntity<List<CommitteViewModel>> GetAllCommittee(long? Status)
        {
            var retModel = new ResponseEntity<List<CommitteViewModel>>();
            try
            {
                var objModel = new List<CommitteViewModel>();

                IQueryable<Entity.CommitteeManagement.DTO.Committee> retData = _dbContext.Committees.Where(c => c.Active == true);
                if (Status.HasValue)
                {
                    if (Status == 2)
                    {
                        retData = _dbContext.Committees.Where(c => c.Active == true || c.Active == false);
                    }
                    else if (Status == 1)
                    {
                        retData = _dbContext.Committees.Where(c => c.Active == true);
                    }
                    else if (Status == 0)
                    {
                        retData = _dbContext.Committees.Where(c => c.Active == false);
                    }
                }
                var ordered = retData
    .OrderBy(c => c.SortOrder)                        // Primary sort by SortOrder (ascending)
    .ThenBy(c => c.CommitteeName);

                objModel = ordered
                    .Select(c => new CommitteViewModel
                    {
                        CommitteeId = c.CommitteeId,
                        CommitteeName = c.CommitteeName,
                        FromDate = c.FromDate,
                        ToDate = c.ToDate,
                        SortOrder = c.SortOrder ?? 0,
                        Active = c.Active
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

        public ResponseEntity<bool> DeleteCommittee(CommitteViewModel objModel)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var Details = _dbContext.Committees.Find(objModel.CommitteeId);



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
