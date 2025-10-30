using ClosedXML.Excel;
using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.FileUpload.ViewModel;
using FOKE.Entity.OperationManagement.DTO;
using FOKE.Entity.OperationManagement.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class FolderMasterRepository : IFolderMasterRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDropDownRepository _dropDownRepository;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;

        public FolderMasterRepository(FOKEDBContext skakErpDBContext, IHttpContextAccessor httpContextAccessor, IDropDownRepository dropDownRepository)
        {
            this._dbContext = skakErpDBContext;
            this._httpContextAccessor = httpContextAccessor;
            this._dropDownRepository = dropDownRepository; // Initialize the field

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

        public async Task<ResponseEntity<FolderViewModel>> CreateFolder(FolderViewModel model)
        {
            var retModel = new ResponseEntity<FolderViewModel>();

            try
            {

                var roleExists = _dbContext.FolderMasters
                       .Any(u => u.FolderName == model.FolderName);
                if (roleExists)
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                    retModel.returnMessage = "Folder name already exists";
                }
                else
                {


                    var folders = new FolderMaster
                    {
                        FolderName = model.FolderName,
                        Description = model.Description,
                        Active = true,
                        CreatedBy = model.loggedinUserId

                    };
                    await _dbContext.FolderMasters.AddAsync(folders);
                    await _dbContext.SaveChangesAsync();
                    model.FolderId = folders.FolderId;
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Folder created successfully";
                    retModel.returnData = model;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The error: " + ex.Message);
            }
            return retModel;
        }

        public async Task<ResponseEntity<FolderViewModel>> UpdateFolder(FolderViewModel model)
        {
            var retModel = new ResponseEntity<FolderViewModel>();

            try
            {
                var folders = await _dbContext.FolderMasters
                    .Where(r => r.FolderId == model.FolderId && r.Active)
                    .SingleOrDefaultAsync();

                if (folders != null)
                {
                    // Checking if the profession already exists
                    var folderExists = await _dbContext.FolderMasters
                        .AnyAsync(r => r.FolderId != model.FolderId && r.FolderName == model.FolderName && r.Active);

                    if (folderExists)
                    {
                        retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                        retModel.returnMessage = "A folder already exists with the same name";
                        return retModel;
                    }

                    folders.FolderName = model.FolderName;
                    folders.Description = model.Description;
                    folders.UpdatedBy = model.loggedinUserId;
                    folders.UpdatedDate = DateTime.UtcNow;

                    await _dbContext.SaveChangesAsync();

                    // Return success
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Folder updated successfully";
                    retModel.returnData = model;
                }
                else
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Folder not found or inactive";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "An internal server error occurred";
            }

            return retModel;
        }
        public ResponseEntity<FolderViewModel> GetFolderById(long FolderId)
        {
            var retModel = new ResponseEntity<FolderViewModel>();
            try
            {
                var objData = _dbContext.FolderMasters.SingleOrDefault(u => u.FolderId == FolderId);
                var objModel = new FolderViewModel();
                objModel.FolderId = objData.FolderId;
                objModel.FolderName = objData.FolderName;
                objModel.Description = objData.Description;
                objModel.Active = objData.Active;

                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel;

            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }
        public ResponseEntity<List<FolderViewModel>> GetAllFolders(long? Status, DateTime? FromDate, DateTime? ToDate)
        {
            var retModel = new ResponseEntity<List<FolderViewModel>>();
            try
            {
                var objModel = new List<FolderViewModel>();

                // Base query: Active users
                IQueryable<Entity.OperationManagement.DTO.FolderMaster> retData = _dbContext.FolderMasters.Where(c => c.Active == true);
                if (Status.HasValue)
                {
                    if (Status == 2)
                    {
                        // Include both Active = true and Active = false
                        retData = _dbContext.FolderMasters.Where(c => c.Active == true || c.Active == false);
                    }
                    else if (Status == 1)
                    {
                        // Status = 1 maps to Active = true
                        retData = _dbContext.FolderMasters.Where(c => c.Active == true);
                    }
                    else if (Status == 0)
                    {
                        // Status = 0 maps to Active = false
                        retData = _dbContext.FolderMasters.Where(c => c.Active == false);
                    }
                }

                if (FromDate.HasValue)
                {
                    retData = retData.Where(c => c.CreatedDate >= FromDate.Value);
                }

                if (ToDate.HasValue)
                {
                    retData = retData.Where(c => c.CreatedDate <= ToDate.Value);
                }

                // Map to ViewModel
                objModel = retData.Select(c => new FolderViewModel()
                {
                    FolderId = c.FolderId,
                    FolderName = c.FolderName,
                    Description = c.Description,
                    Active = c.Active,
                    CreatedDate = c.CreatedDate,
                    CreatedUsername = _dbContext.Users.FirstOrDefault(e => e.UserId == c.CreatedBy).UserName,
                    CreatedBy = _dbContext.Users
                        .Where(u => u.UserId == c.CreatedBy)
                        .Select(u => u.UserId)
                        .FirstOrDefault()
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
        public ResponseEntity<string> ExporttoExcel(string search, long? Statusid, DateTime? FromDate, DateTime? Todate)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                var objData = GetAllFolders(Statusid, FromDate, Todate);

                if (objData.transactionStatus == HttpStatusCode.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        // Set header row
                        var worksheet = workbook.Worksheets.Add("Folder Master");
                        worksheet.Cell(1, 1).Value = "Sl No";
                        worksheet.Cell(1, 2).Value = "Folder Name";
                        worksheet.Cell(1, 4).Value = "Description";
                        worksheet.Cell(1, 5).Value = "Status";
                        worksheet.Cell(1, 6).Value = "CreatedBy";
                        // ... Add more headers if needed

                        // Format header row
                        var headerRow = worksheet.Row(1);
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
                        // ... Apply additional formatting as needed

                        for (int i = 0; i < objData.returnData.Count; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = i + 1; // Serial Number
                            worksheet.Cell(i + 2, 2).Value = objData.returnData[i].FolderName;
                            worksheet.Cell(i + 2, 4).Value = objData.returnData[i].Description;
                            if (objData.returnData[i].Active)
                            {
                                worksheet.Cell(i + 2, 5).Value = "Active";
                            }
                            else
                            {
                                worksheet.Cell(i + 2, 5).Value = "Inactive";
                            }
                            worksheet.Cell(i + 2, 6).Value = objData.returnData[i].CreatedUsername;
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
        public ResponseEntity<bool> DeleteFolder(FolderViewModel objModel)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var memberDetails = _dbContext.FolderMasters.Find(objModel.FolderId);
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
        public ResponseEntity<List<FolderViewModel>> GetLibraryFolders(string? SearchText, string? pageCode)
        {
            var retModel = new ResponseEntity<List<FolderViewModel>>();
            try
            {
                var objModel = new List<FolderViewModel>();

                // Base query: Active users
                var folders = (from l in _dbContext.LookupMasters
                               join f in _dbContext.FolderMasters on l.LookUpId equals f.FolderId
                               join p in _dbContext.ProjectConfigurations on l.LookUpId.ToString() equals p.Value
                               where f.Active == true
                                     && p.CONFIGKEY == pageCode
                               select new
                               {
                                   Folder = f,
                                   LookUp = l,
                                   ProjectConfig = p
                               }).ToList();

                // Map to ViewModel
                objModel = folders.Select(c => new FolderViewModel
                {
                    FolderName = c.Folder.FolderName, // Access FolderName from Folder
                    FolderId = c.Folder.FolderId,     // Access FolderId if needed
                    Description = string.IsNullOrEmpty(c.Folder.Description)
                                      ? ""
                                      : (c.Folder.Description.Length > 50
                                         ? c.Folder.Description.Substring(0, 50) + "..."
                                         : c.Folder.Description)
                }).ToList();
                // **Apply search filter on FolderName**
                if (!string.IsNullOrEmpty(SearchText))
                {
                    objModel = objModel
                        .Where(f => f.FolderName.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }


                //objModel = objModel.OrderByDescending(i => i.DepartmentRefNo).ToList();
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
        public ResponseEntity<List<FolderViewModel>> GetActivityFolders(string? SearchText, string? pageCode)
        {
            var retModel = new ResponseEntity<List<FolderViewModel>>();
            try
            {
                var objModel = new List<FolderViewModel>();

                // Base query: Active users
                var folders = (from l in _dbContext.LookupMasters
                               join f in _dbContext.FolderMasters on l.LookUpId equals f.FolderId
                               join p in _dbContext.ProjectConfigurations on l.LookUpId.ToString() equals p.Value
                               where f.Active == true
                                     && p.CONFIGKEY == pageCode
                               select new
                               {
                                   Folder = f,
                                   LookUp = l,
                                   ProjectConfig = p
                               }).ToList();

                // Map to ViewModel
                objModel = folders.Select(c => new FolderViewModel
                {
                    FolderName = c.Folder.FolderName, // Access FolderName from Folder
                    FolderId = c.Folder.FolderId,     // Access FolderId if needed
                    FirstLetter = string.IsNullOrEmpty(c.Folder.FolderName)
              ? ""
              : c.Folder.FolderName.Substring(0, 1).ToUpper(),
                    Description = string.IsNullOrEmpty(c.Folder.Description)
                                      ? ""
                                      : (c.Folder.Description.Length > 50
                                         ? c.Folder.Description.Substring(0, 50) + "..."
                                         : c.Folder.Description)
                }).ToList();
                // **Apply search filter on FolderName**
                if (!string.IsNullOrEmpty(SearchText))
                {
                    objModel = objModel
                        .Where(f => f.FolderName.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }


                //objModel = objModel.OrderByDescending(i => i.DepartmentRefNo).ToList();
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
        public async Task<ResponseEntity<FileStorageViewModel>> SaveAttachment(List<IFormFile> fileInputs, long? CreatedBy)
        {
            var objResponce = new ResponseEntity<FileStorageViewModel>();
            foreach (var f in fileInputs)
            {
                try
                {
                    FileStorageViewModel objImage = new FileStorageViewModel();

                    var fileName = f.FileName;
                    if (fileName.LastIndexOf(@"\") > 0)
                        fileName = fileName.Substring(fileName.LastIndexOf(@"\") + 1);

                    FileInfo fi = new FileInfo(fileName.ToLower());

                    string[] allowedExtensions = { ".jpeg", ".jpg", ".png" };
                    if (allowedExtensions.Contains(fi.Extension))
                    {
                        var ActualFileName = System.IO.Path.GetFileNameWithoutExtension(f.FileName);

                        objImage.FileExtension = System.IO.Path.GetExtension(f.FileName);
                        objImage.FileName = ActualFileName;
                        objImage.ContentType = f.ContentType;
                        objImage.ContentLength = f.Length;
                        objImage.StorageMode = "LocalServer";

                        using (var memoryStream = new MemoryStream())
                        {
                            await f.CopyToAsync(memoryStream);
                            objImage.FileData = memoryStream.ToArray();
                        }
                        string? FolderName = _dbContext.ProjectConfigurations.Where(c => c.CONFIGKEY == "C0016")
                                             .Select(c => c.Value) // replace `Value` with actual column name
                                             .FirstOrDefault();
                        string Year = DateTime.Now.ToString("yyyy");
                        string storagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", FolderName, "FolderIcons");
                        objImage.FilePath = storagePath;
                        objResponce.returnData = objImage;
                    }

                }
                catch (Exception ex)
                {
                    objResponce.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                    objResponce.returnMessage = "Internal Server Error Occured";
                }

            }

            return objResponce;
        }
        public ResponseEntity<List<FolderViewModel>> GetKnowlegebaseFolders(string? SearchText, string? pageCode)
        {
            var retModel = new ResponseEntity<List<FolderViewModel>>();
            try
            {
                var objModel = new List<FolderViewModel>();

                // Base query: Active users
                var folders = (from l in _dbContext.LookupMasters
                               join f in _dbContext.FolderMasters on l.LookUpId equals f.FolderId
                               join p in _dbContext.ProjectConfigurations on l.LookUpId.ToString() equals p.Value
                               where f.Active == true
                                     && p.CONFIGKEY == pageCode
                               select new
                               {
                                   Folder = f,
                                   LookUp = l,
                                   ProjectConfig = p
                               }).ToList();

                // Map to ViewModel
                objModel = folders.Select(c => new FolderViewModel
                {
                    FolderName = c.Folder.FolderName, // Access FolderName from Folder
                    FolderId = c.Folder.FolderId,     // Access FolderId if needed
                    FileStorageId = c.Folder.FileStorageId,
                    FilePath = _dbContext.FileStorages.Any(i => i.FileStorageId == c.Folder.FileStorageId && i.Active) ? _dbContext.FileStorages.FirstOrDefault(i => i.FileStorageId == c.Folder.FileStorageId && i.Active).FilePath : null,
                    Description = string.IsNullOrEmpty(c.Folder.Description)
                                      ? ""
                                      : (c.Folder.Description.Length > 50
                                         ? c.Folder.Description.Substring(0, 50) + "..."
                                         : c.Folder.Description)
                }).ToList();
                // **Apply search filter on FolderName**
                if (!string.IsNullOrEmpty(SearchText))
                {
                    objModel = objModel
                        .Where(f => f.FolderName.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }


                //objModel = objModel.OrderByDescending(i => i.DepartmentRefNo).ToList();
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
    }
}
