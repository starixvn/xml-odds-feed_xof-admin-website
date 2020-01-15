// --------------------------------------------------------------------------
// <copyright file="GetLicenseeProfileByLicenseeIDCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.3.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using Data.Repository;

    public class GetLicenseeProfileByLicenseeIDCriteria : CriteriaBase
    {
        public short SystemID { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_LicenseeProfileByLicenseeIDSel";
        }
    }
}