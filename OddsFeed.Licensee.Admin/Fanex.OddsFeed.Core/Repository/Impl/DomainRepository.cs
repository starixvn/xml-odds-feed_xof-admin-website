namespace Fanex.OddsFeed.Core.Repository.Impl
{
    using Fanex.Data;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Data.Repository;

    public class DomainRepository : IDomainRepository
    {
        public DataCollection GetAllDomain(ICriteria criteria)
        {
            var res = new DataCollection();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                res.Data = db.ExecuteDataSet();
                res.TotalItem = (int)db.GetOuputParams()["rowcount"];
                return res;
            }
        }

        public DataCollection GetDomainSetting(ICriteria criteria)
        {
            var res = new DataCollection();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                res.Data = db.ExecuteDataSet();
                res.TotalItem = (int)db.GetOuputParams()["rowcount"];
                return res;
            }
        }
    }
}