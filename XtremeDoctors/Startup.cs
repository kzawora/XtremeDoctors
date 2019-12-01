using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Globalization;
using XtremeDoctors.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using XtremeDoctors.Resources;
using XtremeDoctors.Services;
using XtremeDoctors.Models;

namespace XtremeDoctors
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment appEnv)
        {
            Configuration = configuration;
            CurrentEnvironment = appEnv;
        }

        public IConfiguration Configuration { get; }
        private IHostingEnvironment CurrentEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Cookies
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                string conntectionString = Configuration.GetConnectionString("SqliteFile");
                options.UseSqlite(conntectionString);
            });

            // Injectable services
            services.AddScoped<DoctorService>();
            services.AddScoped<AppointmentService>();

            // Identity
            services.AddDefaultIdentity<User>().AddEntityFrameworkStores<ApplicationDbContext>();
            services.ConfigureIdentityInXtremeDoctors(CurrentEnvironment);

            // Localization
            services.AddLocalization(o => { o.ResourcesPath = "Resources"; });
            services.AddSingleton<SharedViewLocalizer>();

            // MVC
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                             .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Error interception
            app.UseStatusCodePagesWithReExecute("/error/{0}");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Identity
            app.UseAuthentication();

            // Networking Stuff
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Localization
            IList<CultureInfo> supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                new CultureInfo("pl-PL"),
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            // MVC
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
