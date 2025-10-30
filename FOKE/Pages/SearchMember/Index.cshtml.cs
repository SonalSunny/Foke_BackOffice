using FOKE.Entity.ProfessionData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.SearchMember
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly IProfessionRepository _professionRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        public IPagedList<ProfessionViewModel> pagedListData { get; private set; }
        [BindProperty]
        public string? ProfessionName { get; set; }
        [BindProperty]
        public string? Description { get; set; }
        [BindProperty]
        public long? Statusid { get; set; }
        public IndexModel(IProfessionRepository professionRepository, ISharedLocalizer sharedLocalizer)
        {
            _professionRepository = professionRepository;
            _sharedLocalizer = sharedLocalizer;
        }
        public void OnGet(string isGoBack)
        {
            setPagedListColumns();
            if (isGoBack?.ToLower() != "y")
            {

                //TempData["PRO_FILTER_STATUS"] = null;
                //TempData["PRO_FILTER_PROFESSION"] = null;
            }
        }


        public JsonResult OnPostApplyFilter()
        {

            //TempData["PRO_FILTER_PROFESSION"] = ProfessionName;
            //TempData["PRO_FILTER_STATUS"] = Statusid.ToString();

            return new JsonResult(true);
        }
        //public JsonResult OnPostDeleteProfession(int? keyid)
        //{
        //    var retData = new ResponseEntity<bool>();
        //    var objModel = new ProfessionViewModel();
        //    objModel.ProfessionId = Convert.ToInt32(keyid);
        //    retData = _professionRepository.DeleteProfession(objModel);
        //    return new JsonResult(retData);
        //}


        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();
            //  objList.Add(new PageListFilterColumns { ColumName = "All", ColumnDescription = _sharedLocalizer.Localize("DD_ALL_TABLE_SEARCH").Value });
            objList.Add(new PageListFilterColumns { ColumName = "FullName", ColumnDescription = _sharedLocalizer.Localize("FullName").Value });
            objList.Add(new PageListFilterColumns { ColumName = "CivilID", ColumnDescription = _sharedLocalizer.Localize("CivilID").Value });
            objList.Add(new PageListFilterColumns { ColumName = "PassportNumber", ColumnDescription = _sharedLocalizer.Localize("PassportNumber").Value });
            objList.Add(new PageListFilterColumns { ColumName = "ContactNumber", ColumnDescription = _sharedLocalizer.Localize("ContactNumber").Value });


            pageListFilterColumns = objList;
        }
        public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc, string nm, string showProject)
        {
            setPagedListColumns();
            //   pageNo = pn ?? 1;  // Default to page 1
            //   pageSize = ps ?? 10;  // Default page size
            //   sortOrder = so;
            //   sortColumn = sc;
            //   globalSearch = gs;
            //   searchField = gsc;
            //   var Status = TempData.Peek("PRO_FILTER_STATUS");
            //   Statusid = GenericUtilities.Convert<long?>(Status);



            //   var objList = _professionRepository.GetAllProfessions(Statusid, gs);
            //   if (objList != null && objList.transactionStatus == System.Net.HttpStatusCode.OK
            //&& objList.returnData != null)
            //   {
            //       pagedListData = PagedList(objList.returnData);
            //   }

            return new PartialViewResult
            {
                ViewName = "_IndexPartial",
                ViewData = ViewData
            };
        }
    }
}





