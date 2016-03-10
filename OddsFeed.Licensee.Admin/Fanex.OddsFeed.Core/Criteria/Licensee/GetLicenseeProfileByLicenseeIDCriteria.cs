// --------------------------------------------------------------------------
// <copyright file="GetLicenseeProfileByLicenseeIDCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.3.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using System;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class GetLicenseeProfileByLicenseeIDCriteria.
    /// </summary>
    public class GetLicenseeProfileByLicenseeIDCriteria : CriteriaBase
    {
        public short SystemID { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_LicenseeProfileByLicenseeIDSel";
        }
    }
}