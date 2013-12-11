// --------------------------------------------------------------------------
// <copyright file="GetTop10LicenseeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.03.18">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Framework.Data;

    /// <summary>
    /// Class GetTop10LicenseeCriteria.
    /// </summary>
    public class GetTop10LicenseeCriteria : CriteriaBase
    {
        public GetTop10LicenseeCriteria()
        {
        }

        public string Systemname { get; set; }
 
        public override string GetSettingKey()
        {
            return "Odds_Top10System_Sel";
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
