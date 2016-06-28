// --------------------------------------------------------------------------
// <copyright file="AffiliateRepository.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Repository.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Data;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Criteria.Misc;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Model.ValueObject;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class AffiliateRepository.
    /// </summary>
    public class AffiliateRepository : RepositoryBase<IAffiliate>, IAffiliateRepository
    {
        /// <summary>
        /// Fetches the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IEnumerable {IAffiliate}.</returns>
        public override IEnumerable<IAffiliate> Fetch(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<Affiliate>(criteria.GetSpParams()).OrderBy(s => s.Name).ToList<Affiliate>();
            }
        }

        /// <summary>
        /// Inserts the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="logCriteria">The log criteria.</param>
        /// <returns>ICommand Result.</returns>
        public ICommandResult Insert(ref InsertAffiliateCriteria criteria)
        {
            // prepare SP parameters
            Hashtable hash = criteria.GetSpParams();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);

                criteria.PartnerID = Convert.ToInt32(db.GetParameterValue("PartnerID"));

                return GetCommandResult(hash);
            }
        }
    }
}
