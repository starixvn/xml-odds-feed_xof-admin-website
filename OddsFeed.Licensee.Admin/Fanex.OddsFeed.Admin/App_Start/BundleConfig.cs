// --------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.10.29">Style Cop</change>
// </history>

namespace Fanex.OddsFeed.Admin.App_Start
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Optimization;

    /// <summary>
    /// Class BundleConfig.
    /// </summary>    
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1123:DoNotPlaceRegionsWithinElements", Justification = "Reviewed.")]
    [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
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
            BundleTable.Bundles.EnableDefaultBundles();

            #region Common Javascript bundle used for all pages

            ////var commonJsBundle = new Bundle("~/Scripts/CommonJs", new JsMinify());
            var commonJsBundle = new Bundle("~/Scripts/CommonJs");
            commonJsBundle.AddDirectory("~/Scripts/libs", "*.js", false);
            commonJsBundle.AddFile("~/Scripts/views/date.visible.js");
            commonJsBundle.AddFile("~/Scripts/views/language.js");
            commonJsBundle.AddFile("~/Scripts/views/list.js");
            ////commonJsBundle.AddFile("~/Scripts/views/Core.js");
            commonJsBundle.AddFile("~/Scripts/views/Header.js");
            commonJsBundle.AddFile("~/Scripts/Libs/msgbox/msgbox-0.0.1.js");
            commonJsBundle.AddFile("~/Scripts/ReCheckIn.js");
            commonJsBundle.AddFile("~/Scripts/OddsFeedAdminManager.js");
            BundleTable.Bundles.Add(commonJsBundle);

            #endregion

            #region Common CSS bundle used for all pages

            var commonCssBundle = new Bundle("~/App_Themes/Default/CommonCss", new CssMinify());
            commonCssBundle.AddFile("~/App_Themes/Default/960.css");
            commonCssBundle.AddFile("~/App_Themes/Default/style.css");
            commonCssBundle.AddFile("~/App_Themes/Default/table.css");
            commonCssBundle.AddFile("~/App_Themes/Default/font.css");
            commonCssBundle.AddFile("~/App_Themes/Default/site.css");
            commonCssBundle.AddFile("~/App_Themes/Default/home.css");
            commonCssBundle.AddFile("~/App_Themes/Default/themes/Aristo/jquery.ui.css");
            commonCssBundle.AddFile("~/App_Themes/Default/jquery.contextMenu.css");
            commonCssBundle.AddFile("~/App_Themes/Default/msgbox/msgbox-0.0.1.css");
            ////commonCssBundle.AddDirectory("~/App_Themes/Default/themes/base", "*.css", false);
            BundleTable.Bundles.Add(commonCssBundle);

            #endregion

            #region Login CSS

            var loginCssBundle = new Bundle("~/App_Themes/Default/LoginCss", new CssMinify());
            loginCssBundle.AddFile("~/App_Themes/Default/Login.css");
            BundleTable.Bundles.Add(loginCssBundle);

            #endregion

            #region Fanex Grid
            //// CSS
            var fanexGridCssBundle = new Bundle("~/App_Themes/Default/Fanex.Grid/Fanex.GridCss", new CssMinify());
            fanexGridCssBundle.AddFile("~/App_Themes/Default/Fanex.Grid/Fanex.Grid.css");
            fanexGridCssBundle.AddFile("~/App_Themes/Default/Fanex.Grid/Fanex.Pagination.css");
            BundleTable.Bundles.Add(fanexGridCssBundle);

            //// JS
            ////var fanexGridJsBundle = new Bundle("~/Scripts/Fanex.Grid/Fanex.GridJs", new JsMinify());
            var fanexGridJsBundle = new Bundle("~/Scripts/Fanex.Grid/Fanex.GridJs");
            fanexGridJsBundle.AddFile("~/scripts/Fanex.Grid/Fanex.Grid.js");
            fanexGridJsBundle.AddFile("~/scripts/Fanex.Grid/Fanex.Pagination.js");
            BundleTable.Bundles.Add(fanexGridJsBundle);
            #endregion

            #region Administrator
            //// JS
            var administratorJsBundle = new Bundle("~/Scripts/Administrator/AddAdministratorJs");
            administratorJsBundle.AddFile("~/scripts/Libs/jquery.watermarkinput.js");
            administratorJsBundle.AddFile("~/scripts/Administrator/AddAdministrator.js");
            administratorJsBundle.AddFile("~/scripts/Administrator/AddAdministratorStep1.js");
            administratorJsBundle.AddFile("~/scripts/Administrator/AddAdministratorStep2.js");
            BundleTable.Bundles.Add(administratorJsBundle);

            var administratorCssBundle = new Bundle("~/App_Themes/Default/Administrator/AdministratorCss", new CssMinify());
            administratorCssBundle.AddFile("~/App_Themes/Default/Administrator/Administrator.css");
            BundleTable.Bundles.Add(administratorCssBundle);

            #endregion

            #region List Administrator
            //// JS
            var listadministratorJsBundle = new Bundle("~/Scripts/Administrator/ListAdministratorJs");
            listadministratorJsBundle.AddFile("~/scripts/Administrator/ListAdministrator.js");
            listadministratorJsBundle.AddFile("~/scripts/Administrator/EditAdministrator.js");
            BundleTable.Bundles.Add(listadministratorJsBundle);

            #endregion

            #region Edit Administrator
            //// JS
            var editadministratorJsBundle = new Bundle("~/Scripts/Administrator/EditAdministratorJs");
            editadministratorJsBundle.AddFile("~/scripts/Administrator/EditAdministrator.js");
            BundleTable.Bundles.Add(editadministratorJsBundle);

            #endregion

            #region Licensee
            //// JS
            var licenseeJsBundle = new Bundle("~/Scripts/Functions/ListLicensee/AddLicenseeJs");
            licenseeJsBundle.AddFile("~/scripts/Functions/ListLicensee/CommonLicensee.js");
            licenseeJsBundle.AddFile("~/scripts/Functions/ListLicensee/AddLicensee.js");
            licenseeJsBundle.AddFile("~/scripts/Functions/ListLicensee/AddLicenseeStep1.js");
            licenseeJsBundle.AddFile("~/scripts/Functions/ListLicensee/AddLicenseeStep2.js");
            licenseeJsBundle.AddFile("~/scripts/Functions/ListLicensee/AddLicenseeStep3.js");
            BundleTable.Bundles.Add(licenseeJsBundle);

            var licenseeCssBundle = new Bundle("~/App_Themes/Default/Licensee/LicenseeCss", new CssMinify());
            licenseeCssBundle.AddFile("~/App_Themes/Default/Licensee/LicenseeEdit.css");
            BundleTable.Bundles.Add(licenseeCssBundle);

            #endregion

            #region List Licensee
            //// CSS
            var listLicenseeCssBundle = new Bundle("~/App_Themes/Default/Licensee/LicenseeEditCss", new CssMinify());
            listLicenseeCssBundle.AddFile("~/App_Themes/Default/Licensee/LicenseeEdit.css");
            BundleTable.Bundles.Add(listLicenseeCssBundle);

            //// JS
            ////var listLicenseeJsBundle = new Bundle("~/Scripts/Functions/ListLicensee/ListLicenseeJs", new JsMinify());
            var listLicenseeJsBundle = new Bundle("~/Scripts/Functions/ListLicensee/ListLicenseeJs");
            listLicenseeJsBundle.AddFile("~/scripts/Functions/ListLicensee/CommonLicensee.js");
            listLicenseeJsBundle.AddFile("~/scripts/Functions/ListLicensee/ListLicensee.js");
            listLicenseeJsBundle.AddFile("~/scripts/Functions/ListLicensee/EditLicensee.js");
            BundleTable.Bundles.Add(listLicenseeJsBundle);
            #endregion

            #region List Domain
            //// CSS
            var listDomainCssBundle = new Bundle("~/App_Themes/Default/Functions/ListLicensee/ListDomainCss", new CssMinify());
            listDomainCssBundle.AddFile("~/App_Themes/Default/Functions/ListDomain/ListDomain.css");
            BundleTable.Bundles.Add(listDomainCssBundle);

            //// JS
            var listDomainJsBundle = new Bundle("~/Scripts/Functions/ListDomain/ListDomainJs");
            listDomainJsBundle.AddFile("~/scripts/Functions/ListDomain/ListDomain.js");
            listDomainJsBundle.AddFile("~/scripts/Functions/ListDomain/RegisterDialog.js");
            listDomainJsBundle.AddFile("~/scripts/Views/Core.js");
            BundleTable.Bundles.Add(listDomainJsBundle);
            #endregion

            #region Affiliate
            //// JS
            ////var affiliateJsBundle = new Bundle("~/Scripts/AffiliateJs", new JsMinify());
            var affiliateJsBundle = new Bundle("~/Scripts/Affiliate/AddAffiliateJs");
            affiliateJsBundle.AddFile("~/scripts/Functions/ListLicensee/CommonLicensee.js");
            affiliateJsBundle.AddFile("~/scripts/Affiliate/CommonAffiliate.js");
            affiliateJsBundle.AddFile("~/scripts/Affiliate/AddAffiliate.js");
            affiliateJsBundle.AddFile("~/scripts/Affiliate/AddAffiliateStep1.js");
            affiliateJsBundle.AddFile("~/scripts/Affiliate/AddAffiliateStep2.js");
            affiliateJsBundle.AddFile("~/scripts/Affiliate/AddAffiliateStep3.js");
            affiliateJsBundle.AddFile("~/scripts/Affiliate/AddAffiliateStep4.js");                        
            BundleTable.Bundles.Add(affiliateJsBundle);

            var affiliateCssBundle = new Bundle("~/App_Themes/Default/Affiliate/AffiliateCss", new CssMinify());
            affiliateCssBundle.AddFile("~/App_Themes/Default/Affiliate/AffiliateEdit.css");
            BundleTable.Bundles.Add(affiliateCssBundle);

            #endregion

            #region SubAccount
            //// JS            
            var subAccJsBundle = new Bundle("~/Scripts/SubAcc/AddSubAccJs");
            subAccJsBundle.AddFile("~/Scripts/SubAccount/AddSubAcc.js");
            subAccJsBundle.AddFile("~/Scripts/SubAccount/AddSubAccStep1.js");
            subAccJsBundle.AddFile("~/Scripts/SubAccount/AddSubAccStep2.js");
            subAccJsBundle.AddFile("~/Scripts/SubAccount/AddSubAccStep3.js");
            BundleTable.Bundles.Add(subAccJsBundle);

            #endregion

            #region Dashboard
            var dashboardJsBundle = new Bundle("~/Scripts/Dashboard/DashboardJs");
            dashboardJsBundle.AddFile("~/scripts/Dashboard/Dashboard.js");
            BundleTable.Bundles.Add(dashboardJsBundle);

            var dashboardCssBundle = new Bundle("~/App_Themes/Default/Dashboard/DashboardCss", new CssMinify());
            dashboardCssBundle.AddFile("~/App_Themes/Default/Dashboard/Dashboard.css");
            BundleTable.Bundles.Add(dashboardCssBundle);
            #endregion
        }
    }
}