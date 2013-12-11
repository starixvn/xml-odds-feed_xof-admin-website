// --------------------------------------------------------------------------
// <copyright file="DeleteLicenseeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.2.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class DeleteLicenseeCriteria.
    /// </summary>
    public class DeleteLicenseeCriteria : CriteriaBase
    {
        public short SystemID { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_LicenseeDel";
        }
    }
}