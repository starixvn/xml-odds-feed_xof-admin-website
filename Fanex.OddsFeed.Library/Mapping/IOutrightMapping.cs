namespace Fanex.OddsFeed.Library.Mapping
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.OddsFeed.Library.Nodes;

    public interface IOutrightMapping
    {
        IEnumerable<Outright> GetOutrightNodes(DataTable dataTable);
    }
}
