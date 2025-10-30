using FOKE.Entity;
using FOKE.Entity.Common;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using X.PagedList;


namespace FOKE.Pages.UserLoginLog
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly IUserRepository _userRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        private readonly IDropDownRepository _dropDownRepository;


        public IPagedList<UserLoginLogViewModel> pagedListData { get; private set; }
        public string EmployeeName { get; set; }
        [BindProperty]
        public long? Roleid { get; set; }
        [BindProperty]
        public DateTime? FromDate { get; set; }
        [BindProperty]
        public DateTime? ToDate { get; set; }
        public List<DropDownViewModel> RoleList { get; set; }


        public IndexModel(IUserRepository userRepository, IDropDownRepository dropDownRepository, ISharedLocalizer sharedLocalizer)
        {
            _userRepository = userRepository;
            _sharedLocalizer = sharedLocalizer;
            _dropDownRepository = dropDownRepository;
        }

        public void OnGet(string isGoBack)
        {
            setPagedListColumns();
            BindDropdowns();
            if (isGoBack?.ToLower() != "y")
            {
                TempData["PRO_FILTER_ROLE"] = null;
                TempData["FILTER_DATE_FROM"] = "";
                TempData["FILTER_DATE_TO"] = "";
            }
            sortColumn = "EmployeeName";
        }

        public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc, string nm, string showProject)
        {
            setPagedListColumns();
            BindDropdowns();
            pageNo = pn;
            pageSize = ps;
            sortOrder = so;
            sortColumn = sc;
            globalSearch = gs;
            searchField = gsc;
            var Role = TempData.Peek("PRO_FILTER_ROLE");
            var FromDate = TempData.Peek("FILTER_DATE_FROM");
            var ToDate = TempData.Peek("FILTER_DATE_TO");

            Roleid = GenericUtilities.Convert<long?>(Role);
            var fromDate = GenericUtilities.Convert<DateTime?>(FromDate);
            var toDate = GenericUtilities.Convert<DateTime?>(ToDate);

            var retModel = _userRepository.GetLoginSessions(Convert.ToInt64(User.Identity.Name), fromDate, toDate, Roleid);
            if (retModel.transactionStatus == HttpStatusCode.OK)
            {
                pagedListData = PagedList(retModel.returnData);
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
            objList.Add(new PageListFilterColumns { ColumName = "EmployeeName", ColumnDescription = _sharedLocalizer.Localize("Employee Name").Value });
            objList.Add(new PageListFilterColumns { ColumName = "EmployeeCode", ColumnDescription = _sharedLocalizer.Localize("Employee Code").Value });
            pageListFilterColumns = objList;
        }

        private void BindDropdowns()
        {
            RoleList = _dropDownRepository.GetRole();
        }

        public JsonResult OnPostApplyFilter()
        {
            // Store filter values in TempData
            TempData["PRO_FILTER_ROLE"] = Roleid.ToString();
            TempData["FILTER_DATE_FROM"] = FromDate?.ToString("yyyy-MM-dd"); // Format the DateTime if it's not null
            TempData["FILTER_DATE_TO"] = ToDate?.ToString("yyyy-MM-dd"); // Format the DateTime if it's not null

            return new JsonResult(true);
        }

        public IActionResult OnPostExportData()
        {
            var FromDate = TempData.Peek("FILTER_DATE_FROM");
            var ToDate = TempData.Peek("FILTER_DATE_TO");
            var Role = TempData.Peek("PRO_FILTER_ROLE");

            Roleid = GenericUtilities.Convert<long?>(Role);
            var fromDate = GenericUtilities.Convert<DateTime?>(FromDate);
            var toDate = GenericUtilities.Convert<DateTime?>(ToDate);

            var empData = _userRepository.ExportUserSessionDatatoExcel("", Roleid, fromDate, toDate);
            var tempFileName = empData.returnData;
            return new JsonResult(new { tFileName = tempFileName, fileName = "UserSessionData.xlsx" });
        }
    }
}
