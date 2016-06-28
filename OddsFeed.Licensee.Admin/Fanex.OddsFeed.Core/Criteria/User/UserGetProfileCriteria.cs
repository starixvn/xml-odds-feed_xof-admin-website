// --------------------------------------------------------------------------
// <copyright file="UserGetProfileCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2012.12.11">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.User
{
    using System.Diagnostics.CodeAnalysis;
    using Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class UserGetProfileCriteria.
    /// </summary>
    public class UserGetProfileCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public byte SystemId { get; set; }
        
        public string IP { get; set; }

        public string NetSessionID { get; set; }        

        public int OnUserId { get; set; }        

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
                
        public override string GetSettingKey()
        {
            return "Odds_User_GetProfile";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}