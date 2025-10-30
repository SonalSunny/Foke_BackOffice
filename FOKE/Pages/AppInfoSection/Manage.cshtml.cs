using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.AppInfoSection.ViewModel;
using FOKE.Entity.Common;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.AppInfoSection
{
    public class ManageModel : BasePageModel
    {

        [BindProperty]
        public AppinfoSectionViewModel inputModel { get; set; }

        private readonly FOKEDBContext _dbContext;

        public readonly IappInfoSectionRepository _infoSectionRepo;

        private readonly IDropDownRepository _dropDownRepository;
        public List<DropDownViewModel> SectionTypeList { get; set; }

        public string? pageErrorMessage { get; set; }
        public long? _userId { get; set; }

        public ManageModel(IappInfoSectionRepository infoSectionRepo, IDropDownRepository dropDownRepository)
        {
            _infoSectionRepo = infoSectionRepo;
            _dropDownRepository = dropDownRepository;
        }

        public void OnGet(long? id, string mode)
        {
            BindDropdowns();
            _formMode = mode;
            isValidRequest = true;
            inputModel = new AppinfoSectionViewModel();
            if (id > 0)
            {
                _userId = id;
                var retData = _infoSectionRepo.GetAppInfoDataByID(Convert.ToInt64(id));
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
            BindDropdowns();
            var Heading = inputModel.HTMLContent;
            if (Heading == null)
            {
                pageErrorMessage = "Please enter Heading";
            }
            else if (formMode.Equals(FormModeEnum.add) && inputModel.HTMLContent == null)
            {
                pageErrorMessage = "Please Upload Image";
            }
            else
            {
                var retData = new ResponseEntity<AppinfoSectionViewModel>();
                if (btnSubmit == "btnSave" && ModelState.IsValid)
                {
                    if (formMode.Equals(FormModeEnum.add))
                    {
                        retData = await _infoSectionRepo.AddAppInfoSection(inputModel);
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
                            inputModel = new AppinfoSectionViewModel();
                            return Page();

                        }
                    }
                    else
                    {
                        retData = await _infoSectionRepo.UpdateAppInfoSection(inputModel);
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

        private void BindDropdowns()
        {
            SectionTypeList = _dropDownRepository.GetSectionTypeList();
        }
    }
}

