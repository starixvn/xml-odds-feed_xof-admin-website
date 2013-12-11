// --------------------------------------------------------------------------
// <copyright file="DeleteAffiliateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.01.09">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class DeleteAffiliateCriteria.
    /// </summary>
    public class DeleteAffiliateCriteria : CriteriaBase
    {
        public DeleteAffiliateCriteria()
        {            
        }

        public int Creator { get; set; }

        public int PartnerID { get; set; }
       
        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>The key of store procedure "Odds_Partner_Del".</returns>
        public override string GetSettingKey()
        {
            return "Odds_Partner_Del";
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