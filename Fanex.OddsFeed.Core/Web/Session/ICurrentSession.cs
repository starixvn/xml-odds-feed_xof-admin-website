namespace Fanex.OddsFeed.Core.Web.Session
{
    public interface ICurrentSession
    {
        string Username { get; set; }

        int UserId { get; set; }

        int OnUserId { get; set; }

        byte UserRole { get; set; }

        byte SystemId { get; set; }

        string SystemName { get; set; }

        string SessionId { get; set; }

        string SystemList { get; set; }

        string ControllerName { get; set; }

        string ActionName { get; set; }

        string LoginIp { get; set; }

        void ClearAll();
    }
}
