// --------------------------------------------------------------------------
// <copyright file="UpdateCompanyIPCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.05.17">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.CompanyIP
{
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class UpdateCompanyIPCriteria.
    /// </summary>
    public class UpdateCompanyIPCriteria : CriteriaBase
    {
        public int Creator { get; set; }

        public string IPFrom { get; set; }

        public string IPTo { get; set; }        

        public string IPFromOld { get; set; }

        public string IPToOld { get; set; }

        public string Description { get; set; }
        
        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_CompanyIP_Setting";
        }
    }
}