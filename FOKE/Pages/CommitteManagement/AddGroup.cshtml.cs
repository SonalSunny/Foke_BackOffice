using FOKE.Entity;
using FOKE.Entity.CommitteeManagement.ViewModel;
using FOKE.Entity.Common;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.CommitteManagement
{
    public class AddGroupModel : BasePageModel
    {
        public string? pageErrorMessage { get; set; }


        [BindProperty]
        public CommitteGroupViewModel inputModel { get; set; }
        public List<DropDownViewModel> CommitteeList { get; set; }
        private readonly ICommitteGroupRepository _committeeGroupRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        private readonly IDropDownRepository _dropDownRepository;
        public AddGroupModel(ICommitteGroupRepository committeeGroupRepository, IDropDownRepository dropDownRepository, ISharedLocalizer sharedLocalizer)
        {
            _committeeGroupRepository = committeeGroupRepository;
            _dropDownRepository = dropDownRepository;
            _sharedLocalizer = sharedLocalizer;
        }
        public void OnGet(long? id, string mode)
        {

            _formMode = mode;
            isValidRequest = true;
            BindDropdowns();
            inputModel = new CommitteGroupViewModel();
            if (id > 0)
            {

                var retData = _committeeGroupRepository.GetCommitteeGroupById(Convert.ToInt64(id));
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

        private void BindDropdowns()
        {
            CommitteeList = _dropDownRepository.GetCommitteeList();
        }

        public async Task<IActionResult> OnPost()
        {
            var retData = new ResponseEntity<CommitteGroupViewModel>();
            if (ModelState.IsValid)
            {
                if (inputModel.GroupId > 0)
                {
                    retData = await _committeeGroupRepository.UpdateCommitteeGroup(inputModel);
                }
                else
                {
                    retData = await _committeeGroupRepository.AddCommitteeGroup(inputModel);
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
                    BindDropdowns();
                    return Page();

                }
            }
            else
            {
                retData.transactionStatus = HttpStatusCode.BadRequest;
                pageErrorMessage = "Fill all Required fields";
                IsSuccessReturn = false;
            }
            BindDropdowns();
            return Page();
        }
    }
}



