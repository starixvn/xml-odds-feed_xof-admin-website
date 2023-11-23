namespace Fanex.OddsFeed.Criteria.Administrator
{
    using Data.Repository;
    using Fanex.Data;

    public class GetExistedAdminCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "Username")]
        public string Username { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        // Ignored
        [SpParam(Ignored = true)]
        public int RoleID { get; set; }

        [SpParam(Ignored = true)]
        public string FirstName { get; set; }

        [SpParam(Ignored = true)]
        public string LastName { get; set; }

        [SpParam(Ignored = true)]
        public string Email { get; set; }

        [SpParam(Ignored = true)]
        public string Password { get; set; }

        public override string GetSettingKey() => "Odds_ExistedAdmin_Sel";

        public override bool IsValid() => true;
    }
}