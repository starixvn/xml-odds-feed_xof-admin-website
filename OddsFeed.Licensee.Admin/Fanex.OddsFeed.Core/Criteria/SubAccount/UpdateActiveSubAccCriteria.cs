// --------------------------------------------------------------------------
// <copyright file="UpdateActiveSubAccCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Class UpdateActiveSubACCCriteria.
    /// </summary>
    public class UpdateActiveSubAccCriteria : CriteriaBase
    {
        public UpdateActiveSubAccCriteria()
        {
            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        [SpParam(SqlParamName = "UserID")]
        public int SubAccountID { get; set; }

        public bool IsActive { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_User_Upd_Active";
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