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
    public class CheckExistAdministratorCriteria : CriteriaBase
    {        
        [Required]
        public int SystemID { get; set; }

        [SpParam(SqlParamName = "RoleID")]
        public int RoleID { get; set; }

        [Required]
        [SpParam(SqlParamName = "Username")]
        public string Username { get; set; }               

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }


        // Ignored 
        [SpParam(Ignored = true)]
        public int UserID { get; set; }

        [SpParam(Ignored = true)]
        public int RoleName { get; set; }

        [SpParam(Ignored = true)]
        public string FirstName { get; set; }

        [SpParam(Ignored = true)]
        public string LastName { get; set; }

        [SpParam(Ignored = true)]
        public string Email { get; set; }

        [SpParam(Ignored = true)]
        public string Creator { get; set; }

        [SpParam(Ignored = true)]
        public bool IsActive { get; set; }

        [SpParam(Ignored = true)]
        public DateTime CreateDate { get; set; }     

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
