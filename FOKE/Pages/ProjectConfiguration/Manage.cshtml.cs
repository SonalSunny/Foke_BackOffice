using FOKE.Entity;
using FOKE.Entity.ProjectConfiguration.ViewModel;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.ProjectConfiguration
{
    public class ManageModel : BasePageModel
    {
        [BindProperty]
        public ConfigurationViewModel inputModel { get; set; }

        private readonly IProjectConfigurationRepository _ProjectConfiqRepository;
        private readonly IDropDownRepository _dropDownRepository;
        public string? pageErrorMessage { get; set; }
        public long? _ConfiqId { get; set; }
        public ManageModel(IProjectConfigurationRepository ProjectConfiqRepository, IDropDownRepository dropDownRepository)
        {
            _ProjectConfiqRepository = ProjectConfiqRepository;
            _dropDownRepository = dropDownRepository;
        }
        public void OnGet(long? id, string mode)
        {
            _formMode = mode;
            isValidRequest = true;
            inputModel = new ConfigurationViewModel();
            if (id > 0)
            {
                _ConfiqId = id;
                var retData = _ProjectConfiqRepository.GetConfiqbyId(Convert.ToInt64(id));
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

            var retData = new ResponseEntity<ConfigurationViewModel>();
            if (ModelState.IsValid)
            {
                retData = await _ProjectConfiqRepository.EditConfiq(inputModel);
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
                    inputModel = new ConfigurationViewModel();
                }
            }
            else
            {
                retData.transactionStatus = HttpStatusCode.BadRequest;
                pageErrorMessage = "Fill all Required Fields";
                IsSuccessReturn = false;
            }
            return Page();
        }
    }
}
