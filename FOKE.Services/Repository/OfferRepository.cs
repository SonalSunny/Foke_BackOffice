using ClosedXML.Excel;
using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.API.APIModel.ViewModel;
using FOKE.Entity.FileUpload.DTO;
using FOKE.Entity.FileUpload.ViewModel;
using FOKE.Entity.OfferData.DTO;
using FOKE.Entity.OfferData.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class OfferRepository : IOfferRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAttachmentRepository _attachmentRepository;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;

        public OfferRepository(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor, IAttachmentRepository attachmentRepository)
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
            }

            _attachmentRepository = attachmentRepository;
        }

        public async Task<ResponseEntity<OfferViewModel>> AddOffer(OfferViewModel model)
        {
            var retModel = new ResponseEntity<OfferViewModel>();
            try
            {

                var userExists = _dbContext.Offers
                        .Any(u => u.Heading == model.Heading);
                if (userExists)
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                    retModel.returnMessage = "Offer Already Exist";
                }
                else
                {
                    if (model.Image != null)
                    {
                        var FileInputData = await SaveSingleFile(model.Image, null, loggedInUser);
                        model.ImagePath = FileInputData.returnData.FilePath;
                        model.FileStorageId = FileInputData.returnData.FileStorageId;
                    }
                    var Offer = new Offer
                    {
                        Heading = model.Heading,
                        Description = model.Description,
                        ShowInMobile = model.ShowInMobile,
                        ShowInWebsite = model.ShowInMobile,
                        ImagePath = model.ImagePath,
                        FileStorageId = model.FileStorageId,
                        Active = true,
                        CreatedBy = loggedInUser,//loginned user employee is
                        CreatedDate = DateTime.UtcNow,
                    };
                    await _dbContext.Offers.AddAsync(Offer);
                    await _dbContext.SaveChangesAsync();
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Offer Added Successfully";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error";
            }
            return retModel;
        }

        public async Task<ResponseEntity<OfferViewModel>> UpdateOffer(OfferViewModel model)
        {
            var retModel = new ResponseEntity<OfferViewModel>();
            try
            {
                var OfferData = _dbContext.Offers.Where(c => c.OfferId == model.OfferId && c.Active == true).FirstOrDefault();
                if (OfferData != null)
                {
                    var OfferExists = _dbContext.Offers
                         .Any(u => u.OfferId != model.OfferId && u.Heading == model.Heading);
                    if (OfferExists)
                    {
                        retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                        retModel.returnMessage = "offer Already Exist";
                    }
                    else
                    {
                        if (model.Image != null)
                        {
                            var FileStorageData = _dbContext.FileStorages.FirstOrDefault(i => i.FileStorageId == OfferData.FileStorageId);
                            if (FileStorageData != null)
                            {
                                if (System.IO.File.Exists(FileStorageData.StorageMode))
                                {
                                    System.IO.File.Delete(FileStorageData.StorageMode);
                                }
                                _dbContext.FileStorages.Remove(FileStorageData);
                                _dbContext.SaveChanges();
                            }
                            var FileInputData = await SaveSingleFile(model.Image, null, loggedInUser);
                            OfferData.ImagePath = FileInputData.returnData.FilePath;
                            OfferData.FileStorageId = FileInputData.returnData.FileStorageId;
                        }

                        OfferData.Heading = model.Heading;
                        OfferData.Description = model.Description;
                        OfferData.ShowInMobile = model.ShowInMobile;
                        OfferData.ShowInWebsite = model.ShowInWebsite;
                        OfferData.ImagePath = OfferData.ImagePath;
                        OfferData.FileStorageId = OfferData.FileStorageId;
                        OfferData.UpdatedDate = DateTime.UtcNow;
                        OfferData.UpdatedBy = loggedInUser;
                        await _dbContext.SaveChangesAsync();
                        retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                        retModel.returnMessage = "Updated Successfully";
                    }
                }
                else
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Offer does not exist";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error";
            }
            return retModel;
        }

        public ResponseEntity<bool> DeleteOffer(OfferViewModel objModel)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var Offerdata = _dbContext.Offers.Find(objModel.OfferId);
                if (objModel.DiffId == 1)
                {
                    Offerdata.Active = false;
                    _dbContext.Entry(Offerdata).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    retModel.returnMessage = "Deactivated";
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    Offerdata.Active = true;
                    _dbContext.Entry(Offerdata).State = EntityState.Modified;
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

        public ResponseEntity<List<OfferViewModel>> GetAllOfferData(long? Status)
        {
            var retModel = new ResponseEntity<List<OfferViewModel>>();
            try
            {
                var objModel = new List<OfferViewModel>();

                var retData = _dbContext.Offers.Include(i => i.FileStorage).ToList();

                if (Status.HasValue)
                {
                    if (Status == 2)
                    {
                        retData = retData.Where(c => c.Active == true || c.Active == false).ToList();
                    }
                    else if (Status == 1)
                    {
                        retData = retData.Where(c => c.Active).ToList();
                    }
                    else if (Status == 0)
                    {
                        retData = retData.Where(c => !c.Active).ToList();
                    }
                }

                objModel = retData.Select(c => new OfferViewModel()
                {
                    OfferId = c.OfferId,
                    Heading = c.Heading,
                    ImagePath = c.ImagePath,
                    ImageName = c.FileStorageId != null ? c.FileStorage.OrgFileName : null,
                    Description = c.Description,
                    ShowInMobile = c.ShowInMobile,
                    ShowInWebsite = c.ShowInWebsite,
                    Active = c.Active,
                    CreatedDate = c.CreatedDate,
                    CreatedBy = _dbContext.Users
                        .Where(u => u.UserId == c.CreatedBy)
                        .Select(u => u.UserId)
                        .FirstOrDefault(),
                    CreatedUsername = _dbContext.Users.FirstOrDefault(e => e.UserId == c.CreatedBy).UserName
                }).ToList();

                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel.OrderBy(i => i.OfferId).ToList();
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message;
            }

            return retModel;
        }

        public ResponseEntity<OfferViewModel> GetOfferByID(long OfferID)
        {
            var retModel = new ResponseEntity<OfferViewModel>();
            try
            {
                var OfferData = _dbContext.Offers.Include(c => c.FileStorage).SingleOrDefault(u => u.OfferId == OfferID);
                var objModel = new OfferViewModel();
                objModel.OfferId = OfferData.OfferId;
                objModel.Heading = OfferData.Heading;
                objModel.Description = OfferData.Description;
                objModel.ImagePath = OfferData.ImagePath;
                objModel.ImageName = OfferData.FileStorageId != null ? OfferData.FileStorage.OrgFileName : null;
                objModel.AttachmentAny = OfferData.ImagePath != null ? true : false;
                objModel.ShowInMobile = OfferData.ShowInMobile;
                objModel.ShowInWebsite = OfferData.ShowInWebsite;
                objModel.Active = OfferData.Active;
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }

        public ResponseEntity<string> ExportUserDatatoExcel(string search, long? statusid)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                var objData = GetAllOfferData(statusid);

                if (objData.transactionStatus == HttpStatusCode.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Offers For Members");
                        worksheet.Cell(1, 1).Value = "Sl No";
                        worksheet.Cell(1, 2).Value = "Offer";
                        worksheet.Cell(1, 3).Value = "Description";
                        worksheet.Cell(1, 4).Value = "Show in Website";
                        worksheet.Cell(1, 5).Value = "Show in Mobile";
                        worksheet.Cell(1, 6).Value = "Created By";
                        worksheet.Cell(1, 7).Value = "Created Date";
                        worksheet.Cell(1, 8).Value = "Status";

                        var headerRow = worksheet.Row(1);
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                        for (int i = 0; i < objData.returnData.Count; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = i + 1;
                            worksheet.Cell(i + 2, 2).Value = objData.returnData[i].Heading;
                            worksheet.Cell(i + 2, 3).Value = objData.returnData[i].Description;
                            if (objData.returnData[i].ShowInWebsite)
                            {
                                worksheet.Cell(i + 2, 4).Value = "Yes";
                            }
                            else
                            {
                                worksheet.Cell(i + 2, 4).Value = "No";
                            }
                            if (objData.returnData[i].ShowInMobile)
                            {
                                worksheet.Cell(i + 2, 5).Value = "Yes";
                            }
                            else
                            {
                                worksheet.Cell(i + 2, 5).Value = "No";
                            }
                            worksheet.Cell(i + 2, 7).Value = objData.returnData[i].CreatedUsername;
                            worksheet.Cell(i + 2, 8).Value = objData.returnData[i].CreatedDate;
                            if (objData.returnData[i].Active)
                            {
                                worksheet.Cell(i + 2, 9).Value = "Active";
                            }
                            else
                            {
                                worksheet.Cell(i + 2, 9).Value = "Inactive";
                            }
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
                        objImage.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "FileStorage", "OfferImages", folderName);

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

        public async Task<ResponseEntity<List<OfferData>>> GetAllOffers()
        {
            var returnData = new ResponseEntity<List<OfferData>>();
            try
            {
                var OfferData = new OfferData();
                var OfferDetails = _dbContext.Offers.Where(i => i.Active && i.ShowInMobile).ToList();
                if (OfferDetails != null)
                {
                    var ReturnData = OfferDetails.Select
                        (i => new OfferData
                        {
                            OfferId = i.OfferId,
                            OfferHeader = i.Heading,
                            OfferDescription = i.Description,
                            ImagePath = i.ImagePath,
                            OfferDate = i.CreatedDate != null ? GenericUtilities.ConvertAndFormatToKuwaitDateTime(i.CreatedDate, GenericUtilities.dateTimeFormat) : null,
                        }).ToList();
                    returnData.returnData = ReturnData.OrderBy(i => i.OfferId).ToList();
                    returnData.transactionStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    returnData.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                    returnData.returnMessage = "No offers Found";
                }
            }
            catch (Exception ex)
            {
                returnData.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                returnData.returnMessage = $"Internal Server Error: {ex.Message}";
            }
            return returnData;
        }
    }
}
