namespace Fanex.OddsFeed.WindowsServiceLibrary.Mapping
{
    using System.Collections.Generic;
    using System.Data;
    using Common.DTO;
    using Common.Schemas;

    public interface IOldMatchMapping
    {
        IEnumerable<Match> GetMatches(
            DataTable dataTable,
            IEnumerable<BetTypeSchema> schemas,
            bool isLive,
            IEnumerable<OddsFlex> oddsFlexs);
    }
}
