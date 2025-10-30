using FOKE.Entity.Common;
using FOKE.Entity.DashBoard;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FOKE.Pages.FeeCollectionReport
{
    public class FeeCollectionModel : PagedListBasePageModel
    {
        private readonly IDropDownRepository _dropDownRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        private readonly IFeeCollectionReport _reportRepository;
        [BindProperty]
        public long? FeeCampaign { get; set; }
        public int PaidCount { get; set; }
        public int UnpaidCount { get; set; }
        public List<DropDownViewModel> CampaignList { get; set; }
        public FeeCollectionModel(ISharedLocalizer sharedLocalizer, IDropDownRepository dropDownRepository, IFeeCollectionReport reportRepository)
        {

            _sharedLocalizer = sharedLocalizer;
            _dropDownRepository = dropDownRepository;
            _reportRepository = reportRepository;
        }
        public void OnGet()
        {
            BindDropdowns();
            if (CampaignList != null && CampaignList.Count > 0)
            {
                FeeCampaign = CampaignList.First().keyID;
            }
        }
        private void BindDropdowns()
        {
            CampaignList = _dropDownRepository.GetAllCampaignList();

        }
        public async Task<IActionResult> OnGetChartDataAsync(long campaignId)
        {
            var paidCount = await _reportRepository.GetPaidCountAsync(campaignId);
            var unpaidCount = await _reportRepository.GetUnpaidCountAsync(campaignId);

            return new JsonResult(new { PaidCount = paidCount, UnpaidCount = unpaidCount });
        }

        public async Task<IActionResult> OnGetAreaPaymentDataAsync(long campaignId)
        {
            var result = _reportRepository.GetAreaCountwithpaidandunpaid(campaignId);

            if (result.transactionStatus != System.Net.HttpStatusCode.OK || result.returnData == null)
            {
                return new JsonResult(new { error = "Failed to load area data." });
            }

            return new JsonResult(result.returnData);
        }

        public async Task<IActionResult> OnGetZonePaymentDataAsync(long campaignId)
        {
            var result = _reportRepository.GetZoneCountwithpaidandunpaid(campaignId);

            if (result.transactionStatus != System.Net.HttpStatusCode.OK || result.returnData == null)
            {
                return new JsonResult(new { error = "Failed to load zone data." });
            }

            return new JsonResult(result.returnData);
        }

        public async Task<IActionResult> OnGetUnitPaymentDataAsync(long campaignId)
        {
            var result = _reportRepository.GetUnitCountWithpaidAndunpaid(campaignId);

            if (result.transactionStatus != System.Net.HttpStatusCode.OK || result.returnData == null)
            {
                return new JsonResult(new { error = "Failed to load unit data." });
            }

            return new JsonResult(result.returnData);
        }

        // Page handler (Razor Page or Controller)
        public async Task<JsonResult> OnGetPaymentTypeDataAsync(long campaignId)
        {
            var result = await _reportRepository.GetPaymentTypeCountsAsync(campaignId);
            return new JsonResult(result.returnData);
        }

        public async Task<JsonResult> OnGetUnpaidByAreaDataAsync(long campaignId)
        {
            var allAreas = _reportRepository.GetAreaCountwithpaidandunpaid(campaignId);
            var unpaidOnly = allAreas.returnData
                .Select(x => new DashBoardViewModel
                {
                    AreaName = x.AreaName,
                    UnpaidMembers = x.UnpaidMembers ?? 0,
                    TotalMembers = x.UnpaidMembers ?? 0
                })
                .ToList();
            return new JsonResult(unpaidOnly);
        }

        public async Task<JsonResult> OnGetPaymentCollectorsDataAsync(long campaignId)
        {
            var result = await _reportRepository.GetPaymentCollectorsData(campaignId);
            return new JsonResult(result.returnData);
        }


        // AJAX handler: returns JSON data for selected campaign
        //public async Task<JsonResult> OnGetChartDataAsync(long campaignId)
        //{
        //    var paidCount = await _reportRepository.GetPaidCountAsync(campaignId);
        //    var unpaidCount = await _reportRepository.GetUnpaidCountAsync(campaignId);

        //    // Add other data as needed for your charts (paidByArea, unpaidByArea, etc.)

        //    return new JsonResult(new
        //    {
        //        PaidCount = 2,//paidCount,
        //        UnpaidCount = 2 //unpaidCount
        //        // Add more data here as needed
        //    });
        //}

        //public async Task OnGetAsync(long? campaignId)
        //{
        //    if (campaignId.HasValue)
        //    {
        //        FeeCampaign = campaignId.Value;

        //        PaidCount = await _reportRepository.GetPaidCountAsync((long)FeeCampaign);
        //        UnpaidCount = await _reportRepository.GetUnpaidCountAsync((long)FeeCampaign);

        //        // Similarly fetch and assign other data for charts
        //        // PaidByArea = await _reportRepository.GetPaidByAreaAsync(FeeCampaign);
        //        // UnpaidByArea = await _reportRepository.GetUnpaidByAreaAsync(FeeCampaign);
        //    }
        //}
    }
}
