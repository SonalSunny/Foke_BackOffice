using FOKE.Entity;
using FOKE.Entity.ContactUs.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.ClientEnquiery
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly IcontactUsRepository _contactUsRepo;
        private readonly IDropDownRepository _dropDownRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        public IPagedList<ClientEnquieryViewModel> pagedListData { get; private set; }
        #region FilterDeclaration

        [BindProperty]
        public long? Statusid { get; set; }

        #endregion
        public IndexModel(IcontactUsRepository contactUsRepo, IDropDownRepository dropDownRepository, ISharedLocalizer sharedLocalizer)
        {
            _contactUsRepo = contactUsRepo;
            _dropDownRepository = dropDownRepository;
            _sharedLocalizer = sharedLocalizer;
        }

        public void OnGet(string isGoBack)
        {
            setPagedListColumns();
            if (isGoBack?.ToLower() != "y")
            {
                TempData["PRO_FILTER_STATUS"] = null;
            }
        }

        public async Task<IActionResult> OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc, string nm, string showProject)
        {
            setPagedListColumns();
            pageNo = pn;
            pageSize = ps;
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

            var objResponce = await _contactUsRepo.GetAllClientEnquiery(Statusid);
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
            objList.Add(new PageListFilterColumns { ColumName = "Name", ColumnDescription = _sharedLocalizer.Localize("MEMBER_NAME").Value });
            objList.Add(new PageListFilterColumns { ColumName = "ContactNo", ColumnDescription = _sharedLocalizer.Localize("CONTACT_NO").Value });
            pageListFilterColumns = objList;
        }

        public async Task<JsonResult> OnPostDeleteEnquiery(int? keyid, int? Id)
        {
            var retData = new ResponseEntity<bool>();
            var objModel = new ClientEnquieryViewModel();
            objModel.Id = Convert.ToInt32(keyid);
            objModel.DiffId = Convert.ToInt32(Id);
            retData = await _contactUsRepo.DeleteEnquiery(objModel);
            return new JsonResult(retData);
        }

        public async Task<JsonResult> OnPostResolveIssue(int? Id, string? Comment)
        {
            var returnData = await _contactUsRepo.ResolveIssue(Id, Comment);
            return new JsonResult(returnData);
        }

        public JsonResult OnPostApplyFilter()
        {
            TempData["PRO_FILTER_STATUS"] = Statusid.ToString();
            return new JsonResult(true);
        }

        public async Task<IActionResult> OnPostExportData()
        {
            var status = TempData.Peek("PRO_FILTER_STATUS");
            Statusid = GenericUtilities.Convert<long?>(status);

            var empData = await _contactUsRepo.ExportClientRequestDatatoExcel("", Statusid);
            var tempFileName = empData.returnData;
            return new JsonResult(new { tFileName = tempFileName, fileName = "ClientEnquieryData.xlsx" });
        }
    }
}
