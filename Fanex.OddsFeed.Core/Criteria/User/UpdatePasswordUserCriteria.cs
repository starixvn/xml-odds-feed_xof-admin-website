namespace Fanex.OddsFeed.Core.Criteria.User
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.Data;

    public class UpdatePasswordUserCriteria : BaseCommand
    {
        public int UserID { get; set; }

        [Display(Name = "Old_Password", ResourceType = typeof(ViewResources))]
        public string OldPassword { get; set; }

        [Display(Name = "New_Password", ResourceType = typeof(ViewResources))]
        public string Password { get; set; }

        [Display(Name = "Confirm_Password", ResourceType = typeof(ViewResources))]
        public string ConfirmPassword { get; set; }

        public int Creator { get; set; }

        public string? IP { get; set; }

        [SpParam(Ignored = true)]
        public string PreController { get; set; }

        [SpParam(Ignored = true)]
        public string PreAction { get; set; }

        public override string GetSettingKey() => "Odds_UserPasswordUpd";

        public override bool IsValid() => true;
    }
}