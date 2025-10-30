using FOKE.Entity;
using FOKE.Entity.Common;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Entity.Localization.ViewModel;
using FOKE.Localization.Models;
using FOKE.Localization.Services;
using FOKE.Models.PageModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using X.PagedList;

namespace FOKE.Pages.CoreModule.Configurations.Localization.old
{
    public class IndexModel : PagedListBasePageModel
    {

        [BindProperty]
        public string globalSearch { get; set; }
        [BindProperty]
        public string culture { get; set; }
        [BindProperty]
        public string module { get; set; }

        [BindProperty]
        public List<DropDownViewModel> languageList { get; set; }

        [BindProperty]
        public List<DropDownViewModel> moduleList { get; set; }

        private ILocalizationService _localizationService { get; set; }
        public IPagedList<LocalizationResourceModel> pagedListData { get; private set; }

        [BindProperty]
        public List<LocalizationResourceModel> localizationResources { get; set; }


        public IndexModel(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }
        public void OnGet()
        {
            culture = "All";
            module = "All";
            localizationResources = new List<LocalizationResourceModel>();
            BindDropdowns();
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
            return new JsonResult(retData);
        }

        public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc, string cl, string mod)
        {
            pageNo = pn;
            pageSize = ps;
            sortOrder = so;
            sortColumn = sc;
            globalSearch = gs;
            searchField = gsc;
            culture = cl;
            module = mod;

            return new PartialViewResult
            {
                ViewName = "_IndexPartial",
                ViewData = ViewData
            };
        }
        public void BindPagedList()
        {
            var pn = pageNo ?? 1;
            var ps = pageSize ?? 10;


            var abc = pagedListData.ToList();

            localizationResources = new List<LocalizationResourceModel>();
            localizationResources = abc;
            hasPagination = abc.Count() > ps ? true : false;
        }


        private void BindDropdowns()
        {
            var languages = new LocalizationLanguages();
            languageList = new List<DropDownViewModel>();
            languageList.Add(new DropDownViewModel { code = "All", name = "All Languages" });
            languages.Languages.ForEach(c => languageList.Add(new DropDownViewModel { code = c.Culture, name = c.Name }));

            var resources = new ResourceModules();
            moduleList = new List<DropDownViewModel>();
            moduleList.Add(new DropDownViewModel { code = "All", name = "All Modules" });
            resources.Modules.ForEach(c => moduleList.Add(new DropDownViewModel { code = c.ModuleCode, name = c.ModuleName }));
        }



        public class PagedListModel<T>
        {
            public PagedList<T> returnData { get; set; }
        }
    }
}
