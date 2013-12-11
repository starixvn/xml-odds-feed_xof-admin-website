// --------------------------------------------------------------------------
// <copyright file="UserLoginUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2012.12.11">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.User
{    
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Framework.Data;

    /// <summary>
    /// Class UserLoginUserCriteria.
    /// </summary>
    public class UserLoginUserCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        public string UserPwd { get; set; }

        public string Validate { get; set; }

        public string LoginDomain { get; set; }

        public string LoginIP { get; set; }

        public string NetSessionID { get; set; }

        public byte UserRole { get; set; }        

        public string SystemIdNameList { get; set; }

        public DateTime LoginDate { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
        
        [SpParam(Ignored = true)]
        public string Doing { get; set; }

        [SpParam(Ignored = true)]
        public string Domain { get; set; }

        [SpParam(Ignored = true)]
        public string BrowserName { get; set; }

        [SpParam(Ignored = true)]
        public string Platform { get; set; }

        [SpParam(Ignored = true)]
        public string CountryCode { get; set; }

        [SpParam(Ignored = true)]
        public int Duration { get; set; }
        
        public override string GetSettingKey()
        {
            return "Odds_User_Login";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}