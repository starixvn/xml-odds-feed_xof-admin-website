namespace Fanex.OddsFeed.Repositories.Impl
{
    using System.Threading.Tasks;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Repositories.Interface;
    using static System.Runtime.CompilerServices.RuntimeHelpers;

    public class LicenseeRepository : ILicenseeRepository
    {
        public async Task<PartnersProfile> GetProfileAllPartnersBySystemId(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                await db.QueryEntityAsync<PartnersProfile>(criteria);
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
