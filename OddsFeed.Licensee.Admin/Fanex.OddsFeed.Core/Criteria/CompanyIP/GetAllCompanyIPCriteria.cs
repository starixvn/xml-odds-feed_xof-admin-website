// --------------------------------------------------------------------------
// <copyright file="GetAllCompanyIPCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.03.26">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.CompanyIP
{
    using Data.Repository;
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class GetAllCompanyIPCriteria.
    /// </summary>
    public class GetAllCompanyIPCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Odds_CompanyIPSel";
        }
    }
}