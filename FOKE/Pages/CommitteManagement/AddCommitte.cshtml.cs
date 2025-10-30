using FOKE.Entity;
using FOKE.Entity.CommitteeManagement.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.CommitteManagement
{
    public class AddCommitteModel : BasePageModel
    {
        public string? pageErrorMessage { get; set; }


        [BindProperty]
        public CommitteViewModel inputModel { get; set; }
        private readonly ICommitteeRepository _committeeRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        public AddCommitteModel(ICommitteeRepository committeeRepository, ISharedLocalizer sharedLocalizer)
        {
            _committeeRepository = committeeRepository;
            _sharedLocalizer = sharedLocalizer;

        }
        public void OnGet(long? id, string mode)
        {

            _formMode = mode;
            isValidRequest = true;
            inputModel = new CommitteViewModel();
            if (id > 0)
            {

                var retData = _committeeRepository.GetCommitteeById(Convert.ToInt64(id));
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
            var retData = new ResponseEntity<CommitteViewModel>();
            if (ModelState.IsValid)
            {
                if (inputModel.CommitteeId > 0)
                {
                    retData = await _committeeRepository.UpdateCommittee(inputModel);
                }
                else
                {
                    retData = await _committeeRepository.AddCommittee(inputModel);
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









