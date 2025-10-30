using FOKE.Entity.ProjectConfiguration.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.ProjectConfiguration
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly IProjectConfigurationRepository _projectconfigurationRepository;
        private readonly IDropDownRepository _dropDownRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        public IPagedList<ConfigurationViewModel> pagedListData { get; private set; }
        public IndexModel(IProjectConfigurationRepository ProjectConfiqRepostory, IDropDownRepository dropDownRepository, ISharedLocalizer sharedLocalizer)
        {
            _projectconfigurationRepository = ProjectConfiqRepostory;
            _dropDownRepository = dropDownRepository;
            _sharedLocalizer = sharedLocalizer;
        }
        public void OnGet(string isGoBack)
        {
            setPagedListColumns();
            sortColumn = "CONFIGKEY";
        }
        public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc, string nm)
        {
            setPagedListColumns();
            pageNo = pn ?? 1;  // Default to page 1
            pageSize = ps ?? 10;  // Default page size
            sortOrder = so;
            sortColumn = sc;
            globalSearch = gs;
            searchField = gsc;

            var objResponse = _projectconfigurationRepository.GetAllConfigs();

            if (objResponse != null && objResponse.transactionStatus == System.Net.HttpStatusCode.OK
                && objResponse.returnData != null)
            {
                pagedListData = PagedList(objResponse.returnData);
            }

            return new PartialViewResult
            {
                ViewName = "_IndexPartial",
                ViewData = ViewData
            };
        }
        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();
            objList.Add(new PageListFilterColumns { ColumName = "All", ColumnDescription = _sharedLocalizer.Localize("DD_ALL_TABLE_SEARCH").Value });
            objList.Add(new PageListFilterColumns { ColumName = "CONFIGKEY", ColumnDescription = _sharedLocalizer.Localize("CONFIGKEY").Value });
            objList.Add(new PageListFilterColumns { ColumName = "CONFIGCODE", ColumnDescription = _sharedLocalizer.Localize("CONFIGCODE").Value });
            objList.Add(new PageListFilterColumns { ColumName = "Description", ColumnDescription = _sharedLocalizer.Localize("CONFIG_DESCRIPTION").Value });
            objList.Add(new PageListFilterColumns { ColumName = "Value", ColumnDescription = _sharedLocalizer.Localize("CONFIG_VALUE").Value });
            pageListFilterColumns = objList;
        }
    }
}
