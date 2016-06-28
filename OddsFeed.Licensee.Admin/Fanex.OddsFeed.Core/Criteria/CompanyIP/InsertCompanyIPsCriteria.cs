// --------------------------------------------------------------------------
// <copyright file="InsertCompanyIPsCriteria.cs" company=" Solutions Vietnam">
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
    /// Class InsertCompanyIPsCriteria.
    /// </summary>
    public class InsertCompanyIPsCriteria : CriteriaBase
    {
        public string ListIPFrom { get; set; }

        public string ListIPTo { get; set; }

        public string ListDescription { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_CompanyIPIns";
        }
    }
}