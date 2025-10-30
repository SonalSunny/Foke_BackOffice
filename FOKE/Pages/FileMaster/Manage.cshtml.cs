using FOKE.Entity;
using FOKE.Entity.Common;
using FOKE.Entity.OperationManagement.ViewModel;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.FileMaster
{
    public class ManageModel : BasePageModel
    {
        [BindProperty]
        public FileViewModel inputModel { get; set; }

        private readonly IFileMasterRepository _filemasterRepository;
        private readonly IDropDownRepository _dropDownRepository;
        public List<DropDownViewModel> FolderList { get; set; }
        public long? Moduleid { get; set; }
        public ManageModel(IFileMasterRepository filemasterRepository, IDropDownRepository dropDownRepository)
        {
            _filemasterRepository = filemasterRepository;
            _dropDownRepository = dropDownRepository;
        }
        public void OnGet(long? id, string mode)
        {
            _formMode = mode;
            isValidRequest = true;
            inputModel = new FileViewModel();
            if (id > 0)
            {
                var retData = _filemasterRepository.GetFileById(Convert.ToInt64(id));
                if (retData.transactionStatus == HttpStatusCode.OK)
                {
                    isValidRequest = true;
                    inputModel = retData.returnData;
                    // Ensure these are set correctly
                    inputModel.AttachmentTypeId = inputModel.FileStorageId != null ? 1 :
                                                inputModel.FileLink != null ? 2 : 0;
                    inputModel.AttachmentAny = inputModel.FileStorageId != null;
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
            //var FileName = inputModel.FileName;
            //var uploadFile = inputModel.UploadFileName;
            //var FileLink = inputModel.FileLink;

            //if (string.IsNullOrEmpty(FileName))
            //{
            //    pageErrorMessage = "Please enter an event name";
            //}
            //else if (uploadFile == null || FileLink == null)
            //{
            //    pageErrorMessage = "Either upload a file or save a link";
            //}
            //else
            //{

            //}
            var retData = new ResponseEntity<FileViewModel>();
            if (ModelState.IsValid)
            {
                if (inputModel.FileId > 0)
                {
                    retData = await _filemasterRepository.UpdateFile(inputModel);
                }
                else
                {
                    retData = await _filemasterRepository.CreateFile(inputModel);
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
        private void BindDropdowns()
        {

            FolderList = _dropDownRepository.GetFolder();
        }
        public JsonResult OnPostDeleteAttachment(long keyid)
        {
            var retData = new ResponseEntity<bool>();
            try
            {
                retData = _filemasterRepository.DeleteAttachment(keyid).Result;
            }
            catch (Exception ex)
            {
                retData.transactionStatus = HttpStatusCode.InternalServerError;
                retData.returnMessage = ex.Message;
            }
            return new JsonResult(retData);
        }
    }
}
