// --------------------------------------------------------------------------
// <copyright file="ICurrentSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2012.8.2">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Web.Session
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Core.Model;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed")]
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
        
        void ClearAll();        
    }
}