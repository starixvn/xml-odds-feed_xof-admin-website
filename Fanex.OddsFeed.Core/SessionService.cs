namespace Fanex.OddsFeed.Core
{
    using Microsoft.AspNetCore.Http;

    public interface ISessionService
    {
        string Captcha { get; set; }
        string Username { get; set; }
        int UserId { get; set; }
        int OnUserId { get; set; }
        byte UserRole { get; set; }
        short SystemId { get; set; }
        string SystemName { get; set; }
        string SessionId { get; set; }
        string SystemList { get; set; }
        string ControllerName { get; set; }
        string ActionName { get; set; }
        string LoginIp { get; set; }
        void ClearAll();
    }

    public class SessionService : ISessionService
    {
        private readonly ISession session;

        public SessionService(IHttpContextAccessor accessor)
        {
            session = accessor.HttpContext.Session;
        }

        public string Captcha
        {
            get => session.Get<string>("Captcha");
            set => session.Set("Captcha", value);
        }

        public string Username
        {
            get => session.Get<string>("Username");
            set => session.Set("Username", value);
        }

        public int UserId
        {
            get => session.Get<int>("UserId");
            set => session.Set("UserId", value);
        }

        public int OnUserId
        {
            get => session.Get<int>("OnUserId");
            set => session.Set("OnUserId", value);
        }

        public byte UserRole
        {
            get => session.Get<byte>("UserRole");
            set => session.Set("UserRole", value);
        }

        public short SystemId
        {
            get => session.Get<short>("SystemId");
            set => session.Set("SystemId", value);
        }

        public string SystemName
        {
            get => session.Get<string>("SystemName");
            set => session.Set("SystemName", value);
        }

        public string SessionId
        {
            get => session.Get<string>("SessionId");
            set => session.Set("SessionId", value);
        }

        public string SystemList
        {
            get => session.Get<string>("SystemList");
            set => session.Set("SystemList", value);
        }

        public string ControllerName
        {
            get => session.Get<string>("ControllerName");
            set => session.Set("ControllerName", value);
        }

        public string ActionName
        {
            get => session.Get<string>("ActionName");
            set => session.Set("ActionName", value);
        }

        public string LoginIp
        {
            get => session.Get<string>("LoginIp");
            set => session.Set("LoginIp", value);
        }

        public void ClearAll()
        {
            session.Clear();
        }
    }
}
