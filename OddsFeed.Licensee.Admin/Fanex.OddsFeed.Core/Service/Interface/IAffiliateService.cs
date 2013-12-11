// --------------------------------------------------------------------------
// <copyright file="IAffiliateService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Service.Interface
{    
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.OddsFeed.Core.Business;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Model;  
    using Fanex.OddsFeed.Core.Model.ValueObject;              
    using Fanex.OddsFeed.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed")]
    public interface IAffiliateService : IBaseService
    {
        IAffiliate GetById(int id);

        IList<Affiliate> GetByNameAndSystemId(byte systemId, string name);

        IDataReader GetAffiliateListReturnDataReader(byte systemId, string name);

        DataCollection GetAffiliateListReturnDataCollection(GetAffiliateByNameCriteria criteria);

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

        IList<PartnerIP> GetPartnerIpByAffId(
            int partnerId);

        IEnumerable<RequestStatistic> GetPartnerRequestStatistic(RequestStatisticCriteria criteria);

        IDataReader GetPartnerRequestStatisticReturnDataReader(RequestStatisticCriteria criteria);

        DataCollection GetPartnerRequestStatisticReturnDataCollection(RequestStatisticCriteria criteria);

        DataSet GetTop10AffReturnDataSet(GetTop10AffiliateCriteria criteria);

        IEnumerable<SelectListItem> GetOddsTypeByAffId(
          int partnerId);

        IEnumerable<SelectListItem> GetOddsType(
          int selectedId = 0);

        string GetCurrentOddsStyleByAffId(
            int partnerId);

        IEnumerable<SelectListItem> GetOddsStyle(
          int selectedId = 0);

        IEnumerable<SelectListItem> GetBettypeByAffId(
            int partnerId);

        IEnumerable<SelectListItem> GetBettypeBySystemId(            
            int selectedId = 0);

        IEnumerable<SelectListItem> GetCountryBySystemId(
            string selectedId = "");

        IEnumerable<SelectListItem> GetCountryByAffId(
         int partnerId);

        IEnumerable<SelectListItem> GetSportByAffId(
            int partnerId);

        IEnumerable<SelectListItem> GetSportBySystemId(
            byte selectedId = 0);

        IEnumerable<SelectListItem> GetLanguageBySystemId(
            string selectedId = "");

        IEnumerable<SelectListItem> GetLanguageByAffId(
           int partnerId);        
                
        IList<Affiliate> GetAllAffiliates();

        IList<Affiliate> GetBySystemId(byte systemId);

        ICommandResult Insert(InsertAffiliateCriteria criteria);

        ICommandResult Update(CriteriaBase criteria);

        ICommandResult Delete(CriteriaBase criteria);

        DataCollection GetAffiliateListReturnDataCollection(CheckExistAffiliateCriteria criteria);
    }
}