using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.API.APIModel.ViewModel;
using FOKE.Entity.AppInfoSection.ViewModel;
using FOKE.Entity.AppInfoSectionData.DTO;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FOKE.Services.Repository
{
    public class AppInfoSectionRepository : IappInfoSectionRepository
    {
        private readonly FOKEDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAttachmentRepository _attachmentRepository;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;

        public AppInfoSectionRepository(FOKEDBContext FOKEDBContext, IHttpContextAccessor httpContextAccessor)
        {
            this._dbContext = FOKEDBContext;
            this._httpContextAccessor = httpContextAccessor;
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

        public async Task<ResponseEntity<AppinfoSectionViewModel>> AddAppInfoSection(AppinfoSectionViewModel model)
        {
            var retModel = new ResponseEntity<AppinfoSectionViewModel>();
            try
            {

                var InfoExists = _dbContext.AppInfoSections
                        .Any(u => u.SectionType == model.SectionType);
                if (InfoExists)
                {
                    retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                    retModel.returnMessage = "Info Already Added For This Section";
                }
                else
                {
                    var InfoData = new AppInfoSection
                    {
                        SectionType = model.SectionType,
                        HTMLContent = model.HTMLContent,
                        ShowInMobile = model.ShowInMobile,
                        ShowInWebsite = model.ShowInMobile,
                        Active = true,
                        CreatedBy = loggedInUser,//loginned user employee is
                        CreatedDate = DateTime.UtcNow,
                    };
                    await _dbContext.AppInfoSections.AddAsync(InfoData);
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

        public async Task<ResponseEntity<AppinfoSectionViewModel>> UpdateAppInfoSection(AppinfoSectionViewModel model)
        {
            var retModel = new ResponseEntity<AppinfoSectionViewModel>();
            try
            {
                var AppInfoSectionData = _dbContext.AppInfoSections.Where(c => c.Id == model.Id && c.Active == true).FirstOrDefault();
                if (AppInfoSectionData != null)
                {

                    AppInfoSectionData.SectionType = model.SectionType;
                    AppInfoSectionData.HTMLContent = model.HTMLContent;
                    AppInfoSectionData.ShowInMobile = model.ShowInMobile;
                    AppInfoSectionData.ShowInWebsite = model.ShowInWebsite;
                    AppInfoSectionData.UpdatedDate = DateTime.UtcNow;
                    AppInfoSectionData.UpdatedBy = loggedInUser;
                    await _dbContext.SaveChangesAsync();
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                    retModel.returnMessage = "Updated Successfully";
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

        public ResponseEntity<bool> DeleteAppInfo(AppinfoSectionViewModel objModel)
        {
            var retModel = new ResponseEntity<bool>();
            try
            {
                var Infodata = _dbContext.AppInfoSections.Find(objModel.Id);
                if (objModel.DiffId == 1)
                {
                    Infodata.Active = false;
                    _dbContext.Entry(Infodata).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    retModel.returnMessage = "Deactivated";
                    retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    Infodata.Active = true;
                    _dbContext.Entry(Infodata).State = EntityState.Modified;
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

        public ResponseEntity<List<AppinfoSectionViewModel>> GetAllAppInfoData(long? Status)
        {
            var retModel = new ResponseEntity<List<AppinfoSectionViewModel>>();
            try
            {
                var objModel = new List<AppinfoSectionViewModel>();

                var retData = _dbContext.AppInfoSections.ToList();

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

                objModel = retData.Select(c => new AppinfoSectionViewModel()
                {
                    Id = c.Id,
                    SectionType = c.SectionType,
                    SectionTypeString = c.SectionType != null ? _dbContext.LookupMasters.FirstOrDefault(i => i.LookUpId == c.SectionType).LookUpName : null,
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
                retModel.returnData = objModel.OrderByDescending(i => i.Id).ToList();
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = ex.Message;
            }

            return retModel;
        }

        public ResponseEntity<AppinfoSectionViewModel> GetAppInfoDataByID(long Id)
        {
            var retModel = new ResponseEntity<AppinfoSectionViewModel>();
            try
            {
                var OfferData = _dbContext.AppInfoSections.SingleOrDefault(u => u.Id == Id);
                var objModel = new AppinfoSectionViewModel();
                objModel.Id = OfferData.Id;
                objModel.SectionType = OfferData.SectionType;
                objModel.HTMLContent = OfferData.HTMLContent;
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

        public async Task<ResponseEntity<List<SectionTypes>>> GetAllSectionTypes()
        {
            var objResponce = new ResponseEntity<List<SectionTypes>>();
            try
            {
                var ReturnData = new List<SectionTypes>();
                var retData = _dbContext.LookupMasters.Where(i => i.Active && i.LookUpTypeId == 7).
                    Select(i => new SectionTypes
                    {
                        Id = i.LookUpId,
                        TypeName = i.LookUpName,
                    }).ToList();
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

        public async Task<ResponseEntity<AppInfoSectionDto>> GetSectionDataByType(long? Type)
        {
            var objResponce = new ResponseEntity<AppInfoSectionDto>();
            try
            {
                var ReturnData = new AppInfoSectionDto();
                var retData = await _dbContext.AppInfoSections.FirstOrDefaultAsync(i => i.Active && i.SectionType == Type);
                if (retData != null)
                {
                    ReturnData.Id = retData.Id;
                    ReturnData.HTMLContent = retData.HTMLContent;
                    objResponce.returnData = ReturnData;
                    objResponce.returnMessage = "Success";
                    objResponce.transactionStatus = System.Net.HttpStatusCode.OK;
                }
                else
                {
                    objResponce.transactionStatus = System.Net.HttpStatusCode.NoContent;
                    objResponce.returnMessage = $"No Data Found";
                }

            }
            catch (Exception ex)
            {
                objResponce.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                objResponce.returnMessage = $"Internal Server Error: {ex.Message}";
            }
            return objResponce;
        }

        public async Task<ResponseEntity<bool>> UpdateLoginLogOutTime(long Type, long DeviceId)
        {
            var objResponce = new ResponseEntity<bool>();
            try
            {
                var ReturnData = false;
                var retData = await _dbContext.DeviceDetails.FirstOrDefaultAsync(i => i.Active && i.DeviceDetailId == DeviceId);
                if (retData != null)
                {
                    if (Type == 1)
                    {
                        retData.LastOpenDateTime = DateTime.UtcNow;
                        _dbContext.Entry(retData).State = EntityState.Modified;
                        _dbContext.SaveChanges();
                    }
                    else
                    {
                        retData.LastClosedDateTime = DateTime.UtcNow;
                        _dbContext.Entry(retData).State = EntityState.Modified;
                        _dbContext.SaveChanges();
                    }
                    objResponce.returnData = true;
                    objResponce.transactionStatus = System.Net.HttpStatusCode.OK;
                    objResponce.returnMessage = "Success";
                }
                else
                {
                    objResponce.transactionStatus = System.Net.HttpStatusCode.NoContent;
                    objResponce.returnMessage = $"No Data Found";
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
