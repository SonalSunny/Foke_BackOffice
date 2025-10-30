using FOKE.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FOKE.Startup
{
    public class SessionValidationFilter : IAsyncPageFilter
    {
        private readonly FOKEDBContext _dbContext;

        public SessionValidationFilter(FOKEDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var path = httpContext.Request.Path.ToString().ToLower();

            var excludedPaths = new List<PathString>
            {
                "/login",
                "/sessiontracker",
                "/registration",
                "/thankyou",
                "/newmember",
                "/ViewMemberStatus",
            };

            if (excludedPaths.Any(p =>
                httpContext.Request.Path.StartsWithSegments(p, StringComparison.OrdinalIgnoreCase)))
            {
                await next(); // allow these pages without session validation
                return;
            }


            var sessionId = httpContext.Session.GetString("SessionId");
            var userId = httpContext.Session.GetInt32("UserId");

            if (string.IsNullOrEmpty(sessionId) || userId == null)
            {
                httpContext.Session.Clear(); // Forcefully remove all session data
                context.Result = new RedirectToPageResult("/SessionTracker");
                return;
            }

            //var user = _dbContext.Users.FirstOrDefault(u => u.UserId == userId);
            //if (user == null || user.ActiveSessionId != sessionId)
            //{
            //    httpContext.Session.Clear(); // Forcefully remove all session data
            //    context.Result = new RedirectToPageResult("/SessionTracker");
            //    return;
            //}

            await next(); // continue if session is valid
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }
    }
}