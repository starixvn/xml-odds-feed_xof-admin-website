namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using Data.Repository;
    using Fanex.Data;

    public class UpdateSettingIPCriteria : CriteriaBase
    {
        public int PartnerID { get; set; }

        [SpParam(SqlParamName = "IPFrom")]
        public string IpFrom { get; set; }

        [SpParam(SqlParamName = "IPTo")]
        public string IpTo { get; set; }

        [SpParam(SqlParamName = "IPFromOld")]
        public string IpFromOld { get; set; }

        [SpParam(SqlParamName = "IPToOld")]
        public string IpToOld { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Odds_Partner_Upd_Setting_IP";

        public override bool IsValid() => true;
    }
}