namespace Fanex.OddsFeed.Admin
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Data.SqlServer;
    using Fanex.Logging;
    using Fanex.Logging.Sentry;
    using Fanex.OddsFeed.Admin.Controllers;
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
