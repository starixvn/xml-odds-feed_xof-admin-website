namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using System.ComponentModel.DataAnnotations;
    using Data.Repository;

    public class AdminList_GetTop10Criteria : CriteriaBase
    {
        [Required]
        public byte SystemId { get; set; }

        public string Username { get; set; }

        public override string GetSettingKey() => "Odds_AdminList_Top10_Sel";

        public override bool IsValid() => true;
    }
}