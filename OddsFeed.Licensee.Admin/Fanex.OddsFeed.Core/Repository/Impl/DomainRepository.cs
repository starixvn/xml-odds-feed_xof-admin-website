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

        #region IDomainRepository Members

        public int CreateDomain(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                db.ExecuteNonQuery();

                return (int)db.GetOuputParams()["errcode"];
            }
        }

        public int UpdateDomain(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                db.ExecuteNonQuery();

                return (int)db.GetOuputParams()["errcode"];
            }
        }

        public int DeleteDomain(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                db.ExecuteNonQuery();

                return (int)db.GetOuputParams()["errcode"];
            }
        }

        #endregion
    }
}
