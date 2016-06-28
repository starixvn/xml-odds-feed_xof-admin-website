// --------------------------------------------------------------------------
// <copyright file="GetDomainByAffiliateIDCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class GetDomainByAffiliateIDCriteria.
    /// </summary>
    public class GetDomainByAffiliateIDCriteria : CriteriaBase
    {
        public GetDomainByAffiliateIDCriteria()
        {            
        }

        public int PartnerID { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsOddsDomain { get; set; }
        
        public override string GetSettingKey()
        {
            return "Odds_Domain_SelByPartnerId";
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
