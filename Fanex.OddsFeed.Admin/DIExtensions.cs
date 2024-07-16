namespace Fanex.OddsFeed.Admin
{
    using Fanex.Data.Repository;
    using Fanex.Logging;
    using Fanex.OddsFeed.Core;
    using Fanex.OddsFeed.Core.Context;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Services.Impl;
    using Fanex.OddsFeed.Services.Interface;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using ONEbook.UM.DependencyInjection;
    using WebOptimizer;

    public static class DIExtensions
    {
        private const string CommonAffiliateJSPath = "js/Affiliate/CommonAffiliate.js";
        private const string CommonLicenseeJSPath = "js/Functions/ListLicensee/CommonLicensee.js";
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDynamicRepository, DynamicRepository>();

            InjectServices(services, configuration);
        }

        public static void RegisterBundle(this IServiceCollection services)
        {
            services.AddWebOptimizer(pipeline =>
            {
                RegisterBundlesJS(pipeline);
                RegisterBundlesCSS(pipeline);
            });
        }

        private static void InjectServices(IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
            services.AddSingleton(appSettings);

            services.AddOneBookUm(configuration);

            services.AddSingleton<ILogger, Logger>();
            services.AddSingleton<ICurrentContext, CurrentContext>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IBaseService, BaseService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IDashboardService, DashboardService>();
            services.AddSingleton<IAffiliateService, AffiliateService>();
            services.AddSingleton<ILicenseeService, LicenseeService>();
            services.AddSingleton<ISubAccService, SubAccService>();
        }

        private static void RegisterBundlesCSS(IAssetPipeline pipeline)
        {
            AddCssBundleLogin(pipeline);
            AddCssBundleVerifyOTP(pipeline);
            AddCssBundleFanexgrid(pipeline);
            AddCssBundleDashboard(pipeline);
            AddCssBundleCommon(pipeline);
            AddCssBundleAdministrator(pipeline);
            AddCssBundleAffiliate(pipeline);
            AddCssBundleLicensee(pipeline);

            pipeline.MinifyCssFiles("minify/css/*.css");
        }
        private static void RegisterBundlesJS(IAssetPipeline pipeline)
        {
            AddJSBundleLogin(pipeline);
            AddJSBundleVerifyOTP(pipeline);
            AddJSBundleUpdatePassword(pipeline);
            AddJSBundleFanexgrid(pipeline);
            AddJSBundleDashboard(pipeline);
            AddJSBundleCommon(pipeline);
            AddJSBundleListCompanyIP(pipeline);
            AddJSBundleAddAdministrator(pipeline);
            AddJSBundleEditAdministrator(pipeline);
            AddJSBundleListAdministrator(pipeline);
            AddJSBundleAffiliate(pipeline);
            AddJSBundleListLicensee(pipeline);
            AddJSBundleAddLicensee(pipeline);
            AddJSBundleEditAffiliateInfo(pipeline);
            AddJSBundleEditAffiliateSetting(pipeline);
            AddJSBundleEditAffiliateProfile(pipeline);
            AddJSBundleSubAccountList(pipeline);
            AddJSBundleAddSubAccount(pipeline);
            AddJSBundleListDomain(pipeline);
            AddJSBundleEditInfoLicensee(pipeline);
            AddJSBundleEditProfileLicensee(pipeline);
            AddJSBundleEditsettinglicensee(pipeline);
            AddJSBundleRequestLicensee(pipeline);
            AddJSBundleRequestAffiliate(pipeline);
            AddJSBundleAddAffiliate(pipeline);

            pipeline.MinifyJsFiles("minify/js/*.js");
        }

        private static void AddCssBundleLicensee(IAssetPipeline pipeline)
        {
            pipeline.AddCssBundle("minify/css/licensee.min.css",
                "css/Licensee/LicenseeEdit.css");
        }

        private static void AddCssBundleAffiliate(IAssetPipeline pipeline)
        {
            pipeline.AddCssBundle("minify/css/affiliate.min.css",
                "css/Affiliate/AffiliateEdit.css");
        }

        private static void AddCssBundleAdministrator(IAssetPipeline pipeline)
        {
            pipeline.AddCssBundle("minify/css/administrator.min.css",
                "css/Administrator/Administrator.css");
        }

        private static void AddCssBundleCommon(IAssetPipeline pipeline)
        {
            pipeline.AddCssBundle("minify/css/common.min.css",
                "css/960.css",
                "css/style.css",
                "css/site.css",
                "css/home.css",
                "css/themes/Aristo/jquery.ui.css",
                "css/jquery.contextMenu.css",
                "css/msgbox/msgbox-0.0.1.css",
                "css/select2.min.css");
        }

        private static void AddCssBundleDashboard(IAssetPipeline pipeline)
        {
            pipeline.AddCssBundle("minify/css/dashboard.min.css",
                "css/Dashboard/Dashboard.css");
        }

        private static void AddCssBundleFanexgrid(IAssetPipeline pipeline)
        {
            pipeline.AddCssBundle("minify/css/fanexgrid.min.css",
                "css/Fanex.Grid/Fanex.Grid.css",
                "css/Fanex.Grid/Fanex.Pagination.css");
        }

        private static void AddCssBundleLogin(IAssetPipeline pipeline)
        {
            pipeline.AddCssBundle("minify/css/login.min.css",
                "css/login.css");
        }

        private static void AddCssBundleVerifyOTP(IAssetPipeline pipeline)
        {
            pipeline.AddCssBundle("minify/css/verifyotp.min.css",
                "css/verifyotp.css");
        }

        private static void AddJSBundleAddAffiliate(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/addaffiliate.min.js",
                CommonLicenseeJSPath,
                CommonAffiliateJSPath,
                "js/Affiliate/AddAffiliate.js",
                "js/Affiliate/AddAffiliateStep1.js",
                "js/Affiliate/AddAffiliateStep2.js",
                "js/Affiliate/AddAffiliateStep3.js",
                "js/Affiliate/AddAffiliateStep4.js",
                "js/Affiliate/EditAffiliate.js");
        }

        private static void AddJSBundleRequestAffiliate(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/requestaffiliate.min.js",
                CommonAffiliateJSPath,
                "js/Affiliate/Request.js");
        }

        private static void AddJSBundleRequestLicensee(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/requestlicensee.min.js",
                CommonAffiliateJSPath,
                "js/Licensee/Request.js");
        }

        private static void AddJSBundleEditsettinglicensee(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/editsettinglicensee.min.js",
                CommonLicenseeJSPath,
                "js/Functions/ListLicensee/EditLicenseeSetting.js");
        }

        private static void AddJSBundleEditProfileLicensee(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/editprofilelicensee.min.js",
                CommonLicenseeJSPath,
                "js/Functions/ListLicensee/EditLicenseeProfile.js");
        }

        private static void AddJSBundleEditInfoLicensee(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/editinfolicensee.min.js",
                CommonLicenseeJSPath,
                "js/Functions/ListLicensee/EditLicenseeInfo.js");
        }

        private static void AddJSBundleListDomain(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/listdomain.min.js",
                "js/Functions/ListDomain/ListDomain.js",
                "js/Functions/ListDomain/RegisterDialog.js",
                "js/Views/Core.js");
        }

        private static void AddJSBundleAddSubAccount(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/addsubaccount.min.js",
                "js/SubAccount/AddSubAcc.js",
                "js/SubAccount/AddSubAccStep1.js",
                "js/SubAccount/AddSubAccStep2.js",
                "js/SubAccount/AddSubAccStep3.js");
        }

        private static void AddJSBundleSubAccountList(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/subaccountlist.min.js",
                "js/SubAccount/SubAccList.js",
                "js/SubAccount/EditSubAcc.js");
        }

        private static void AddJSBundleEditAffiliateProfile(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/editaffiliateprofile.min.js",
                CommonLicenseeJSPath,
                CommonAffiliateJSPath,
                "js/Affiliate/EditAffiliateStep3.js");
        }

        private static void AddJSBundleEditAffiliateSetting(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/editaffiliatesetting.min.js",
               CommonAffiliateJSPath,
               "js/Affiliate/EditAffiliateStep2.js");
        }

        private static void AddJSBundleEditAffiliateInfo(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/editaffiliateinfo.min.js",
               CommonAffiliateJSPath,
               "js/Affiliate/EditAffiliateInfo.js");
        }

        private static void AddJSBundleAddLicensee(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/addlicensee.min.js",
                CommonLicenseeJSPath,
                "js/Functions/ListLicensee/AddLicensee.js",
                "js/Functions/ListLicensee/AddLicenseeStep1.js",
                "js/Functions/ListLicensee/AddLicenseeStep2.js",
                "js/Functions/ListLicensee/AddLicenseeStep3.js");
        }

        private static void AddJSBundleListLicensee(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/listlicensee.min.js",
                CommonLicenseeJSPath,
                "js/Functions/ListLicensee/ListLicensee.js",
                "js/Functions/ListLicensee/EditLicensee.js");
        }

        private static void AddJSBundleAffiliate(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/affiliate.min.js",
                CommonAffiliateJSPath,
                "js/Affiliate/EditAffiliate.js",
                "js/Affiliate/AffiliateList.js");
        }

        private static void AddJSBundleListAdministrator(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/listadministrator.min.js",
                "js/Libs/jquery.watermarkinput.js",
                "js/Administrator/ListAdministrator.js",
                "js/Administrator/EditAdministrator.js");
        }

        private static void AddJSBundleEditAdministrator(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/editadministrator.min.js",
                "js/Administrator/EditAdministrator.js");
        }

        private static void AddJSBundleAddAdministrator(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/addadministrator.min.js",
                "js/Libs/jquery.watermarkinput.js",
                "js/Administrator/AddAdministrator.js",
                "js/Administrator/AddAdministratorStep1.js",
                "js/Administrator/AddAdministratorStep2.js");
        }

        private static void AddJSBundleListCompanyIP(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/listcompanyip.min.js",
                "js/Functions/CompanyIP/CommonCompanyIP.js",
                "js/Functions/CompanyIP/InsertCompanyIP.js");
        }

        private static void AddJSBundleCommon(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/common.min.js",
                "js/Libs/jquery.js",
                "js/Libs/jquery.ui.js",
                "js/Libs/jquery.contextMenu.js",
                "js/Libs/msgbox/msgbox-0.0.1.js",
                "js/Libs/select2.min.js",
                "js/Libs/common.js",
                "js/Libs/Core.js",
                "js/Views/Header.js",
                "js/Views/date.visible.js",
                "js/ReCheckIn.js",
                "js/OddsFeedAdminManager.js");
        }

        private static void AddJSBundleDashboard(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/dashboard.min.js",
                "js/Dashboard/Dashboard.js");
        }

        private static void AddJSBundleFanexgrid(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/fanexgrid.min.js",
                "js/Fanex.Grid/moment.min.js",
                "js/Fanex.Grid/numeral.min.js",
                "js/Fanex.Grid/Fanex.Grid.js",
                "js/Fanex.Grid/Fanex.Pagination.js");
        }

        private static void AddJSBundleUpdatePassword(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("minify/js/updatepassword.min.js",
                "js/User/UpdatePassword.js");
        }

        private static void AddJSBundleLogin(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("/minify/js/login.min.js",
                "js/Libs/jquery.js",
                "js/login/login.js");
        }

        private static void AddJSBundleVerifyOTP(IAssetPipeline pipeline)
        {
            pipeline.AddJavaScriptBundle("/minify/js/verifyotp.min.js",
                "js/Libs/jquery.js",
                "js/login/VerifyOTP.js");
        }
    }
}
