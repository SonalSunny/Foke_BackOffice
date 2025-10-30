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
    public class FileMasterRepository : IFileMasterRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDropDownRepository _dropDownRepository;
        private readonly IAttachmentRepository _attachRepository;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;

        public FileMasterRepository(FOKEDBContext skakErpDBContext, IHttpContextAccessor httpContextAccessor, IDropDownRepository dropDownRepository, IAttachmentRepository attachRepository)
        {
            this._dbContext = skakErpDBContext;
            this._httpContextAccessor = httpContextAccessor;
            this._dropDownRepository = dropDownRepository; // Initialize the field
            this._attachRepository = attachRepository;
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

            _attachRepository = attachRepository;
        }

        public ResponseEntity<FileViewModel> GetFileById(long FileId)
        {
            var retModel = new ResponseEntity<FileViewModel>();
            try
            {
                // Correct Include statement - use navigation property, not scalar ID
                var objData = _dbContext.FileMasters
                    .Include(f => f.FolderMaster) // ✅ Correct - uses navigation property
                    .SingleOrDefault(u => u.FileId == FileId);

                if (objData != null)
                {
                    //var fileStorage = _dbContext.FileStorages.FirstOrDefault(fs => fs.FileStorageId == objData.FileStorageId && fs.Active);
                    var objModel = new FileViewModel
                    {
                        FileId = objData.FileId,
                        FileName = objData.FileName,
                        FolderId = objData.FolderId,
                        // Simplified - no need for extra query since we included Folder
                        FolderName = objData.FolderMaster?.FolderName,
                        UploadFileName = objData.UploadFileName,
                        FileRefNo = objData.FileRefNo,
                        Description = objData.Description,
                        FileLink = objData.FileLink,
                        Active = objData.Active, // Use actual value from DB
                        CreatedBy = objData.CreatedBy, // Use stored value
                        CreatedDate = objData.CreatedDate, // Use stored value
                        FileStorageId = objData.FileStorageId,
                        FilePath = _dbContext.FileStorages.Where(c => c.FileStorageId == objData.FileStorageId).Select(c => c.FilePath).FirstOrDefault()
                    };
                    retModel.returnData = objModel;
                    retModel.transactionStatus = HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message; // Always include error message
            }
            return retModel;
        }

        public async Task<ResponseEntity<FileViewModel>> CreateFile(FileViewModel model)
        {
            var Returndata = new ResponseEntity<FileViewModel>();

            try
            {
                var ExistingData = _dbContext.FileMasters.Any(i => i.FileName != null && model.FileName != null && i.FileName.Trim().ToLower() == model.FileName.Trim().ToLower());

                if (ExistingData)
                {
                    Returndata.transactionStatus = HttpStatusCode.InternalServerError;
                    Returndata.returnMessage = "File Already Exists";
                }
                else
                {
                    if (model.Attachment != null && model.Attachment.Any())
                    {
                        var DocUpload = await SaveAttachment(model.Attachment, model.CreatedBy, model.FolderId);
                        Console.WriteLine($"Debug: DocUpload Status = {DocUpload.transactionStatus}");
                        Console.WriteLine($"Debug: DocUpload FilePath = {DocUpload.returnData?.FilePath}");
                        if (DocUpload?.returnData?.FilePath == null)
                        {
                            Returndata.transactionStatus = HttpStatusCode.InternalServerError;
                            Returndata.returnMessage = "Failed to save attachment: FilePath is null.";
                            return Returndata;
                        }
                        model.FileStorageId = DocUpload.returnData.FileStorageId;
                        model.UploadFileName = DocUpload.returnData.FilePath;
                    }

                    var files = new FileMaster
                    {
                        FileName = model.FileName,
                        Description = model.Description,
                        FolderId = model.FolderId,
                        FileRefNo = model.FileRefNo,
                        FileLink = model.FileLink,
                        Active = true,
                        CreatedBy = loggedInUser,//loginned user employee is
                        CreatedDate = DateTime.Now,
                        UploadFileName = model.UploadFileName,
                        FileStorageId = model.FileStorageId
                    };

                    await _dbContext.FileMasters.AddAsync(files);
                    await _dbContext.SaveChangesAsync();


                    Returndata.transactionStatus = HttpStatusCode.OK;
                    Returndata.returnMessage = "New File Added Successfully";
                }
            }
            catch (Exception ex)
            {
                Returndata.transactionStatus = HttpStatusCode.InternalServerError;
                Returndata.returnMessage = "Server Error";
            }

            return Returndata;
        }


        public async Task<ResponseEntity<FileStorageViewModel>> SaveAttachment(List<IFormFile> fileInputs, long? CreatedBy, long? folderid)
        {
            var objResponse = new ResponseEntity<FileStorageViewModel>();

            if (fileInputs == null || !fileInputs.Any())
            {
                objResponse.transactionStatus = HttpStatusCode.BadRequest;
                objResponse.returnMessage = "No file(s) uploaded.";
                return objResponse;
            }

            try
            {
                foreach (var f in fileInputs)
                {
                    var fileName = Path.GetFileName(f.FileName);
                    var fileExtension = Path.GetExtension(fileName)?.ToLower();

                    string[] allowedExtensions = { ".jpeg", ".jpg", ".png", ".doc", ".docx", ".pdf", ".txt", ".gif", ".xls", ".xlsx" };
                    if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension))
                        continue;

                    string actualFileName = Path.GetFileNameWithoutExtension(fileName);

                    // 🔍 Get FolderName from FolderMaster (no ModuleId used)
                    string? folderName = _dbContext.FolderMasters
                        .Where(c => c.FolderId == folderid)
                        .Select(c => c.FolderName)
                        .FirstOrDefault();

                    if (string.IsNullOrEmpty(folderName))
                    {
                        objResponse.transactionStatus = HttpStatusCode.BadRequest;
                        objResponse.returnMessage = "Invalid folder selected.";
                        return objResponse;
                    }

                    string year = DateTime.Now.ToString("yyyy");


                    string storagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "FileStorage", folderName, year, folderid.ToString());

                    var objImage = new FileStorageViewModel
                    {
                        FileName = actualFileName,
                        FileExtension = fileExtension,
                        ContentType = f.ContentType,
                        ContentLength = f.Length,
                        StorageMode = "LocalServer",
                        CreatedBy = CreatedBy,
                        FolderId = folderid,
                        FilePath = storagePath
                    };

                    using (var memoryStream = new MemoryStream())
                    {
                        await f.CopyToAsync(memoryStream);
                        objImage.FileData = memoryStream.ToArray();
                    }

                    var attachResponse = await _attachRepository.SaveAttachment(objImage);
                    if (attachResponse.returnData != null)
                    {
                        objImage.FileStorageId = attachResponse.returnData.FileStorageId;
                        objImage.FileName = attachResponse.returnData.FileName;
                    }

                    objResponse.returnData = objImage;
                    objResponse.transactionStatus = HttpStatusCode.OK;
                    break; // Process only the first valid file; remove if multiple should be saved
                }
            }
            catch (Exception ex)
            {
                objResponse.transactionStatus = HttpStatusCode.InternalServerError;
                objResponse.returnMessage = "Internal Server Error: " + ex.Message;
            }

            return objResponse;
        }



        //public async Task<ResponseEntity<FileStorage>> SaveAttachment(List<IFormFile> fileInputs, long? CreatedBy, long? folderid)
        //{
        //var objResponse = new ResponseEntity<FileStorage>();

        //foreach (var file in fileInputs)
        //{
        //    try
        //    {
        //        string fileExtension = Path.GetExtension(file.FileName).ToLower();
        //        string[] allowedExtensions = { ".jpeg", ".jpg", ".png", ".doc", ".docx", ".pdf", ".txt", ".gif", ".xls", ".xlsx" };

        //        if (allowedExtensions.Contains(fileExtension))
        //        {
        //            string folderName = DateTime.Now.ToString("yyyy");
        //            string storagePath = Path.Combine("wwwroot", "FileStorage", "Files", folderName);

        //            if (!Directory.Exists(storagePath))
        //                Directory.CreateDirectory(storagePath);

        //            string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        //            string fullFilePath = Path.Combine(storagePath, uniqueFileName);
        //            string relativeFilePath = $"/FileStorage/Files/{folderName}/{uniqueFileName}"; // Store only relative path

        //            using (var stream = new FileStream(fullFilePath, FileMode.Create))
        //            {
        //                await file.CopyToAsync(stream);
        //            }

        //            objResponse.returnData = new FileStorage
        //            {
        //                FilePath = relativeFilePath
        //            };
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        objResponse.transactionStatus = HttpStatusCode.InternalServerError;
        //        objResponse.returnMessage = "Internal Server Error Occurred";
        //    }
        //}

        //return objResponse;
        //    var objResponce = new ResponseEntity<FileStorageViewModel>();
        //    foreach (var f in fileInputs)
        //    {
        //        try
        //        {
        //            FileStorageViewModel objImage = new FileStorageViewModel();

        //            var fileName = f.FileName;
        //            if (fileName.LastIndexOf(@"\") > 0)
        //                fileName = fileName.Substring(fileName.LastIndexOf(@"\") + 1);

        //            FileInfo fi = new FileInfo(fileName.ToLower());

        //            string[] allowedExtensions = { ".jpeg", ".jpg", ".png", ".doc", ".docx", ".pdf", ".txt", ".gif", ".xls", ".xlsx" };
        //            if (allowedExtensions.Contains(fi.Extension))
        //            {
        //                var ActualFileName = System.IO.Path.GetFileNameWithoutExtension(f.FileName);

        //                objImage.FileExtension = System.IO.Path.GetExtension(f.FileName);
        //                objImage.FileName = ActualFileName;
        //                objImage.ContentType = f.ContentType;
        //                objImage.ContentLength = f.Length;
        //                objImage.StorageMode = "LocalServer";

        //                using (var memoryStream = new MemoryStream())
        //                {
        //                    await f.CopyToAsync(memoryStream);
        //                    objImage.FileData = memoryStream.ToArray();
        //                }
        //                string? FolderName = _dbContext.ProjectConfigurations.Where(c => c.CONFIGKEY == "C0016")
        //                                     .Select(c => c.Value) // replace `Value` with actual column name
        //                                     .FirstOrDefault();
        //                string Year = DateTime.Now.ToString("yyyy");
        //                string storagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", FolderName, moduleid.ToString(), Year, folderid.ToString());
        //                objImage.FilePath = storagePath;

        //                var attachResponse = await _attachRepository.SaveAttachment(objImage);
        //                if (attachResponse.returnData != null)
        //                {
        //                    objImage.FileName = attachResponse.returnData.FileName;
        //                    objImage.FileStorageId = attachResponse.returnData.FileStorageId;
        //                }
        //                objResponce.returnData = objImage;
        //            }
        //        }

        //        catch (Exception ex)
        //        {
        //            objResponce.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
        //            objResponce.returnMessage = "Internal Server Error Occured";
        //        }

        //}
        public async Task<ResponseEntity<bool>> DeleteAttachment(long attachmentId)
        {
            var response = new ResponseEntity<bool>();
            try
            {
                // Find the attachment in the database
                var attachment = await _dbContext.FileMasters.FindAsync(attachmentId);
                if (attachment != null)
                {
                    // Debugging: Log the attachment details
                    Console.WriteLine($"Deleting attachment: ID = {attachment.FileId}, Path = {attachment.UploadFileName}");

                    // Delete the file from the file system
                    var filePath = Path.Combine("wwwroot", attachment.UploadFileName.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    // Remove the attachment from the database
                    attachment.UploadFileName = null;
                    await _dbContext.SaveChangesAsync();


                    response.transactionStatus = HttpStatusCode.OK;
                    response.returnMessage = "Attachment deleted successfully.";
                }
                else
                {
                    response.transactionStatus = HttpStatusCode.NotFound;
                    response.returnMessage = "Attachment not found.";
                }
            }
            catch (Exception ex)
            {
                response.transactionStatus = HttpStatusCode.InternalServerError;
                response.returnMessage = ex.Message;
            }
            return response;
        }
        public ResponseEntity<List<FileViewModel>> GetAllFiles(long? Status)
        {
            var retModel = new ResponseEntity<List<FileViewModel>>();
            try
            {
                IQueryable<FileMaster> query = _dbContext.FileMasters;

                // Filter by Status
                if (Status.HasValue)
                {
                    if (Status == 2)
                    {
                        // Include both active and inactive
                        query = query.Where(c => c.Active == true || c.Active == false);
                    }
                    else if (Status == 1)
                    {
                        query = query.Where(c => c.Active == true);
                    }
                    else if (Status == 0)
                    {
                        query = query.Where(c => c.Active == false);
                    }
                }
                else
                {
                    query = query.Where(c => c.Active == true);
                }

                var list = query.ToList();

                var objModel = list.Select(c =>
                {
                    var fileStorage = _dbContext.FileStorages.FirstOrDefault(fs => fs.FileStorageId == c.FileStorageId && fs.Active);

                    return new FileViewModel()
                    {
                        FileId = c.FileId,
                        FileName = c.FileName,
                        Description = c.Description,
                        FolderId = c.FolderId,
                        UploadFileName = c.UploadFileName,
                        Active = c.Active,
                        CreatedDate = c.CreatedDate,
                        CreatedBy = _dbContext.Users
                            .Where(u => u.UserId == c.CreatedBy)
                            .Select(u => u.UserId)
                            .FirstOrDefault(),
                        FolderName = _dbContext.FolderMasters
                            .Where(u => u.FolderId == c.FolderId)
                            .Select(u => u.FolderName)
                            .FirstOrDefault(),
                        FileRefNo = c.FileRefNo,
                        AttachmentAny = fileStorage != null && fileStorage.Active,
                        FileLink = c.FileLink,
                        FilePath = fileStorage?.FilePath

                    };
                })
                .OrderByDescending(i => i.FileName)
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

        //public ResponseEntity<List<FileViewModel>> GetAllFiles(long? Status)
        //{
        //    var retModel = new ResponseEntity<List<FileViewModel>>();
        //    try
        //    {
        //        var objModel = new List<FileViewModel>();

        //        IQueryable<Entity.OperationManagement.DTO.FileMaster> retData = _dbContext.FileMasters.Where(c => c.Active == true);

        //        if (Status.HasValue)
        //        {
        //            IQueryable<FileMaster> query = _dbContext.FileMasters;

        //            if (Status == 2)
        //            {
        //                query = query.Where(c => c.Active == true || c.Active == false);
        //            }
        //            else if (Status == 1)
        //            {
        //                query = query.Where(c => c.Active == true);
        //            }
        //            else if (Status == 0)
        //            {
        //                query = query.Where(c => c.Active == false);
        //            }

        //            retData = query;
        //        }

        //        objModel = retData.Select(c => new FileViewModel()
        //        {
        //            FileId = c.FileId,
        //            FileName = c.FileName,
        //            Description = c.Description,
        //            FolderId = c.FolderId,
        //            UploadFileName = c.UploadFileName,
        //            Active = c.Active,
        //            CreatedDate = c.CreatedDate,
        //            CreatedBy = _dbContext.Users
        //                .Where(u => u.UserId == c.CreatedBy)
        //                .Select(u => u.UserId)
        //                .FirstOrDefault(),
        //            FolderName = _dbContext.FolderMasters
        //                        .Where(u => u.FolderId == c.FolderId)
        //                        .Select(u => u.FolderName)
        //                         .FirstOrDefault(),
        //            FileRefNo = c.FileRefNo,
        //            AttachmentAny = c.FileStorageId != null ? c.FileStorage.Active : false,
        //            FileLink = c.FileLink,
        //            FilePath = _dbContext.FileStorages.Any(i => i.FileStorageId == c.FileStorageId && i.Active) ? _dbContext.FileStorages.FirstOrDefault(i => i.FileStorageId == c.FileStorageId && i.Active).FilePath : null,

        //        }).ToList();

        //        objModel = objModel.OrderByDescending(i => i.FileName).ToList();
        //        retModel.transactionStatus = System.Net.HttpStatusCode.OK;
        //        retModel.returnData = objModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
        //        retModel.returnMessage = ex.Message;
        //    }

        //    return retModel;
        //}

        public ResponseEntity<bool> DeleteFile(FileViewModel objModel)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var memberDetails = _dbContext.FileMasters.Find(objModel.FileId);
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

        public ResponseEntity<string> ExporttoExcel(string search, long? Statusid)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                var objData = GetAllFiles(Statusid);

                if (objData.transactionStatus == HttpStatusCode.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        // Set header row
                        var worksheet = workbook.Worksheets.Add("File Master");
                        worksheet.Cell(1, 1).Value = "Sl No";
                        worksheet.Cell(1, 3).Value = "Folder Name";
                        worksheet.Cell(1, 4).Value = "File Reference No";
                        worksheet.Cell(1, 5).Value = "File Name";
                        worksheet.Cell(1, 6).Value = "Description";
                        worksheet.Cell(1, 7).Value = "Status";
                        // ... Add more headers if needed

                        // Format header row
                        var headerRow = worksheet.Row(1);
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
                        // ... Apply additional formatting as needed

                        for (int i = 0; i < objData.returnData.Count; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = i + 1; // Serial Number
                            worksheet.Cell(i + 2, 3).Value = objData.returnData[i].FolderName;
                            worksheet.Cell(i + 2, 4).Value = objData.returnData[i].FileRefNo;
                            worksheet.Cell(i + 2, 5).Value = objData.returnData[i].FileName;
                            worksheet.Cell(i + 2, 6).Value = objData.returnData[i].Description;
                            if (objData.returnData[i].Active)
                            {
                                worksheet.Cell(i + 2, 7).Value = "Active";
                            }
                            else
                            {
                                worksheet.Cell(i + 2, 7).Value = "Inactive";
                            }



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

        public FileStorageViewModel GetAttachment(string encryptedID)
        {


            var objResponce = new FileStorageViewModel();
            try
            {
                var MasterId = GenericUtilities.Convert<long>(encryptedID);

                long attacheID = Convert.ToInt64(MasterId);
                var objAttachment = _dbContext.FileMasters
                                             .Where(c => c.FileId == MasterId)
                                             .Include(c => c.FileStorage)
                                             .FirstOrDefault();

                if (objAttachment != null)
                {
                    // objResponce.returnData.AttachmentId = objAttachment.AttachmentId;


                    objResponce.FileName = objAttachment.FileName;
                    objResponce.ContentType = objAttachment.FileStorage.ContentType;
                    objResponce.ContentLength = objAttachment.FileStorage.ContentLength;
                    objResponce.FileExtension = objAttachment.FileStorage.FileExtension;
                    objResponce.FileStorageId = objAttachment.FileStorage.FileStorageId;

                    var filepath = objAttachment.FileStorage.FilePath;

                    // Read the file from disk and copy it to the memory stream

                    using (var memory = new MemoryStream())
                    {
                        try
                        {
                            if (!File.Exists(filepath))
                            {
                                throw new FileNotFoundException($"File not found: {filepath}");
                            }
                            using (var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                            {
                                stream.CopyTo(memory);
                            }

                            memory.Position = 0;
                            objResponce.RetFileData = Convert.ToBase64String(memory.ToArray());
                        }
                        catch (Exception ex)
                        {
                            objResponce.Message = $"An error occurred while processing the file. Details: {ex.Message}. StackTrace: {ex.StackTrace}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objResponce.Message = $"An error occurred while processing the file. Details: {ex.Message}. StackTrace: {ex.StackTrace}";
            }

            return objResponce;
        }

        public async Task<ResponseEntity<FileViewModel>> UpdateFile(FileViewModel model)
        {
            var Returndata = new ResponseEntity<FileViewModel>();
            try
            {
                var Existingata = _dbContext.FileMasters.Any(i => i.FileId != model.FileId && i.Active && i.FileName.Trim().ToLower() == model.FileName.Trim().ToLower());
                if (Existingata)
                {
                    Returndata.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                    Returndata.returnMessage = "File Already Exists";
                    return Returndata; // Return early to avoid further processing
                }

                var AllFiles = _dbContext.FileMasters.FirstOrDefault(i => i.FileId == model.FileId && i.Active);

                if (AllFiles != null)
                {
                    // Check if model.Attachment is not null and contains files
                    if (model.Attachment != null && model.Attachment.Any())
                    {
                        var DocUpload = await SaveAttachment(model.Attachment, model.CreatedBy, model.FolderId);
                        model.UploadFileName = DocUpload.returnData.FilePath;
                        model.FileStorageId = DocUpload.returnData.FileStorageId;
                    }
                    else
                    {
                        // Preserve the existing URL if no new file is uploaded
                        model.UploadFileName = AllFiles.UploadFileName;
                    }

                    AllFiles.FileName = model.FileName;
                    AllFiles.Description = model.Description;
                    AllFiles.FileRefNo = model.FileRefNo;
                    AllFiles.FolderId = model.FolderId;
                    AllFiles.FileLink = model.FileLink;
                    AllFiles.UploadFileName = model.UploadFileName;
                    AllFiles.UpdatedBy = model.loggedinUserId;
                    AllFiles.UpdatedDate = DateTime.Now;
                    AllFiles.FileStorageId = model.FileStorageId;
                    await _dbContext.SaveChangesAsync();

                    Returndata.transactionStatus = System.Net.HttpStatusCode.OK;
                    Returndata.returnMessage = "File Updated Successfully";
                }
                else
                {
                    Returndata.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    Returndata.returnMessage = "Invalid Event";
                }
            }
            catch (Exception ex)
            {
                Returndata.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                Returndata.returnMessage = "Server Error: " + ex.Message; // Include the exception message for debugging
            }
            return Returndata;
        }

        public ResponseEntity<List<FileViewModel>> GetLibraryFiles(long? FolderId, string? SearchText)
        {
            var retModel = new ResponseEntity<List<FileViewModel>>();

            try
            {
                // Base query: Active files
                IQueryable<FileMaster> query = _dbContext.FileMasters.Where(c => c.Active == true);

                // Filter by FolderId if provided
                if (FolderId.HasValue && FolderId != 0)
                {
                    query = query.Where(c => c.FolderId == FolderId);
                }

                // LEFT JOIN FileMasters with FileStorages (Includes files with and without storage)
                var objModel = query
                    .GroupJoin(
                        _dbContext.FileStorages.Where(fs => fs.Active == true),   // Join with FileStorages
                        fm => fm.FileStorageId,    // Key from FileMasters
                        fs => fs.FileStorageId,    // Key from FileStorages
                        (fm, fileStorageGroup) => new { FileMaster = fm, FileStorage = fileStorageGroup.FirstOrDefault() }
                    )
                    .Select(joined => new FileViewModel
                    {
                        FileId = joined.FileMaster.FileId,
                        FileName = joined.FileMaster.FileName,
                        FolderId = joined.FileMaster.FolderId,
                        FileLink = joined.FileMaster.FileLink ?? (joined.FileStorage != null ? joined.FileStorage.FilePath : null), // Use FileLink if FileStorageId is NULL
                        UpdatedOrCreatedDate = joined.FileMaster.UpdatedDate ?? joined.FileMaster.CreatedDate,
                        FileExtension = joined.FileStorage != null ? joined.FileStorage.FileExtension : null,
                        ContentLength = joined.FileStorage != null ? joined.FileStorage.ContentLength / 1024 : 0, // Convert bytes to KB
                        AttachmentAny = joined.FileStorage != null, // True if file exists in storage
                        FilePath = joined.FileStorage != null ? joined.FileStorage.FilePath : null // Path only if exists
                    })
                    .OrderByDescending(f => f.FileName);

                // Apply search filter if needed
                if (!string.IsNullOrEmpty(SearchText))
                {
                    SearchText = SearchText.ToLower(); // Convert search text to lowercase

                    objModel = objModel
                        .Where(f => f.FileName.ToLower().Contains(SearchText)) // Convert FileName to lowercase
                        .OrderByDescending(f => f.FileName); // Reapply ordering
                }


                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel.ToList(); // Execute query and return list
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message; // Include error message for debugging
            }


            return retModel;
        }

        //public ResponseEntity<List<FileViewModel>> Getfilemanagerfiles(string? SearchText, string pageCode)
        //{
        //    var retModel = new ResponseEntity<List<FileViewModel>>();

        //    try
        //    {
        //        var objModel = new List<FileViewModel>();

        //        var folders = (from l in _dbContext.LookupMasters
        //                       join f in _dbContext.FolderMasters on l.LookUpId equals f.FolderId
        //                       join p in _dbContext.ProjectConfigurations on l.LookUpId.ToString() equals p.Value
        //                       where f.Active == true && p.CONFIGKEY == pageCode
        //                       select new
        //                       {
        //                           f.FolderName,
        //                           f.FolderId,
        //                           f.Description
        //                       }).ToList();

        //        // Transform result to FileViewModel
        //        objModel = folders.Select(c => new FileViewModel
        //        {
        //            FolderName = c.FolderName,
        //            FolderId = c.FolderId,
        //            Description = string.IsNullOrEmpty(c.Description)
        //                            ? ""
        //                            : (c.Description.Length > 50
        //                                ? c.Description.Substring(0, 50) + "..."
        //                                : c.Description),
        //            FileCount = _dbContext.FileMasters
        //            .Count(f => f.FolderId == c.FolderId && f.Active) // assuming `Active` is bool
        //        }).ToList();

        //        // Apply search filter on FolderName if provided
        //        if (!string.IsNullOrEmpty(SearchText))
        //        {
        //            objModel = objModel
        //                .Where(f => f.FolderName != null && f.FolderName.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
        //                .ToList();
        //        }

        //        retModel.transactionStatus = System.Net.HttpStatusCode.OK;
        //        retModel.returnData = objModel;
        //    }
        //    catch (Exception ex)
        //    {
        //        retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
        //        retModel.returnMessage = ex.Message;
        //    }

        //    return retModel;
        //}


        public ResponseEntity<List<FileViewModel>> Getfilemanagerfiles(string? SearchText, string pageCode)
        {
            var retModel = new ResponseEntity<List<FileViewModel>>();

            try
            {
                var objModel = new List<FileViewModel>();

                // Fetch folders directly from FolderMasters where Active = true
                var folders = _dbContext.FolderMasters
                    .Where(f => f.Active)
                    .Select(f => new
                    {
                        f.FolderName,
                        f.FolderId,
                        f.Description
                    })
                    .ToList();

                // Transform result to FileViewModel
                objModel = folders.Select(c => new FileViewModel
                {
                    FolderName = c.FolderName,
                    FolderId = c.FolderId,
                    Description = string.IsNullOrEmpty(c.Description)
                                   ? ""
                                   : (c.Description.Length > 50
                                       ? c.Description.Substring(0, 50) + "..."
                                       : c.Description),
                    FileCount = _dbContext.FileMasters
                                .Count(f => f.FolderId == c.FolderId && f.Active)
                }).ToList();

                // Apply search filter if SearchText is provided
                if (!string.IsNullOrEmpty(SearchText))
                {
                    objModel = objModel
                        .Where(f => f.FolderName != null &&
                                    f.FolderName.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

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

    }
}
