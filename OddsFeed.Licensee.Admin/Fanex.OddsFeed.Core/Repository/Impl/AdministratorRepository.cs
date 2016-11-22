// --------------------------------------------------------------------------
// <copyright file="AdministratorRepository.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013.03.05">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Repository.Impl
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Data;
    using Fanex.OddsFeed.Core.Criteria.Misc;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Framework.Data;
    using Fanex.Data.Repository;    /// <summary>
                              /// Class AdministratorRepository.
                              /// </summary>
    public class AdministratorRepository : RepositoryBase<IAdministrator>, IAdministratorRepository
    {
        /// <summary>
        /// Inserts the admin.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>ICommand Result.</returns>
        public ICommandResult InsertAdmin(ICriteria criteria)
        {
            // prepare SP parameters
            var hash = criteria.GetSpParams();
            
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);

                return GetCommandResult(hash);
            }
        }
    }
}
