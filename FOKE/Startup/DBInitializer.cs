using FOKE.DataAccess;
using FOKE.Entity.Identity.ViewModel;
using FOKE.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace FOKE.Program
{
    public static class DBInitializer
    {
        public static async Task Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                try
                {
                    var context = serviceScope.ServiceProvider.GetService<FOKEDBContext>();
                    if (context != null)
                    {
                        context.Database.Migrate();
                        var _userManager = serviceScope.ServiceProvider.GetRequiredService<IUserRepository>();
                        var _roleManager = serviceScope.ServiceProvider.GetRequiredService<IRoleRepository>();

                        var role = new RoleViewModel()
                        {
                            RoleName = "Admin",
                            Active = true,

                        };
                        var RoleData = await _roleManager.SaveRole(role);
                        if (RoleData.returnData != null)
                        {
                            var user = new UserViewModel()
                            {
                                Username = "Admin",
                                Password = "123",
                                RoleId = (long)RoleData.returnData.RoleId,
                                Active = true
                            };
                            await _userManager.RegisterUser(user);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
