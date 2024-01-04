namespace Fanex.OddsFeed.Criteria.Administrator
{
    using System.ComponentModel.DataAnnotations;

    public class DeleteAdminCriteria : BaseNonQueryCommand
    {
        [Required]
        public int AdminID { get; set; }

        [Required]
        public bool Isactive { get; set; }

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