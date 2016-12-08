namespace Fanex.OddsFeed.WebServiceLibrary.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface IObjectDbAdapter : IDisposable
    {
        Hashtable GetOuputParams();

        IEnumerable<TResult> Query<TResult>(Hashtable inputParams);
    }
}