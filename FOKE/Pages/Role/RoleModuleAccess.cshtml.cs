using FOKE.Entity.Common;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Localization;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FOKE.Pages.Role
{
    public class RoleModuleAccessModel : BasePageModel
    {
        private readonly ISharedLocalizer _sharedLocalizer;
        private readonly IMenuRepository _menuRepository;
        private readonly IDropDownRepository _dropDownRepository;
        public RoleModuleAccessModel(IMenuRepository menuRepository, IDropDownRepository dropDownRepository, ISharedLocalizer sharedLocalizer)
        {
            _menuRepository = menuRepository;
            _dropDownRepository = dropDownRepository;
            _sharedLocalizer = sharedLocalizer;
        }

        [BindProperty]
        public RoleAdministrationViewModel roleAdministrationModel { get; set; }
        [BindProperty]
        public List<DropDownViewModel> roleGroupList { get; set; }

        public async Task<IActionResult> OnPost()
        {
            roleAdministrationModel.RoleCode = "FOKE";
            var objModel = roleAdministrationModel;
            if (Request.Form["btnSubmit"] == "btnGetRoleInfo")
            {
                ModelState.Clear();
                roleAdministrationModel = await _menuRepository.GetPermissionsByRoleIdAsync(roleAdministrationModel?.RoleCode, roleAdministrationModel.RoleId);

            }
            else if (Request.Form["btnSubmit"] == "btnSave")
            {

                var response = await _menuRepository.SaveRoleAdministrations(objModel, (int)objModel.RoleId);
                if (response.transactionStatus != HttpStatusCode.OK)
                {
                    pageErrorMessage = response.returnMessage;
                }
                else
                {
                    ModelState.Clear();
                    IsSuccessReturn = true;
                    sucessMessage = response.returnMessage;
                }

            }
            BindDropdowns();
            return Page();
        }


        public void OnGet()
        {
            BindDropdowns();
        }

        private void BindDropdowns()
        {
            roleGroupList = _dropDownRepository.GetRole();

        }
    }
}
