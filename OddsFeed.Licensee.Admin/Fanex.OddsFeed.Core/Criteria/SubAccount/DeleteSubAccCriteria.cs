// --------------------------------------------------------------------------
// <copyright file="DeleteSubAccCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.01.09">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class DeleteSubACCCriteria.
    /// </summary>
    public class DeleteSubAccCriteria : Framework.Data.CriteriaBase
    {
        public DeleteSubAccCriteria()
        {            
        }

        [SpParam(SqlParamName = "UserID")]
        public int SubAccountID { get; set; }
               
        public override string GetSettingKey()
        {
            return "Odds_User_Del";
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