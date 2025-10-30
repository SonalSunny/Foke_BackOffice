using FOKE.Services.Interface;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace FOKE.Pages.ViewMemberStatus
{
    public class IndexModel : PageModel
    {
        public readonly IMembershipFormRepository _membershipFormRepository;
        public IConfiguration Configuration { get; }

        public IndexModel(IMembershipFormRepository membershipFormRepository, IConfiguration configuration)
        {
            _membershipFormRepository = membershipFormRepository;
            Configuration = configuration;

        }
        public string? MemberName { get; set; }
        public long? Status { get; set; }
        public async void OnGet(string token)
        {
            var principal = ValidateToken(token);
            if (principal == null)
            {
                // Handle invalid token (redirect or show error)
                return;
            }
            var deviceId = principal.FindFirst("deviceId")?.Value;
            var civilId = principal.FindFirst("civilId")?.Value;
            var devicePrimaryid = principal.FindFirst("devicePrimaryId")?.Value;

            var ReturnData = await _membershipFormRepository.GetMemberStatus(civilId);
            if (ReturnData.transactionStatus == HttpStatusCode.OK)
            {
                MemberName = ReturnData.returnData.Name;
                Status = ReturnData.returnData.Count;
            }
        }

        private ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return principal;
            }
            catch
            {
                return null;
            }
        }

    }
}
