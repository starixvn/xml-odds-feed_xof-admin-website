// --------------------------------------------------------------------------
// <copyright file="UpdateLicenseeActiveCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.2.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using System;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class UpdateLicenseeActiveCriteria.
    /// </summary>
    public class UpdateLicenseeActiveCriteria : CriteriaBase
    {
        public short SystemID { get; set; }

        public bool IsActive { get; set; }

        public bool IsActiveAllAffiliate { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_LicenseeActiveUpd";
        }
    }
}