using FOKE.Entity;
using FOKE.Entity.DepartmentMaster.ViewModel;
//using FOKE.Entity.Department.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.Department
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly IDepartmentRepository _departmentRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        //private readonly IDropDownRepository _dropDownRepository;



        public IPagedList<DepartmentViewModel> pagedListData { get; private set; }

        #region FilterDeclaration

        [BindProperty]
        public long? Statusid { get; set; }
        [BindProperty]
        public string? DeptName { get; set; }


        #endregion
        public IndexModel(IDepartmentRepository departmentRepository, ISharedLocalizer sharedLocalizer)
        {
            //_dropDownRepository = dropDownRepository;
            _sharedLocalizer = sharedLocalizer;
            _departmentRepository = departmentRepository;
        }
        public void OnGet(string isGoBack)
        {
            setPagedListColumns();
            if (isGoBack?.ToLower() != "y")
            {
                TempData["PRO_FILTER_DEPT"] = null;
                TempData["PRO_FILTER_STATUS"] = null;
            }

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

            //  var DepartmentName = TempData.Peek("PRO_FILTER_DEPT");
            var Status = TempData.Peek("PRO_FILTER_STATUS");


            Statusid = GenericUtilities.Convert<long?>(Status);
            if (Statusid == null)
            {
                Statusid = 1;
            }

            //var DepNameId = !string.IsNullOrEmpty(DepartmentName?.ToString()) ? Convert.ToInt64(DepartmentName) : 0;
            var DepartmentName = GenericUtilities.Convert<string>(TempData.Peek("PRO_FILTER_DEPT"));

            var objResponce = _departmentRepository.GetAllDepartments(Statusid, DepartmentName);
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

        public JsonResult OnPostDeleteDepartment(int? keyid, int? Id)
        {
            var retData = new ResponseEntity<bool>();
            var objModel = new DepartmentViewModel();
            objModel.DepartmentId = Convert.ToInt32(keyid);
            objModel.DiffId = Convert.ToInt32(Id);
            retData = _departmentRepository.DeleteDepartment(objModel);
            return new JsonResult(retData);
        }
















        //public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc,
        //    string nm)
        //{
        //    setPagedListColumns();
        //    pageNo = pn;
        //    pageSize = ps;
        //    sortOrder = so;
        //    sortColumn = sc;
        //    globalSearch = gs;
        //    searchField = gsc;
        //    var fromDate = TempData.Peek("FILTER_DATE_FROM");
        //    var toDate = TempData.Peek("FILTER_DATE_TO");
        //    var Status = TempData.Peek("PRO_FILTER_STATUS");
        //    FromDate = GenericUtilities.Convert<DateTime?>(fromDate);
        //    ToDate = GenericUtilities.Convert<DateTime?>(toDate);
        //    Statusid = GenericUtilities.Convert<long?>(Status);

        //    //var objResponce = _departmentRepository.GetAllDepartments(Statusid, FromDate, ToDate);
        //    //if (objResponce.transactionStatus == System.Net.HttpStatusCode.OK)
        //    //{

        //    //    pagedListData = PagedList(objResponce.returnData);
        //    //}

        //    return new PartialViewResult
        //    {
        //        ViewName = "_IndexPartial",
        //        ViewData = ViewData
        //    };
        //}
        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();
            objList.Add(new PageListFilterColumns { ColumName = "All", ColumnDescription = "All" });
            objList.Add(new PageListFilterColumns { ColumName = "DepartmentName", ColumnDescription = "Department" });
            pageListFilterColumns = objList;
        }
        public JsonResult OnPostApplyFilter()
        {
            TempData["PRO_FILTER_DEPT"] = DeptName;
            // Store filter values in TempData
            TempData["PRO_FILTER_STATUS"] = Statusid.ToString();

            return new JsonResult(true);
        }


    }
}
