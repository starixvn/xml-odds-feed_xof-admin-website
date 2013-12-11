// --------------------------------------------------------------------------
// <copyright file="IAdministrator.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013.03.05">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Domain
{
    using System;
    using System.Diagnostics.CodeAnalysis;    
    using Fanex.OddsFeed.Framework.Domain;

    /// <summary>
    /// Interface IAdministrator.
    /// </summary>
    public interface IAdministrator : IEntity<int, string>
    {
    }
}
