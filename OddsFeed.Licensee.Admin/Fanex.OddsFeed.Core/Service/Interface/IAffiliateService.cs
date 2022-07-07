namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Model;

    public interface IAffiliateService
    {
        IDataReader GetAffiliateListReturnDataReader(byte systemId, string name);

        bool GetStatusById(int id);

        IEnumerable<SelectListItem> GetDomain(
            bool? isActive = null,
            bool? isOddsDomain = null,
            int selectedId = 0);

        IEnumerable<SelectListItem> GetDomainByAffId(
            int partnerId,
            bool? isActive = null,
            bool? isOddsDomain = null,
            int selectedId = 0);

        IList<PartnerIP> GetPartnerIpByAffId(int partnerId);

        IDataReader GetPartnerRequestStatisticReturnDataReader(RequestStatisticCriteria criteria);

        DataCollection GetPartnerRequestStatisticReturnDataCollection(RequestStatisticCriteria criteria);

        DataSet GetTop10AffReturnDataSet(GetTop10AffiliateCriteria criteria);

        IEnumerable<SelectListItem> GetOddsTypeByAffId(int partnerId);

        IEnumerable<SelectListItem> GetOddsType(int selectedId = 0);

        IEnumerable<SelectListItem> GetOddsStyle(int selectedId = 0);

        IEnumerable<SelectListItem> GetBettypeByAffId(int partnerId);

        IEnumerable<SelectListItem> GetBettypeBySystemId(int selectedId = 0);

        IEnumerable<SelectListItem> GetCountryBySystemId(string selectedId = "");

        IEnumerable<SelectListItem> GetCountryByAffId(int partnerId);

        IEnumerable<SelectListItem> GetSportByAffId(int partnerId);

        IEnumerable<SelectListItem> GetSportBySystemId(byte selectedId = 0);

        IEnumerable<SelectListItem> GetLanguageBySystemId(string selectedId = "");

        IEnumerable<SelectListItem> GetLanguageByAffId(int partnerId);

        IList<Affiliate> GetAllAffiliates();

        IList<Affiliate> GetBySystemId(byte systemId);

        CommandResult Insert(InsertAffiliateCriteria criteria);

        DataCollection GetAffiliateListReturnDataCollection(CheckExistAffiliateCriteria criteria);

        DataCollection GetAffiliateListReturnDataCollection(GetAffiliateByNameCriteria criteria);
    }
}