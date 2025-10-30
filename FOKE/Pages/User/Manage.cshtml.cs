using FOKE.DataAccess;
using FOKE.Entity;
using FOKE.Entity.Common;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.User
{
    public class ManageModel : BasePageModel
    {

        [BindProperty]
        public UserViewModel inputModel { get; set; }
        private readonly FOKEDBContext _dbContext;


        private readonly IUserRepository _userRepository;
        private readonly IDropDownRepository _dropDownRepository;
        public string? pageErrorMessage { get; set; }
        [BindProperty]
        public List<DropDownViewModel> EmployeeList { get; set; }
        public List<DropDownViewModel> RoleList { get; set; }
        public string Password { get; set; }
        public long? _userId { get; set; }
        public ManageModel(IUserRepository userRepository, IDropDownRepository dropDownRepository)
        {
            _userRepository = userRepository;
            _dropDownRepository = dropDownRepository;
        }
        public void OnGet(long? id, string mode)
        {
            _formMode = mode;
            isValidRequest = true;
            inputModel = new UserViewModel();
            if (id > 0)
            {
                _userId = id;
                var retData = _userRepository.GetUserbyId(Convert.ToInt64(id));
                if (retData.transactionStatus == HttpStatusCode.OK)
                {
                    isValidRequest = true;
                    inputModel = retData.returnData;
                    Password = inputModel.Password;
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
            var Username = inputModel.Username;
            var password = inputModel.Password;
            if (Username == null || password == null)
            {
                pageErrorMessage = "Please enter Username and Password";
            }
            else
            {
                var retData = new ResponseEntity<UserViewModel>();
                if (btnSubmit == "btnSave" && ModelState.IsValid)
                {
                    if (formMode.Equals(FormModeEnum.add))
                    {
                        retData = await _userRepository.RegisterUser(inputModel);
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
                            inputModel = new UserViewModel();
                            BindDropdowns();
                            return Page();

                        }
                    }
                    else
                    {

                        retData = await _userRepository.UpdateUser(inputModel);
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
            BindDropdowns();
            return Page();
        }
        private void BindDropdowns()
        {
            //EmployeeList = _dropDownRepository.GetEmployee();
            RoleList = _dropDownRepository.GetRole();

        }
    }
}
