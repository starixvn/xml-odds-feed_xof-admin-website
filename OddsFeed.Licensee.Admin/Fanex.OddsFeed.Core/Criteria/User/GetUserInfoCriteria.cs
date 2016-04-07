// --------------------------------------------------------------------------
// <copyright file="GetUserInfoCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.User
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class GetUserInfoCriteria.
    /// </summary>
    public class GetUserInfoCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_User_GetInfo";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
