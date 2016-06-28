// --------------------------------------------------------------------------
// <copyright file="Constant.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="thai.tran" date="09-20-2013">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Business
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Class Constant.
    /// </summary>
    public static class Constant
    { 
        public const string SPACE = " ";     

        public const string OK = "OK";        

        public const string ERROR = "ERROR";

        public const string UAT = "UAT";

        public const string LIVE = "LIVE";

        public const string SiteId = "SiteId";

        public const string Environment = "Environment";

        public const int SearchAllSite = -9999;

        /// <summary>
        /// Enumerate Policy.
        /// </summary>
        public enum Policy
        {
            /// <summary>
            /// The none policy.
            /// </summary>
            None = 0,

            /// <summary>
            /// The read policy.
            /// </summary>
            Read = 1,

            /// <summary>
            /// The host policy.
            /// </summary>
            Host = 2
        }

        /// <summary>
        /// Enumerate Crud.
        /// </summary>
        public enum Crud
        {
            /// <summary>
            /// The read crud.
            /// </summary>
            Read = 0,

            /// <summary>
            /// The host crud.
            /// </summary>
            Host = 1
        }

        /// <summary>
        /// Enumerate Level.
        /// </summary>
        public enum Level
        {
            /// <summary>
            /// The user level.
            /// </summary>
            User = 0,

            /// <summary>
            /// The site admin level.
            /// </summary>
            SiteAdmin = 50,

            /// <summary>
            /// The portal admin level.
            /// </summary>
            PortalAdmin = 100
        }

        /// <summary>
        /// Enumerate UserRole.
        /// </summary>
        public enum UserRole
        {
            /// <summary>
            /// It administrator.
            /// </summary>
            ITAdministrator = 1,

            /// <summary>
            /// The licensee.
            /// </summary>
            Licensee = 2,

            /// <summary>
            /// The licensee sub account.
            /// </summary>
            LicenseeSubAccount = 3
        }
    }
}
