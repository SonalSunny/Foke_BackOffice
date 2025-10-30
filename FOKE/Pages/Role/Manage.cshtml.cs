using FOKE.Entity;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.Role
{
    public class ManageModel : BasePageModel
    {
        [BindProperty]
        public RoleViewModel inputModel { get; set; }

        private readonly IRoleRepository _roleRepository;
        // private readonly IDropDownRepository _dropDownRepository;
        public string? pageErrorMessage { get; set; }
        public long? _roleId { get; set; }
        public ManageModel(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;

        }
        public void OnGet(long? id, string mode)
        {
            _formMode = mode;
            isValidRequest = true;
            inputModel = new RoleViewModel();
            if (id > 0)
            {
                _roleId = id;
                var retData = _roleRepository.GetRolebyId(Convert.ToInt64(id));
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
            var rolename = inputModel.RoleName;
            var description = inputModel.RoleDescription;

            // Check if RoleName or RoleDescription is null
            if (rolename == null)
            {
                pageErrorMessage = "Fill all the required fields";
                return Page(); // Return the page with an error message
            }
            else
            {
                var retData = new ResponseEntity<RoleViewModel>();

                // Handle form submission
                if (btnSubmit == "btnSave" && ModelState.IsValid)
                {
                    if (formMode.Equals(FormModeEnum.add))
                    {
                        retData = await _roleRepository.SaveRole(inputModel);

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
                            inputModel = new RoleViewModel();

                            return Page(); // Return the page after successful save
                        }
                    }







                    else

                    {
                        retData = await _roleRepository.UpdateRole(inputModel);
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
                            inputModel = new RoleViewModel();
                        }
                    }
                }
                // If none of the above conditions are met, return the page anyway
                return Page();
            }
        }

    }
}
