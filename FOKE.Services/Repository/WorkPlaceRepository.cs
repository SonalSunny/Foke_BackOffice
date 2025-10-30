using ClosedXML.Excel;
using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.WorkPlaceData.DTO;
using FOKE.Entity.WorkPlaceData.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class WorkPlaceRepository : IWorkPlaceRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;
        public WorkPlaceRepository(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor)
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
            catch (Exception ex)
            {
                Console.WriteLine("The error: " + ex.Message);
            }
        }

        public async Task<ResponseEntity<WorkPlaceViewModel>> SaveWorkPlace(WorkPlaceViewModel model)
        {
            var retModel = new ResponseEntity<WorkPlaceViewModel>();

            try
            {
                var roleExists = _dbContext.WorkPlace
                       .Any(u => u.WorkPlaceName == model.WorkPlaceName);
                if (roleExists)
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                    retModel.returnMessage = "Workplace Already Exists";
                }
                else
                {
                    var workplace = new WorkPlace
                    {
                        WorkPlaceName = model.WorkPlaceName,
                        Description = model.Description,
                        Active = true,
                        CreatedBy = model.loggedinUserId

                    };
                    await _dbContext.WorkPlace.AddAsync(workplace);
                    await _dbContext.SaveChangesAsync();
                    model.WorkPlaceId = workplace.WorkPlaceId;
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Saved Successfully";
                    retModel.returnData = model;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The error: " + ex.Message);
            }
            return retModel;
        }

        public ResponseEntity<bool> DeleteWorkPlace(WorkPlaceViewModel objModel)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var role = _dbContext.WorkPlace.Find(objModel.WorkPlaceId);

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

        public ResponseEntity<WorkPlaceViewModel> GetWorkPlacebyId(long wpId)
        {
            var retModel = new ResponseEntity<WorkPlaceViewModel>();
            try
            {
                var objRole = _dbContext.WorkPlace.SingleOrDefault(u => u.WorkPlaceId == wpId);
                var objModel = new WorkPlaceViewModel();
                objModel.WorkPlaceId = objRole.WorkPlaceId;
                objModel.WorkPlaceName = objRole.WorkPlaceName;
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

        public async Task<ResponseEntity<WorkPlaceViewModel>> UpdateWorkPlace(WorkPlaceViewModel model)
        {
            var retModel = new ResponseEntity<WorkPlaceViewModel>();
            try
            {
                var workplace = await _dbContext.WorkPlace.Where(r => r.WorkPlaceId == model.WorkPlaceId && r.Active).SingleOrDefaultAsync();
                if (workplace != null)
                {
                    var workplaceExists = await _dbContext.WorkPlace.AnyAsync(r => r.WorkPlaceId != model.WorkPlaceId && r.WorkPlaceName == model.WorkPlaceName && r.Active);

                    if (workplaceExists)
                    {
                        retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                        retModel.returnMessage = "A workplace already exists with the same name";
                        return retModel;
                    }

                    workplace.WorkPlaceName = model.WorkPlaceName;
                    workplace.Description = model.Description;
                    workplace.UpdatedBy = model.loggedinUserId;
                    workplace.UpdatedDate = DateTime.UtcNow;

                    await _dbContext.SaveChangesAsync();

                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Workplace updated successfully";
                    retModel.returnData = model;
                }
                else
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Workplace not found or inactive";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "An internal server error occurred";
            }
            return retModel;
        }


        public ResponseEntity<List<WorkPlaceViewModel>> GetAllWorkPlace(long? Status, string workplacename)
        {
            var retModel = new ResponseEntity<List<WorkPlaceViewModel>>();
            var objModel = new List<WorkPlaceViewModel>();
            try
            {
                var prof = _dbContext.WorkPlace.AsQueryable();

                if (Status.HasValue)
                {
                    if (Status == 1)
                    {
                        prof = prof.Where(c => c.Active == true);
                    }
                    else if (Status == 0)
                    {
                        prof = prof.Where(c => c.Active == false);
                    }
                }
                else
                {
                    prof = prof.Where(c => c.Active == true);
                }

                if (!string.IsNullOrEmpty(workplacename))
                {
                    prof = prof.Where(c => c.WorkPlaceName.Contains(workplacename));
                }

                objModel = prof.Select(c => new WorkPlaceViewModel()
                {
                    WorkPlaceId = c.WorkPlaceId,
                    WorkPlaceName = c.WorkPlaceName,
                    Description = c.Description.Length > 75 ? c.Description.Substring(0, 75) + " See more..." : c.Description,
                    Active = c.Active,
                    CreatedUsername = _dbContext.Users.FirstOrDefault(e => e.UserId == c.CreatedBy).UserName,
                }).ToList();

                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel.OrderByDescending(i => i.WorkPlaceId).ToList();
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }

        public ResponseEntity<string> ExportWorkPlaceToExcel(long? Status, string search)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                var objData = GetAllWorkPlace(null, null);

                if (objData.transactionStatus == HttpStatusCode.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("WorkPlace Master");
                        worksheet.Cell(1, 1).Value = "Sl No";
                        worksheet.Cell(1, 2).Value = "WorkPlace Name";
                        worksheet.Cell(1, 3).Value = "Description";
                        worksheet.Cell(1, 4).Value = "Status";
                        worksheet.Cell(1, 5).Value = "Created By";

                        var headerRow = worksheet.Row(1);
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                        for (int i = 0; i < objData.returnData.Count; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = i + 1;
                            worksheet.Cell(i + 2, 2).Value = objData.returnData[i].WorkPlaceName;
                            worksheet.Cell(i + 2, 3).Value = objData.returnData[i].Description;

                            if (objData.returnData[i].Active)
                            {
                                worksheet.Cell(i + 2, 4).Value = "Active";
                            }
                            else
                            {
                                worksheet.Cell(i + 2, 4).Value = "InActive";
                            }
                            worksheet.Cell(i + 2, 5).Value = objData.returnData[i].CreatedUsername;
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
