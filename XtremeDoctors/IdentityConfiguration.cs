using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace XtremeDoctors
{
    public static class IServiceCollectionExtensions
    {
        public static void ConfigureIdentityInXtremeDoctors(this IServiceCollection services, IHostingEnvironment env)
        {
            bool restrictivePasswordRules = env.IsProduction();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = restrictivePasswordRules;
                options.Password.RequireLowercase = restrictivePasswordRules;
                options.Password.RequireNonAlphanumeric = restrictivePasswordRules;
                options.Password.RequireUppercase = restrictivePasswordRules;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
        }
    }
}
