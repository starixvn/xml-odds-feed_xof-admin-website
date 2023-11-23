namespace Fanex.OddsFeed.Models
{
    public class DomainSetting
    {
        public int Row { get; set; }
        public short DomainId { get; set; }
        public string DomainName { get; set; }

        public short SystemId { get; set; }
        public string SystemName { get; set; }

        public string Username { get; set; }
        public string Site { get; set; }
        public bool IsAllowed { get; set; }
        public string Remark { get; set; }
    }
}