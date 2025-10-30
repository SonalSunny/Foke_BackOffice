using FOKE.Entity.ChangePassword.ViewModel;
using FOKE.Models.PageModels;
using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;





namespace FOKE.Pages
{
    public class ChangePasswordModel : BasePageModel
    {
        public string? pageErrorMessage { get; set; }
        [BindProperty]
        public ChangePasswordViewModel Input { get; set; }

        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ClaimsPrincipal claimsPrincipal = null;
        long? loggedInUser = null;
        public class PasswordCheckDto
        {
            public string? OldPassword { get; set; }
        }
        public ChangePasswordModel(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {

            _httpContextAccessor = httpContextAccessor;

            _userRepository = userRepository;


        }
        private long? GetLoggedInUserId()
        {
            var claimsPrincipal = _httpContextAccessor?.HttpContext?.User;
            if (claimsPrincipal?.Identity?.IsAuthenticated ?? false)
            {


                var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.Name);
                if (userIdClaim != null && long.TryParse(userIdClaim.Value, out long userId) && userId > 0)
                {
                    return userId;
                }
            }
            return null;
        }


        public void OnGet()
        {

        }




        public async Task<IActionResult> OnPostAsync()
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
                return Page();

            // Get the logged-in user's ID
            var userId = GetLoggedInUserId();
            if (userId == null)
                return Unauthorized(); // Return Unauthorized if the user is not logged in


            var retModel = await _userRepository.ChangePassword(new ChangePasswordViewModel
            {
                OldPassword = Input.OldPassword,
                NewPassword = Input.NewPassword,
                loggedinUserId = userId
            });





            if (retModel.transactionStatus != HttpStatusCode.OK)
            {
                pageErrorMessage = retModel.returnMessage;
                IsSuccessReturn = false;
            }
            else
            {
                ModelState.Clear();
                IsSuccessReturn = true;
                sucessMessage = retModel.returnMessage;
            }

            return Page();



        }



        //public async Task<IActionResult> OnPostValidatePasswordAsync(string oldPassword)
        //{
        //    //var userIdClaim = User.FindFirst("Userid")?.Value;
        //    //if (!long.TryParse(userIdClaim, out var userId))
        //    //    return new JsonResult(false);

        //    //var isValid = await _userRepository.ValidateCurrentPasswordAsync(userId, oldPassword);
        //    return Page();

        //}


        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostValidatePasswordAsync(string oldPassword)
        {
            var userId = GetLoggedInUserId();
            if (userId.HasValue)
            {
                bool isValid = await _userRepository.ValidateCurrentPasswordAsync(userId.Value, oldPassword);
                return new JsonResult(isValid);
            }
            return new JsonResult(false); // User not authenticated
        }




    }
}
