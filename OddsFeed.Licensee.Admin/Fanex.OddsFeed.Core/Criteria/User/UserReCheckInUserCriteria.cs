// --------------------------------------------------------------------------
// <copyright file="UserReCheckInUserCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.03.21">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.User
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class UserReCheckInUserCriteria.
    /// </summary>
    public class UserReCheckInUserCriteria : CriteriaBase
    {       
        public int OnUserId { get; set; }

        public bool IsKickedOut { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_User_ReCheckIn";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}