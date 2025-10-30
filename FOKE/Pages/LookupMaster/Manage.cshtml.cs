using FOKE.Entity;
using FOKE.Entity.Common;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.LookupMaster
{
    public class ManageModel : BasePageModel
    {

        [BindProperty]
        public LookupViewModel inputModel { get; set; }
        private readonly ILookupRepository _lookupRepository;
        private readonly IDropDownRepository _dropDownRepository;
        public string? pageErrorMessage { get; set; }
        public string LooKUPName { get; set; }
        public string LookUpTYpeName { get; set; }
        public string Description { get; set; }
        public long? _teamid { get; set; }
        public long? LookUPTypeId { get; set; }
        public List<DropDownViewModel> LookUpTypeList { get; set; }
        public ManageModel(ILookupRepository lookupRepository, IDropDownRepository dropDownRepository)
        {
            _lookupRepository = lookupRepository;
            _dropDownRepository = dropDownRepository;
        }
        public void OnGet(long? id, string mode)
        {
            _formMode = mode;
            isValidRequest = true;
            inputModel = new LookupViewModel();
            if (id > 0)
            {
                _teamid = id;
                var retData = _lookupRepository.GetLookUpbyId(Convert.ToInt64(id));
                if (retData.transactionStatus == HttpStatusCode.OK)
                {

                    isValidRequest = true;
                    inputModel = retData.returnData;
                    LookUPTypeId = inputModel.LookUpTypeId;
                }
                else
                {
                    isValidRequest = false;
                    pageErrorMessage = retData.returnMessage;
                }
            }
            BindDropdowns();
        }
        public async Task<IActionResult> OnPost()
        {

            var retData = new ResponseEntity<LookupViewModel>();
            if (ModelState.IsValid)
            {
                retData = await _lookupRepository.AddLookUp(inputModel);
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
                    inputModel = new LookupViewModel();
                }
            }
            else
            {
                retData.transactionStatus = HttpStatusCode.BadRequest;
                pageErrorMessage = "Fill all the required fields";
                IsSuccessReturn = false;
            }
            BindDropdowns();
            return Page();
        }
        private void BindDropdowns()
        {
            LookUpTypeList = _dropDownRepository.GetLookupTypeList();

        }
    }
}
