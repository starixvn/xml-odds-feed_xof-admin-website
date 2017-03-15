namespace Fanex.OddsFeed.WindowsServiceLibrary.Mapping
{
    using System.Collections.Generic;
    using System.Data;
    using Common.DTO;
    using Common.Schemas;

    public interface IMatchMapping
    {
        IEnumerable<LeagueDocument> GetMatches(
            DataTable dataTable,
            IEnumerable<BetTypeSchema> schemas,
            IEnumerable<OddsFlexGroup> oddsFlexs,
            int currentVersion);
    }
}