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
        public static void InitializRolesForXtremeDoctors(this IApplicationBuilder app, RoleManager<IdentityRole> roleManager)
        {
            void InitializeRole(string id)
            {
                var role = new IdentityRole { Id = id, Name = id, NormalizedName = id };
                IdentityResult result = roleManager.CreateAsync(role).Result;
                if (!result.Succeeded) throw new Exception("INIT ERROR");
            }

            bool dataNotInitialized = (roleManager.FindByIdAsync(Roles.Admin).Result == null);
            if (dataNotInitialized)
            {
                InitializeRole(Roles.Patient);
                InitializeRole(Roles.Receptionist);
                InitializeRole(Roles.Admin);
            }
        }

        public static void InitializeUsersForXtremeDoctors(this IApplicationBuilder app, UserManager<User> userManager)
        {
            bool dataNotInitialized = (userManager.FindByEmailAsync("admin@wp.pl").Result == null);
            if (dataNotInitialized)
            {
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    var services = serviceScope.ServiceProvider;
                    var dbContext = services.GetService<ApplicationDbContext>();

                    void InitializeUser(string email, string password, string role,
                                        int? id = null, string name = null, string surname = null)
                    {
                        User user = new User
                        {
                            UserName = email,
                            Email = email,
                        };

                        if (role == Roles.Patient)
                        {
                            Patient patient = new Patient {Id = id.Value, Name = name, Surname = surname};
                            dbContext.Patients.Add(patient);
                            dbContext.SaveChanges();
                            user.PatientId = patient.Id;
                        }

                        IdentityResult result = userManager.CreateAsync(user, password).Result;
                        if (!result.Succeeded) throw new Exception("INIT ERROR");
                        result = userManager.AddToRoleAsync(user, role).Result;
                        if (!result.Succeeded) throw new Exception("INIT ERROR");
                    }

                    // If you change this, make sure you'll step into this branch (dataNotInitialized == true).
                    InitializeUser("harold@wp.pl", "harold", Roles.Patient, 1, "Harold", "HideThePain");
                    InitializeUser("recept@wp.pl", "recept", Roles.Receptionist);
                    InitializeUser("admin@wp.pl", "admin1", Roles.Admin);
                }
            }
        }
    }
}
