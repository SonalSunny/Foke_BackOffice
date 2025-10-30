using ClosedXML.Excel;
using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.Localization.DTO;
using FOKE.Entity.Localization.ViewModel;
using FOKE.Localization.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Net;

namespace FOKE.Localization.Services
{
    public class LocalizationService : ILocalizationService
    {
        private readonly FOKEDBContext _context;
        private readonly IMemoryCache _cache;
        private readonly IResourceManagerService _resourceManagerService;
        public delegate HtmlString Localizer(string resourceKey, params object[] args);
        private Localizer _localizer;
        public LocalizationService(FOKEDBContext context, IMemoryCache cache, IResourceManagerService resourceManagerService)
        {
            _cache = cache;
            _context = context;
            _resourceManagerService = resourceManagerService;
        }

        public async Task<ResponseEntity<bool>> SyncLanguageResources()
        {
            var objResponce = new ResponseEntity<bool>();
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _context.LocalizationResources.ExecuteUpdateAsync(setters => setters
                            .SetProperty(d => d.Active, false)
                );

                var languageRes = _context.LocalizationResources;
                var resourceData = _resourceManagerService.GetResourceBaseData();
                var languageList = new LocalizationLanguages().Languages;
                var moduleList = new ResourceModules().Modules;

                if (resourceData != null)
                {
                    foreach (var module in moduleList)
                    {
                        var moduleResource = resourceData.Where(c => c.ModuleCode == module.ModuleCode);
                        if (moduleResource != null)
                        {
                            var languageResource = moduleResource.Where(c => languageList.Any(x => x.Culture == c.LanguageCode));
                            foreach (var trn in moduleResource)
                            {
                                var resData = languageRes.Where(c => c.Name == trn.Name && c.Module == module.ModuleCode && c.Culture == trn.LanguageCode).FirstOrDefault();
                                if (resData != null)
                                {
                                    resData.Description = trn.Description;
                                    resData.Value = trn.Value;
                                    resData.Active = true;
                                }
                                else
                                {
                                    _context.LocalizationResources.Add(new LocalizationResource
                                    {
                                        Active = true,
                                        Name = trn.Name,
                                        Value = trn.Value,
                                        Culture = trn.LanguageCode,
                                        Module = module.ModuleCode,
                                        Description = trn.Description,
                                        CustomValue = trn.customValue ?? string.Empty
                                    });
                                }
                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                transaction.Commit();
                _cache.Remove("LanguageResource");
                objResponce.returnData = true;
                objResponce.transactionStatus = System.Net.HttpStatusCode.OK;
                objResponce.returnMessage = "Synced Successfully";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                objResponce.returnData = false;
                objResponce.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                objResponce.returnMessage = "Interval Server Error Occurer";
            }
            return objResponce;
        }
        public ResponseEntity<List<LocalizationResourceModel>> GetLanguageResources(string module)
        {
            var retModel = new ResponseEntity<List<LocalizationResourceModel>>();
            retModel.returnData = new List<LocalizationResourceModel>();
            try
            {
                var list = new List<LocalizationResourceModel>();
                var resourceData = _resourceManagerService.GetResourceBaseData();

                var languageList = new LocalizationLanguages().Languages;
                var moduleList = new ResourceModules().Modules;

                //if (!string.IsNullOrEmpty(language) && language != "All")
                //{
                //    resourceData = resourceData.Where(c => c.Culture == language).ToList();
                //}
                var languageRes = _context.LocalizationResources.ToList();

                if (!string.IsNullOrEmpty(module) && module.ToLower() != "all")
                {
                    resourceData = resourceData.Where(c => c.ModuleCode == module).ToList();
                    moduleList = moduleList.Where(c => c.ModuleCode == module).ToList();
                }

                foreach (var objModule in moduleList)
                {
                    var moduleResource = resourceData?.Where(c => c.ModuleCode == objModule.ModuleCode).ToList();
                    if (moduleResource != null)
                    {
                        var distTransalation = moduleResource.Select(c => c.Name).Distinct();

                        foreach (var transalation in distTransalation)
                        {
                            var localization = moduleResource.Where(c => c.Name == transalation).ToList();
                            var dbLocalization = languageRes.Where(c => c.Name == transalation && c.Module == objModule.ModuleCode).ToList();
                            if (localization != null)
                            {
                                var objRes = new LocalizationResourceModel();
                                objRes.Module = objModule.ModuleCode;
                                objRes.Name = localization[0].Name;
                                objRes.Description = localization[0].Description;
                                objRes.LocalizationArabic = localization.Where(c => c.LanguageCode == "ar-AE").FirstOrDefault()?.Value;
                                objRes.LocalizationEnglish = localization.Where(c => c.LanguageCode == "en-US").FirstOrDefault()?.Value;

                                var dblocalizationArabic = dbLocalization?.Where(c => c.Culture == "ar-AE").FirstOrDefault()?.CustomValue;
                                if (!string.IsNullOrEmpty(dblocalizationArabic))
                                {
                                    objRes.LocalizationArabic = dblocalizationArabic;
                                }
                                var dblocalizationEnglish = dbLocalization?.Where(c => c.Culture == "en-US").FirstOrDefault()?.CustomValue;
                                if (!string.IsNullOrEmpty(dblocalizationEnglish))
                                {
                                    objRes.LocalizationEnglish = dblocalizationEnglish;
                                }
                                list.Add(objRes);
                            }
                        }
                    }
                }
                retModel.transactionStatus = System.Net.HttpStatusCode.OK;
                retModel.returnData = list;
            }
            catch (Exception ex)
            {
                retModel.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                retModel.returnMessage = "Internal Server Error";
            }
            return retModel;
        }
        public ResponseEntity<bool> UpdateLocalizationResource(List<LocalizationResourceModel> localizationResourceModels)
        {

            var objResponce = new ResponseEntity<bool>();
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var languageRes = _context.LocalizationResources.Where(c => c.Active == true).ToList();

                var resourceData = _resourceManagerService.GetResourceBaseData();
                if (resourceData != null)
                {
                    foreach (var res in localizationResourceModels)
                    {
                        var localizedData = resourceData.Where(c => c.Name == res.Name && c.ModuleCode == res.Module).ToList();

                        if (localizedData != null)
                        {
                            foreach (var lData in localizedData)
                            {
                                var customValue = "";
                                if (lData.LanguageCode == "ar-AE")
                                {
                                    customValue = res.LocalizationArabic;
                                }
                                if (lData.LanguageCode == "en-US")
                                {
                                    customValue = res.LocalizationEnglish;
                                }

                                var resData = languageRes.Where(c => c.Culture == lData.LanguageCode && c.Name == lData.Name
                                && c.Module == lData.ModuleCode).FirstOrDefault();
                                if (resData != null)
                                {
                                    if (localizedData != null)
                                    {
                                        resData.Description = lData.Description;
                                        resData.CustomValue = customValue ?? string.Empty;
                                    }
                                }
                                else
                                {
                                    _context.LocalizationResources.Add(new LocalizationResource
                                    {
                                        Active = true,
                                        Name = lData.Name,
                                        Value = lData.Value,
                                        Culture = lData.LanguageCode,
                                        Module = lData.ModuleCode,
                                        Description = lData.Description,
                                        CustomValue = customValue ?? string.Empty
                                    });
                                }
                            }
                        }
                    }
                    _context.SaveChanges();
                }
                transaction.Commit();
                _cache.Remove("LanguageResource");
                objResponce.returnData = true;
                objResponce.transactionStatus = System.Net.HttpStatusCode.OK;
                objResponce.returnMessage = "Updated Successfully";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                objResponce.returnData = false;
                objResponce.transactionStatus = System.Net.HttpStatusCode.InternalServerError;
                objResponce.returnMessage = "Interval Server Error Occurer";
            }
            return objResponce;
        }

        public List<LanguageResourceModel> GetLanguageResourceData()
        {
            var objList = new List<LanguageResourceModel>();
            try
            {
                var resourceData = _context.LocalizationResources.Where(c => c.Active == true).ToList();
                foreach (var objResource in resourceData)
                {
                    objList.Add(new LanguageResourceModel
                    {
                        Culture = objResource.Culture,
                        CustomValue = objResource.CustomValue,
                        Name = objResource.Name,
                        Value = objResource.Value,
                        Module = objResource.Module
                    });
                }
            }
            catch (Exception ex) { }
            return objList;
        }

        public LanguageResourceModel GetStringResource(string resourceKey, string culture)
        {

            var languageResources = _cache.GetOrCreate<List<LanguageResourceModel>>
                (
                    "LanguageResource",
                     cacheEntry =>
                    {
                        cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                        cacheEntry.AbsoluteExpiration = DateTime.UtcNow.AddMinutes(30);
                        cacheEntry.Priority = CacheItemPriority.High;

                        return GetLanguageResourceData();
                    }
                );

            if (languageResources != null)
            {
                return languageResources.Where(c => c.Name == resourceKey && c.Culture == culture).FirstOrDefault();
            }
            return null;
        }

        public ResponseEntity<string> ExportLocalizationDatatoExcel(string search)
        {
            var retModel = new ResponseEntity<string>();
            try
            {
                var objData = GetLanguageResources("");

                if (objData.transactionStatus == HttpStatusCode.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Transalation");
                        worksheet.Cell(1, 1).Value = "Module";
                        worksheet.Cell(1, 2).Value = "Name";
                        worksheet.Cell(1, 3).Value = "Description";
                        worksheet.Cell(1, 4).Value = "LocalizationArabic";
                        worksheet.Cell(1, 5).Value = "LocalizationEnglish";

                        var headerRow = worksheet.Row(1);
                        headerRow.Style.Font.Bold = true;
                        headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;

                        for (int i = 0; i < objData.returnData.Count; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = objData.returnData[i].Module;
                            worksheet.Cell(i + 2, 2).Value = objData.returnData[i].Name;
                            worksheet.Cell(i + 2, 3).Value = objData.returnData[i].Description;
                            worksheet.Cell(i + 2, 4).Value = objData.returnData[i].LocalizationArabic;
                            worksheet.Cell(i + 2, 5).Value = objData.returnData[i].LocalizationEnglish;
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
