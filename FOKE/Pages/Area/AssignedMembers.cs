using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;


namespace FOKE.Pages.Area
{
    public class AssignedMembersModel : PagedListBasePageModel
    {
        [BindProperty(SupportsGet = true)]
        public PostMembershipViewModel inputModel { get; set; }
        private readonly ISharedLocalizer _sharedLocalizer;
        private readonly IMembershipFormRepository _membershipFormRepository;
        private readonly IAreaRepository _areaRepository;

        public AssignedMembersModel(ISharedLocalizer sharedLocalizer, IMembershipFormRepository membershipFormRepository, IAreaRepository areaRepository)
        {
            _sharedLocalizer = sharedLocalizer;
            _membershipFormRepository = membershipFormRepository;
            _areaRepository = areaRepository;
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
        //public IActionResult OnGetGetDetails(string keyword, string column,long area)
        //{
        //    var response = _areaRepository.GetMemberDetails(keyword, column,area);

        //    // Return entire response as JSON
        //    return new JsonResult(response);
        //}

        // AJAX handler method

        public IActionResult OnGetGetDetails(string keyword, string searchText, long areaId)
        {
            var members = _areaRepository.GetMembersAssignedToArea(areaId, keyword, searchText);
            return new JsonResult(new { returnData = members });
        }







    }





}
