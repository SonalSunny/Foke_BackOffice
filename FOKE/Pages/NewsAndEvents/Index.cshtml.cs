using FOKE.Entity;
using FOKE.Entity.NewsAndEventsData.ViewModel;
using FOKE.Localization;
//using FOKE.Entity.NewsAndEvents.ViewModel;
//using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.NewsAndEvents
{
    public class IndexModel : PagedListBasePageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public readonly INewsAndEventsRepository _newsandeventsRepository;
        private readonly ISharedLocalizer _sharedLocalizer;

        public IPagedList<NewsAndEventsViewModel> pagedListData { get; private set; }
        #region FilterDeclaration

        [BindProperty]
        public string NewsHeading { get; set; }
        [BindProperty]
        public string? Description { get; set; }
        [BindProperty]
        public DateTime? NewsDate { get; set; }
        [BindProperty]
        public string? ImageURL { get; set; }
        [BindProperty]
        public long? CreatedEmployee { get; set; }
        [BindProperty]
        public long? Statusid { get; set; }


        #endregion
        public IndexModel(ISharedLocalizer sharedLocalizer, INewsAndEventsRepository newsRepository)
        {
            _sharedLocalizer = sharedLocalizer;
            _newsandeventsRepository = newsRepository;
        }
        public void OnGet(string isGoBack)
        {
            setPagedListColumns();
            if (isGoBack?.ToLower() != "y")
            {
                TempData["PRO_FILTER_NEWS_NAME"] = "";
                TempData["PRO_FILTER_STATUS"] = null;
                TempData["PRO_FILTER_NEWS_DATE"] = null;
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

            var NewsHeading = TempData.Peek("PRO_FILTER_NEWS_NAME");
            var Status = TempData.Peek("PRO_FILTER_STATUS");
            //  var FromDate = TempData.Peek("PRO_FILTER_NEWS_DATE");
            //  NewsDate = GenericUtilities.Convert<DateTime?>(FromDate);

            Statusid = GenericUtilities.Convert<long?>(Status);
            if (Statusid == null)
            {
                Statusid = 1;
            }

            var objResponce = _newsandeventsRepository.GetAllNewsAndEvents(Statusid);
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

        public JsonResult OnPostDeleteNews(int? keyid, int? Id)
        {
            var retData = new ResponseEntity<bool>();
            var objModel = new NewsAndEventsViewModel();
            objModel.Id = Convert.ToInt32(keyid);
            retData = _newsandeventsRepository.DeleteNewsAndEvents(objModel);
            return new JsonResult(retData);
        }

        public JsonResult OnPostApplyFilter()
        {
            // Store filter values in TempData
            TempData["PRO_FILTER_STATUS"] = Statusid.ToString();

            TempData["PRO_FILTER_NEWS_DATE"] = NewsDate;
            return new JsonResult(true);
        }

        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();
            objList.Add(new PageListFilterColumns { ColumName = "All", ColumnDescription = _sharedLocalizer.Localize("DD_ALL_TABLE_SEARCH").Value });
            objList.Add(new PageListFilterColumns { ColumName = "Heading", ColumnDescription = _sharedLocalizer.Localize("News/Events Heading").Value });

            pageListFilterColumns = objList;
        }

        public IActionResult OnPostExportData()
        {
            var empData = "";
            var tempFileName = "";
            return new JsonResult(new { tFileName = tempFileName, fileName = "NewsAndEventsMaster.xlsx" });
        }
    }
}
