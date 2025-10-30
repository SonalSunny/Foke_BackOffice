using ClosedXML.Excel;
using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Entity.RoleData.DTO;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class RoleRepository : IRoleRepository
    {

        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;
        public RoleRepository(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor)
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

        public async Task<ResponseEntity<RoleViewModel>> SaveRole(RoleViewModel model)
        {
            var retModel = new ResponseEntity<RoleViewModel>();

            try
            {

                var roleExists = _dbContext.Roles
                       .Any(u => u.RoleName == model.RoleName);
                if (roleExists)
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                    retModel.returnMessage = "Role Name Already Exists";
                }
                else
                {


                    var role = new Role
                    {
                        RoleName = model.RoleName,
                        RoleDescription = model.RoleDescription,
                        Active = true,
                        CreatedBy = model.loggedinUserId

                    };
                    await _dbContext.Roles.AddAsync(role);
                    await _dbContext.SaveChangesAsync();
                    model.RoleId = role.RoleId;
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Saved Successfully";
                    retModel.returnData = model;

                }
            }
            catch (Exception ex)
            {


            }
            return retModel;
        }


        public ResponseEntity<bool> DeleteRole(RoleViewModel objModel)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var role = _dbContext.Roles.Find(objModel.RoleId);

                if (role.Active)
                {
                    role.Active = false;

                    _dbContext.Entry(role).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    retModel.returnMessage = "Deactivated";
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;

                }
                else
                {
                    role.Active = true;
                    _dbContext.Entry(role).State = EntityState.Modified;
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

        public ResponseEntity<RoleViewModel> GetRolebyId(long roleId)
        {
            var retModel = new ResponseEntity<RoleViewModel>();
            try
            {
                var objRole = _dbContext.Roles
                     .SingleOrDefault(u => u.RoleId == roleId);
                var objModel = new RoleViewModel();
                objModel.RoleId = objRole.RoleId;
                objModel.RoleName = objRole.RoleName;
                objModel.RoleDescription = objRole.RoleDescription;


                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel;

            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }



        public async Task<ResponseEntity<RoleViewModel>> UpdateRole(RoleViewModel model)
        {
            var retModel = new ResponseEntity<RoleViewModel>();

            try
            {
                // Find the role by RoleId
                var role = await _dbContext.Roles
                    .Where(r => r.RoleId == model.RoleId && r.Active) // Ensure the role is active
                    .SingleOrDefaultAsync();

                if (role != null)
                {
                    // Check if the new RoleName already exists for another role
                    var roleExists = await _dbContext.Roles
                        .AnyAsync(r => r.RoleId != model.RoleId && r.RoleName == model.RoleName && r.Active);

                    if (roleExists)
                    {
                        retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                        retModel.returnMessage = "Role already exists with the same name";
                        return retModel; // Early return in case of an existing role
                    }

                    // Update the role details
                    role.RoleName = model.RoleName;
                    role.RoleDescription = model.RoleDescription;


                    role.UpdatedBy = model.loggedinUserId;
                    role.UpdatedDate = DateTime.UtcNow;

                    // Save changes asynchronously
                    await _dbContext.SaveChangesAsync();

                    // Return success
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Role updated successfully";
                    retModel.returnData = model; // Optionally return the updated model
                }
                else
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Role not found or inactive";
                }
            }
            catch (Exception ex)
            {
                // Log the exception (assuming you have a logger in place)
                //  _logger.LogError(ex, "An error occurred while updating the role");

                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "An internal server error occurred";
            }

            return retModel;
        }


        public ResponseEntity<List<RoleViewModel>> GetAllRole(long? Status, string roleName)
        {
            var retModel = new ResponseEntity<List<RoleViewModel>>();
            var objModel = new List<RoleViewModel>();
            try
            {
                // Start with the base query
                var role = _dbContext.Roles.AsQueryable();

                // Apply the "Active" status filter
                if (Status.HasValue)
                {
                    if (Status == 1)
                    {
                        role = role.Where(c => c.Active == true); // Only active 
                    }
                    else if (Status == 0)
                    {
                        role = role.Where(c => c.Active == false); // Only inactive 
                    }
                    else if (Status == 2)
                    {
                        // Status == 2 means both active and inactive, no need to filter based on "Active"
                    }
                }
                else
                {

                    role = role.Where(c => c.Active == true);
                }


                if (!string.IsNullOrEmpty(roleName))
                {
                    role = role.Where(c => c.RoleName.Contains(roleName));
                }

                // Execute the query and map results to the view model
                objModel = role.Select(c => new RoleViewModel()
                {
                    RoleId = c.RoleId,
                    RoleName = c.RoleName,
                    RoleDescription = c.RoleDescription.Length > 75 ? c.RoleDescription.Substring(0, 75) + " See more..." : c.RoleDescription,
                    Active = c.Active,
                    CreatedUsername = _dbContext.Users.FirstOrDefault(e => e.UserId == c.CreatedBy).UserName,
                }).ToList();

                // Set the response status and data
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel;
            }
            catch (Exception ex)
            {
                // Optionally log the exception for debugging
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;


        }
        public ResponseEntity<string> ExportRoleDatatoExcel(long? Status, string search)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                var objData = GetAllRole(null, null);

                if (objData.transactionStatus == HttpStatusCode.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        // Set header row
                        var worksheet = workbook.Worksheets.Add("Role Master");
                        worksheet.Cell(1, 1).Value = "Sl No";
                        worksheet.Cell(1, 2).Value = "Role Name";
                        worksheet.Cell(1, 3).Value = "Description";
                        worksheet.Cell(1, 4).Value = "Status";
                        worksheet.Cell(1, 5).Value = "Createdy";

                        // worksheet.Cell(1, 5).Value = "Created Date";

                        // ... Add more headers if needed

                        // Format header row
                        var headerRow = worksheet.Row(1);
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
                        // ... Apply additional formatting as needed

                        for (int i = 0; i < objData.returnData.Count; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = i + 1; // Serial Number
                            worksheet.Cell(i + 2, 2).Value = objData.returnData[i].RoleName;
                            worksheet.Cell(i + 2, 3).Value = objData.returnData[i].RoleDescription;

                            if (objData.returnData[i].Active)
                            {
                                worksheet.Cell(i + 2, 4).Value = "Active";
                            }
                            else
                            {
                                worksheet.Cell(i + 2, 4).Value = "InActive";
                            }
                            worksheet.Cell(i + 2, 5).Value = objData.returnData[i].CreatedUsername;


                            // ... Add more data cells if needed
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            stream.Position = 0;
                            byte[] fileBytes = stream.ToArray();
                            retModel.returnData = GenericUtilities.SetReportData(fileBytes, ".xlsx");
                            retModel.transactionStatus = HttpStatusCode.OK;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
            }
            return retModel;
        }
    }
}
