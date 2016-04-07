// --------------------------------------------------------------------------
// <copyright file="EditLicenseeInfoCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Class EditLicenseeInfoCriteria.
    /// </summary>
    public class EditLicenseeInfoCriteria : CriteriaBase
    {
        public short SystemID { get; set; }

        public int LicenseeID { get; set; }

        public bool IsActive { get; set; }

        public int Creator { get; set; }
        
        public string IP { get; set; }

        public string ContactEmail { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }


        // Ignored
        [SpParam(Ignored = true)]
        public string SystemName { get; set; }

        [SpParam(Ignored = true)]
        public bool AddExistingLicensee { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_LicenseeInfoUpd";
        }
    }
}