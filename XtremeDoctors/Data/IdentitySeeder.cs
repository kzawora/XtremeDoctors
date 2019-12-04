using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using XtremeDoctors.Models;
using Microsoft.Extensions.DependencyInjection;

namespace XtremeDoctors.Data
{
    public static class IApplicationBuilderExtensions
    {
    

        public static void InitializeUsersAndRolesForXtremeDoctors(this IApplicationBuilder app, UserManager<User> userManager)
        {
            bool dataNotInitialized = (userManager.FindByEmailAsync("admin@wp.pl").Result == null);
            if (dataNotInitialized)
            {
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    var services = serviceScope.ServiceProvider;
                    var dbContext = services.GetService<ApplicationDbContext>();

                    void InitializeUser(string email, string password, string role)
                    {
                        User user = new User
                        {
                            UserName = email,
                            Email = email,
                        };

                        IdentityResult result = userManager.CreateAsync(user, password).Result;
                        if (!result.Succeeded) throw new Exception("INIT ERROR");
                        result = userManager.AddToRoleAsync(user, role).Result;
                        if (!result.Succeeded) throw new Exception("INIT ERROR");
                    }

                    // If you change this, make sure you'll step into this branch (dataNotInitialized == true).
                    InitializeUser("admin@wp.pl", "admin1", Roles.Admin);
                    InitializeUser("harold@wp.pl", "harold", Roles.Patient);
                    InitializeUser("recept@wp.pl", "recept", Roles.Receptionist);
                }
            }
        }
    }
}
