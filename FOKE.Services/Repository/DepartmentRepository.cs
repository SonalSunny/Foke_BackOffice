using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.DepartmentMaster.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;
        public DepartmentRepository(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor)
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

        public async Task<ResponseEntity<DepartmentViewModel>> AddDepartment(DepartmentViewModel model)
        {
            var retModel = new ResponseEntity<DepartmentViewModel>();

            try
            {

                var DeptExists = _dbContext.Departments
                       .Any(u => u.DepartmentName == model.DepartmentName);
                if (DeptExists)
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                    retModel.returnMessage = "Department Already Exists";
                }
                else
                {


                    var dept = new Entity.DepartmentMaster.DTO.Department
                    {
                        DepartmentName = model.DepartmentName,
                        Description = model.Description,
                        Active = true,
                        CreatedBy = model.loggedinUserId

                    };
                    await _dbContext.Departments.AddAsync(dept);
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

        public async Task<ResponseEntity<DepartmentViewModel>> UpdateDepartment(DepartmentViewModel model)
        {
            var retModel = new ResponseEntity<DepartmentViewModel>();

            try
            {
                var dept = await _dbContext.Departments
                    .Where(r => r.DepartmentId == model.DepartmentId && r.Active)
                    .SingleOrDefaultAsync();

                if (dept != null)
                {

                    var deptExists = await _dbContext.Departments
                        .AnyAsync(r => r.DepartmentId != model.DepartmentId && r.DepartmentName == model.DepartmentName && r.Active);

                    if (deptExists)
                    {
                        retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                        retModel.returnMessage = "Department already exists";
                        return retModel;
                    }

                    dept.DepartmentName = model.DepartmentName;
                    dept.Description = model.Description;
                    dept.UpdatedBy = model.loggedinUserId;
                    dept.UpdatedDate = DateTime.UtcNow;

                    await _dbContext.SaveChangesAsync();

                    // Return success
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Department updated successfully";
                    retModel.returnData = model;
                }
                else
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Department not found or inactive";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "An internal server error occurred";
            }

            return retModel;
        }

        public ResponseEntity<DepartmentViewModel> GetDepartmentbyId(long deptId)
        {
            var retModel = new ResponseEntity<DepartmentViewModel>();
            try
            {
                var objRole = _dbContext.Departments
                     .SingleOrDefault(u => u.DepartmentId == deptId);
                var objModel = new DepartmentViewModel();
                objModel.DepartmentId = objRole.DepartmentId;
                objModel.DepartmentName = objRole.DepartmentName;
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

        public ResponseEntity<List<DepartmentViewModel>> GetAllDepartments(long? Status, string? Dept)
        {
            var retModel = new ResponseEntity<List<DepartmentViewModel>>();
            try
            {
                var objModel = new List<DepartmentViewModel>();

                IQueryable<Entity.DepartmentMaster.DTO.Department> retData = _dbContext.Departments.Where(c => c.Active == true);
                if (Status.HasValue)
                {
                    if (Status == 2)
                    {
                        retData = _dbContext.Departments.Where(c => c.Active == true || c.Active == false);
                    }
                    else if (Status == 1)
                    {
                        retData = _dbContext.Departments.Where(c => c.Active == true);
                    }
                    else if (Status == 0)
                    {
                        retData = _dbContext.Departments.Where(c => c.Active == false);
                    }
                }


                if (!string.IsNullOrEmpty(Dept))
                {
                    retData = retData.Where(c => (c.DepartmentName ?? "").ToLower().Contains(Dept.ToLower()));
                }

                objModel = retData.Select(c => new DepartmentViewModel()
                {
                    DepartmentId = c.DepartmentId,
                    DepartmentName = c.DepartmentName,

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

        public ResponseEntity<bool> DeleteDepartment(DepartmentViewModel objModel)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var deptDetails = _dbContext.Departments.Find(objModel.DepartmentId);



                if (deptDetails.Active)
                {
                    deptDetails.Active = false;

                    _dbContext.Entry(deptDetails).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    retModel.returnMessage = "Deactivated";
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;

                }
                else
                {
                    deptDetails.Active = true;
                    _dbContext.Entry(deptDetails).State = EntityState.Modified;
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
