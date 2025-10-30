using FOKE.Entity;
using FOKE.Entity.OperationManagement.ViewModel;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.FolderMaster
{
    public class ManageModel : BasePageModel
    {
        [BindProperty]
        public FolderViewModel inputModel { get; set; }

        private readonly IFolderMasterRepository _foldermasterRepository;
        private readonly IDropDownRepository _dropDownRepository;
        public ManageModel(IFolderMasterRepository foldermasterRepository)
        {
            _foldermasterRepository = foldermasterRepository;
        }
        public void OnGet(long? id, string mode)
        {
            _formMode = mode;
            isValidRequest = true;
            inputModel = new FolderViewModel();
            if (id > 0)
            {
                var retData = _foldermasterRepository.GetFolderById(Convert.ToInt64(id));
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
            var foldername = inputModel.FolderName;
            if (string.IsNullOrEmpty(foldername))
            {
                pageErrorMessage = "Please name the folder";
            }
            else
            {
                var retData = new ResponseEntity<FolderViewModel>();
                if (btnSubmit == "btnSave")
                {
                    if (formMode.Equals(FormModeEnum.add))
                    {
                        retData = await _foldermasterRepository.CreateFolder(inputModel);
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
                            inputModel = new FolderViewModel();
                            return Page();
                        }
                    }
                    else
                    {
                        retData = await _foldermasterRepository.UpdateFolder(inputModel);
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
                        pageErrorMessage = "Use 8 or more characters with a mix of letters, numbers, and symbols.";
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
