// --------------------------------------------------------------------------
// <copyright file="UpdateAdminSystemCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013.03.05">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class UpdateAdminSystemCriteria.
    /// </summary>
    public class UpdateAdminSystemCriteria : Framework.Data.CriteriaBase
    {
        public UpdateAdminSystemCriteria()
        {
        }

        [Required]
        [SpParam(SqlParamName = "AdminID")]
        public int UserID { get; set; }

        public string UserName { get; set; }

        [SpParam(SqlParamName = "ListSystemId")]
        public string ListSystemId { get; set; }

        public string[] SystemList
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ListSystemId))
                {
                    return null;
                }

                return ListSystemId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }

            set
            {
                ListSystemId = Utilities.StringArrayToString(value);
            }
        }

        public int Creator { get; set; }

        public string IP { get; set; }
        
        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
     
        public override string GetSettingKey()
        {
            return "Odds_Admin_UpdSystem";
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
