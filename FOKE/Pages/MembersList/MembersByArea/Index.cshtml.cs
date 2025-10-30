using FOKE.Entity;
using FOKE.Entity.Common;
using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.MembersList.MembersByArea
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly IMembershipFormRepository _membershipFormRepository;
        private readonly IDropDownRepository _dropDownRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        public IPagedList<PostMembershipViewModel> pagedListData { get; private set; }

        [BindProperty]
        public long? Areaid { get; set; }
        [BindProperty]
        public long? ProffessionID { get; set; }
        [BindProperty]
        public long? WorkPlaceId { get; set; }
        [BindProperty]
        public long? UnitId { get; set; }
        [BindProperty]
        public long? ZoneId { get; set; }
        [BindProperty]
        public long? DepartmentId { get; set; }
        [BindProperty]
        public string? Radio { get; set; }
        [BindProperty]
        public long? Searchfield { get; set; }
        [BindProperty]
        public List<DropDownViewModel> ProffessionList { get; set; }
        public List<DropDownViewModel> WorkPlaceList { get; set; }
        public List<DropDownViewModel> AreaList { get; set; }
        public List<DropDownViewModel> DepartmentList { get; set; }
        public List<DropDownViewModel> UnitList { get; set; }
        public List<DropDownViewModel> Zonelist { get; set; }
        public IndexModel(IMembershipFormRepository membershipFormRepository, ISharedLocalizer sharedLocalizer, IDropDownRepository dropDownRepository)
        {
            _membershipFormRepository = membershipFormRepository;
            _sharedLocalizer = sharedLocalizer;
            _dropDownRepository = dropDownRepository;
        }
        public void OnGet(string? Value, long? searchfield)
        {
            BindDropdowns();
            Radio = Value ?? "Unpaid";
            Searchfield = searchfield;

        }

        public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc, string nm, string showProject, long? searchfield, string? Value)
        {
            Searchfield = searchfield;
            Radio = Value;
            pageNo = pn ?? 1;
            pageSize = ps ?? 10;
            sortOrder = so;
            sortColumn = sc;
            globalSearch = gs;
            searchField = gsc;
            var Proffesion = TempData.Peek("PRO_FILTER_PROFESSION");
            ProffessionID = GenericUtilities.Convert<long?>(Proffesion);
            var Workplace = TempData.Peek("PRO_FILTER_WORKPLACE");
            WorkPlaceId = GenericUtilities.Convert<long?>(Workplace);
            var Unitid = TempData.Peek("PRO_FILTER_DEPT");
            UnitId = GenericUtilities.Convert<long?>(Unitid);
            var Department = TempData.Peek("PRO_FILTER_UNIT");
            DepartmentId = GenericUtilities.Convert<long?>(Department);
            var Zone = TempData.Peek("PRO_FILTER_ZONE");
            ZoneId = GenericUtilities.Convert<long?>(Zone);
            if (!searchfield.HasValue)
            {
                var areaTemp = TempData.Peek("PRO_FILTER_AREA");
                searchfield = GenericUtilities.Convert<long?>(areaTemp);
            }
            var userIdStr = User.FindFirst("UserId")?.Value ?? User.Identity.Name;
            long.TryParse(userIdStr, out long userId);

            //✅ Call repo with area and user filter
            var response = _membershipFormRepository.GetAllMembersByArea(searchfield, ProffessionID, WorkPlaceId, UnitId, DepartmentId, ZoneId, userId, Radio);

            if (response.transactionStatus == System.Net.HttpStatusCode.OK)
            {
                pagedListData = PagedList(response.returnData);
            }

            return new PartialViewResult
            {
                ViewName = "_IndexPartial",
                ViewData = ViewData
            };
        }

        public JsonResult OnPostApplyFilter()
        {

            TempData["PRO_FILTER_PROFESSION"] = ProffessionID.ToString();
            TempData["PRO_FILTER_AREA"] = Areaid.ToString();
            TempData["PRO_FILTER_WORKPLACE"] = WorkPlaceId.ToString();
            TempData["PRO_FILTER_DEPT"] = DepartmentId.ToString();
            TempData["PRO_FILTER_ZONE"] = ZoneId.ToString();
            TempData["PRO_FILTER_UNIT"] = UnitId.ToString();

            return new JsonResult(true);
        }

        private void BindDropdowns()
        {
            WorkPlaceList = _dropDownRepository.GetWorkPlace();
            ProffessionList = _dropDownRepository.GetProffession();
            DepartmentList = _dropDownRepository.GetDepartmentList();
            UnitList = _dropDownRepository.GetUnit();
            Zonelist = _dropDownRepository.GetZone();
            var userIdStr = User.FindFirst("UserId")?.Value ?? User.Identity.Name;

            if (!string.IsNullOrEmpty(userIdStr) && long.TryParse(userIdStr, out long userId))
            {
                AreaList = _dropDownRepository.GetAreasByUser(userId);

                // Auto-select the only area if there's just one
                //if (AreaList.Count == 1)
                //{
                //    Areaid = AreaList.First().keyID;
                //}
            }
            else
            {
                AreaList = new List<DropDownViewModel>();
            }

        }
        public async Task<IActionResult> OnPostSaveRemark(long? id, string? inputValue, string? radio, long? searchfield)
        {
            var retData = await _membershipFormRepository.UpdateRemark(id, inputValue);
            return RedirectToPage("/MembersList/MembersByArea/Index", new
            {
                Value = radio,
                searchfield = searchfield
            });
        }

    }
}
























