namespace Fanex.OddsFeed.Core.Repository.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;

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

        public ICommandResult Insert(ref InsertSubAccCriteria criteria)
        {
            // prepare SP parameters
            var hash = criteria.GetSpParams();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                
                criteria.SubAccountID = Convert.ToInt32(db.GetParameterValue("UserID"));

                return GetCommandResult(hash);
            }
        }      
    }
}
