// --------------------------------------------------------------------------
// <copyright file="DeleteAdminCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013.03.05">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using Framework.Data;

    /// <summary>
    /// Class DeleteAdminCriteria.
    /// </summary>
    public class DeleteAdminCriteria : CriteriaBase
    {
        public DeleteAdminCriteria()
        {            
        }

        [Required]
        public int AdminID { get; set; }

        [Required]
        public bool Isactive { get; set; }

        public string ErrMsg { get; set; }
               
        public override string GetSettingKey()
        {
            return "Odds_Admin_Del";
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