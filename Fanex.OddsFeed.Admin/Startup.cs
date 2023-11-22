namespace Fanex.OddsFeed.Admin
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Data.SqlServer;
    using Fanex.Logging;
    using Fanex.Logging.Sentry;
    using Fanex.OddsFeed.Admin.Models;
    using Fanex.OddsFeed.Core.Repository.Impl;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Core.Service.Impl;
    using Fanex.OddsFeed.Core.Service.Interface;
    using Fanex.OddsFeed.Core.Web.Context;
    using Fanex.OddsFeed.Core.Web.Session;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using ONEbook.UM.AspNetCore;
    using ONEbook.UM.AspNetCore.Filters;
    using ONEbook.UM.Configurations;
    using ONEbook.UM.Helpers;
    using ONEbook.UM.Services;
    using ONEbook.UM.Transport;
    using Sentry;
    using WebOptimizer;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();
            services.AddWebOptimizer(pipeline =>
            {
                RegisterBundlesJS(pipeline);
                RegisterBundlesCSS(pipeline);
            });
            services.AddControllersWithViews().AddControllersAsServices();

            LogManager
               .SetDefaultLogCategory(Configuration.GetSection("Fanex.Logging:LogCategory").Value)
               .Use(new List<ILogEngine>
               {
                    new SentryLogging(new SentryEngineOptions
                    {
                        Dsn = new Dsn(Configuration.GetSection("Fanex.Logging:SentryEngineOptions:Dsn").Value)
                    })
               });

            var connections = Configuration.GetSection("ConnectionStrings")
                                                            .GetChildren()
                                                            .AsEnumerable()
                                                            .Where(x => !string.IsNullOrWhiteSpace(x.GetValue<string>("Name")))
                                                            .ToDictionary(x => x.GetValue<string>("Name"),
                                                            y => new ConnectionConfiguration(y.GetValue<string>("Name"), y.GetValue<string>("ConnectionString"), y.GetValue<string>("ProviderName")));

            DbSettingProviderManager
                .StartNewSession()
                .Use(connections)
                .WithSqlServer()
                .UseDefaultDbSettingProvider(resourcePath: $"~/App_Data/SpNames.xml")
                .Run();

            InjectServices(services);
            InjectRepository(services);
        }

        private void RegisterBundlesCSS(IAssetPipeline pipeline)
        {
            pipeline.MinifyCssFiles();

            pipeline.AddCssBundle("minify/css/login.min.css", 
                "css/login.css");
            pipeline.AddCssBundle("minify/css/fanexgrid.min.css", 
                "css/Fanex.Grid/Fanex.Grid.css",
                "css/Fanex.Grid/Fanex.Pagination.css");
            pipeline.AddCssBundle("minify/css/dashboard.min.css", 
                "css/Dashboard/Dashboard.css");
            pipeline.AddCssBundle("minify/css/common.min.css",
                "css/960.css",
                "css/style.css",
                "css/table.css",
                "css/font.css",
                "css/site.css",
                "css/home.css",
                "css/themes/Aristo/jquery.ui.css",
                "css/jquery.contextMenu.css",
                "css/msgbox/msgbox-0.0.1.css",
                "css/select2.min.css");
            pipeline.AddCssBundle("minify/css/administrator.min.css",
                "css/Administrator/Administrator.css");
            pipeline.AddCssBundle("minify/css/listcompanyip.min.css",
                "css/Affiliate/AffiliateEdit.css");
            pipeline.AddCssBundle("minify/css/affiliate.min.css",
                "css/Affiliate/AffiliateEdit.css");
            pipeline.AddCssBundle("minify/css/licenseeedit.min.css",
                "css/Licensee/LicenseeEdit.css");
            pipeline.AddCssBundle("minify/css/licensee.min.css",
                "css/Licensee/LicenseeEdit.css");
        }

        private void RegisterBundlesJS(IAssetPipeline pipeline)
        {
            pipeline.MinifyJsFiles();

            pipeline.AddJavaScriptBundle("minify/js/updatepassword.min.js", 
                "js/User/UpdatePassword.js");
            pipeline.AddJavaScriptBundle("minify/js/fanexgrid.min.js",
                "js/Fanex.Grid/moment.min.js",
                "js/Fanex.Grid/numeral.min.js",
                "js/Fanex.Grid/Fanex.Grid.js",
                "js/Fanex.Grid/Fanex.Pagination.js");
            pipeline.AddJavaScriptBundle("minify/js/dashboard.min.js",
                "js/Dashboard/Dashboard.js");
            pipeline.AddJavaScriptBundle("minify/js/login.min.js",
                "js/Libs/jquery.js",
                "js/Login/Login.js");
            pipeline.AddJavaScriptBundle("minify/js/common.min.js",
                "js/Libs/jquery.js",
                "js/Libs/jquery.ui.js",
                "js/Libs/jquery.contextMenu.js",
                "js/Libs/msgbox/msgbox-0.0.1.js",
                "js/Libs/select2.min.js",
                "js/Libs/birthdaypicker.js",
                "js/Libs/common.js",
                "js/Libs/Core.js",
                "js/Libs/fullcalendar.js",
                "js/Libs/fxHeader.js",
                "js/Libs/modernizr.js",
                "js/Libs/monPickr-0.0.3.js",
                "js/Views/date.visible.js",
                "js/Views/language.js",
                "js/Views/Header.js",
                "js/ReCheckIn.js",
                "js/OddsFeedAdminManager.js");
            pipeline.AddJavaScriptBundle("minify/js/listcompanyip.min.js",
                "js/Functions/CompanyIP/CommonCompanyIP.js",
                "js/Functions/CompanyIP/InsertCompanyIP.js");
            pipeline.AddJavaScriptBundle("minify/js/addadministrator.min.js",
                "js/Libs/jquery.watermarkinput.js",
                "js/Administrator/AddAdministrator.js",
                "js/Administrator/AddAdministratorStep1.js",
                "js/Administrator/AddAdministratorStep2.js");
            pipeline.AddJavaScriptBundle("minify/js/editadministrator.min.js",
                "js/Administrator/EditAdministrator.js");
            pipeline.AddJavaScriptBundle("minify/js/listadministrator.min.js",
                "js/Libs/jquery.watermarkinput.js",
                "js/Administrator/ListAdministrator.js",
                "js/Administrator/EditAdministrator.js");
            pipeline.AddJavaScriptBundle("minify/js/affiliate.min.js",
                "js/Affiliate/CommonAffiliate.js",
                "js/Affiliate/EditAffiliate.js",
                "js/Affiliate/AffiliateList.js");
            pipeline.AddJavaScriptBundle("minify/js/listlicensee.min.js",
                "js/Functions/ListLicensee/CommonLicensee.js",
                "js/Functions/ListLicensee/ListLicensee.js",
                "js/Functions/ListLicensee/EditLicensee.js");
            pipeline.AddJavaScriptBundle("minify/js/addlicensee.min.js",
                "js/Functions/ListLicensee/CommonLicensee.js",
                "js/Functions/ListLicensee/AddLicensee.js",
                "js/Functions/ListLicensee/AddLicenseeStep1.js",
                "js/Functions/ListLicensee/AddLicenseeStep2.js",
                "js/Functions/ListLicensee/AddLicenseeStep3.js");
            pipeline.AddJavaScriptBundle("minify/js/editaffiliateinfo.min.js",
               "js/Affiliate/CommonAffiliate.js",
               "js/Affiliate/EditAffiliateInfo.js");
            pipeline.AddJavaScriptBundle("minify/js/editaffiliatesetting.min.js",
               "js/Affiliate/CommonAffiliate.js",
               "js/Affiliate/EditAffiliateStep2.js");
            pipeline.AddJavaScriptBundle("minify/js/editaffiliateprofile.min.js",
               "js/Functions/ListLicensee/CommonLicensee.js",
                "js/Affiliate/CommonAffiliate.js",
                "js/Affiliate/EditAffiliateStep3.js");
        }

        private void InjectServices(IServiceCollection services)
        {
            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            services.AddSingleton(appSettings);

            #region ONEbookUM
            services.AddSingleton(new UMClientConfiguration().Initialize(Configuration));
            services.AddSingleton<CheckUMFilterAttribute>();
            services.AddSingleton<IUMCacheService, UMCacheService>();
            services.AddSingleton<IUMRestClient, UMRestClient>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); 
            services.AddSingleton<IHttpContextHelper, HttpContextHelper>(); 
            services.AddSingleton<IMaintenanceService, MaintenanceService>();
            #endregion ONEbookUM

            services.AddSingleton<ICurrentContext, CurrentContext>();
            services.AddSingleton<ICurrentSession, CurrentSession>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IBaseService, BaseService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IDashboardService, DashboardService>();    
            services.AddSingleton<IAffiliateService, AffiliateService>();    
            services.AddSingleton<ILicenseeService, LicenseeService>();    
        }

        private void InjectRepository(IServiceCollection services)
        {
            services.AddSingleton<IDynamicRepository, DynamicRepository>();
            services.AddSingleton<IBaseRepository, BaseRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IAffiliateRepository, AffiliateRepository>();
            services.AddSingleton<ILicenseeRepository, LicenseeRepository>();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseCookiePolicy();
            app.UseHttpsRedirection();
            app.UseWebOptimizer();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=ShowDashboard}/{id?}");
            });
        }
    }
}
