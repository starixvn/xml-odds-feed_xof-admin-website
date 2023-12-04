namespace Fanex.OddsFeed.Criteria.User
{
    using System;
    using Fanex.Data.Repository;
    using Fanex.Data;

    public class UserLoginUserCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        public string UserPwd { get; set; }

        public string LoginDomain { get; set; }

        public string LoginIP { get; set; }

        public string SystemIdNameList { get; set; }

        [SpParam(Ignored = true)]
        public string Validate { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Odds_User_Login";

        public override bool IsValid() => !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(UserPwd);
    }
}