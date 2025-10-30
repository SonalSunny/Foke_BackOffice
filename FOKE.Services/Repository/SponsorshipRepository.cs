using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.FileUpload.DTO;
using FOKE.Entity.FileUpload.ViewModel;
using FOKE.Entity.SponsorshipData.DTO;
using FOKE.Entity.SponsorshipData.ViewModel;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class SponsorshipRepository : ISponsorshipRepository
    {

        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAttachmentRepository _attachmentRepository;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;

        public SponsorshipRepository(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor, IAttachmentRepository attachmentRepository)
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

        public async Task<ResponseEntity<SponsorshipViewModel>> AddSponsor(SponsorshipViewModel model)
        {
            var retModel = new ResponseEntity<SponsorshipViewModel>();
            try
            {

                if (model == null || string.IsNullOrEmpty(model.SponsorshipName))
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Invalid input parameters.";
                    return retModel;
                }
                else
                {
                    if (model.Image != null)
                    {
                        var FileInputData = await SaveSingleFile(model.Image, null, loggedInUser);
                        model.ImagePath = FileInputData.returnData.FilePath;
                        model.FileStorageId = FileInputData.returnData.FileStorageId;
                    }
                    var sponsorData = new Sponsorship
                    {
                        SponsorshipName = model.SponsorshipName,
                        ContactPerson = model.ContactPerson,
                        MobileNo = model.MobileNo,
                        Email = model.Email,
                        CampaignId = model.CampaignId,
                        SponsorShipLabel = model.SponsorShipLabel,
                        SortOrder = model.SortOrder,
                        ImageUrl = model.ImagePath,
                        FileStorageId = model.FileStorageId,
                        Notes = model.Notes,
                        Active = true,
                        CreatedBy = loggedInUser,//loginned user employee is
                        CreatedDate = DateTime.UtcNow,
                    };
                    await _dbContext.Sponsorships.AddAsync(sponsorData);
                    await _dbContext.SaveChangesAsync();
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Added Successfully";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error";
            }
            return retModel;
        }

        public async Task<ResponseEntity<SponsorshipViewModel>> UpdateSponsor(SponsorshipViewModel model)
        {
            var retModel = new ResponseEntity<SponsorshipViewModel>();
            try
            {
                var SponsorData = _dbContext.Sponsorships.Where(c => c.SponsorshipId == model.SponsorshipId && c.Active == true).FirstOrDefault();
                if (SponsorData != null)
                {
                    var sponsorData = _dbContext.Sponsorships
                         .Any(u => u.SponsorshipId != model.SponsorshipId && u.SponsorShipLabel == model.SponsorShipLabel);
                    if (sponsorData)
                    {
                        retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                        retModel.returnMessage = "Sponsor Name Already Exist";
                    }
                    else
                    {
                        if (model.Image != null)
                        {
                            var FileStorageData = _dbContext.FileStorages.FirstOrDefault(i => i.FileStorageId == SponsorData.FileStorageId);
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
                            SponsorData.ImageUrl = FileInputData.returnData.FilePath;
                            SponsorData.FileStorageId = FileInputData.returnData.FileStorageId;
                        }
                        SponsorData.SponsorshipName = model.SponsorshipName;
                        SponsorData.ContactPerson = model.ContactPerson;
                        SponsorData.MobileNo = model.MobileNo;
                        SponsorData.Email = model.Email;
                        SponsorData.CampaignId = model.CampaignId;
                        SponsorData.SponsorShipLabel = model.SponsorShipLabel;
                        SponsorData.SortOrder = model.SortOrder;
                        SponsorData.Notes = model.Notes;
                        SponsorData.UpdatedDate = DateTime.UtcNow;
                        SponsorData.UpdatedBy = loggedInUser;

                        await _dbContext.SaveChangesAsync();
                        retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                        retModel.returnMessage = "Updated Successfully";
                    }
                }
                else
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.BadRequest;
                    retModel.returnMessage = "Data does not exist";
                }
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Server Error";
            }
            return retModel;
        }

        public ResponseEntity<bool> DeleteSponsor(SponsorshipViewModel objModel)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var SponsorData = _dbContext.Sponsorships.Find(objModel.SponsorshipId);
                if (objModel.DiffId == 1)
                {
                    SponsorData.Active = false;
                    _dbContext.Entry(SponsorData).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    retModel.returnMessage = "Deactivated";
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    SponsorData.Active = true;
                    _dbContext.Entry(SponsorData).State = EntityState.Modified;
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

        public ResponseEntity<List<SponsorshipViewModel>> GetAllSponsorData(long? Status)
        {
            var retModel = new ResponseEntity<List<SponsorshipViewModel>>();
            try
            {
                var objModel = new List<SponsorshipViewModel>();

                var retData = _dbContext.Sponsorships.Include(c => c.Campaign).ToList();

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

                objModel = retData.Select(c => new SponsorshipViewModel()
                {
                    SponsorshipId = c.SponsorshipId,
                    SponsorshipName = c.SponsorshipName,
                    ContactPerson = c.ContactPerson,
                    Email = c.Email,
                    MobileNo = c.MobileNo,
                    CampaignName = c.CampaignId != null ? c.Campaign.CampaignName : null,
                    SortOrder = c.SortOrder,
                    ImagePath = c.ImageUrl,
                    Active = c.Active,
                    CreatedDate = c.CreatedDate,
                    CreatedBy = _dbContext.Users
                        .Where(u => u.UserId == c.CreatedBy)
                        .Select(u => u.UserId)
                        .FirstOrDefault(),
                    CreatedUsername = _dbContext.Users.FirstOrDefault(e => e.UserId == c.CreatedBy).UserName
                }).ToList();

                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel.OrderByDescending(i => i.SponsorshipId).ToList();
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message;
            }

            return retModel;
        }

        public ResponseEntity<SponsorshipViewModel> GetSponsorByID(long SponsorId)
        {
            var retModel = new ResponseEntity<SponsorshipViewModel>();
            try
            {
                var SponsorData = _dbContext.Sponsorships.Include(c => c.FileStorage).SingleOrDefault(u => u.SponsorshipId == SponsorId);
                var objModel = new SponsorshipViewModel();
                objModel.SponsorshipId = SponsorData.SponsorshipId;
                objModel.SponsorshipName = SponsorData.SponsorshipName;
                objModel.ContactPerson = SponsorData.ContactPerson;
                objModel.MobileNo = SponsorData.MobileNo;
                objModel.Email = SponsorData.Email;
                objModel.CampaignId = SponsorData.CampaignId;
                objModel.SponsorShipLabel = SponsorData.SponsorShipLabel;
                objModel.SortOrder = SponsorData.SortOrder;
                objModel.Notes = SponsorData.Notes;
                objModel.ImagePath = SponsorData.ImageUrl;
                objModel.ImageName = SponsorData.FileStorageId != null ? SponsorData.FileStorage.OrgFileName : null;
                objModel.AttachmentAny = SponsorData.ImageUrl != null ? true : false;
                objModel.Active = SponsorData.Active;
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = objModel;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
            }
            return retModel;
        }

        //public ResponseEntity<string> ExportUserDatatoExcel(string search, long? statusid)
        //{
        //    var retModel = new ResponseEntity<string>();
        //    try
        //    {
        //        var objData = GetAllOfferData(statusid);

        //        if (objData.transactionStatus == HttpStatusCode.OK)
        //        {
        //            using (var workbook = new XLWorkbook())
        //            {
        //                var worksheet = workbook.Worksheets.Add("Offers For Members");
        //                worksheet.Cell(1, 1).Value = "Sl No";
        //                worksheet.Cell(1, 2).Value = "Offer";
        //                worksheet.Cell(1, 3).Value = "Description";
        //                worksheet.Cell(1, 4).Value = "Show in Website";
        //                worksheet.Cell(1, 5).Value = "Show in Mobile";
        //                worksheet.Cell(1, 6).Value = "Created By";
        //                worksheet.Cell(1, 7).Value = "Created Date";
        //                worksheet.Cell(1, 8).Value = "Status";

        //                var headerRow = worksheet.Row(1);
        //                headerRow.Style.Font.Bold = true;
        //                headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

        //                for (int i = 0; i < objData.returnData.Count; i++)
        //                {
        //                    worksheet.Cell(i + 2, 1).Value = i + 1;
        //                    worksheet.Cell(i + 2, 2).Value = objData.returnData[i].Heading;
        //                    worksheet.Cell(i + 2, 3).Value = objData.returnData[i].Description;
        //                    if (objData.returnData[i].ShowInWebsite)
        //                    {
        //                        worksheet.Cell(i + 2, 4).Value = "Yes";
        //                    }
        //                    else
        //                    {
        //                        worksheet.Cell(i + 2, 4).Value = "No";
        //                    }
        //                    if (objData.returnData[i].ShowInMobile)
        //                    {
        //                        worksheet.Cell(i + 2, 5).Value = "Yes";
        //                    }
        //                    else
        //                    {
        //                        worksheet.Cell(i + 2, 5).Value = "No";
        //                    }
        //                    worksheet.Cell(i + 2, 7).Value = objData.returnData[i].CreatedUsername;
        //                    worksheet.Cell(i + 2, 8).Value = objData.returnData[i].CreatedDate;
        //                    if (objData.returnData[i].Active)
        //                    {
        //                        worksheet.Cell(i + 2, 9).Value = "Active";
        //                    }
        //                    else
        //                    {
        //                        worksheet.Cell(i + 2, 9).Value = "Inactive";
        //                    }
        //                }

        //                using (var stream = new MemoryStream())
        //                {
        //                    workbook.SaveAs(stream);
        //                    stream.Position = 0;
        //                    byte[] fileBytes = stream.ToArray();
        //                    retModel.returnData = GenericUtilities.SetReportData(fileBytes, ".xlsx");
        //                    retModel.transactionStatus = HttpStatusCode.OK;
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        retModel.transactionStatus = HttpStatusCode.InternalServerError;
        //    }
        //    return retModel;
        //}

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
                        objImage.FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "FileStorage", "SponsorLogo", folderName);

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
    }
}
