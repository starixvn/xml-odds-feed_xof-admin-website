namespace Fanex.OddsFeed.Core.Repository.Impl
{
    using System;
    using System.Collections;
    using Fanex.Data;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Core.Repository.Interface;
    using Data.Repository;
    using Fanex.OddsFeed.Core.Model;
    using System.Linq;

    public class BaseRepository : IBaseRepository
    {
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

        public DataGrid<T> GetDataGrid<T>(ICriteria criteria)
        {
            var hash = criteria.GetSpParams();
            var data = new DataGrid<T>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(hash);
                data.Data = db.Query<T>().ToList();
                data.TotalItems = (int)db.GetOuputParams()["rowcount"];

                return data;
            }
        }

        public CommandResult ExecuteNonQuery(ICriteria criteria)
        {
            var hash = criteria.GetSpParams();
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