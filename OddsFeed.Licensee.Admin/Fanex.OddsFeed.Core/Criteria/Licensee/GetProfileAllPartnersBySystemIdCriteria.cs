namespace Fanex.OddsFeed.Core.Criteria.Licensee
{
    using Data.Repository;

    public class GetProfileAllPartnersBySystemIdCriteria : CriteriaBase
    {
        public int SystemID { get; set; }

        public string AffsOddsType { get; set; }
        public string AffsLanguageList { get; set; }
        public string AffsSportIDList { get; set; }
        public string AffsCountryCodeList { get; set; }
        public string AffsLeagueIDList { get; set; }
        public string AffsOutrightIDList { get; set; }
        public string AffsBettypeIDList { get; set; }

        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = string.Empty;

        public override string GetSettingKey() => "Odds_GetProfileAllPartnersBySystemId";

        public override bool IsValid() => true;
    }
}