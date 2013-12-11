// --------------------------------------------------------------------------
// <copyright file="GetActivateByLicenseeIDCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using System.Diagnostics.CodeAnalysis;
    using Framework.Data;

    /// <summary>
    /// Class GetActivateByLicenseeIDCriteria.
    /// </summary>
    public class GetActivateByLicenseeIDCriteria : CriteriaBase
    {
        public GetActivateByLicenseeIDCriteria()
        {            
        }
        
        public int SystemID { get; set; }        

        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>String "Odds_Licensee_GetActivate".</returns>
        public override string GetSettingKey()
        {
            return "Odds_Licensee_GetActivate";
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
