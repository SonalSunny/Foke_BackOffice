//using FOKE.Entity.EmployeeManagement.ViewModel;
//using FOKE.Entity.Holidays.ViewModel;
using FOKE.Models.PageModels;
using Microsoft.AspNetCore.Mvc;


namespace FOKE.Pages.Notifications.Compose
{
    public class ManageModel : BasePageModel
    {
        //[BindProperty]
        //public HolidayViewModel inputModel { get; set; }

        //private readonly IHolidayRepository _holidayRepository;
        //public readonly SkakErpDBContext _dbContext;
        public ManageModel()
        {
            //_holidayRepository = holidayRepository;
            //_dbContext = dbContext;
        }

        public void OnGet(long? id, string mode)
        {
            _formMode = mode; // Set the form mode (add/edit)
            isValidRequest = true;
            //inputModel = new HolidayViewModel();
            //if (id > 0)
            //{
            //    var retData = _holidayRepository.GetHolidayById(Convert.ToInt64(id));
            //    if (retData.transactionStatus == HttpStatusCode.OK)
            //    {
            //        isValidRequest = true;
            //        inputModel = retData.returnData;
            //        inputModel.HolidayOn = inputModel.HolidayOn != default ? inputModel.HolidayOn : null;

            //    }
            //    else
            //    {
            //        isValidRequest = false;
            //        pageErrorMessage = retData.returnMessage;
            //    }
            //}
            //else
            //{
            //    inputModel.Year = DateTime.Now.Year.ToString();
            //    inputModel.SortOrder = GetNextAvailableSortOrderForYear(inputModel.Year);
            //}
        }

        public async Task<IActionResult> OnPost()
        {
            //if (!ValidateSortOrder())
            //{
            //    return Page();
            //}
            //var HolidayName = inputModel.HolidayName;
            //var SortOrder = inputModel.SortOrder;
            //var HolidayDate = inputModel.HolidayOn;
            //var HolidayYear = inputModel.Year;

            //if (string.IsNullOrEmpty(HolidayName))
            //{
            //    pageErrorMessage = "Please enter a holiday name";
            //}
            //else if (SortOrder == 0)
            //{
            //    pageErrorMessage = "Please enter a sorting number";
            //}
            //else if (HolidayDate == null)
            //{
            //    pageErrorMessage = "Please select a holiday date.";
            //}
            //else if (HolidayYear != HolidayDate.Value.Year.ToString())
            //{
            //    pageErrorMessage = "The selected year does not match the year of the holiday date.";
            //}
            //else
            //{
            //    var retData = new ResponseEntity<HolidayViewModel>();
            //    if (btnSubmit == "btnSave")
            //    {
            //        if (formMode.Equals(FormModeEnum.add))
            //        {
            //            retData = await _holidayRepository.CreateHoliday(inputModel);
            //            if (retData.transactionStatus != HttpStatusCode.OK)
            //            {
            //                pageErrorMessage = retData.returnMessage;
            //                IsSuccessReturn = false;
            //            }
            //            else
            //            {
            //                ModelState.Clear();
            //                IsSuccessReturn = true;
            //                sucessMessage = retData.returnMessage;
            //                inputModel = new HolidayViewModel();
            //                return Page();
            //            }
            //        }
            //        else
            //        {
            //            retData = await _holidayRepository.UpdateHoliday(inputModel);
            //            if (retData.transactionStatus != HttpStatusCode.OK)
            //            {
            //                pageErrorMessage = retData.returnMessage;
            //                IsSuccessReturn = false;
            //            }
            //            else
            //            {
            //                ModelState.Clear();
            //                IsSuccessReturn = true;
            //                sucessMessage = retData.returnMessage;
            //            }
            //        }
            //        return Page();
            //    }
            //    else
            //    {
            //        if (btnSubmit == "btnSave")
            //        {
            //            retData.transactionStatus = HttpStatusCode.BadRequest;
            //            pageErrorMessage = "Use 8 or more characters with a mix of letters, numbers, and symbols.";
            //            IsSuccessReturn = false;
            //        }
            //        else
            //        {
            //            ModelState.Clear();
            //        }
            //    }
            //}
            return Page();
        }


    }
}
