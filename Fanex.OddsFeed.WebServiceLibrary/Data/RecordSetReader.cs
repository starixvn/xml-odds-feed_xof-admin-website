namespace Fanex.OddsFeed.WebServiceLibrary.Data
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;

    public class RecordSetReader : IRecordSetReader, IDisposable
    {
        private readonly GridReader _reader;

        public RecordSetReader(GridReader reader)
        {
            _reader = reader;
        }

        public void Dispose()
        {
            if (_reader != null)
            {
                _reader.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        public IEnumerable<TResult> Read<TResult>()
        {
            return _reader.Read<TResult>();
        }
    }
}
