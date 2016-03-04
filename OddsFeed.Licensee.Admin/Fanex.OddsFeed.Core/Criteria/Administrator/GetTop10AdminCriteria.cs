// --------------------------------------------------------------------------
// <copyright file="GetTop10AdminCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013.03.05">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using Data.Repository;
    using Framework.Data;

    /// <summary>
    /// Class GetTop10AdminCriteria.
    /// </summary>
    public class GetTop10AdminCriteria : Framework.Data.CriteriaBase
    {
        public GetTop10AdminCriteria()
        {
        }

        [SpParam(SqlParamName = "Username")]
        public string Username { get; set; }
             
        public override string GetSettingKey()
        {
            return "Odds_Top10Admin_Sel";
        }

        /// <summary>
        /// Check data's constrains.
        /// </summary>
        /// <returns>True: valid; False: invalid.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}
