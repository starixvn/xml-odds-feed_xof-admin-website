// --------------------------------------------------------------------------
// <copyright file="EditLicenseeSettingCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Class EditLicenseeSettingCriteria.
    /// </summary>
    public class EditLicenseeSettingCriteria : CriteriaBase
    {
        public short SystemID { get; set; }
        
        public string DomainList { get; set; }

        public string ListIPFrom { get; set; }

        public string ListIPTo { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }


        // Ignored
        [SpParam(Ignored = true)]
        public string SystemName { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_LicenseeSettingUpd";
        }
    }
}
