namespace Fanex.OddsFeed.Common.DTO
{
    public class OddFlex
    {
        public int OddId { get; set; }

        public int BetType { get; set; }

        public string BetChoice { get; set; }

        public decimal Odds { get; set; }

        public string OddsInfo { get; set; }
    }
}
