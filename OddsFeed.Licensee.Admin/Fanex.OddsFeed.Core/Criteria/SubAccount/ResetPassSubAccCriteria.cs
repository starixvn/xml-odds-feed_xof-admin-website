namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.OddsFeed.Core.Web;
    using Data.Repository;

    public class ResetPassSubAccCriteria : CriteriaBase
    {
        public ResetPassSubAccCriteria()
        {
            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        [SpParam(SqlParamName = "UserID")]
        public int SubAccountID { get; set; }

        [Display(Name = "New_Password", ResourceType = typeof(ViewResources))]
        public string Password { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        [SpParam(Ignored = true)]
        [Display(Name = "Confirm_Password", ResourceType = typeof(ViewResources))]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        
        public override string GetSettingKey()
        {
            return "Odds_User_ResetPass";
        }
        
        public override bool IsValid()
        {
            return true;
        }
    }
}