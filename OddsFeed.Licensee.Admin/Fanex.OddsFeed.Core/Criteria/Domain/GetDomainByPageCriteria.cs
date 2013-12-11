// --------------------------------------------------------------------------
// <copyright file="GetDomainByPageCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.02.22">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Domain
{
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class GetDomainByPageCriteria.
    /// </summary>
    public class GetDomainByPageCriteria : CriteriaBase
    {        
        public string DomainName { get; set; }

        public int DomainId { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_DomainSelByPage";
        }
    }
}
