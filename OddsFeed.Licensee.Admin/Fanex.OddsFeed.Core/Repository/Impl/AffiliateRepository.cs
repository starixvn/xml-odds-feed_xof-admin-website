namespace Fanex.OddsFeed.Core.Repository.Impl
{
    using System;
    using Fanex.Data;
    using Fanex.OddsFeed.Core.Command;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Framework.Data;

    public class AffiliateRepository : IAffiliateRepository
    {
        public ICommandResult Insert(ref InsertAffiliateCriteria criteria)
        {
            // prepare SP parameters
            var hash = criteria.GetSpParams();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);

                criteria.PartnerID = Convert.ToInt32(db.GetParameterValue("PartnerID"));

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