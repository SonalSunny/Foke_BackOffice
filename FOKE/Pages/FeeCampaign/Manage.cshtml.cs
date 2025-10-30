using FOKE.Entity;
using FOKE.Entity.CampaignData.ViewModel;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.FeeCampaign
{
    public class ManageModel : BasePageModel
    {
        [BindProperty]
        public CampaignViewModel inputModel { get; set; }

        private readonly ICampaignRepository _campaignRepository;
        public string? pageErrorMessage { get; set; }
        public long? _professionId { get; set; }
        public ManageModel(ICampaignRepository campaignRepository)
        {
            _campaignRepository = campaignRepository;

        }
        public void OnGet(long? id, string mode)
        {
            _formMode = mode;
            isValidRequest = true;
            inputModel = new CampaignViewModel();
            if (id > 0)
            {
                _professionId = id;
                var retData = _campaignRepository.GetCampaignId(Convert.ToInt64(id));
                if (retData.transactionStatus == HttpStatusCode.OK)
                {
                    isValidRequest = true;
                    inputModel = retData.returnData;

                }
                else
                {
                    isValidRequest = false;
                    pageErrorMessage = retData.returnMessage;
                }
            }


        }
        public async Task<IActionResult> OnPost()
        {
            var retData = new ResponseEntity<CampaignViewModel>();
            if (ModelState.IsValid)
            {
                if (inputModel.CampaignId > 0)
                {
                    retData = await _campaignRepository.UpdateCampaign(inputModel);
                }
                else
                {
                    retData = await _campaignRepository.AddCampaign(inputModel);
                }

                if (retData.transactionStatus != HttpStatusCode.OK)
                {
                    pageErrorMessage = retData.returnMessage;
                    IsSuccessReturn = false;
                }
                else
                {
                    ModelState.Clear();
                    IsSuccessReturn = true;
                    sucessMessage = retData.returnMessage;
                    return Page();

                }

            }
            else
            {
                retData.transactionStatus = HttpStatusCode.BadRequest;
                pageErrorMessage = "Fill all Required fields";
                IsSuccessReturn = false;
            }
            return Page();
        }

    }
}
