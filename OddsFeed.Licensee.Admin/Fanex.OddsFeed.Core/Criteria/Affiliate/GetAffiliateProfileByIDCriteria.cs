// --------------------------------------------------------------------------
// <copyright file="GetAffiliateProfileByIDCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.08.06">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class GetAffiliateProfileByIDCriteria.
    /// </summary>    
    public class GetAffiliateProfileByIDCriteria : CriteriaBase
    {
        public int PartnerID { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_PartnerProfileSelByID";
        }
    }
}