// --------------------------------------------------------------------------
// <copyright file="SubAccRepository.cs" company="Nexcel Solutions Vietnam">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Data;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Criteria.Misc;
    using Fanex.OddsFeed.Core.Criteria.User;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Model.ValueObject;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class SubACCRepository.
    /// </summary>
    public class SubAccRepository : RepositoryBase<IUser>, ISubAccRepository
    {
        /// <summary>
        /// Fetches the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IEnumerable {IUser}.</returns>
        public override IEnumerable<IUser> Fetch(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.Query<User>(criteria.GetSpParams()).OrderBy(s => s.Name).ToList<User>();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Inserts the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="logCriteria">The log criteria.</param>
        /// <returns>ICommand Result.</returns>
        public ICommandResult Insert(ref InsertSubAccCriteria criteria, ICriteria logCriteria)
        {
            // prepare SP parameters
            Hashtable hash = criteria.GetSpParams();

            hash = logCriteria.AddSpParams(hash);
            hash = (new ResultCriteria()).AddSpParams(hash);

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);

                criteria.SubAccountID = Convert.ToInt32(db.GetParameterValue("UserID"));

                return GetCommandResult(hash);
            }
        }      
    }
}
