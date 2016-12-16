namespace Fanex.OddsFeed.Admin.App_Start
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Optimization;

    /// <summary>
    /// Class BundleConfig.
    /// </summary>    
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1123:DoNotPlaceRegionsWithinElements", Justification = "Reviewed.")]
    [SuppressMessage(
           "StyleCop.CSharp.StylecopPlus.Rules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]    
    public class BundleConfig
    {
        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            #region Common Javascript bundle used for all pages

            var commonJsBundle = new Bundle("~/Scripts/CommonJs");
            commonJsBundle.IncludeDirectory("~/Scripts/libs", "*.js", false);
            commonJsBundle.Include("~/Scripts/views/date.visible.js");
            commonJsBundle.Include("~/Scripts/views/language.js");
            commonJsBundle.Include("~/Scripts/views/Header.js");
            commonJsBundle.Include("~/Scripts/Libs/msgbox/msgbox-0.0.1.js");
            commonJsBundle.Include("~/Scripts/ReCheckIn.js");
            commonJsBundle.Include("~/Scripts/OddsFeedAdminManager.js");
            BundleTable.Bundles.Add(commonJsBundle);

            #endregion

            #region Common CSS bundle used for all pages

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
            BundleTable.Bundles.Add(commonCssBundle);

            #endregion

            #region Login CSS

            var loginCssBundle = new Bundle("~/App_Themes/Default/LoginCss", new CssMinify());
            loginCssBundle.Include("~/App_Themes/Default/Login.css");
            BundleTable.Bundles.Add(loginCssBundle);

            #endregion

            #region Fanex Grid
            //// CSS
            var fanexGridCssBundle = new Bundle("~/App_Themes/Default/Fanex.Grid/Fanex.GridCss", new CssMinify());
            fanexGridCssBundle.Include("~/App_Themes/Default/Fanex.Grid/Fanex.Grid.css");
            fanexGridCssBundle.Include("~/App_Themes/Default/Fanex.Grid/Fanex.Pagination.css");
            BundleTable.Bundles.Add(fanexGridCssBundle);

            //// JS
            ////var fanexGridJsBundle = new Bundle("~/Scripts/Fanex.Grid/Fanex.GridJs", new JsMinify());
            var fanexGridJsBundle = new Bundle("~/Scripts/Fanex.Grid/Fanex.GridJs");
            fanexGridJsBundle.Include("~/scripts/Fanex.Grid/Fanex.Grid.js");
            fanexGridJsBundle.Include("~/scripts/Fanex.Grid/Fanex.Pagination.js");
            BundleTable.Bundles.Add(fanexGridJsBundle);
            #endregion

            #region Administrator
            //// JS
            var administratorJsBundle = new Bundle("~/Scripts/Administrator/AddAdministratorJs");
            administratorJsBundle.Include("~/scripts/Libs/jquery.watermarkinput.js");
            administratorJsBundle.Include("~/scripts/Administrator/AddAdministrator.js");
            administratorJsBundle.Include("~/scripts/Administrator/AddAdministratorStep1.js");
            administratorJsBundle.Include("~/scripts/Administrator/AddAdministratorStep2.js");
            BundleTable.Bundles.Add(administratorJsBundle);

            var administratorCssBundle = new Bundle("~/App_Themes/Default/Administrator/AdministratorCss", new CssMinify());
            administratorCssBundle.Include("~/App_Themes/Default/Administrator/Administrator.css");
            BundleTable.Bundles.Add(administratorCssBundle);

            #endregion

            #region List Administrator
            //// JS
            var listadministratorJsBundle = new Bundle("~/Scripts/Administrator/ListAdministratorJs");
            listadministratorJsBundle.Include("~/scripts/Administrator/ListAdministrator.js");
            listadministratorJsBundle.Include("~/scripts/Administrator/EditAdministrator.js");
            BundleTable.Bundles.Add(listadministratorJsBundle);

            #endregion

            #region Edit Administrator
            //// JS
            var editadministratorJsBundle = new Bundle("~/Scripts/Administrator/EditAdministratorJs");
            editadministratorJsBundle.Include("~/scripts/Administrator/EditAdministrator.js");
            BundleTable.Bundles.Add(editadministratorJsBundle);

            #endregion

            #region Licensee
            //// JS
            var licenseeJsBundle = new Bundle("~/Scripts/Functions/ListLicensee/AddLicenseeJs");
            licenseeJsBundle.Include("~/scripts/Functions/ListLicensee/CommonLicensee.js");
            licenseeJsBundle.Include("~/scripts/Functions/ListLicensee/AddLicensee.js");
            licenseeJsBundle.Include("~/scripts/Functions/ListLicensee/AddLicenseeStep1.js");
            licenseeJsBundle.Include("~/scripts/Functions/ListLicensee/AddLicenseeStep2.js");
            licenseeJsBundle.Include("~/scripts/Functions/ListLicensee/AddLicenseeStep3.js");
            BundleTable.Bundles.Add(licenseeJsBundle);

            var licenseeCssBundle = new Bundle("~/App_Themes/Default/Licensee/LicenseeCss", new CssMinify());
            licenseeCssBundle.Include("~/App_Themes/Default/Licensee/LicenseeEdit.css");
            BundleTable.Bundles.Add(licenseeCssBundle);

            #endregion

            #region List Licensee
            //// CSS
            var listLicenseeCssBundle = new Bundle("~/App_Themes/Default/Licensee/LicenseeEditCss", new CssMinify());
            listLicenseeCssBundle.Include("~/App_Themes/Default/Licensee/LicenseeEdit.css");
            BundleTable.Bundles.Add(listLicenseeCssBundle);

            //// JS
            ////var listLicenseeJsBundle = new Bundle("~/Scripts/Functions/ListLicensee/ListLicenseeJs", new JsMinify());
            var listLicenseeJsBundle = new Bundle("~/Scripts/Functions/ListLicensee/ListLicenseeJs");
            listLicenseeJsBundle.Include("~/scripts/Functions/ListLicensee/CommonLicensee.js");
            listLicenseeJsBundle.Include("~/scripts/Functions/ListLicensee/ListLicensee.js");
            listLicenseeJsBundle.Include("~/scripts/Functions/ListLicensee/EditLicensee.js");
            BundleTable.Bundles.Add(listLicenseeJsBundle);
            #endregion

            #region List Domain
            //// CSS
            var listDomainCssBundle = new Bundle("~/App_Themes/Default/Functions/ListLicensee/ListDomainCss", new CssMinify());
            listDomainCssBundle.Include("~/App_Themes/Default/Functions/ListDomain/ListDomain.css");
            BundleTable.Bundles.Add(listDomainCssBundle);

            //// JS
            var listDomainJsBundle = new Bundle("~/Scripts/Functions/ListDomain/ListDomainJs");
            listDomainJsBundle.Include("~/scripts/Functions/ListDomain/ListDomain.js");
            listDomainJsBundle.Include("~/scripts/Functions/ListDomain/RegisterDialog.js");
            listDomainJsBundle.Include("~/scripts/Views/Core.js");
            BundleTable.Bundles.Add(listDomainJsBundle);
            #endregion

            #region Affiliate
            //// JS
            ////var affiliateJsBundle = new Bundle("~/Scripts/AffiliateJs", new JsMinify());
            var affiliateJsBundle = new Bundle("~/Scripts/Affiliate/AddAffiliateJs");
            affiliateJsBundle.Include("~/scripts/Functions/ListLicensee/CommonLicensee.js");
            affiliateJsBundle.Include("~/scripts/Affiliate/CommonAffiliate.js");
            affiliateJsBundle.Include("~/scripts/Affiliate/AddAffiliate.js");
            affiliateJsBundle.Include("~/scripts/Affiliate/AddAffiliateStep1.js");
            affiliateJsBundle.Include("~/scripts/Affiliate/AddAffiliateStep2.js");
            affiliateJsBundle.Include("~/scripts/Affiliate/AddAffiliateStep3.js");
            affiliateJsBundle.Include("~/scripts/Affiliate/AddAffiliateStep4.js");                        
            BundleTable.Bundles.Add(affiliateJsBundle);

            var affiliateCssBundle = new Bundle("~/App_Themes/Default/Affiliate/AffiliateCss", new CssMinify());
            affiliateCssBundle.Include("~/App_Themes/Default/Affiliate/AffiliateEdit.css");
            BundleTable.Bundles.Add(affiliateCssBundle);

            #endregion

            #region SubAccount
            //// JS            
            var subAccJsBundle = new Bundle("~/Scripts/SubAcc/AddSubAccJs");
            subAccJsBundle.Include("~/Scripts/SubAccount/AddSubAcc.js");
            subAccJsBundle.Include("~/Scripts/SubAccount/AddSubAccStep1.js");
            subAccJsBundle.Include("~/Scripts/SubAccount/AddSubAccStep2.js");
            subAccJsBundle.Include("~/Scripts/SubAccount/AddSubAccStep3.js");
            BundleTable.Bundles.Add(subAccJsBundle);

            #endregion

            #region Dashboard
            var dashboardJsBundle = new Bundle("~/Scripts/Dashboard/DashboardJs");
            dashboardJsBundle.Include("~/scripts/Dashboard/Dashboard.js");
            BundleTable.Bundles.Add(dashboardJsBundle);

            var dashboardCssBundle = new Bundle("~/App_Themes/Default/Dashboard/DashboardCss", new CssMinify());
            dashboardCssBundle.Include("~/App_Themes/Default/Dashboard/Dashboard.css");
            BundleTable.Bundles.Add(dashboardCssBundle);
            #endregion
        }
    }
}