namespace Fanex.OddsFeed.Common.DTO
{
    using System.Collections.Generic;

    public class OutrightDocument
    {
        public int Version { get; set; }

        public IList<Outright> Outrights { get; set; }
    }
}
