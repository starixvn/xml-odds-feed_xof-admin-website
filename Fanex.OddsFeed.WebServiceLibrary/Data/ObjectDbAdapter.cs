namespace Fanex.OddsFeed.WebServiceLibrary.Data
{
    using Fanex.Data;
    using System.Collections;
    using System.Collections.Generic;

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

        public Hashtable GetOuputParams()
        {
            return ObjectDb.GetOuputParams();
        }

        public IEnumerable<TResult> Query<TResult>(Hashtable inputParams)
        {
            return ObjectDb.Query<TResult>(inputParams);
        }
    }
}