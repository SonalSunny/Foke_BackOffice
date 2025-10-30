using FOKE.Entity;
using FOKE.Entity.OfferData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.Offers
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly IOfferRepository _offerRepository;
        private readonly IDropDownRepository _dropDownRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        public IPagedList<OfferViewModel> pagedListData { get; private set; }
        #region FilterDeclaration

        [BindProperty]
        public long? Statusid { get; set; }

        #endregion
        public IndexModel(IOfferRepository offerRepository, IDropDownRepository dropDownRepository, ISharedLocalizer sharedLocalizer)
        {
            _offerRepository = offerRepository;
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

        public IActionResult OnGetPagedList(int? pn, int? ps, string so, string sc, string gs, string gsc, string nm, string showProject)
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

            var objResponce = _offerRepository.GetAllOfferData(Statusid);
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
            objList.Add(new PageListFilterColumns { ColumName = "Heading", ColumnDescription = _sharedLocalizer.Localize("OFFER_NAME").Value });
            pageListFilterColumns = objList;
        }

        public JsonResult OnPostDeleteOffer(int? keyid, int? Id)
        {
            var retData = new ResponseEntity<bool>();
            var objModel = new OfferViewModel();
            objModel.OfferId = Convert.ToInt32(keyid);
            objModel.DiffId = Convert.ToInt32(Id);
            retData = _offerRepository.DeleteOffer(objModel);
            return new JsonResult(retData);
        }

        public JsonResult OnPostApplyFilter()
        {

            // Store filter values in TempData
            TempData["PRO_FILTER_STATUS"] = Statusid.ToString();
            return new JsonResult(true);
        }

        public IActionResult OnPostExportData()
        {
            var status = TempData.Peek("PRO_FILTER_STATUS");
            Statusid = GenericUtilities.Convert<long?>(status);

            var empData = _offerRepository.ExportUserDatatoExcel("", Statusid);
            var tempFileName = empData.returnData;
            return new JsonResult(new { tFileName = tempFileName, fileName = "OfferData.xlsx" });
        }
    }
}
