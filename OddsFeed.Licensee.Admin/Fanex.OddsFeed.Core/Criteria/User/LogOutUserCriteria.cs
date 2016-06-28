// --------------------------------------------------------------------------
// <copyright file="LogOutUserCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.01.22">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.User
{
    using System.Diagnostics.CodeAnalysis;
    using Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class LogOutUserCriteria.
    /// </summary>
    public class LogOutUserCriteria : CriteriaBase
    {       
        public int OnUserId { get; set; }      

        public override string GetSettingKey()
        {
            return "Odds_User_Logout";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}