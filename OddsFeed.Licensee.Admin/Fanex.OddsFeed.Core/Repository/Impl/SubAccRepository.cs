namespace Fanex.OddsFeed.Core.Repository.Impl
{
    using System;
    using Fanex.Data;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Core.Repository.Interface;

    public class SubAccRepository : ISubAccRepository
    {
        public CommandResult Insert(ref InsertSubAccCriteria criteria)
        {
            // prepare SP parameters
            var hash = criteria.GetSpParams();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);

                criteria.SubAccountID = Convert.ToInt32(db.GetParameterValue("UserID"));

                var errCode = hash["errcode"];
                var errMsg = hash["errmsg"];

                return new CommandResult
                {
                    Code = (errCode is DBNull) ? 0 : Convert.ToInt32(errCode),
                    Message = (errMsg is DBNull) ? string.Empty : Convert.ToString(errMsg)
                };
            }
        }
    }
}