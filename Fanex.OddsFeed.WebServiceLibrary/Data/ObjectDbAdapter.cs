namespace Fanex.OddsFeed.WebServiceLibrary.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;

    public class ObjectDbAdapter : IObjectDbAdapter
    {
        private readonly string _settingKey;
        private IObjectDb _objectDb;

        public ObjectDbAdapter(string settingKey)
        {
            _settingKey = settingKey;
        }

        private IObjectDb ObjectDb
        {
            get
            {
                return _objectDb ?? (_objectDb = new ObjectDb(_settingKey));
            }
        }

        public void Dispose()
        {
            if (_objectDb != null)
            {
                _objectDb.Dispose();
            }
        }

        public int ExecuteNonQuery(Hashtable inputParams)
        {
            return ObjectDb.ExecuteNonQuery(inputParams);
        }

        public Hashtable GetOuputParams()
        {
            return ObjectDb.GetOuputParams();
        }

        public IEnumerable<TResult> Query<TResult>(Hashtable inputParams)
        {
            return ObjectDb.Query<TResult>(inputParams);
        }

        public TResult QueryEntity<TResult>(Hashtable inputParams)
        {
            return ObjectDb.QueryEntity<TResult>(inputParams);
        }

        public IRecordSetReader QueryMultiple(Hashtable inputParams)
        {
            var reader = ObjectDb.QueryMultiple(inputParams);

            return new RecordSetReader(reader);
        }

        public IDataReader ExecuteReader(Hashtable inputParams)
        {
            return ObjectDb.ExecuteReader(inputParams);
        }

        public object ExecuteScalar(Hashtable inputParams)
        {
            return ObjectDb.ExecuteScalar(inputParams);
        }
    }
}
