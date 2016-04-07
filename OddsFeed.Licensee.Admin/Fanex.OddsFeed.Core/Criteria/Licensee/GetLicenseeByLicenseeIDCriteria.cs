// --------------------------------------------------------------------------
// <copyright file="GetLicenseeByLicenseeIDCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.2.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using Data.Repository;
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class GetLicenseeByLicenseeIDCriteria.
    /// </summary>
    public class GetLicenseeByLicenseeIDCriteria : CriteriaBase
    {
        public short SystemID { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_LicenseeByIDSel";
        }
    }
}