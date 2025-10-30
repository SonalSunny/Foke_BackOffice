using FOKE.DataAccess;
using FOKE.Entity.CampaignData.ViewModel;
using FOKE.Entity.Common;
using FOKE.Entity.MembershipFee.ViewModel;
using FOKE.Entity.MembershipIssuedData.ViewModel;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.PayMembershipFee
{
    public class PayFeeModel : BasePageModel
    {

        [BindProperty]
        public CampaignViewModel inputModel { get; set; }
        [BindProperty]
        public MembershipFeeViewModel PaymentInput { get; set; }
        public PostMembershipViewModel PaymentInputModel { get; set; }
        public string? pageErrorMessage { get; set; }
        private readonly ICampaignRepository _campaignRepository;
        private readonly IFeeCollectionReport _feeCollectionReport;
        [BindProperty]
        public string? Type { get; set; }

        public PayFeeModel(IDropDownRepository dropDownRepository, ICampaignRepository campaignRepository, IFeeCollectionReport feeCollectionReport)
        {
            _dropDownRepository = dropDownRepository;
            _campaignRepository = campaignRepository;
            _feeCollectionReport = feeCollectionReport;
        }
        public async Task<IActionResult> OnPostAsync()
        {
            BindDropdowns();
            if (!ModelState.IsValid)
            {
                pageErrorMessage = "Please fill all required fields.";
                IsSuccessReturn = false;
                return Page();
            }
            var result = await _campaignRepository.UpdateMembershipFeeAsync(PaymentInput);
            //var result = await _feeCollectionReport.FeeCollection(PaymentInput);

            if (result.transactionStatus == HttpStatusCode.OK)
            {
                IsSuccessReturn = true;
                sucessMessage = result.returnMessage;
            }
            else
            {
                IsSuccessReturn = false;
                pageErrorMessage = result.returnMessage;
            }

            return Page();


        }




        public List<DropDownViewModel> PaymentTypeList { get; set; }
        private readonly FOKEDBContext _dbContext;
        private readonly IDropDownRepository _dropDownRepository;




        public void OnGet(long? id, string? type)
        {
            Type = type;
            inputModel = new CampaignViewModel();
            PaymentInput = new MembershipFeeViewModel(); // Ensure it's initialized

            if (id > 0)
            {
                var retData = _campaignRepository.GetAllCampaign(1);
                if (retData.transactionStatus == HttpStatusCode.OK)
                {
                    var selectedCampaign = retData.returnData.FirstOrDefault();

                    if (selectedCampaign != null)
                    {
                        inputModel = selectedCampaign;
                        isValidRequest = true;

                        PaymentInput.Campaign = inputModel.CampaignId;
                        PaymentInput.MemberID = id.Value;
                    }
                    else
                    {
                        isValidRequest = false;
                        pageErrorMessage = "No active campaigns found.";
                    }
                }
                else
                {
                    isValidRequest = false;
                    pageErrorMessage = retData.returnMessage;
                }
            }
            else
            {
                isValidRequest = false;
                pageErrorMessage = "Invalid member ID.";
            }

            BindDropdowns();
        }


        private void BindDropdowns()
        {
            PaymentTypeList = _dropDownRepository.GetPaymentTypes();
        }


    }
}
