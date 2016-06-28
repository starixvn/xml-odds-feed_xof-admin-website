// --------------------------------------------------------------------------
// <copyright file="ICurrentContext.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Web.Context
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed")]
    public interface ICurrentContext
    {
        string IP { get; }

        string IP4Address { get; }

        string IP6Address { get; }

        string DomainName { get; }

        string FullDomainName { get; }

        string BrowserName { get; }

        string Platform { get; }

        string CountryCode { get; }
    }
}