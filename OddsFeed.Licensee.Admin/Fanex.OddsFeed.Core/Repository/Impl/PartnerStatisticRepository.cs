// --------------------------------------------------------------------------
// <copyright file="PartnerStatisticRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
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
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Data;
    using Fanex.OddsFeed.Core.Criteria.Misc;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class PartnerStatisticRepository.
    /// </summary>
    public class PartnerStatisticRepository : RepositoryBase<RequestStatistic>
    {
        /// <summary>
        /// Fetches the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IEnumerable {RequestStatistic}.</returns>
        public override IEnumerable<RequestStatistic> Fetch(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.Query<RequestStatistic>(criteria.GetSpParams()).OrderBy(s => s.Partner).ToList<RequestStatistic>();
                }
            }
            catch
            {
                throw;
            }
        }     
    }
}
