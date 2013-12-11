// --------------------------------------------------------------------------
// <copyright file="GetOddStyleByLicenseeIDCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using System.Diagnostics.CodeAnalysis;
    using Core.Business;
    using Framework.Data;

    /// <summary>
    /// Class GetOddStyleByLicenseeIDCriteria.
    /// </summary>
    public class GetOddStyleByLicenseeIDCriteria : CriteriaBase
    {
        public GetOddStyleByLicenseeIDCriteria()
        {            
        }
        
        public int SystemID { get; set; }        
        
        public override string GetSettingKey()
        {
            return "Odds_OddsStyleSelBySystemId";
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
