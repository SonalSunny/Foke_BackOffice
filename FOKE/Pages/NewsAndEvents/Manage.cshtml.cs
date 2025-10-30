using FOKE.Entity;
using FOKE.Entity.Common;
using FOKE.Entity.NewsAndEventsData.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.NewsAndEvents
{
    public class ManageModel : BasePageModel
    {
        [BindProperty]
        public NewsAndEventsViewModel inputModel { get; set; }

        public List<DropDownViewModel> TypeList { get; set; }
        private readonly IDropDownRepository _dropDownRepository;
        private readonly ISharedLocalizer _sharedLocalizer;
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly INewsAndEventsRepository _newsandeventsRepository;
        public ManageModel(IDropDownRepository dropDownRepository, INewsAndEventsRepository newsandeventsRepository, ISharedLocalizer sharedLocalizer, IAttachmentRepository attachmentRepository)
        {
            _newsandeventsRepository = newsandeventsRepository;
            _dropDownRepository = dropDownRepository;
            _sharedLocalizer = sharedLocalizer;
            _attachmentRepository = attachmentRepository;
        }

        public void OnGet(long? id, string mode)
        {
            _formMode = mode; // Set the form mode (add/edit)
            isValidRequest = true;
            inputModel = new NewsAndEventsViewModel();
            BindDropdowns();
            if (id > 0)
            {
                var retData = _newsandeventsRepository.GetNewsAndEventsByID(Convert.ToInt64(id));
                if (retData.transactionStatus == HttpStatusCode.OK)
                {
                    isValidRequest = true;
                    inputModel = retData.returnData;

                    if (inputModel.Date != default)
                    {
                        inputModel.Date = inputModel.Date;
                    }
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
            BindDropdowns();

            var retData = new ResponseEntity<NewsAndEventsViewModel>();

            if (ModelState.IsValid)
            {
                if (inputModel.Id > 0)
                {
                    retData = await _newsandeventsRepository.UpdateNewsAndEvents(inputModel);
                }
                else
                {
                    retData = await _newsandeventsRepository.CreateNewsAndEvents(inputModel);
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
                    inputModel = new NewsAndEventsViewModel(); // clear form
                    return Page();
                }
            }
            else
            {
                retData.transactionStatus = HttpStatusCode.BadRequest;
                pageErrorMessage = "Fill all required fields.";
                IsSuccessReturn = false;
            }
            return Page();
        }

        public JsonResult OnPostDeleteAttachment(long keyid)
        {
            var retData = new ResponseEntity<bool>();
            try
            {
                retData = _newsandeventsRepository.DeleteAttachment(keyid).Result;
            }
            catch (Exception ex)
            {
                retData.transactionStatus = HttpStatusCode.InternalServerError;
                retData.returnMessage = ex.Message;
            }
            return new JsonResult(retData);
        }
        private void BindDropdowns()
        {
            TypeList = _dropDownRepository.GetTypeList();
        }
    }
}
