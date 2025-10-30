using FOKE.Entity;
using FOKE.Entity.Common;
using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.ExpiredMembersList
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
        public List<DropDownViewModel> ProffessionList { get; set; }
        public List<DropDownViewModel> WorkPlaceList { get; set; }
        public List<DropDownViewModel> AreaList { get; set; }
        public IndexModel(IMembershipFormRepository membershipFormRepository, ISharedLocalizer sharedLocalizer, IDropDownRepository dropDownRepository)
        {
            _membershipFormRepository = membershipFormRepository;
            _sharedLocalizer = sharedLocalizer;
            _dropDownRepository = dropDownRepository;
        }
        public void OnGet(string isGoBack)
        {
            setPagedListColumns();
            BindDropdowns();
            if (isGoBack?.ToLower() != "y")
            {
                TempData["PRO_FILTER_AREA"] = null;
                TempData["PRO_FILTER_PROFESSION"] = null;
                TempData["PRO_FILTER_WORKPLACE"] = null;
            }
        }
        public class ExportRequestModel
        {
            public string GlobalSearch { get; set; }
            public string GlobalSearchColumn { get; set; }
        }

        public JsonResult OnPostApplyFilter()
        {

            TempData["PRO_FILTER_PROFESSION"] = ProffessionID.ToString();
            TempData["PRO_FILTER_AREA"] = Areaid.ToString();
            TempData["PRO_FILTER_WORKPLACE"] = WorkPlaceId.ToString(); ;

            return new JsonResult(true);
        }


        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();

            objList.Add(new PageListFilterColumns { ColumName = "Name", ColumnDescription = _sharedLocalizer.Localize("Full Name").Value });
            objList.Add(new PageListFilterColumns { ColumName = "ReferenceNumber", ColumnDescription = _sharedLocalizer.Localize("Member ID").Value });
            objList.Add(new PageListFilterColumns { ColumName = "CivilId", ColumnDescription = _sharedLocalizer.Localize("Civil ID").Value });
            objList.Add(new PageListFilterColumns { ColumName = "Email", ColumnDescription = _sharedLocalizer.Localize("Email").Value });
            objList.Add(new PageListFilterColumns { ColumName = "PhoneNo", ColumnDescription = _sharedLocalizer.Localize("Contact Number").Value });
            pageListFilterColumns = objList;
        }


        public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc, string nm, string showProject)
        {
            setPagedListColumns();
            BindDropdowns();
            pageNo = pn ?? 1;  // Default to page 1
            pageSize = ps ?? 10;  // Default page size
            sortOrder = so;
            sortColumn = sc;
            globalSearch = gs;
            searchField = gsc;
            var Area = TempData.Peek("PRO_FILTER_AREA");
            Areaid = GenericUtilities.Convert<long?>(Area);
            var Proffesion = TempData.Peek("PRO_FILTER_PROFESSION");
            ProffessionID = GenericUtilities.Convert<long?>(Proffesion);
            var Workplace = TempData.Peek("PRO_FILTER_WORKPLACE");
            WorkPlaceId = GenericUtilities.Convert<long?>(Workplace);



            var response = _membershipFormRepository.GetAllExpiredMembers(Areaid, ProffessionID, WorkPlaceId);
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
            AreaList = _dropDownRepository.GetArea();
        }
        public IActionResult OnPostExportData([FromBody] ExportRequestModel data)
        {
            var Area = TempData.Peek("PRO_FILTER_AREA");
            var Areaid = GenericUtilities.Convert<long?>(Area);

            // Use the received data
            var empData = _membershipFormRepository.ExportExpiredMembersDatatoExcel(
                Areaid, null, null, data.GlobalSearchColumn, data.GlobalSearch
            );

            var tempFileName = empData.returnData;

            return new JsonResult(new { tFileName = tempFileName, fileName = "UnpaidMembersList.xlsx" });
        }

        public IActionResult OnPostExportPdf([FromBody] ExportRequestModel request)
        {
            var Area = TempData.Peek("PRO_FILTER_AREA");
            var Areaid = GenericUtilities.Convert<long?>(Area);
            var data = _membershipFormRepository.ExportExpiredMemberstoPdf(Areaid, null, null, request.GlobalSearchColumn, request.GlobalSearch);
            return new JsonResult(new { tFileName = data.returnData, fileName = "UnpaidMembersList.pdf" });
        }
    }
}
