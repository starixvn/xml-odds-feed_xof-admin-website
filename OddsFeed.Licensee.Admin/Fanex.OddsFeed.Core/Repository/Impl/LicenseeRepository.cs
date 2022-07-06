namespace Fanex.OddsFeed.Core.Repository.Impl
{
    using Fanex.Data;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Data.Repository;

    public class LicenseeRepository : ILicenseeRepository
    {
        public PartnersProfile GetProfileAllPartnersBySystemId(ICriteria criteria)
        {
            var partnersProfile = new PartnersProfile();

            var hash = criteria.GetSpParams();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);

                partnersProfile.AffsOddsType = hash["affsoddstype"].ToString();
                partnersProfile.AffsLanguageList = hash["affslanguagelist"].ToString();
                partnersProfile.AffsSportIDList = hash["affssportidlist"].ToString();
                partnersProfile.AffsCountryCodeList = hash["affscountrycodelist"].ToString();
                partnersProfile.AffsLeagueIDList = hash["affsleagueidlist"].ToString();
                partnersProfile.AffsOutrightIDList = hash["affsoutrightidlist"].ToString();
                partnersProfile.AffsBettypeIDList = hash["affsbettypeidlist"].ToString();
            }

            return partnersProfile;
        }
    }
}