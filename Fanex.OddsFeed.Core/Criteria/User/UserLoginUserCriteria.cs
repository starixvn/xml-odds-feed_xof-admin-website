namespace Fanex.OddsFeed.Core.Criteria.User
{
    using System;
    using Data.Repository;
    using Fanex.Data;
    public class UserLoginUserCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        public string UserPwd { get; set; }

        public string LoginDomain { get; set; }

        public string LoginIP { get; set; }

        public string SystemIdNameList { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        [SpParam(Ignored = true)]
        public DateTime LoginDate { get; set; }

        [SpParam(Ignored = true)]
        public byte UserRole { get; set; }

        [SpParam(Ignored = true)]
        public string NetSessionID { get; set; }

        [SpParam(Ignored = true)]
        public string Validate { get; set; }

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

        public override string GetSettingKey() => "Odds_User_Login";

        public override bool IsValid() => true;
    }
}