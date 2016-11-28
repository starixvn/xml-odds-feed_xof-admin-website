// --------------------------------------------------------------------------
// <copyright file="LicenseeRepository.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.2.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Repository.Impl
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Data;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class LicenseeRepository.
    /// </summary>
    public class LicenseeRepository : RepositoryBase<ILicensee>, ILicenseeRepository
    {
        /// <summary>
        /// Gets the profile all partners by system identifier.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Partners Profile.</returns>
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
