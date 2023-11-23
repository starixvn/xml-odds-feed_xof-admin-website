using Microsoft.AspNetCore.Http;

namespace Fanex.OddsFeed.Core.Session
{
    public class CurrentSession : ICurrentSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();

        public const string INTERNALUSERNAME = "InternalUsername";

        public const string INTERNALUSERID = "InternalUserId";

        public const string INTERNALONUSERID = "InternalOnUserId";

        public const string INTERNALUSERROLE = "InternalUserRole";

        public const string INTERNALSYSTEMID = "InternalSystemId";

        public const string INTERNALSYSTEMNAME = "InternalSystemName";

        public const string INTERNALSYSTEMLIST = "InternalSystemList";

        public const string INTERNALNETSESSIONID = "InternalNetSessionId";

        public const string INTERNALMENUITEMDIC = "InternalMenuItemList";

        public const string SESSIONCONTROLLERNAME = "ControllerName";

        public const string SESSIONACTIONNAME = "ActionName";

        public const string LoginIpSession = "LoginIpSession";

        public string Captcha
        {
            get
            {
                var c = _httpContextAccessor.HttpContext?.Session.GetString("captchastring");
                return c ?? string.Empty;
            }
            set
            {
                _httpContextAccessor.HttpContext?.Session.SetString("captchastring", value);
            }
        }
        public string Username
        {
            get
            {
                var username = _httpContextAccessor.HttpContext?.Session.GetString(INTERNALUSERNAME);
                return username ?? string.Empty;
            }
            set
            {
                _httpContextAccessor.HttpContext?.Session.SetString(INTERNALUSERNAME, value);
            }
        }
        public int UserId
        {
            get
            {
                if (int.TryParse(_httpContextAccessor.HttpContext?.Session.GetString(INTERNALUSERID), out int userId))
                {
                    return userId;
                }
                return 0;
            }
            set
            {
                _httpContextAccessor.HttpContext?.Session.SetString(INTERNALUSERID, value.ToString());
            }
        }
        public int OnUserId
        {
            get
            {
                if (int.TryParse(_httpContextAccessor.HttpContext?.Session.GetString("InternalOnUserId"), out int onUserId))
                {
                    return onUserId;
                }
                return 0;
            }
            set
            {
                _httpContextAccessor.HttpContext?.Session.SetString("InternalOnUserId", value.ToString());
            }
        }

        public byte UserRole
        {
            get
            {
                if (byte.TryParse(_httpContextAccessor.HttpContext?.Session.GetString("InternalUserRole"), out byte userRole))
                {
                    return userRole;
                }
                return 0;
            }
            set
            {
                _httpContextAccessor.HttpContext?.Session.SetString("InternalUserRole", value.ToString());
            }
        }

        public short SystemId
        {
            get
            {
                if (short.TryParse(_httpContextAccessor.HttpContext?.Session.GetString("InternalSystemId"), out short systemId))
                {
                    return systemId;
                }
                return 0;
            }
            set
            {
                _httpContextAccessor.HttpContext?.Session.SetString("InternalSystemId", value.ToString());
            }
        }

        public string SystemName
        {
            get
            {
                return _httpContextAccessor.HttpContext?.Session.GetString("InternalSystemName") ?? string.Empty;
            }
            set
            {
                _httpContextAccessor.HttpContext?.Session.SetString("InternalSystemName", value);
            }
        }

        public string SessionId
        {
            get
            {
                return _httpContextAccessor.HttpContext?.Session.GetString("InternalNetSessionId") ?? string.Empty;
            }
            set
            {
                _httpContextAccessor.HttpContext?.Session.SetString("InternalNetSessionId", value);
            }
        }

        public string SystemList
        {
            get
            {
                return _httpContextAccessor.HttpContext?.Session.GetString("InternalSystemList") ?? string.Empty;
            }
            set
            {
                _httpContextAccessor.HttpContext?.Session.SetString("InternalSystemList", value);
            }
        }

        public string ControllerName
        {
            get
            {
                return _httpContextAccessor.HttpContext?.Session.GetString("SessionControllerName") ?? string.Empty;
            }
            set
            {
                _httpContextAccessor.HttpContext?.Session.SetString("SessionControllerName", value);
            }
        }

        public string ActionName
        {
            get
            {
                return _httpContextAccessor.HttpContext?.Session.GetString("SessionActionName") ?? string.Empty;
            }
            set
            {
                _httpContextAccessor.HttpContext?.Session.SetString("SessionActionName", value);
            }
        }

        public string LoginIp
        {
            get
            {
                return _httpContextAccessor.HttpContext?.Session.GetString("LoginIpSession") ?? string.Empty;
            }
            set
            {
                _httpContextAccessor.HttpContext?.Session.SetString("LoginIpSession", value);
            }
        }

        public void ClearAll()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Session.Clear();
            }
        }
    }
}
