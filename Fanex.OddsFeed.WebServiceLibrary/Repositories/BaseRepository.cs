namespace Fanex.OddsFeed.WebServiceLibrary.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Fanex.Data.Repository;

    public abstract class BaseRepository
    {
        protected const string ErrorCode = "errcode";
        protected const string ErrorMsg = "errmsg";
        protected const string Language = "language";
        protected const string TotalRecords = "totalrecords";
        protected const string Criteria = "criteria";
        protected const int InvalidCriteriaCode = -2;

        protected virtual IObjectDbAdapter GetObjectDb(string key)
        {
            return new ObjectDbAdapter(key);
        }

        protected virtual IEnumerable<TResult> GetListOf<TResult>(ICriteria criteria)
        {
            using (var db = GetObjectDb(criteria.GetSettingKey()))
            {
                var result = db.Query<TResult>(criteria.GetSpParams());

                return result ?? Enumerable.Empty<TResult>();
            }
        }

        //protected virtual CommandResult ExecuteCriteria(ICriteria criteria)
        //{
        //    using (var db = GetObjectDb(criteria.GetSettingKey()))
        //    {
        //        var inputParams = criteria.GetSpParams();

        //        inputParams.Add(Language, "E");
        //        inputParams.Add(ErrorCode, 0);
        //        inputParams.Add(ErrorMsg, string.Empty);

        //        db.ExecuteNonQuery(inputParams);

        //        var outputParams = db.GetOuputParams();

        //        return new CommandResult(Converter.ToInt(outputParams[ErrorCode]), Converter.ToString2(outputParams[ErrorMsg]));
        //    }
        //}

        //protected virtual object ExecuteScalar(ICriteria criteria)
        //{
        //    using (var db = GetObjectDb(criteria.GetSettingKey()))
        //    {
        //        return db.ExecuteScalar(criteria.GetSpParams());
        //    }
        //}
    }
}
