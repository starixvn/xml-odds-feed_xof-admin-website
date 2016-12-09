namespace Fanex.OddsFeed.Common.DTO
{
    using global::System;
    using global::System.Collections.Generic;

    [Serializable]
    public class OutrightOdds
    {
        public int OddsId { get; set; }

        public string OddNameEnglish { get; set; }

        public string OddNameTraditionalChinese { get; set; }

        public string OddNameSimplifiedChinese { get; set; }

        public decimal Value { get; set; }
    }
}
