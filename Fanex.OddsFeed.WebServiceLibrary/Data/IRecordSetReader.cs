namespace Fanex.OddsFeed.WebServiceLibrary.Data
{
    using System.Collections.Generic;

    public interface IRecordSetReader
    {
        IEnumerable<TResult> Read<TResult>();
    }
}
