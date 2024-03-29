﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using XtremeDoctors.Data;
using XtremeDoctors.Models;
using XtremeDoctors.Resources;
using XtremeDoctors.Services;

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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Cookies
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // CSRF
            services.AddAntiforgery(options => {});

            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                string conntectionString = Configuration.GetConnectionString("SqliteFile");
                options.UseSqlite(conntectionString);
            });

            // OpenAPI (Swagger)
            services.AddSwaggerGen(conf =>
            {
                conf.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "XtremeDoctors RestFUL API",
                    Version = "v1",
                    Contact = new OpenApiContact { Email = "biuro@extremedoctor.com", Name = "Doctor Abina", },
                });
            });

            // Injectable services
            services.AddScoped<PatientService>();
            services.AddScoped<DoctorService>();
            services.AddScoped<AppointmentService>();
            services.AddScoped<UserService>().AddHttpContextAccessor();

            // Response caching
            services.AddResponseCaching();

            // Identity
            services.AddIdentity<User, IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.ConfigureIdentityInXtremeDoctors(CurrentEnvironment);

            // Localization
            services.AddLocalization(o => { o.ResourcesPath = "Resources"; });
            services.AddSingleton<SharedViewLocalizer>();

            // MVC
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                             .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            // Migrate
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                var dbContext = services.GetService<ApplicationDbContext>();
                dbContext.Database.Migrate();
            }

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
            app.InitializRolesForXtremeDoctors(roleManager);
            app.InitializeUsersForXtremeDoctors(env, userManager);

            // OpenAPI (Swagger)
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                options.RoutePrefix = "swagger";
            });

            // Networking Stuff
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Response caching
            app.UseResponseCaching();

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
