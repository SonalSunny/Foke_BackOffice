using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.OfferData.ViewModel;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.Offers
{
    public class ManageModel : BasePageModel
    {

        [BindProperty]
        public OfferViewModel inputModel { get; set; }

        private readonly FOKEDBContext _dbContext;

        private readonly IOfferRepository _offerRepository;

        private readonly IDropDownRepository _dropDownRepository;
        public string? pageErrorMessage { get; set; }
        public long? _userId { get; set; }

        public ManageModel(IOfferRepository offerRepository, IDropDownRepository dropDownRepository)
        {
            _offerRepository = offerRepository;
            _dropDownRepository = dropDownRepository;
        }

        public void OnGet(long? id, string mode)
        {
            _formMode = mode;
            isValidRequest = true;
            inputModel = new OfferViewModel();
            if (id > 0)
            {
                _userId = id;
                var retData = _offerRepository.GetOfferByID(Convert.ToInt64(id));
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
            var Heading = inputModel.Heading;
            if (Heading == null)
            {
                pageErrorMessage = "Please enter Heading";
            }
            else if (formMode.Equals(FormModeEnum.add) && inputModel.Image == null)
            {
                pageErrorMessage = "Please Upload Image";
            }
            else
            {
                var retData = new ResponseEntity<OfferViewModel>();
                if (btnSubmit == "btnSave" && ModelState.IsValid)
                {
                    if (formMode.Equals(FormModeEnum.add))
                    {
                        retData = await _offerRepository.AddOffer(inputModel);
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
                            inputModel = new OfferViewModel();
                            return Page();

                        }
                    }
                    else
                    {
                        retData = await _offerRepository.UpdateOffer(inputModel);
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
                        }
                    }
                }
                else
                {
                    if (btnSubmit == "btnSave")
                    {
                        retData.transactionStatus = HttpStatusCode.BadRequest;
                        pageErrorMessage = "Use 8 or more characters with a mix of letters,numbers,symbols.";
                        IsSuccessReturn = false;
                    }
                    else
                    {
                        ModelState.Clear();
                    }
                }
            }
            return Page();
        }
    }
}

