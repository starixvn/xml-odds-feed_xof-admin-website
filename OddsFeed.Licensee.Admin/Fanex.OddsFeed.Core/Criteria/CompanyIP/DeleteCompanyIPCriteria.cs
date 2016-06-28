// --------------------------------------------------------------------------
// <copyright file="DeleteCompanyIPCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.CompanyIP
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class DeleteCompanyIPCriteria.
    /// </summary>    
    public class DeleteCompanyIPCriteria : CriteriaBase
    {
        public DeleteCompanyIPCriteria()
        {
        }

        public int Creator { get; set; }

        public string IP { get; set; }

        public string IPFromOld { get; set; }

        public string IPToOld { get; set; }                

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
       
        public override string GetSettingKey()
        {
            return "Odds_CompanyIP_DeleteSetting";
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
