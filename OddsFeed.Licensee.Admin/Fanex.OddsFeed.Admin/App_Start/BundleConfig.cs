namespace Fanex.OddsFeed.Admin.App_Start
{
    using System.Web.Optimization;

    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            RegisterBundlesForCommon(bundles);

            #region Login CSS

            var loginCssBundle = new Bundle("~/App_Themes/Default/LoginCss", new CssMinify());
            loginCssBundle.Include("~/App_Themes/Default/Login.css");
            bundles.Add(loginCssBundle);

            #endregion Login CSS

            #region Fanex Grid

            //// CSS
            var fanexGridCssBundle = new Bundle("~/App_Themes/Default/Fanex.Grid/Fanex.GridCss", new CssMinify());
            fanexGridCssBundle.Include("~/App_Themes/Default/Fanex.Grid/Fanex.Grid.css");
            fanexGridCssBundle.Include("~/App_Themes/Default/Fanex.Grid/Fanex.Pagination.css");
            bundles.Add(fanexGridCssBundle);

            //// JS
            ////var fanexGridJsBundle = new Bundle("~/Scripts/Fanex.Grid/Fanex.GridJs", new JsMinify());
            var fanexGridJsBundle = new Bundle("~/Scripts/Fanex.Grid/Fanex.GridJs");
            fanexGridJsBundle.Include("~/scripts/Fanex.Grid/moment.min.js");
            fanexGridJsBundle.Include("~/scripts/Fanex.Grid/numeral.min.js");
            fanexGridJsBundle.Include("~/scripts/Fanex.Grid/Fanex.Grid.js");
            fanexGridJsBundle.Include("~/scripts/Fanex.Grid/Fanex.Pagination.js");
            bundles.Add(fanexGridJsBundle);

            #endregion Fanex Grid

            RegisterBundlesForAdministrator(bundles);

            RegisterBundlesForLicensee(bundles);

            RegisterBundlesForDomain(bundles);

            RegisterBundlesForAffiliate(bundles);

            RegisterBundlesForSubAccount(bundles);

            RegisterBundlesForDashboard(bundles);
        }

        private static void RegisterBundlesForCommon(BundleCollection bundles)
        {
            var commonJsBundle = new Bundle("~/Scripts/CommonJs");
            commonJsBundle.IncludeDirectory("~/Scripts/libs", "*.js", false);
            commonJsBundle.Include("~/Scripts/views/date.visible.js");
            commonJsBundle.Include("~/Scripts/views/language.js");
            commonJsBundle.Include("~/Scripts/views/Header.js");
            commonJsBundle.Include("~/Scripts/Libs/msgbox/msgbox-0.0.1.js");
            commonJsBundle.Include("~/Scripts/ReCheckIn.js");
            commonJsBundle.Include("~/Scripts/OddsFeedAdminManager.js");
            bundles.Add(commonJsBundle);

            var commonCssBundle = new Bundle("~/App_Themes/Default/CommonCss", new CssMinify());
            commonCssBundle.Include("~/App_Themes/Default/960.css");
            commonCssBundle.Include("~/App_Themes/Default/style.css");
            commonCssBundle.Include("~/App_Themes/Default/table.css");
            commonCssBundle.Include("~/App_Themes/Default/font.css");
            commonCssBundle.Include("~/App_Themes/Default/site.css");
            commonCssBundle.Include("~/App_Themes/Default/home.css");
            commonCssBundle.Include("~/App_Themes/Default/themes/Aristo/jquery.ui.css");
            commonCssBundle.Include("~/App_Themes/Default/jquery.contextMenu.css");
            commonCssBundle.Include("~/App_Themes/Default/msgbox/msgbox-0.0.1.css");
            bundles.Add(commonCssBundle);
        }

        private static void RegisterBundlesForAdministrator(BundleCollection bundles)
        {
            #region Administrator

            //// JS
            var administratorJsBundle = new Bundle("~/Scripts/Administrator/AddAdministratorJs");
            administratorJsBundle.Include("~/scripts/Libs/jquery.watermarkinput.js");
            administratorJsBundle.Include("~/scripts/Administrator/AddAdministrator.js");
            administratorJsBundle.Include("~/scripts/Administrator/AddAdministratorStep1.js");
            administratorJsBundle.Include("~/scripts/Administrator/AddAdministratorStep2.js");
            bundles.Add(administratorJsBundle);

            var administratorCssBundle = new Bundle("~/App_Themes/Default/Administrator/AdministratorCss", new CssMinify());
            administratorCssBundle.Include("~/App_Themes/Default/Administrator/Administrator.css");
            bundles.Add(administratorCssBundle);

            #endregion Administrator

            #region List Administrator

            //// JS
            var listadministratorJsBundle = new Bundle("~/Scripts/Administrator/ListAdministratorJs");
            listadministratorJsBundle.Include("~/scripts/Administrator/ListAdministrator.js");
            listadministratorJsBundle.Include("~/scripts/Administrator/EditAdministrator.js");
            bundles.Add(listadministratorJsBundle);

            #endregion List Administrator

            #region Edit Administrator

            //// JS
            var editadministratorJsBundle = new Bundle("~/Scripts/Administrator/EditAdministratorJs");
            editadministratorJsBundle.Include("~/scripts/Administrator/EditAdministrator.js");
            bundles.Add(editadministratorJsBundle);

            #endregion Edit Administrator
        }

        private static void RegisterBundlesForLicensee(BundleCollection bundles)
        {
            #region Licensee

            //// JS
            var licenseeJsBundle = new Bundle("~/Scripts/Functions/ListLicensee/AddLicenseeJs");
            licenseeJsBundle.Include("~/scripts/Functions/ListLicensee/CommonLicensee.js");
            licenseeJsBundle.Include("~/scripts/Functions/ListLicensee/AddLicensee.js");
            licenseeJsBundle.Include("~/scripts/Functions/ListLicensee/AddLicenseeStep1.js");
            licenseeJsBundle.Include("~/scripts/Functions/ListLicensee/AddLicenseeStep2.js");
            licenseeJsBundle.Include("~/scripts/Functions/ListLicensee/AddLicenseeStep3.js");
            bundles.Add(licenseeJsBundle);

            var licenseeCssBundle = new Bundle("~/App_Themes/Default/Licensee/LicenseeCss", new CssMinify());
            licenseeCssBundle.Include("~/App_Themes/Default/Licensee/LicenseeEdit.css");
            bundles.Add(licenseeCssBundle);

            #endregion Licensee

            #region List Licensee

            //// CSS
            var listLicenseeCssBundle = new Bundle("~/App_Themes/Default/Licensee/LicenseeEditCss", new CssMinify());
            listLicenseeCssBundle.Include("~/App_Themes/Default/Licensee/LicenseeEdit.css");
            bundles.Add(listLicenseeCssBundle);

            //// JS
            ////var listLicenseeJsBundle = new Bundle("~/Scripts/Functions/ListLicensee/ListLicenseeJs", new JsMinify());
            var listLicenseeJsBundle = new Bundle("~/Scripts/Functions/ListLicensee/ListLicenseeJs");
            listLicenseeJsBundle.Include("~/scripts/Functions/ListLicensee/CommonLicensee.js");
            listLicenseeJsBundle.Include("~/scripts/Functions/ListLicensee/ListLicensee.js");
            listLicenseeJsBundle.Include("~/scripts/Functions/ListLicensee/EditLicensee.js");
            bundles.Add(listLicenseeJsBundle);

            #endregion List Licensee
        }

        private static void RegisterBundlesForDomain(BundleCollection bundles)
        {
            var listDomainCssBundle = new Bundle("~/App_Themes/Default/Functions/ListLicensee/ListDomainCss", new CssMinify());
            listDomainCssBundle.Include("~/App_Themes/Default/Functions/ListDomain/ListDomain.css");
            bundles.Add(listDomainCssBundle);

            var listDomainJsBundle = new Bundle("~/Scripts/Functions/ListDomain/ListDomainJs");
            listDomainJsBundle.Include("~/scripts/Functions/ListDomain/ListDomain.js");
            listDomainJsBundle.Include("~/scripts/Functions/ListDomain/RegisterDialog.js");
            listDomainJsBundle.Include("~/scripts/Views/Core.js");
            bundles.Add(listDomainJsBundle);
        }

        private static void RegisterBundlesForAffiliate(BundleCollection bundles)
        {
            var affiliateJsBundle = new Bundle("~/Scripts/Affiliate/AddAffiliateJs");
            affiliateJsBundle.Include("~/scripts/Functions/ListLicensee/CommonLicensee.js");
            affiliateJsBundle.Include("~/scripts/Affiliate/CommonAffiliate.js");
            affiliateJsBundle.Include("~/scripts/Affiliate/AddAffiliate.js");
            affiliateJsBundle.Include("~/scripts/Affiliate/AddAffiliateStep1.js");
            affiliateJsBundle.Include("~/scripts/Affiliate/AddAffiliateStep2.js");
            affiliateJsBundle.Include("~/scripts/Affiliate/AddAffiliateStep3.js");
            affiliateJsBundle.Include("~/scripts/Affiliate/AddAffiliateStep4.js");
            bundles.Add(affiliateJsBundle);

            var affiliateCssBundle = new Bundle("~/App_Themes/Default/Affiliate/AffiliateCss", new CssMinify());
            affiliateCssBundle.Include("~/App_Themes/Default/Affiliate/AffiliateEdit.css");
            bundles.Add(affiliateCssBundle);
        }

        private static void RegisterBundlesForSubAccount(BundleCollection bundles)
        {
            var subAccJsBundle = new Bundle("~/Scripts/SubAcc/AddSubAccJs");
            subAccJsBundle.Include("~/Scripts/SubAccount/AddSubAcc.js");
            subAccJsBundle.Include("~/Scripts/SubAccount/AddSubAccStep1.js");
            subAccJsBundle.Include("~/Scripts/SubAccount/AddSubAccStep2.js");
            subAccJsBundle.Include("~/Scripts/SubAccount/AddSubAccStep3.js");
            bundles.Add(subAccJsBundle);
        }

        private static void RegisterBundlesForDashboard(BundleCollection bundles)
        {
            var dashboardJsBundle = new Bundle("~/Scripts/Dashboard/DashboardJs");
            dashboardJsBundle.Include("~/scripts/Dashboard/Dashboard.js");
            bundles.Add(dashboardJsBundle);

            var dashboardCssBundle = new Bundle("~/App_Themes/Default/Dashboard/DashboardCss", new CssMinify());
            dashboardCssBundle.Include("~/App_Themes/Default/Dashboard/Dashboard.css");
            bundles.Add(dashboardCssBundle);
        }
    }
}