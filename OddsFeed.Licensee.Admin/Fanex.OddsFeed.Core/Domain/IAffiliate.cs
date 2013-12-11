// --------------------------------------------------------------------------
// <copyright file="IAffiliate.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Domain
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Domain;

    /// <summary>
    /// Interface IAffiliate.
    /// </summary>
    public interface IAffiliate : IEntity<int, string>
    {
        short SystemID { get; set; }

        short ProfileID { get; set; }

        string Site { get; set; }

        bool IsAllowed { get; set; }

        string Remark { get; set; }

        DateTime CreatedDate { get; set; }

        DateTime LastUpdate { get; set; }

        DateTime LastRequest { get; set; }

        string Creator { get; set; }

        string ContactPerson { get; set; }

        string ContactEmail { get; set; }
    }
}