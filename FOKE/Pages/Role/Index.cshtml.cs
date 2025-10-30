using FOKE.Entity;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.Role
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly IRoleRepository _roleRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        public IPagedList<RoleViewModel> pagedListData { get; private set; }
        [BindProperty]
        public long? Statusid { get; set; }
        public IndexModel(IRoleRepository roleRepository, ISharedLocalizer sharedLocalizer)
        {
            _roleRepository = roleRepository;
            _sharedLocalizer = sharedLocalizer;
        }
        public void OnGet(string isGoBack)
        {
            setPagedListColumns();
            //  setFilterDropdowns();
            if (isGoBack?.ToLower() != "y")
            {

                TempData["PRO_FILTER_STATUS"] = null;
            }
            //  sortColumn = "Role";
        }


        public JsonResult OnPostApplyFilter()
        {

            // Store filter values in TempData
            TempData["PRO_FILTER_STATUS"] = Statusid.ToString();

            return new JsonResult(true);
        }
        public JsonResult OnPostDeleteRole(int? keyid)
        {
            var retData = new ResponseEntity<bool>();
            var objModel = new RoleViewModel();
            objModel.RoleId = Convert.ToInt32(keyid);
            retData = _roleRepository.DeleteRole(objModel);
            return new JsonResult(retData);
        }
        public IActionResult OnPostExportData()
        {
            var Status = TempData.Peek("PRO_FILTER_STATUS");
            Statusid = GenericUtilities.Convert<long?>(Status);


            if (Statusid == null)
            {
                Statusid = 1;
            }
            var empData = _roleRepository.ExportRoleDatatoExcel(Statusid, "");
            var tempFileName = empData.returnData;
            return new JsonResult(new { tFileName = tempFileName, fileName = "RoleMaster.xlsx" });


        }

        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();
            objList.Add(new PageListFilterColumns { ColumName = "All", ColumnDescription = _sharedLocalizer.Localize("DD_ALL_TABLE_SEARCH").Value });
            objList.Add(new PageListFilterColumns { ColumName = "Role", ColumnDescription = _sharedLocalizer.Localize("ROLE NAME").Value });
            //objList.Add(new PageListFilterColumns { ColumName = "DepartmentName", ColumnDescription = _sharedLocalizer.Localize("DPT_MASTER_NAME").Value });

            pageListFilterColumns = objList;
        }
        public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc, string nm, string showProject)
        {
            //var pn = pageNo ?? 1;
            //var ps = pageSize ?? 10;

            setPagedListColumns();
            pageNo = pn ?? 1;  // Default to page 1
            pageSize = ps ?? 10;  // Default page size
            sortOrder = so;
            sortColumn = sc;
            globalSearch = gs;
            searchField = gsc;
            var Status = TempData.Peek("PRO_FILTER_STATUS");
            Statusid = GenericUtilities.Convert<long?>(Status);
            //if (Statusid == null)
            //{
            //    Statusid = 2;
            //}



            var objList = _roleRepository.GetAllRole(Statusid, gs);
            if (objList != null && objList.transactionStatus == System.Net.HttpStatusCode.OK
         && objList.returnData != null)
            {
                pagedListData = PagedList(objList.returnData);
            }

            return new PartialViewResult
            {
                ViewName = "_IndexPartial",
                ViewData = ViewData
            };
            //if (objList.returnData.Count() == 0 && pn > 1)
            //{
            //    pn = 1;
            //}
            //var sourceData = objList.returnData;
            //try
            //{
            //    if (!string.IsNullOrEmpty(sc))
            //    {
            //        var param = sc;
            //        var propertyInfo = typeof(RoleViewModel).GetProperty(param);

            //        if (so == "0")
            //        {
            //            var orderBy = sourceData.OrderByDescending(x => propertyInfo?.GetValue(x, null));
            //            sourceData = orderBy.ToList();
            //        }
            //        else
            //        {
            //            var orderBy = sourceData.OrderBy(x => propertyInfo?.GetValue(x, null));
            //            sourceData = orderBy.ToList();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{

            //}

            //sortColumn = sc;
            //sortOrder = so;

            //pagedListData = sourceData.ToPagedList(pn, ps);
            //hasPagination = sourceData.Count() > ps ? true : false;

            //pageNo = pn;
            //pageSize = ps;
            //return new PartialViewResult
            //{
            //    ViewName = "_IndexPartial",
            //    ViewData = ViewData
            //};
        }
    }
}





