namespace Fanex.OddsFeed.Models
{
    using System.Collections.Generic;

    public class DataGrid<T>
    {
        public IList<T> Data { get; set; }
        public int TotalItems { get; set; }
    }
}