using FOKE.Entity;
using FOKE.Entity.CampaignData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FOKE.Pages.FeeCampaign
{
    public class IndexModel : PagedListBasePageModel
    {
        public readonly ICampaignRepository _campaignRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        public int ActiveCampaignCount { get; set; }
        public IPagedList<CampaignViewModel> pagedListData { get; private set; }

        [BindProperty]
        public long? Statusid { get; set; }
        public IndexModel(ISharedLocalizer sharedLocalizer, ICampaignRepository campaignRepository)
        {
            _campaignRepository = campaignRepository;
            _sharedLocalizer = sharedLocalizer;
        }
        //public void OnGet(string isGoBack)
        public async Task OnGetAsync(string isGoBack)
        {
            setPagedListColumns();
            if (isGoBack?.ToLower() != "y")
            {

                TempData["PRO_FILTER_STATUS"] = null;
            }
            ActiveCampaignCount = await _campaignRepository.GetActiveCampaignCountAsync();
        }


        public JsonResult OnPostApplyFilter()
        {
            TempData["PRO_FILTER_STATUS"] = Statusid.ToString();

            return new JsonResult(true);
        }
        public void setPagedListColumns()
        {
            pageListFilterColumns = new List<PageListFilterColumns>();
            var objList = new List<PageListFilterColumns>();
            objList.Add(new PageListFilterColumns { ColumName = "All", ColumnDescription = _sharedLocalizer.Localize("DD_ALL_TABLE_SEARCH").Value });
            objList.Add(new PageListFilterColumns { ColumName = "CampaignName", ColumnDescription = _sharedLocalizer.Localize("CampaignName").Value });


            pageListFilterColumns = objList;
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
                Statusid = 2;
            }


            var objList = _campaignRepository.GetAllCampaign(Statusid);
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
        }
        public async Task<IActionResult> OnPostCreateCollectionSheetAsync(long campaignId)
        {
            var result = await _campaignRepository.CreateCollectionSheetAsync(campaignId);
            if (result)
                return new JsonResult(new { success = true });
            else
                return new JsonResult(new { success = false, message = "Maximum 2 active campaigns allowed. Please deactivate one before continuing." });
        }
        public async Task<IActionResult> OnPostToggleCampaignStatusAsync(long campaignId, string action)
        {
            try
            {
                bool activate = action == "activate";
                var result = await _campaignRepository.ToggleCampaignStatusAsync(campaignId, activate);
                if (!result)
                    return BadRequest("Status update failed.");

                return new JsonResult(new { success = true });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // e.g., "Only 2 campaigns can be active"
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}





