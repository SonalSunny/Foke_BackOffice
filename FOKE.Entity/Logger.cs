using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FOKE.Entity
{
    public class Logger
    {
        long? loggedInUser;
        public string _ipAddress;
        public string _operatingSystem;
        public string _userBrowser;
        private readonly IHttpContextAccessor _httpContextAccessor;
        ClaimsPrincipal claimsPrincipal = null;
        public string _loggerId;

        public string Path;
        public string Method;
        public string HttpMethod;
        public string QueryString;
        public string AuditAction;
        public string Description;
        public string AccessClaim;
        public string PageCode;
        public int? StatusCode;
        public List<string> Items { get; set; }
        public List<LoggerActivityLog> ActivityLogs { get; set; }
        public Logger(IHttpContextAccessor httpContextAccessor)
        {
            Items = new List<string>();

            Guid obj = Guid.NewGuid();
            _loggerId = obj.ToString();
            _httpContextAccessor = httpContextAccessor;
            try
            {
                claimsPrincipal = _httpContextAccessor?.HttpContext?.User as ClaimsPrincipal;
                var isAuthenticated = claimsPrincipal?.Identity?.IsAuthenticated ?? false;
                if (isAuthenticated)
                {
                    var userIdentity = claimsPrincipal?.Identity?.Name;
                    if (userIdentity != null)
                    {
                        long userid = 0;
                        Int64.TryParse(userIdentity, out userid);
                        if (userid > 0)
                        {
                            loggedInUser = userid;
                        }
                    }
                }
                var userAgent = _httpContextAccessor?.HttpContext?.Request?.Headers["User-Agent"];
                if (!string.IsNullOrEmpty(userAgent))
                {
                    var parser = UAParser.Parser.GetDefault();
                    var clientInfo = parser.Parse(userAgent);
                    _operatingSystem = clientInfo.OS.Family;
                    _userBrowser = clientInfo.UA?.ToString();
                    _ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
    public class LoggerActivityLog
    {
        public long? MenuId { get; set; }
        public long? ActionTransactionId { get; set; }
        public string ActivityAction { get; set; }
        public string Description { get; set; }
        public string UserRemark { get; set; }
    }
}
