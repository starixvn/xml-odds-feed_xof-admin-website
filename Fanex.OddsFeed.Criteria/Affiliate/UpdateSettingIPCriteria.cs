namespace Fanex.OddsFeed.Criteria.Affiliate
{
    public class UpdateSettingIPCriteria : BaseNonQueryCommand
    {
        public int PartnerID { get; set; }
        public string IpFrom { get; set; }
        public string IpTo { get; set; }
        public string IpFromOld { get; set; }
        public string IpToOld { get; set; }

        public override string GetSettingKey() => "Odds_Partner_Upd_Setting_IP";

        public override bool IsValid() => true;
    }
}