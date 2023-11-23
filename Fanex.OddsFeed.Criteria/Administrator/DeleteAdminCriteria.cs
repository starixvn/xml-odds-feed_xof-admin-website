namespace Fanex.OddsFeed.Criteria.Administrator
{
    using System.ComponentModel.DataAnnotations;
    using Data.Repository;

    public class DeleteAdminCriteria : BaseCommand
    {
        [Required]
        public int AdminID { get; set; }

        [Required]
        public bool Isactive { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_Admin_Del";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}