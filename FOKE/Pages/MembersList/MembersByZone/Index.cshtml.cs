using FOKE.Entity;
using FOKE.Entity.Common;
using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
namespace FOKE.Pages.MembersList.MembersByZone
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly IMembershipFormRepository _membershipFormRepository;
        private readonly IDropDownRepository _dropDownRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        public IPagedList<PostMembershipViewModel> pagedListData { get; private set; }
        //public IPagedList<MembershipViewModel> pagedListData { get; private set; }
        [BindProperty]
        public long? Zoneid { get; set; }
        [BindProperty]
        public long? ProffessionID { get; set; }
        [BindProperty]
        public long? WorkPlaceId { get; set; }
        [BindProperty]
        public string? Radio { get; set; }
        [BindProperty]
        public long? Searchfield { get; set; }
        [BindProperty]
        public List<DropDownViewModel> ProffessionList { get; set; }
        public List<DropDownViewModel> WorkPlaceList { get; set; }
        public List<DropDownViewModel> ZoneList { get; set; }
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


        public JsonResult OnPostApplyFilter()
        {

            TempData["PRO_FILTER_PROFESSION"] = ProffessionID.ToString();
            TempData["PRO_FILTER_ZONE"] = Zoneid.ToString();
            TempData["PRO_FILTER_WORKPLACE"] = WorkPlaceId.ToString(); ;

            return new JsonResult(true);
        }



        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();
            //  objList.Add(new PageListFilterColumns { ColumName = "All", ColumnDescription = _sharedLocalizer.Localize("DD_ALL_TABLE_SEARCH").Value });
            objList.Add(new PageListFilterColumns { ColumName = "Name", ColumnDescription = _sharedLocalizer.Localize("FullName").Value });
            objList.Add(new PageListFilterColumns { ColumName = "CivilId", ColumnDescription = _sharedLocalizer.Localize("CivilID").Value });
            objList.Add(new PageListFilterColumns { ColumName = "PassportNo", ColumnDescription = _sharedLocalizer.Localize("PassportNumber").Value });
            objList.Add(new PageListFilterColumns { ColumName = "ContactNo", ColumnDescription = _sharedLocalizer.Localize("ContactNumber").Value });


            pageListFilterColumns = objList;
        }
        public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc, string nm, string showProject, long? searchfield, string? Value)
        {
            Searchfield = searchfield;
            Radio = Value;
            setPagedListColumns();
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
            if (!searchfield.HasValue)
            {
                var areaTemp = TempData.Peek("PRO_FILTER_ZONE");
                searchfield = GenericUtilities.Convert<long?>(areaTemp);
            }
            var userIdStr = User.FindFirst("UserId")?.Value ?? User.Identity.Name;
            long.TryParse(userIdStr, out long userId);



            // ✅ Call repo with area and user filter
            var response = _membershipFormRepository.GetAllMembersByZone(searchfield, ProffessionID, WorkPlaceId, userId, Radio);

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

        private void BindDropdowns()
        {
            WorkPlaceList = _dropDownRepository.GetWorkPlace();
            ProffessionList = _dropDownRepository.GetProffession();
            // ZoneList = _dropDownRepository.GetZone();
            var userIdStr = User.FindFirst("UserId")?.Value ?? User.Identity.Name;

            if (!string.IsNullOrEmpty(userIdStr) && long.TryParse(userIdStr, out long userId))
            {
                ZoneList = _dropDownRepository.GetZonesByUser(userId);

                // Auto-select the only area if there's just one
                //if (AreaList.Count == 1)
                //{
                //    Areaid = AreaList.First().keyID;
                //}
            }
            else
            {
                ZoneList = new List<DropDownViewModel>();
            }
        }
        public async Task<IActionResult> OnPostSaveRemark(long? id, string? inputValue, string? radio, long? searchfield)
        {
            var retData = await _membershipFormRepository.UpdateRemark(id, inputValue);
            return RedirectToPage("/MembersList/MembersByZone/Index", new
            {
                Value = radio,
                searchfield = searchfield
            });
        }
    }
}
