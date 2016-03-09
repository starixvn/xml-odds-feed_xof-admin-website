// --------------------------------------------------------------------------
// <copyright file="UserGetPermissionCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.01.09">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.User
{    
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class UserGetPermissionCriteria.
    /// </summary>
    public class UserGetPermissionCriteria : CriteriaBase
    {
        public UserGetPermissionCriteria()
        {
            ErrCode = 0;
            ErrMsg = string.Empty;
        }
        
        public int UserID { get; set; }

        public byte SystemID { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_User_GetPermission";
        }

        /// <summary>
        /// Check data's constrains.
        /// </summary>
        /// <returns>True: valid; False: invalid.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}