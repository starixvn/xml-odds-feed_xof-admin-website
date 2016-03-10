// --------------------------------------------------------------------------
// <copyright file="GetAllDomainCriteria.cs" company="Nexcel Solutions Vietnam">
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
    using Data.Repository;
    /// <summary>
    /// Class GetAllDomainCriteria.
    /// </summary>
    public class GetAllDomainCriteria : CriteriaBase
    {
        public GetAllDomainCriteria()
        {            
        }        

        public bool? IsActive { get; set; }

        public bool? IsOddsDomain { get; set; }


        // Ignored
        [SpParam(Ignored = true)]
        public byte SystemID { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_Domain_GetAll";
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
