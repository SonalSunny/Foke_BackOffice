
using FOKE.Entity.Identity.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FOKE.Models.PageModels
{

    public class BasePageModel : PageModel
    {
        [BindProperty]
        public string? pageMode { get; set; }
        [BindProperty]
        public bool isViewMode { get; set; }
        [BindProperty]
        public bool isValidRequest { get; set; }
        public bool IsSuccessReturn { get; set; }
        public string pageErrorMessage { get; set; }
        public string sucessMessage { get; set; }
        [BindProperty]
        public string? btnSubmit { get; set; }
        [BindProperty]
        public string? _formMode { get; set; }

        public UserClaims _UserClaims;

        [BindProperty]
        public FormModeEnum formMode
        {
            get
            {
                if (_formMode == "add" || string.IsNullOrEmpty(_formMode))
                {
                    return FormModeEnum.add;
                }
                else if (_formMode == "edit")
                {
                    return FormModeEnum.edit;
                }
                else if (_formMode == "view")
                {
                    return FormModeEnum.view;
                }
                else if (_formMode == "delete")
                {
                    return FormModeEnum.delete;
                }
                else
                {
                    return FormModeEnum.invalid;
                }
            }
        }

        public bool CheckClaim(string claim)
        {
            if (_UserClaims != null)
            {
                if (_UserClaims.IsAdmin)
                    return true;
                else
                {
                    if (_UserClaims.claims != null && _UserClaims.claims.Any(c => c.ClaimType == claim.ToString()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        //public List<MenuClaim> pageClaims { get; set; }

        [BindProperty]
        public string? _sessionId { get; set; }
    }
    public enum FormModeEnum
    {
        add,
        edit,
        view,
        delete,
        invalid,
        details
    }
}
