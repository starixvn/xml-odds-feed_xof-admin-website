// --------------------------------------------------------------------------
// <copyright file="CurrentSession.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2012.8.2">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Web.Session
{
    using System;
    using System.Web;

    /// <summary>
    /// Class CurrentSession.
    /// </summary>
    public class CurrentSession : ICurrentSession
    {
        #region const

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

        #endregion const

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username
        {
            get
            {
                return
                (HttpContext.Current != null)
                ? Convert.ToString(HttpContext.Current.Session[INTERNALUSERNAME])
                : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[INTERNALUSERNAME] = value;
            }
        }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId
        {
            get
            {
                return
                (HttpContext.Current != null)
                ? Convert.ToInt32(HttpContext.Current.Session[INTERNALUSERID])
                : 0;
            }

            set
            {
                HttpContext.Current.Session[INTERNALUSERID] = value;
            }
        }

        /// <summary>
        /// Gets or sets the on user identifier.
        /// </summary>
        /// <value>The on user identifier.</value>
        public int OnUserId
        {
            get
            {
                return
                (HttpContext.Current != null)
                ? Convert.ToInt32(HttpContext.Current.Session[INTERNALONUSERID])
                : 0;
            }

            set
            {
                HttpContext.Current.Session[INTERNALONUSERID] = value;
            }
        }

        /// <summary>
        /// Gets or sets the user role.
        /// </summary>
        /// <value>The user role.</value>
        public byte UserRole
        {
            get
            {
                return
                (HttpContext.Current != null)
                ? Convert.ToByte(HttpContext.Current.Session[INTERNALUSERROLE])
                : Convert.ToByte(0);
            }

            set
            {
                HttpContext.Current.Session[INTERNALUSERROLE] = value;
            }
        }

        /// <summary>
        /// Gets or sets the system identifier.
        /// </summary>
        /// <value>The system identifier.</value>
        public byte SystemId
        {
            get
            {
                return
                (HttpContext.Current != null)
                ? Convert.ToByte(HttpContext.Current.Session[INTERNALSYSTEMID])
                : Convert.ToByte(0);
            }

            set
            {
                HttpContext.Current.Session[INTERNALSYSTEMID] = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the system.
        /// </summary>
        /// <value>The name of the system.</value>
        public string SystemName
        {
            get
            {
                return
                (HttpContext.Current != null)
                ? Convert.ToString(HttpContext.Current.Session[INTERNALSYSTEMNAME])
                : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[INTERNALSYSTEMNAME] = value;
            }
        }

        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>The session identifier.</value>
        public string SessionId
        {
            get
            {
                return
                (HttpContext.Current != null)
                ? Convert.ToString(HttpContext.Current.Session[INTERNALNETSESSIONID])
                : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[INTERNALNETSESSIONID] = value;
            }
        }

        /// <summary>
        /// Gets or sets the system list.
        /// </summary>
        /// <value>The system list.</value>
        public string SystemList
        {
            get
            {
                return
                (HttpContext.Current != null)
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
                return
                (HttpContext.Current != null)
                ? Convert.ToString(HttpContext.Current.Session[SESSIONCONTROLLERNAME])
                : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[SESSIONCONTROLLERNAME] = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the action.
        /// </summary>
        /// <value>The name of the action.</value>
        public string ActionName
        {
            get
            {
                return
                (HttpContext.Current != null)
                ? Convert.ToString(HttpContext.Current.Session[SESSIONACTIONNAME])
                : string.Empty;
            }

            set
            {
                HttpContext.Current.Session[SESSIONACTIONNAME] = value;
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