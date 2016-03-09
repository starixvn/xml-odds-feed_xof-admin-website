// --------------------------------------------------------------------------
// <copyright file="AssignLicensee_SubAccCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.SubAccount
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class AssignLicensee_SubACCCriteria.
    /// </summary>
    public class AssignLicensee_SubAccCriteria : Framework.Data.CriteriaBase
    {
        public AssignLicensee_SubAccCriteria()
        {
        }

        [SpParam(SqlParamName = "AdminID")]
        public int UserID { get; set; }
        
        [SpParam(SqlParamName = "ListSystemId")]
        public string ListSystemId { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }


        // Ignored
        [SpParam(Ignored = true)]
        public string UserName { get; set; }

        [SpParam(Ignored = true)]
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
