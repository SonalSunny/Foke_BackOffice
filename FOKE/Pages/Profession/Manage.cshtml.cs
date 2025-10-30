using FOKE.Entity;
using FOKE.Entity.ProfessionData.ViewModel;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.Profession
{
    public class ManageModel : BasePageModel
    {
        [BindProperty]
        public ProfessionViewModel inputModel { get; set; }

        private readonly IProfessionRepository _professionRepository;
        public string? pageErrorMessage { get; set; }
        public long? _professionId { get; set; }
        public ManageModel(IProfessionRepository professionRepository)
        {
            _professionRepository = professionRepository;

        }
        public void OnGet(long? id, string mode)
        {
            _formMode = mode;
            isValidRequest = true;
            inputModel = new ProfessionViewModel();
            if (id > 0)
            {
                _professionId = id;
                var retData = _professionRepository.GetProfessionbyId(Convert.ToInt64(id));
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
            var professionname = inputModel.ProfessionName;
            var description = inputModel.Description;

            if (professionname == null)
            {
                pageErrorMessage = "Enter profession";
                return Page();
            }
            else
            {
                var retData = new ResponseEntity<ProfessionViewModel>();

                if (btnSubmit == "btnSave" && ModelState.IsValid)
                {
                    if (formMode.Equals(FormModeEnum.add))
                    {
                        retData = await _professionRepository.SaveProfession(inputModel);

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
                            inputModel = new ProfessionViewModel();

                            return Page();
                        }
                    }
                    else

                    {
                        retData = await _professionRepository.UpdateProfession(inputModel);
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
                            inputModel = new ProfessionViewModel();
                        }
                    }
                }
                return Page();
            }
        }

    }
}
