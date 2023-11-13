namespace Fanex.OddsFeed.Core.Repository.Impl
{
    using System.Threading.Tasks;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using static System.Runtime.CompilerServices.RuntimeHelpers;

    public class LicenseeRepository : ILicenseeRepository
    {
        public async Task<PartnersProfile> GetProfileAllPartnersBySystemId(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                await db.QueryEntityAsync<PartnersProfile>(criteria);
                var partnersProfile = new PartnersProfile();
                var hash = db.GetOutputParams();

                return new PartnersProfile
                {
                    AffsOddsType = hash["affsoddstype"].ToString(),
                    AffsLanguageList = hash["affslanguagelist"].ToString(),
                    AffsSportIDList = hash["affssportidlist"].ToString(),
                    AffsCountryCodeList = hash["affscountrycodelist"].ToString(),
                    AffsLeagueIDList = hash["affsleagueidlist"].ToString(),
                    AffsOutrightIDList = hash["affsoutrightidlist"].ToString(),
                    AffsBettypeIDList = hash["affsbettypeidlist"].ToString()
                };
            }
        }
    }
}
