using FOKE.Entity;
using FOKE.Entity.WorkPlaceData.ViewModel;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.WorkPlace
{
    public class ManageModel : BasePageModel
    {
        [BindProperty]
        public WorkPlaceViewModel inputModel { get; set; }

        private readonly IWorkPlaceRepository _workplaceRepository;
        public string? pageErrorMessage { get; set; }
        public long? _workplaceId { get; set; }
        public ManageModel(IWorkPlaceRepository workplaceRepository)
        {
            _workplaceRepository = workplaceRepository;

        }
        public void OnGet(long? id, string mode)
        {
            _formMode = mode;
            isValidRequest = true;
            inputModel = new WorkPlaceViewModel();
            if (id > 0)
            {
                _workplaceId = id;
                var retData = _workplaceRepository.GetWorkPlacebyId(Convert.ToInt64(id));
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
            var workplacename = inputModel.WorkPlaceName;
            var description = inputModel.Description;

            if (workplacename == null)
            {
                pageErrorMessage = "Enter Work Place";
                return Page();
            }
            else
            {
                var retData = new ResponseEntity<WorkPlaceViewModel>();

                if (btnSubmit == "btnSave" && ModelState.IsValid)
                {
                    if (formMode.Equals(FormModeEnum.add))
                    {
                        retData = await _workplaceRepository.SaveWorkPlace(inputModel);

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
                            inputModel = new WorkPlaceViewModel();

                            return Page();
                        }
                    }
                    else

                    {
                        retData = await _workplaceRepository.UpdateWorkPlace(inputModel);
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
                            inputModel = new WorkPlaceViewModel();
                        }
                    }
                }
                return Page();
            }
        }

    }
}
