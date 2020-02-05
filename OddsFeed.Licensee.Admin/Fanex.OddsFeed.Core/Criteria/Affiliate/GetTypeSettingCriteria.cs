// --------------------------------------------------------------------------
// <copyright file="GetTypeSettingCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;

    /// <summary>
    /// Class GetTypeSettingCriteria.
    /// </summary>
    public class GetTypeSettingCriteria : CriteriaBase
    {
        public GetTypeSettingCriteria()
        {
        }

        public byte SystemID { get; set; }

        public string Type { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_SystemSettingsSel";
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