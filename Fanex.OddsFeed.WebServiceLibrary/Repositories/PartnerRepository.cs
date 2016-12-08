namespace Fanex.OddsFeed.WebServiceLibrary.Repositories
{
    using System.Collections;
    using System.Linq;
    using Common.Converters;
    using Common.DataObjects;
    using Common.DTO;
    using Criteria;
    using Fanex.Common;

    public class PartnerRepository : BaseRepository, IPartnerRepository
    {
        private const string All = "all";

        public AuthenticatedPartner GetAuthenticatedPartner(CheckPermissionCriteria criteria)
        {
            using (var db = GetObjectDb(criteria.GetSettingKey()))
            {
                var inputParams = GetInputParams(criteria);

                var profileDataList = db.Query<ProfileData>(inputParams);
                var outputParams = db.GetOuputParams();

                return new AuthenticatedPartner
                {
                    SystemName = Converter.ToString2(outputParams["systemname"]),
                    UserProfile = profileDataList.Count() == 0 ? null : GetUserProfile(profileDataList.FirstOrDefault()),
                    Error = ErrorBuilder.GetError(Converter.ToInt(outputParams[ErrorCode]), outputParams[ErrorMsg].ToString())
                };
            }
        }

        private static Hashtable GetInputParams(CheckPermissionCriteria criteria)
        {
            var inputParams = criteria.GetSpParams();

            inputParams.Add(ErrorCode, 0);
            inputParams.Add(ErrorMsg, string.Empty);

            return inputParams;
        }

        private static UserProfile GetUserProfile(ProfileData profileData)
        {
            return new UserProfile
            {
                OddsType = ListConverter.ConvertToListInt(profileData.OddsType),
                OddRegion = profileData.OddsRegion,
                DelayTimes = ListConverter.ConvertToListInt(profileData.DelayTimes),
                Languages = ListConverter.ConvertToListString(profileData.LanguageList),
                Sports = string.Equals(profileData.SportIDList, All) ? null : ListConverter.ConvertToListInt(profileData.SportIDList),
                Countries = string.Equals(profileData.CountryCodeList, All) ? null : ListConverter.ConvertToListString(profileData.CountryCodeList),
                Leagues = string.Equals(profileData.LeagueIDList, All) ? null : ListConverter.ConvertToListInt(profileData.LeagueIDList),
                Outrights = string.Equals(profileData.OutrightIDList, All) ? null : ListConverter.ConvertToListInt(profileData.OutrightIDList),
                BetTypes = string.Equals(profileData.BetTypeIDList, All) ? null : ListConverter.ConvertToListInt(profileData.BetTypeIDList),
                LeagueKeyword = profileData.LeagueKeyword.ToLower(),
                OutrightsKeyword = profileData.LeagueKeyword_O.ToLower(),
                Domain = profileData.Domain
            };
        }
    }
}
