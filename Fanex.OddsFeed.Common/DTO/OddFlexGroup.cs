namespace Fanex.OddsFeed.Common.DTO
{
    using System.Collections.Generic;

    public class OddFlexGroup
    {
        public int OddsId { get; set; }

        public IList<OddFlex> OddsFlexs { get; set; }
    }
}
