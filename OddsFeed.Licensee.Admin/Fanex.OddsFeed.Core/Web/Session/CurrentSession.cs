namespace Fanex.OddsFeed.Core.Web.Session
{
    using System;
    using System.Web;

    public class CurrentSession : ICurrentSession
    {
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

        public string Username
        {
            get
            {
                return (HttpContext.Current != null)
                    ? Convert.ToString(HttpContext.Current.Session[INTERNALUSERNAME])
                    : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[INTERNALUSERNAME] = value;
            }
        }

        public int UserId
        {
            get
            {
                return (HttpContext.Current != null)
                    ? Convert.ToInt32(HttpContext.Current.Session[INTERNALUSERID])
                    : 0;
            }

            set
            {
                HttpContext.Current.Session[INTERNALUSERID] = value;
            }
        }

        public int OnUserId
        {
            get
            {
                return (HttpContext.Current != null)
                    ? Convert.ToInt32(HttpContext.Current.Session[INTERNALONUSERID])
                    : 0;
            }

            set
            {
                HttpContext.Current.Session[INTERNALONUSERID] = value;
            }
        }

        public byte UserRole
        {
            get
            {
                return (HttpContext.Current != null)
                    ? Convert.ToByte(HttpContext.Current.Session[INTERNALUSERROLE])
                    : Convert.ToByte(0);
            }

            set
            {
                HttpContext.Current.Session[INTERNALUSERROLE] = value;
            }
        }

        public short SystemId
        {
            get
            {
                return (HttpContext.Current != null)
                    ? Convert.ToInt16(HttpContext.Current.Session[INTERNALSYSTEMID])
                    : Convert.ToInt16(0);
            }

            set
            {
                HttpContext.Current.Session[INTERNALSYSTEMID] = value;
            }
        }

        public string SystemName
        {
            get
            {
                return (HttpContext.Current != null)
                    ? Convert.ToString(HttpContext.Current.Session[INTERNALSYSTEMNAME])
                    : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[INTERNALSYSTEMNAME] = value;
            }
        }

        public string SessionId
        {
            get
            {
                return (HttpContext.Current != null)
                    ? Convert.ToString(HttpContext.Current.Session[INTERNALNETSESSIONID])
                    : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[INTERNALNETSESSIONID] = value;
            }
        }

        public string SystemList
        {
            get
            {
                return (HttpContext.Current != null)
                    ? Convert.ToString(HttpContext.Current.Session[INTERNALSYSTEMLIST])
                    : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[INTERNALSYSTEMLIST] = value;
            }
        }

        public string ControllerName
        {
            get
            {
                return (HttpContext.Current != null)
                    ? Convert.ToString(HttpContext.Current.Session[SESSIONCONTROLLERNAME])
                    : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[SESSIONCONTROLLERNAME] = value;
            }
        }

        public string ActionName
        {
            get
            {
                return (HttpContext.Current != null)
                    ? Convert.ToString(HttpContext.Current.Session[SESSIONACTIONNAME])
                    : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[SESSIONACTIONNAME] = value;
            }
        }

        public string LoginIp
        {
            get
            {
                return (HttpContext.Current != null)
                    ? Convert.ToString(HttpContext.Current.Session[LoginIpSession])
                    : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[LoginIpSession] = value;
            }
        }

        public void ClearAll()
        {
            if (!ReferenceEquals(HttpContext.Current, null))
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
            }
        }
    }
}