namespace Fanex.OddsFeed.Repositories.Impl
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.OddsFeed.Models;
    using Fanex.OddsFeed.Repositories.Interface;

    public class AffiliateRepository : IAffiliateRepository
    {
        public async Task<CommandResult> Insert(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                await db.ExecuteNonQueryAsync(criteria);
                var errCode = db.GetOutputParams()["errCode"];
                var errMsg = db.GetOutputParams()["errMsg"];
                return new CommandResult
                {
                    Code = (errCode is DBNull) ? 0 : Convert.ToInt32(errCode),
                    Message = (errMsg is DBNull) ? string.Empty : Convert.ToString(errMsg)
                };
            }
        }
    }
}