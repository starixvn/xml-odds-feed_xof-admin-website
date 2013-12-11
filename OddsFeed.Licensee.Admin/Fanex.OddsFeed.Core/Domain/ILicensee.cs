// --------------------------------------------------------------------------
// <copyright file="ILicensee.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.2.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Domain
{
    using System;
    using System.Diagnostics.CodeAnalysis;    
    using Fanex.OddsFeed.Framework.Domain;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed")]
    public interface ILicensee : IEntity<int, string>
    {
        short SystemID { get; set; }

        string SystemName { get; set; }

        int Creator { get; set; }

        DateTime CreatedDate { get; set; }

        DateTime LastUpdate { get; set; }

        int LicenseeID { get; set; }

        bool IsActive { get; set; }
    }
}