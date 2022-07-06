namespace Fanex.OddsFeed.Core.Repository.Impl
{
    using System;
    using System.Collections;
    using System.Data;
    using Fanex.Data;
    using Fanex.OddsFeed.Core.Command;
    using Fanex.OddsFeed.Core.Criteria.Misc;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;

    public class BaseRepository : IBaseRepository
    {
        public object ReturnSingleValue(ICriteria criteria)
        {
            // prepare SP parameters
            var hash = criteria.GetSpParams();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(hash);
                return db.ExecuteScalar();
            }
        }

        public IDataReader ReturnDataReader(ICriteria criteria, ICriteria logCriteria)
        {
            // prepare SP parameters
            var hash = criteria.GetSpParams();

            hash = logCriteria.AddSpParams(hash);
            hash = (new ResultCriteria()).AddSpParams(hash);

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(hash);
                return db.ExecuteReader();
            }
        }

        public IDataReader ReturnDataReader(ICriteria criteria)
        {
            // prepare SP parameters
            var hash = criteria.GetSpParams();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(hash);
                return db.ExecuteReader();
            }
        }

        public DataSet ReturnDataSet(ICriteria criteria, ICriteria logCriteria)
        {
            // prepare SP parameters
            var hash = criteria.GetSpParams();

            hash = logCriteria.AddSpParams(hash);
            hash = (new ResultCriteria()).AddSpParams(hash);

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(hash);
                return db.ExecuteDataSet();
            }
        }

        public DataSet ReturnDataSet(ICriteria criteria)
        {
            // prepare SP parameters
            var hash = criteria.GetSpParams();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(hash);
                return db.ExecuteDataSet();
            }
        }

        public DataCollection ReturnDataCollection(ICriteria criteria, ICriteria logCriteria)
        {
            // prepare SP parameters
            var hash = criteria.GetSpParams();

            hash = logCriteria.AddSpParams(hash);
            hash = (new ResultCriteria()).AddSpParams(hash);

            var res = new DataCollection();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(hash);
                res.Data = db.ExecuteDataSet();
                res.TotalItem = (int)db.GetOuputParams()["rowcount"];

                return res;
            }
        }

        public DataCollection ReturnDataCollection(ICriteria criteria)
        {
            // prepare SP parameters
            var hash = criteria.GetSpParams();

            var res = new DataCollection();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(hash);
                res.Data = db.ExecuteDataSet();
                res.TotalItem = (int)db.GetOuputParams()["rowcount"];

                return res;
            }
        }

        public ICommandResult ExecuteNonQuery(ICriteria criteria)
        {
            var hash = criteria.GetSpParams();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);

                return GetCommandResult(hash);
            }
        }

        public ICommandResult ExecuteNonQuery(ICriteria criteria, ICriteria logCriteria)
        {
            var hash = criteria.GetSpParams();

            hash = logCriteria.AddSpParams(hash);
            hash = (new ResultCriteria()).AddSpParams(hash);

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);

                return GetCommandResult(hash);
            }
        }

        public CommandResult GetCommandResult(Hashtable hash)
        {
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