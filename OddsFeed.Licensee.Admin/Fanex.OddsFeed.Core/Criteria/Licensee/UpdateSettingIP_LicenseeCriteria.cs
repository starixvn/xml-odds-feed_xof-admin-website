namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using System.ComponentModel.DataAnnotations;
    using Data.Repository;

    public class UpdateSettingIP_LicenseeCriteria : ResultCriteria
    {
        [Required]
        public int Creator { get; set; }

        [Required]
        public byte SystemId { get; set; }

        [SpParam(SqlParamName = "IPFrom")]
        public string IpFrom { get; set; }

        [SpParam(SqlParamName = "IPTo")]
        public string IpTo { get; set; }

        [SpParam(SqlParamName = "IPFromOld")]
        public string IpFromOld { get; set; }

        [SpParam(SqlParamName = "IPToOld")]
        public string IpToOld { get; set; }

        public override string GetSettingKey() => "Odds_Upd_Setting_IP_Licensee";

        public override bool IsValid() => true;
    }
}