namespace Fanex.OddsFeed.WebServiceLibrary.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;

    public interface IObjectDbAdapter : IDisposable
    {
        int ExecuteNonQuery(Hashtable inputParams);

        IDataReader ExecuteReader(Hashtable inputParams);

        object ExecuteScalar(Hashtable inputParams);

        Hashtable GetOuputParams();

        IEnumerable<TResult> Query<TResult>(Hashtable inputParams);

        TResult QueryEntity<TResult>(Hashtable inputParams);

        IRecordSetReader QueryMultiple(Hashtable inputParams);
    }
}
