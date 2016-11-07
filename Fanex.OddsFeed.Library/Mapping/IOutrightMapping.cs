namespace Fanex.OddsFeed.Library.Mapping
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.OddsFeed.Library.Nodes;

    public interface IOutrightMapping
    {
        IEnumerable<EventNode> GetEventNodes(DataTable dataTable);
    }
}
