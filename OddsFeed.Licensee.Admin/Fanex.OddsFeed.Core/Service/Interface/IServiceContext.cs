// --------------------------------------------------------------------------
// <copyright file="IServiceContext.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.6">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed")]
    public interface IServiceContext
    {
        /// <summary>
        /// Gets or sets who perform a business action.
        /// </summary>
        /// <value>Id of user login.</value>
        int UserId { get; set; }

        /// <summary>
        /// Gets or sets which system user be in.
        /// </summary>
        /// <value>System id of user login.</value>
        byte SystemId { get; set; }

        string Ip { get; set; }
    }
}
