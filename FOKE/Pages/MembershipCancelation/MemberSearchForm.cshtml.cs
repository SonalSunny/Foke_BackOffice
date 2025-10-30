using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FOKE.Pages.MembershipCancelation
{
    public class MemberSearchFormModel : PagedListBasePageModel
    {
        private readonly ISharedLocalizer _sharedLocalizer;
        [BindProperty]
        public PostMembershipViewModel inputModel { get; set; }
        private readonly IMembershipFormRepository _membershipFormRepository;
        public MemberSearchFormModel(ISharedLocalizer sharedLocalizer, IMembershipFormRepository membershipFormRepository)
        {
            _sharedLocalizer = sharedLocalizer;
            _membershipFormRepository = membershipFormRepository;
        }
        public void OnGet(string isGoBack)
        {
            setPagedListColumns();
            if (isGoBack?.ToLower() != "y")
            {
                TempData["PRO_FILTER_STATUS"] = null;
            }
        }
        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();
            objList.Add(new PageListFilterColumns { ColumName = "All", ColumnDescription = _sharedLocalizer.Localize("DD_ALL_TABLE_SEARCH").Value });
            objList.Add(new PageListFilterColumns { ColumName = "FullName", ColumnDescription = _sharedLocalizer.Localize("Full Name").Value });
            objList.Add(new PageListFilterColumns { ColumName = "CivilID", ColumnDescription = _sharedLocalizer.Localize("Civil ID").Value });
            objList.Add(new PageListFilterColumns { ColumName = "Passport Number", ColumnDescription = _sharedLocalizer.Localize("Passport Number").Value });
            objList.Add(new PageListFilterColumns { ColumName = "Contact Number", ColumnDescription = _sharedLocalizer.Localize("Contact Number").Value });

            pageListFilterColumns = objList;
        }
        public IActionResult OnGetGetDetails(string keyword, string searchText)
        {
            var response = _membershipFormRepository.GetMemberDetails(keyword, searchText);

            // Return entire response as JSON
            return new JsonResult(response);
        }
    }
}
