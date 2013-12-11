// --------------------------------------------------------------------------
// <copyright file="GetDomainSettingByPageCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.02.22">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Domain
{
    using System;
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class GetDomainSettingByPageCriteria.
    /// </summary>
    public class GetDomainSettingByPageCriteria : CriteriaBase
    {
        public short DomainID { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_DomainSettingSelByPage";
        }
    }
}
