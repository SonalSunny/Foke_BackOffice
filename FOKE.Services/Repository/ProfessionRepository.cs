using ClosedXML.Excel;
using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.ProfessionData.DTO;
using FOKE.Entity.ProfessionData.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class ProfessionRepository : IProfessionRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;
        public ProfessionRepository(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor)
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

        public async Task<ResponseEntity<ProfessionViewModel>> SaveProfession(ProfessionViewModel model)
        {
            var retModel = new ResponseEntity<ProfessionViewModel>();

            try
            {

                var roleExists = _dbContext.Professions
                       .Any(u => u.ProffessionName == model.ProfessionName);
                if (roleExists)
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                    retModel.returnMessage = "Profession Already Exists";
                }
                else
                {
                    var profession = new Profession
                    {
                        ProffessionName = model.ProfessionName,
                        Description = model.Description,
                        Active = true,
                        CreatedBy = model.loggedinUserId

                    };
                    await _dbContext.Professions.AddAsync(profession);
                    await _dbContext.SaveChangesAsync();
                    model.ProfessionId = profession.ProfessionId;
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

        public ResponseEntity<bool> DeleteProfession(ProfessionViewModel objModel)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var role = _dbContext.Professions.Find(objModel.ProfessionId);

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

        public ResponseEntity<ProfessionViewModel> GetProfessionbyId(long profId)
        {
            var retModel = new ResponseEntity<ProfessionViewModel>();
            try
            {
                var objRole = _dbContext.Professions
                     .SingleOrDefault(u => u.ProfessionId == profId);
                var objModel = new ProfessionViewModel();
                objModel.ProfessionId = objRole.ProfessionId;
                objModel.ProfessionName = objRole.ProffessionName;
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

        public async Task<ResponseEntity<ProfessionViewModel>> UpdateProfession(ProfessionViewModel model)
        {
            var retModel = new ResponseEntity<ProfessionViewModel>();

            try
            {
                var profession = await _dbContext.Professions
                    .Where(r => r.ProfessionId == model.ProfessionId && r.Active)
                    .SingleOrDefaultAsync();

                if (profession != null)
                {
                    var professionExists = await _dbContext.Professions
                        .AnyAsync(r => r.ProfessionId != model.ProfessionId && r.ProffessionName == model.ProfessionName && r.Active);

                    if (professionExists)
                    {
                        retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                        retModel.returnMessage = "Profession already exists with the same name";
                        return retModel;
                    }

                    profession.ProffessionName = model.ProfessionName;
                    profession.Description = model.Description;
                    profession.UpdatedBy = model.loggedinUserId;
                    profession.UpdatedDate = DateTime.UtcNow;

                    await _dbContext.SaveChangesAsync();

                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Profession updated successfully";
                    retModel.returnData = model;
                }
                else
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Profession not found or inactive";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "An internal server error occurred";
            }

            return retModel;
        }

        public ResponseEntity<List<ProfessionViewModel>> GetAllProfessions(long? Status, string professionname)
        {
            var retModel = new ResponseEntity<List<ProfessionViewModel>>();
            var objModel = new List<ProfessionViewModel>();
            try
            {
                var prof = _dbContext.Professions.AsQueryable();

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


                if (!string.IsNullOrEmpty(professionname))
                {
                    prof = prof.Where(c => c.ProffessionName.Contains(professionname));
                }

                objModel = prof.Select(c => new ProfessionViewModel()
                {
                    ProfessionId = c.ProfessionId,
                    ProfessionName = c.ProffessionName,
                    Description = c.Description.Length > 75 ? c.Description.Substring(0, 75) + " See more..." : c.Description,
                    Active = c.Active,
                    CreatedUsername = _dbContext.Users.FirstOrDefault(e => e.UserId == c.CreatedBy).UserName,
                }).ToList();

                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }

        public ResponseEntity<string> ExportProfessionToExcel(long? Status, string search)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                var objData = GetAllProfessions(null, null);

                if (objData.transactionStatus == HttpStatusCode.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Profession Master");
                        worksheet.Cell(1, 1).Value = "Sl No";
                        worksheet.Cell(1, 2).Value = "Profession Name";
                        worksheet.Cell(1, 3).Value = "Description";
                        worksheet.Cell(1, 4).Value = "Status";
                        worksheet.Cell(1, 5).Value = "Created By";

                        var headerRow = worksheet.Row(1);
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                        for (int i = 0; i < objData.returnData.Count; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = i + 1;
                            worksheet.Cell(i + 2, 2).Value = objData.returnData[i].ProfessionName;
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
