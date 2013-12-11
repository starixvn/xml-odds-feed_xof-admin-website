// --------------------------------------------------------------------------
// <copyright file="EditLicenseeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.2.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class EditLicenseeCriteria.
    /// </summary>
    public class EditLicenseeCriteria : CriteriaBase
    {
        public short SystemID { get; set; }

        public string SystemName { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_SystemUpd";
        }
    }
}