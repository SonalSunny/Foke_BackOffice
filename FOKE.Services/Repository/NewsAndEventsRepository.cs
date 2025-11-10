using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.API.APIModel.ViewModel;
using FOKE.Entity.FileUpload.DTO;
using FOKE.Entity.FileUpload.ViewModel;
using FOKE.Entity.NewsAndEventsData.DTO;
using FOKE.Entity.NewsAndEventsData.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class NewsAndEventsRepository : INewsAndEventsRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDropDownRepository _dropDownRepository;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;

        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IWebHostEnvironment _env;

        public NewsAndEventsRepository(FOKEDBContext dbContext, IHttpContextAccessor httpContextAccessor, IAttachmentRepository attachRepository, IWebHostEnvironment env, IDropDownRepository dropDownRepository)
        {
            this._dbContext = dbContext;
            this._httpContextAccessor = httpContextAccessor;
            this._attachmentRepository = attachRepository;
            this._dropDownRepository = dropDownRepository;
            _env = env ?? throw new ArgumentNullException(nameof(env));

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

        public async Task<ResponseEntity<NewsAndEventsViewModel>> CreateNewsAndEvents(NewsAndEventsViewModel model)
        {
            var retModel = new ResponseEntity<NewsAndEventsViewModel>();

            try
            {

                var ExistingData = _dbContext.NewsAndEvents
                    .Any(i => i.Heading.Trim().ToLower() == model.Heading.Trim().ToLower());

                if (ExistingData)
                {
                    retModel.transactionStatus = HttpStatusCode.Conflict;
                    retModel.returnMessage = "This Event Already Exists";
                    //  return Returndata;
                }
                else
                {
                    if (model.Image != null)
                    {
                        var FileInputData = await SaveSingleFile(model.Image, null, loggedInUser);
                        model.ImagePath = FileInputData.returnData.FilePath;
                        model.FileStorageId = FileInputData.returnData.FileStorageId;
                    }
                    var News = new NewsAndEvent
                    {
                        Heading = model.Heading,
                        Description = model.Description,
                        Date = model.Date,
                        ShowInMobile = model.ShowInMobile,
                        ShowInWebsite = model.ShowInWebsite,
                        ImagePath = model.ImagePath,
                        FileStorageId = model.FileStorageId,
                        Active = true,
                        CreatedBy = loggedInUser,
                        CreatedDate = DateTime.UtcNow,
                        Type = model.Type
                    };
                    await _dbContext.NewsAndEvents.AddAsync(News);
                    await _dbContext.SaveChangesAsync();
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "News Added Successfully";
                }


            }
            catch (Exception ex)
            {
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error: " + ex.Message;
            }

            return retModel;
        }

        public async Task<ResponseEntity<NewsAndEventsViewModel>> UpdateNewsAndEvents(NewsAndEventsViewModel model)
        {
            var retModel = new ResponseEntity<NewsAndEventsViewModel>();

            try
            {
                var existingNews = _dbContext.NewsAndEvents
                    .FirstOrDefault(n => n.Id == model.Id && n.Active == true);

                if (existingNews != null)
                {

                    bool headingExists = _dbContext.NewsAndEvents
                        .Any(n => n.Id != model.Id && n.Heading == model.Heading);

                    if (headingExists)
                    {
                        retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                        retModel.returnMessage = "News heading already exists.";
                        return retModel;
                    }


                    if (model.Image != null)
                    {
                        var oldFileData = _dbContext.FileStorages
                            .FirstOrDefault(f => f.FileStorageId == existingNews.FileStorageId);

                        if (oldFileData != null)
                        {
                            if (System.IO.File.Exists(oldFileData.StorageMode))
                                System.IO.File.Delete(oldFileData.StorageMode);

                            _dbContext.FileStorages.Remove(oldFileData);
                            await _dbContext.SaveChangesAsync();
                        }

                        var fileUploadResult = await SaveSingleFile(model.Image, null, loggedInUser);

                        existingNews.ImagePath = fileUploadResult.returnData.FilePath;
                        existingNews.FileStorageId = fileUploadResult.returnData.FileStorageId;
                    }


                    existingNews.Heading = model.Heading;
                    existingNews.Description = model.Description;
                    existingNews.Date = model.Date;
                    existingNews.Type = model.Type;
                    existingNews.ShowInMobile = model.ShowInMobile;
                    existingNews.ShowInWebsite = model.ShowInWebsite;
                    existingNews.UpdatedBy = loggedInUser;
                    existingNews.UpdatedDate = DateTime.UtcNow;

                    await _dbContext.SaveChangesAsync();

                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "News updated successfully.";
                }
                else
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "News not found.";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "An unexpected error occurred.";
                // Optional: log exception
            }

            return retModel;
        }

        public string GenerateNewsAndEventsId()
        {
            var lastNewsData = _dbContext.NewsAndEvents.OrderByDescending(i => i.Id).FirstOrDefault();

            var lastNumber = (lastNewsData?.Id ?? 0) + 1;

            return $"NEWS{lastNumber:000}";
        }

        public ResponseEntity<bool> DeleteNewsAndEvents(NewsAndEventsViewModel model)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var existingData = _dbContext.NewsAndEvents.FirstOrDefault(i => i.Id == model.Id);
                if (existingData != null)
                {
                    existingData.Active = false;
                    _dbContext.Entry(existingData).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    retModel.returnMessage = "Deactivated";
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }

        public ResponseEntity<NewsAndEventsViewModel> GetNewsAndEventsByID(long newsId)
        {
            var retModel = new ResponseEntity<NewsAndEventsViewModel>();

            try
            {
                var newsData = _dbContext.NewsAndEvents
                    .Include(n => n.FileStorage)
                    .SingleOrDefault(n => n.Id == newsId);

                if (newsData != null)
                {
                    var objModel = new NewsAndEventsViewModel
                    {
                        Id = newsData.Id,
                        Heading = newsData.Heading,
                        Description = newsData.Description,
                        Date = newsData.Date,
                        ImagePath = newsData.ImagePath,
                        ImageName = newsData.FileStorageId != null ? newsData.FileStorage.OrgFileName : null,
                        AttachmentAny = !string.IsNullOrEmpty(newsData.ImagePath),
                        ShowInMobile = newsData.ShowInMobile,
                        ShowInWebsite = newsData.ShowInWebsite,
                        Type = newsData.Type,
                        Active = newsData.Active
                    };

                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnData = objModel;
                }
                else
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.NotFound;
                    retModel.returnMessage = "News entry not found.";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "An error occurred while fetching the news.";
            }

            return retModel;
        }

        public ResponseEntity<List<NewsAndEventsViewModel>> GetAllNewsAndEvents(long? Status)
        {
            var retModel = new ResponseEntity<List<NewsAndEventsViewModel>>();

            try
            {
                IQueryable<NewsAndEvent> retData = _dbContext.NewsAndEvents.AsQueryable();

                // Filter by Status
                if (Status.HasValue)
                {
                    switch (Status)
                    {
                        case 2:
                            // All: active and inactive
                            break;
                        case 1:
                            retData = retData.Where(c => c.Active);
                            break;
                        case 0:
                            retData = retData.Where(c => !c.Active);
                            break;
                    }
                }
                else
                {
                    // Default: only active
                    retData = retData.Where(c => c.Active);
                }

                // Project to ViewModel
                var objModel = retData
                    .OrderByDescending(c => c.Id)
                    .Select(c => new NewsAndEventsViewModel
                    {
                        Id = c.Id,
                        Heading = c.Heading,
                        Description = c.Description,
                        Date = c.Date,
                        ImagePath = c.ImagePath,
                        Active = c.Active,
                        ShowInWebsite = c.ShowInWebsite,
                        ShowInMobile = c.ShowInMobile,
                        Type = c.Type
                    })
                    .ToList();

                retModel.transactionStatus = HttpStatusCode.OK;
                retModel.returnData = objModel;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server error: " + ex.Message;
            }

            return retModel;
        }

        //public async Task<ResponseEntity<FileStorage>> SaveAttachment(IFormFile fileInput, long? CreatedBy)
        //{
        //    var objResponse = new ResponseEntity<FileStorage>();

        //    try
        //    {
        //        if (fileInput == null || fileInput.Length == 0)
        //        {
        //            objResponse.transactionStatus = HttpStatusCode.BadRequest;
        //            objResponse.returnMessage = "No file was uploaded.";
        //            return objResponse;
        //        }


        //        string baseFolder = "FileStorage";


        //        string fileExtension = Path.GetExtension(fileInput.FileName).ToLower();
        //        string[] allowedExtensions = { ".jpeg", ".jpg", ".png", ".gif", ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt" };

        //        if (!allowedExtensions.Contains(fileExtension))
        //        {
        //            objResponse.transactionStatus = HttpStatusCode.BadRequest;
        //            objResponse.returnMessage = "Invalid file type.";
        //            return objResponse;
        //        }


        //        string yearFolder = DateTime.Now.ToString("yyyy");
        //        string relativeFolderPath = Path.Combine(baseFolder, "NewsMainImages", yearFolder);


        //        string absoluteFolderPath = Path.Combine(_env.WebRootPath, relativeFolderPath);

        //        // Create directory if not exists
        //        if (!Directory.Exists(absoluteFolderPath))
        //            Directory.CreateDirectory(absoluteFolderPath);


        //        string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        //        string fullFilePath = Path.Combine(absoluteFolderPath, uniqueFileName);

        //        // Save the file to disk
        //        using (var stream = new FileStream(fullFilePath, FileMode.Create))
        //        {
        //            await fileInput.CopyToAsync(stream);
        //        }

        //        // Build relative URL path for web access
        //        string relativeFilePath = Path.Combine("/", relativeFolderPath.Replace("\\", "/"), uniqueFileName);

        //        objResponse.returnData = new FileStorage
        //        {
        //            FilePath = relativeFilePath,
        //            FileName = fileInput.FileName
        //        };

        //        objResponse.transactionStatus = HttpStatusCode.OK;
        //        objResponse.returnMessage = "File uploaded successfully.";
        //    }
        //    catch (Exception ex)
        //    {
        //        objResponse.transactionStatus = HttpStatusCode.InternalServerError;
        //        objResponse.returnMessage = $"An error occurred: {ex.Message}";
        //    }

        //    return objResponse;
        //}

        public async Task<ResponseEntity<bool>> DeleteAttachment(long attachmentId)
        {
            var response = new ResponseEntity<bool>();
            try
            {
                // Find the attachment in the database
                var attachment = await _dbContext.NewsAndEvents.FindAsync(attachmentId);
                if (attachment != null)
                {

                    var filePath = Path.Combine("wwwroot", attachment.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    // Remove the attachment from the database
                    attachment.ImagePath = null;
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

        public async Task<ResponseEntity<FileStorage>> SaveSingleFile(IFormFile fileInputs, string? AttachmentMasterId, long? CreatedBy)
        {
            var objResponce = new ResponseEntity<FileStorage>();

            if (fileInputs == null)
            {
                objResponce.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                objResponce.returnMessage = "Invalid input parameters.";
                return objResponce;
            }

            try
            {
                if (fileInputs != null)
                {
                    var fileName = fileInputs.FileName;
                    if (fileName.LastIndexOf(@"\") > 0)
                        fileName = fileName.Substring(fileName.LastIndexOf(@"\") + 1);

                    FileInfo fi = new FileInfo(fileName.ToLower());

                    if (GenericUtilities.IsAllowedExtension(fi.Extension))
                    {
                        FileStorageViewModel objImage = new FileStorageViewModel();

                        var actualFileName = Path.GetFileNameWithoutExtension(fileInputs.FileName);

                        objImage.FileExtension = Path.GetExtension(fileInputs.FileName);
                        objImage.FileName = actualFileName;
                        objImage.ContentType = fileInputs.ContentType;
                        objImage.ContentLength = fileInputs.Length;
                        objImage.StorageMode = "LocalServer";

                        using (var memoryStream = new MemoryStream())
                        {
                            await fileInputs.CopyToAsync(memoryStream);
                            objImage.FileData = memoryStream.ToArray();
                        }

                        string folderName = DateTime.UtcNow.ToString("yyyy/MM");
                        objImage.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "FileStorage", "NewsAndEventsImages", folderName);

                        var attachResponse = await _attachmentRepository.SaveAttachment(objImage);

                        if (attachResponse.returnData != null)
                        {
                            attachResponse.returnData.OrgFileName = objImage.FileName;
                            objResponce.returnData = attachResponse.returnData;
                            objResponce.transactionStatus = System.Net.HttpStatusCode.OK;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objResponce.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                objResponce.returnMessage = $"Internal Server Error: {ex.Message}";
            }
            return objResponce;
        }

        public async Task<ResponseEntity<NewsAndEventsPageData>> GetAllNewsEventsData(long Type, int Pagenumber, string civilId, long devicePrimaryID)
        {
            var returnData = new ResponseEntity<NewsAndEventsPageData>();
            try
            {
                int pageSize = 20; // How many items per page
                var ReturnPageData = new NewsAndEventsPageData();
                Pagenumber = Pagenumber != null ? Pagenumber : Pagenumber >= 1 ? Pagenumber : 1;
                var PageData = new List<NewsAndEvent>();
                var DeviceData = _dbContext.DeviceDetails.FirstOrDefault(i => i.DeviceDetailId == devicePrimaryID && !(i.IsForceLogout));
                if (DeviceData != null)
                {
                    if (Pagenumber != null)
                    {
                        PageData = _dbContext.NewsAndEvents.Any(i => i.Active && i.ShowInMobile) ? _dbContext.NewsAndEvents.Where(i => i.Active && i.ShowInMobile).OrderByDescending(i => i.Date).Skip((Pagenumber - 1) * pageSize).Take(pageSize).ToList() : null;
                    }
                    if (PageData != null)
                    {
                        var LookUpMaster = _dbContext.LookupMasters.Where(i => i.Active).ToList();
                        if (Type != 0)//news
                        {
                            PageData = PageData.Where(i => i.Type == Type).ToList();
                        }
                        var ReturnData = PageData.Select
                                (i => new NewsAndEventsData
                                {
                                    Id = i.Id,
                                    Heading = i.Heading,
                                    Description = i.Description,
                                    ImagePath = i.ImagePath,
                                    Type = i.Type != null ? LookUpMaster.FirstOrDefault(c => c.LookUpId == i.Type).LookUpName : null,
                                    Date = i.Date != null ? GenericUtilities.ConvertAndFormatToKuwaitDateTime(i.Date, GenericUtilities.dateTimeFormat) : null,
                                }).ToList();
                        ReturnPageData.NewsOrEventsData = ReturnData;
                        ReturnPageData.NextPageNumber = ReturnPageData.NewsOrEventsData.Any() ? Pagenumber + 1 : 0;
                        ReturnPageData.PageCount = ReturnPageData.NewsOrEventsData.Any() ? ReturnPageData.NewsOrEventsData.Count() : 0;
                        ReturnPageData.IsUnread = _dbContext.NotificationLogs.Where(i => i.MemberCivilId.Trim().ToLower() == civilId.Trim().ToLower() && i.DeviceId == devicePrimaryID).Any(i => !i.IsRead) ? true : false;
                        returnData.returnData = ReturnPageData;
                        returnData.transactionStatus = System.Net.HttpStatusCode.OK;
                    }
                    else
                    {
                        returnData.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                        returnData.returnMessage = "No Items Found";
                    }
                }
                else
                {
                    returnData.transactionStatus = System.Net.HttpStatusCode.Locked;
                    returnData.returnMessage = "Device Is Blocked";
                }
            }
            catch (Exception ex)
            {
                returnData.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                returnData.returnMessage = $"Internal Server Error: {ex.Message}";
            }
            return returnData;
        }

        public async Task<ResponseEntity<List<EventTypes>>> GetAllEventTypes()
        {
            var objResponce = new ResponseEntity<List<EventTypes>>();
            try
            {
                var ReturnData = new List<EventTypes>();
                var data = new List<EventTypes>();
                var retData = _dbContext.LookupMasters.Where(i => i.Active && i.LookUpTypeId == 6).
                    Select(i => new EventTypes
                    {
                        Id = i.LookUpId,
                        TypeName = i.LookUpName,
                    }).ToList();

                var Alldata = new EventTypes
                {
                    Id = 0,
                    TypeName = "All"
                };
                retData.Add(Alldata);
                ReturnData = retData.OrderBy(i => i.Id).ToList();
                objResponce.returnData = ReturnData;
                objResponce.returnMessage = "Success";
                objResponce.transactionStatus = System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                objResponce.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                objResponce.returnMessage = $"Internal Server Error: {ex.Message}";
            }
            return objResponce;
        }
    }
}
