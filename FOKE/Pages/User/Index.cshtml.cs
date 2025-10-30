using FOKE.Entity;
using FOKE.Entity.Common;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.User
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly IUserRepository _userRepository;
        private readonly IDropDownRepository _dropDownRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        public IPagedList<UserViewModel> pagedListData { get; private set; }
        #region FilterDeclaration

        [BindProperty]
        public string EmployeeName { get; set; }
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public long? Roleid { get; set; }
        [BindProperty]
        public long? CreatedEmployee { get; set; }
        [BindProperty]
        public long? Statusid { get; set; }
        [BindProperty]
        public DateTime? PasswordExpirydateFrom { get; set; }
        [BindProperty]
        public DateTime? PasswordExpirydateTo { get; set; }
        public List<DropDownViewModel> EmployeeList { get; set; }
        public List<DropDownViewModel> RoleList { get; set; }
        #endregion
        public IndexModel(IUserRepository userRepository, IDropDownRepository dropDownRepository, ISharedLocalizer sharedLocalizer)
        {
            _userRepository = userRepository;
            _dropDownRepository = dropDownRepository;
            _sharedLocalizer = sharedLocalizer;
        }
        public void OnGet(string isGoBack)
        {
            setPagedListColumns();
            BindDropdowns();
            if (isGoBack?.ToLower() != "y")
            {
                TempData["FILTER_DATE_FROM"] = "";
                TempData["FILTER_DATE_TO"] = "";
                TempData["PRO_FILTER_STATUS"] = null;
                TempData["PRO_FILTER_ROLE"] = null;
            }
            sortColumn = "EmployeeName";
        }
        public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc,
            string nm, string showProject)
        {
            setPagedListColumns();
            pageNo = pn;
            pageSize = ps;
            sortOrder = so;
            sortColumn = sc;
            globalSearch = gs;
            searchField = gsc;
            var FromDate = TempData.Peek("FILTER_DATE_FROM");
            var ToDate = TempData.Peek("FILTER_DATE_TO");
            var Status = TempData.Peek("PRO_FILTER_STATUS");
            var Role = TempData.Peek("PRO_FILTER_ROLE");

            //PasswordExpirydateFrom = GenericUtilities.Convert<string>(UserName);
            Statusid = GenericUtilities.Convert<long?>(Status);
            if (Statusid == null)
            {
                Statusid = 1;
            }
            Roleid = GenericUtilities.Convert<long>(Role);
            PasswordExpirydateFrom = GenericUtilities.Convert<DateTime?>(FromDate);
            PasswordExpirydateTo = GenericUtilities.Convert<DateTime?>(ToDate);

            var objResponce = _userRepository.GetAllUsers(Statusid, Roleid, PasswordExpirydateFrom, PasswordExpirydateTo);
            if (objResponce.transactionStatus == System.Net.HttpStatusCode.OK)
            {

                pagedListData = PagedList(objResponce.returnData);
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
            objList.Add(new PageListFilterColumns { ColumName = "Username", ColumnDescription = _sharedLocalizer.Localize("USER_NAME").Value });
            objList.Add(new PageListFilterColumns { ColumName = "EmployeeName", ColumnDescription = _sharedLocalizer.Localize("EMPLOYEE").Value });
            objList.Add(new PageListFilterColumns { ColumName = "EmployeeNumber", ColumnDescription = _sharedLocalizer.Localize("EMPLOYEE_NUMBER").Value });
            pageListFilterColumns = objList;
        }
        public JsonResult OnPostDeleteUser(int? keyid, int? Id)
        {
            var retData = new ResponseEntity<bool>();
            var objModel = new UserViewModel();
            objModel.UserId = Convert.ToInt32(keyid);
            objModel.DiffId = Convert.ToInt32(Id);
            retData = _userRepository.DeleteUser(objModel);
            return new JsonResult(retData);
        }

        private void BindDropdowns()
        {
            //EmployeeList = _dropDownRepository.GetEmployee();
            RoleList = _dropDownRepository.GetRole();

        }
        public JsonResult OnPostApplyFilter()
        {

            // Store filter values in TempData
            TempData["PRO_FILTER_STATUS"] = Statusid.ToString();
            TempData["PRO_FILTER_ROLE"] = Roleid.ToString();
            TempData["FILTER_DATE_FROM"] = PasswordExpirydateFrom?.ToString("yyyy-MM-dd"); // Format the DateTime if it's not null
            TempData["FILTER_DATE_TO"] = PasswordExpirydateTo?.ToString("yyyy-MM-dd"); // Format the DateTime if it's not null

            return new JsonResult(true);
        }
        public IActionResult OnPostExportData()
        {
            var FromDate = TempData.Peek("FILTER_DATE_FROM");
            var ToDate = TempData.Peek("FILTER_DATE_TO");
            var Role = TempData.Peek("PRO_FILTER_ROLE");
            var status = TempData.Peek("PRO_FILTER_STATUS");

            Roleid = GenericUtilities.Convert<long?>(Role);
            PasswordExpirydateFrom = GenericUtilities.Convert<DateTime?>(FromDate);
            PasswordExpirydateTo = GenericUtilities.Convert<DateTime?>(ToDate);
            Statusid = GenericUtilities.Convert<long?>(status);

            var empData = _userRepository.ExportUserDatatoExcel("", Statusid, Roleid, PasswordExpirydateFrom, PasswordExpirydateTo);
            var tempFileName = empData.returnData;
            return new JsonResult(new { tFileName = tempFileName, fileName = "UserMaster.xlsx" });
        }
    }
}
