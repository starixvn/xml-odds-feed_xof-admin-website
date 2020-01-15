// --------------------------------------------------------------------------
// <copyright file="NapSettings.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2012.10.17">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Configuration
{
    using System.Configuration;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Bug", "S4275:Getters and setters should access the expected fields", Justification = "<Pending>")]
    public class NapSettings : ConfigurationSection
    {
        #region consts

        private const string __NAP_SETTINGS = "NapSettings";
        private const string __ENVIRONMENT = "Environment";
        private const string __IS_SITE_LOGIN = "IsSiteLogin";
        private const string __SITE_ID = "SiteId";
        private const string __LB_URL = "LBUrl";
        private const string __UM = "UM";

        #endregion consts

        private static readonly NapSettings _settings = ConfigurationManager.GetSection(__NAP_SETTINGS) as NapSettings;

        public static NapSettings Settings
        {
            get
            {
                return _settings;
            }
        }

        [ConfigurationProperty(__SITE_ID, IsRequired = true)]
        public int SiteId
        {
            get { return (int)this[__SITE_ID]; }
            set { this[__SITE_ID] = value; }
        }

        [ConfigurationProperty(__IS_SITE_LOGIN, IsRequired = true)]
        public int IsSiteLogin
        {
            get { return (int)this[__IS_SITE_LOGIN]; }
            set { this[__IS_SITE_LOGIN] = value; }
        }

        [ConfigurationProperty(__ENVIRONMENT, IsRequired = true)]
        public string Environment
        {
            get { return (string)this[__ENVIRONMENT]; }
            set { this[__ENVIRONMENT] = value; }
        }

        [ConfigurationProperty(__LB_URL, IsRequired = true)]
        public string LbServiceUrl
        {
            get { return (string)this[__LB_URL]; }
            set { this[__LB_URL] = value; }
        }

        [ConfigurationProperty(__UM, IsRequired = true)]
        public bool IsUMTime
        {
            get { return (bool)this[__UM]; }
            set { this[__UM] = value; }
        }
    }
}