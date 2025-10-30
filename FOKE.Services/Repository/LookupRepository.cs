using ClosedXML.Excel;
using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.Identity.DTO;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class LookupRepository : ILookupRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;

        public LookupRepository(FOKEDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this._dbContext = dbContext;

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
        public ResponseEntity<List<LookupViewModel>> GetAllLookups(long? Status, long? LookupTypeId, string? LookUpName)
        {
            var retModel = new ResponseEntity<List<LookupViewModel>>();
            try
            {
                var objModel = new List<LookupViewModel>();

                var retData = _dbContext.LookupMasters.Where(c => c.Active == true);

                if (LookupTypeId.GetValueOrDefault() != 0)
                {
                    retData = retData.Where(c => c.LookUpTypeId == LookupTypeId);
                }
                if (Status.HasValue)
                {
                    if (Status == 2)
                    {
                        retData = _dbContext.LookupMasters.Where(c => c.Active == true || c.Active == false);
                    }
                    else if (Status == 1)
                    {
                        retData = _dbContext.LookupMasters.Where(c => c.Active == true);
                    }
                    else if (Status == 0)
                    {
                        retData = _dbContext.LookupMasters.Where(c => c.Active == false);
                    }
                }
                if (!string.IsNullOrEmpty(LookUpName))
                {
                    retData = retData.Where(c => (c.LookUpName ?? "").ToLower().Contains(LookUpName.ToLower()));
                }
                objModel = retData.Select(c => new LookupViewModel()
                {
                    LookUpId = c.LookUpId,
                    LookUpName = c.LookUpName,
                    LookUpTypeId = c.LookUpTypeId,
                    LookUpTypeName = _dbContext.LookupTypeMasters
                        .Where(r => r.LookUpTypeId == c.LookUpTypeId)
                        .Select(r => r.LookUpTypeName)
                        .FirstOrDefault(),
                    Description = c.Description.Length > 75 ? c.Description.Substring(0, 75) + " See more..." : c.Description,
                    Active = c.Active,
                    CreatedUsername = _dbContext.Users.FirstOrDefault(e => e.UserId == c.CreatedBy).UserName,
                    CreatedDate = c.CreatedDate
                }).ToList();

                objModel = objModel.OrderByDescending(i => i.CreatedDate).ToList();
                // Set response
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message; // Include error message for debugging
            }

            return retModel;
        }

        public ResponseEntity<bool> DeleteTeam(LookupViewModel objModel)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var memberDetails = _dbContext.LookupMasters.Find(objModel.LookUpId);
                if (objModel.DiffId == 1)
                {
                    memberDetails.Active = false;
                    _dbContext.Entry(memberDetails).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    retModel.returnMessage = "Deactivated";
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    memberDetails.Active = true;
                    _dbContext.Entry(memberDetails).State = EntityState.Modified;
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
        public ResponseEntity<LookupViewModel> GetLookUpbyId(long LookUpId)
        {
            var retModel = new ResponseEntity<LookupViewModel>();
            try
            {
                var objUser = _dbContext.LookupMasters
                     .SingleOrDefault(u => u.LookUpId == LookUpId);
                var objModel = new LookupViewModel();
                objModel.LookUpId = objUser.LookUpId;
                objModel.LookUpName = objUser.LookUpName;
                objModel.LookUpTypeId = objUser.LookUpTypeId;
                objModel.LookUpTypeName = _dbContext.LookupTypeMasters
                       .Where(e => e.LookUpTypeId == (long)objUser.LookUpTypeId)
                       .Select(e => e.LookUpTypeName)
                       .FirstOrDefault();
                objModel.Description = objUser.Description;
                objModel.Active = objUser.Active;
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel;

            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }
        public ResponseEntity<string> ExportTeamDatatoExcel(string search, long? statusid, long? LookupTypeId, string? LookUpName)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                var objData = GetAllLookups(statusid, LookupTypeId, LookUpName);

                if (objData.transactionStatus == HttpStatusCode.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("LookUp Master");
                        worksheet.Cell(1, 1).Value = "Sl No";
                        worksheet.Cell(1, 2).Value = "LookUp Type Name";
                        worksheet.Cell(1, 3).Value = "LookUp Name";
                        worksheet.Cell(1, 4).Value = "Description";
                        worksheet.Cell(1, 5).Value = "CreatedBy";

                        var headerRow = worksheet.Row(1);
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                        for (int i = 0; i < objData.returnData.Count; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = i + 1;
                            worksheet.Cell(i + 2, 2).Value = objData.returnData[i].LookUpTypeName;
                            worksheet.Cell(i + 2, 3).Value = objData.returnData[i].LookUpName;
                            worksheet.Cell(i + 2, 4).Value = objData.returnData[i].Description;
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
        public Task<ResponseEntity<LookupViewModel>> AddLookUp(LookupViewModel objModel)
        {
            var retModel = new ResponseEntity<LookupViewModel>();
            try
            {
                if (_dbContext.LookupMasters.Any(c => c.LookUpId != objModel.LookUpId
                && c.LookUpTypeId == objModel.LookUpTypeId
                && c.LookUpName == objModel.LookUpName))
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Name Already Exists";
                    return Task.FromResult(retModel);
                }

                var modelData = _dbContext.LookupMasters.Find(objModel.LookUpId);
                if (modelData != null)
                {
                    modelData.LookUpName = objModel.LookUpName;
                    modelData.LookUpTypeId = objModel.LookUpTypeId;
                    modelData.Description = objModel.Description;
                    modelData.UpdatedBy = loggedInUser;
                }
                else
                {
                    modelData = new LookupMaster();
                    modelData.LookUpName = objModel.LookUpName;
                    modelData.LookUpTypeId = objModel.LookUpTypeId;
                    modelData.Description = objModel.Description;
                    modelData.Active = true;
                    modelData.CreatedBy = loggedInUser;
                    _dbContext.LookupMasters.Add(modelData);
                }
                _dbContext.SaveChanges();
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnMessage = "Saved Successfully";
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error Occured";
            }
            return Task.FromResult(retModel);
        }
    }
}
