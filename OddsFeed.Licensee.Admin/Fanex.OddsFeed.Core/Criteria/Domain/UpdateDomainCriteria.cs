// --------------------------------------------------------------------------
// <copyright file="UpdateDomainCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.02.22">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Domain
{
    using System;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class UpdateDomainCriteria.
    /// </summary>
    public class UpdateDomainCriteria : CriteriaBase
    {
        public short DomainID { get; set; }

        public string DomainName { get; set; }

        public string IP { get; set; }

        public string ISP { get; set; }

        public int Creator { get; set; }

        public string CurrentIP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_DomainUpd";
        }
    }
}
