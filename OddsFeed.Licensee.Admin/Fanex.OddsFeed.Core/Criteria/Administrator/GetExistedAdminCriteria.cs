// --------------------------------------------------------------------------
// <copyright file="GetExistedAdminCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013.03.05">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using Framework.Data;

    /// <summary>
    /// Class GetExistedAdminCriteria.
    /// </summary>
    public class GetExistedAdminCriteria : CriteriaBase
    {
        public GetExistedAdminCriteria()
        {
        }

        [SpParam(SqlParamName = "Username")]
        public string Username { get; set; }

        [SpParam(SqlParamName = "RoleID")]
        public int RoleID { get; set; }

        [SpParam(SqlParamName = "FirstName")]
        public string FirstName { get; set; }

        [SpParam(SqlParamName = "LastName")]
        public string LastName { get; set; }

        [SpParam(SqlParamName = "Email")]
        public string Email { get; set; }

        [SpParam(SqlParamName = "Password")]
        public string Password { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
                
        public override string GetSettingKey()
        {
            return "Odds_ExistedAdmin_Sel";
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
