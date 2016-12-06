namespace Fanex.OddsFeed.Common.DTO
{
    public class AuthenticatedPartner
    {
        public string SystemName { get; set; }

        public UserProfile UserProfile { get; set; }

        public Error Error { get; set; }
    }
}
