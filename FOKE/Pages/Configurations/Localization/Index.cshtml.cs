//using Castle.DynamicProxy;
using FOKE.Entity;
using FOKE.Entity.Common;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Entity.Localization.ViewModel;
using FOKE.Localization;
using FOKE.Localization.Models;
using FOKE.Localization.Services;
using FOKE.Models.PageModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using X.PagedList;

namespace FOKE.Pages.Configurations.Localization
{
    public class IndexModel : PagedListBasePageModel
    {
        #region PageProperties
        [BindProperty]
        public string module { get; set; }

        [BindProperty]
        public List<DropDownViewModel> moduleList { get; set; }
        public IPagedList<LocalizationResourceModel> pagedListData { get; private set; }

        [BindProperty]
        public List<LocalizationResourceModel> localizationResources { get; set; }

        private readonly ILocalizationService _localizationService;
        private readonly ISharedLocalizer _sharedLocalizer;

        [BindProperty]
        public string moduleCode { get; set; }

        #endregion PageProperties

        public IndexModel(ILocalizationService localizationService, ISharedLocalizer sharedLocalizer)
        {
            _localizationService = localizationService;
            _sharedLocalizer = sharedLocalizer;
        }
        public void OnGet()
        {
            TempData["TRANSLATION_MODULE"] = null;
            module = "";
            sortColumn = "Module";
            localizationResources = new List<LocalizationResourceModel>();
            BindDropdowns();
            setPagedListColumns();
        }
        public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc, string nm)
        {
            setPagedListColumns();
            pageNo = pn;
            pageSize = ps;
            sortOrder = so;
            sortColumn = sc;
            globalSearch = gs;
            searchField = gsc;

            var department = TempData.Peek("TRANSLATION_MODULE");

            moduleCode = GenericUtilities.Convert<string>(department);

            var objResponce = _localizationService.GetLanguageResources(moduleCode);
            if (objResponce.transactionStatus == System.Net.HttpStatusCode.OK)
            {
                pagedListData = PagedList(objResponce.returnData);
                localizationResources = pagedListData.ToList();
            }

            return new PartialViewResult
            {
                ViewName = "_IndexPartial",
                ViewData = ViewData
            };
        }

        public JsonResult OnPostApplyFilter()
        {
            TempData["TRANSLATION_MODULE"] = moduleCode;
            return new JsonResult(true);
        }

        public async Task<JsonResult> OnPostUpdateTranslations()
        {
            var retData = new ResponseEntity<UserSettingsViewModel>();
            if (btnSubmit == "btnSync")
            {
                var response = await _localizationService.SyncLanguageResources();
                 if (response.transactionStatus != HttpStatusCode.OK)
                {
                    pageErrorMessage = response.returnMessage;
                }
                else
                {
                    ModelState.Clear();
                    IsSuccessReturn = true;
                    retData.transactionStatus = response.transactionStatus;
                    retData.returnMessage = response.returnMessage;
                }
            }
            else if (btnSubmit == "btnSave")
            {
                var response = _localizationService.UpdateLocalizationResource(localizationResources);
                if (response.transactionStatus != HttpStatusCode.OK)
                {
                    retData.returnMessage = response.returnMessage;
                }
                else
                {
                    ModelState.Clear();
                    IsSuccessReturn = true;
                    retData.transactionStatus = response.transactionStatus;
                    retData.returnMessage = response.returnMessage;
                }
            }
            else if (btnSubmit == "btnApplyFilter")
            {
                TempData["TRANSLATION_MODULE"] = moduleCode;
                retData.transactionStatus = HttpStatusCode.OK;
            }
            return new JsonResult(retData);
        }
        public JsonResult OnPostExportData()
        {
            var empData = _localizationService.ExportLocalizationDatatoExcel("");
            var tempFileName = empData.returnData;
            return new JsonResult(new { tFileName = tempFileName, fileName = "LocalizationData.xlsx" });
        }

        private void BindDropdowns()
        {
            var resources = new ResourceModules();
            moduleList = new List<DropDownViewModel>();
            //moduleList.Add(new DropdownViewModel { code = "All", name = "All Modules" });
            resources.Modules.ForEach(c => moduleList.Add(new DropDownViewModel { code = c.ModuleCode, name = c.ModuleName }));
        }

        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();
            objList.Add(new PageListFilterColumns { ColumName = "All", ColumnDescription = _sharedLocalizer.Localize("DD_ALL_TABLE_SEARCH").Value });
            objList.Add(new PageListFilterColumns { ColumName = "Module", ColumnDescription = _sharedLocalizer.Localize("TRANSLATION_MODULE").Value });
            objList.Add(new PageListFilterColumns { ColumName = "Name", ColumnDescription = _sharedLocalizer.Localize("TRANSLATION_FIELD_NAME").Value });
            objList.Add(new PageListFilterColumns { ColumName = "Description", ColumnDescription = _sharedLocalizer.Localize("TRANSLATION_FIELD_DESCRIPTION").Value });
            objList.Add(new PageListFilterColumns { ColumName = "LocalizationEnglish", ColumnDescription = _sharedLocalizer.Localize("TRANSLATION_LANGUAGE_ENGLISH").Value });
            objList.Add(new PageListFilterColumns { ColumName = "LocalizationArabic", ColumnDescription = _sharedLocalizer.Localize("TRANSLATION_LANGUAGE_ARABIC").Value });
            pageListFilterColumns = objList;
        }
    }
}
