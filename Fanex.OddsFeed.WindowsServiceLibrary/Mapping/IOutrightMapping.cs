namespace Fanex.OddsFeed.WindowsServiceLibrary.Mapping
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.OddsFeed.Library.Nodes;

    public interface IOutrightMapping
    {
        IEnumerable<Outright> GetOutrights(DataTable dataTable);
    }
}
