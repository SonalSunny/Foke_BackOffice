using FOKE.Entity;
using FOKE.Entity.AccountsData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.AccountData
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly IAccountsRepostory _accountsRepository;
        private readonly IDropDownRepository _dropDownRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        public IPagedList<AccountViewModel> pagedListData { get; private set; }

        [BindProperty]
        public long? Statusid { get; set; }

        public IndexModel(IAccountsRepostory accountsRepository, ISharedLocalizer sharedLocalizer, IDropDownRepository dropDownRepository)
        {
            _accountsRepository = accountsRepository;
            _sharedLocalizer = sharedLocalizer;
            _dropDownRepository = dropDownRepository;
        }

        public void OnGet(string isGoBack)
        {
            setPagedListColumns();
            BindDropdowns();
            if (isGoBack?.ToLower() != "y")
            {
                TempData["PRO_FILTER_STATUS"] = null;
            }
        }


        public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc, string nm, string showProject)
        {
            setPagedListColumns();
            pageNo = pn ?? 1;  // Default to page 1
            pageSize = ps ?? 10;  // Default page size
            sortOrder = so;
            sortColumn = sc;
            globalSearch = gs;
            searchField = gsc;
            var Status = TempData.Peek("PRO_FILTER_STATUS");

            Statusid = GenericUtilities.Convert<long?>(Status);
            if (Statusid == null)
            {
                Statusid = 1;
            }

            var response = _accountsRepository.GetAllAccountsData(Statusid);
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

        }

        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();
            objList.Add(new PageListFilterColumns { ColumName = "All", ColumnDescription = _sharedLocalizer.Localize("DD_ALL_TABLE_SEARCH").Value });
            objList.Add(new PageListFilterColumns { ColumName = "CategoryName", ColumnDescription = _sharedLocalizer.Localize("Category").Value });
            objList.Add(new PageListFilterColumns { ColumName = "TotalAmount", ColumnDescription = _sharedLocalizer.Localize("Ammount").Value });
            objList.Add(new PageListFilterColumns { ColumName = "RefNo", ColumnDescription = _sharedLocalizer.Localize("RefNo").Value });
            pageListFilterColumns = objList;
        }

        public JsonResult OnPostDeleteAccount(int? keyid, int? Id)
        {
            var retData = new ResponseEntity<bool>();
            var objModel = new AccountViewModel();
            objModel.Id = Convert.ToInt32(keyid);
            objModel.DiffId = Convert.ToInt32(Id);
            retData = _accountsRepository.DeleteAccountData(objModel);
            return new JsonResult(retData);
        }

        public JsonResult OnPostApplyFilter()
        {

            // Store filter values in TempData
            TempData["PRO_FILTER_STATUS"] = Statusid.ToString();
            return new JsonResult(true);
        }
        //public IActionResult OnPostExportData()
        //{
        //    var Area = TempData.Peek("PRO_FILTER_AREA");
        //    Areaid = GenericUtilities.Convert<long?>(Area);
        //    var empData = _membershipFormRepository.ExportMembersDatatoExcel(Areaid, 0, 0);
        //   // var empData = _membershipFormRepository.GetAllAcceptedMembers(Areaid, 0, 0);
        //    var tempFileName = empData.returnData;
        //    return new JsonResult(new { tFileName = tempFileName, fileName = "AllMembersList.xlsx" });
        //}
    }
}
