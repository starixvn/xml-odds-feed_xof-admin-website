namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using Data.Repository;

    public class DeleteSettingIP_AffiliateCriteria : CriteriaBase
    {
        public int PartnerId { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        public string IPFromOld { get; set; }

        public string IPToOld { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Odds_Delete_Setting_IP_Partner";

        public override bool IsValid() => true;
    }
}