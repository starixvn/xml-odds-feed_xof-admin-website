// --------------------------------------------------------------------------
// <copyright file="GetLanguageByAffiliateIDCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class GetLanguageByAffiliateIDCriteria.
    /// </summary>
    public class GetLanguageByAffiliateIDCriteria : CriteriaBase
    {
        public GetLanguageByAffiliateIDCriteria()
        {            
        }

        public int PartnerID { get; set; }        
        
        public override string GetSettingKey()
        {
            return "Odds_Language_SelByPartnerId";
        }

        /// <summary>
        /// Check data's constrains.
        /// </summary>
        /// <returns>True: valid; False: invalid..</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}
