// --------------------------------------------------------------------------
// <copyright file="CheckExistAdministratorCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2013.06.26">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class CheckExistAdministratorCriteria.
    /// </summary>
    public class CheckExistAdministratorCriteria : Framework.Data.CriteriaBase
    {
        public int UserID { get; set; }

        [Required]
        public int SystemID { get; set; }

        [Required]
        [SpParam(SqlParamName = "Username")]
        public string Username { get; set; }

        [SpParam(SqlParamName = "RoleID")]
        public int RoleID { get; set; }

        [SpParam(SqlParamName = "RoleName")]
        public int RoleName { get; set; }

        [SpParam(SqlParamName = "FirstName")]
        public string FirstName { get; set; }

        [SpParam(SqlParamName = "LastName")]
        public string LastName { get; set; }

        [SpParam(SqlParamName = "Email")]
        public string Email { get; set; }

        public string Creator { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
        
        public override string GetSettingKey()
        {
            return "Odds_Admin_CheckExist";
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
