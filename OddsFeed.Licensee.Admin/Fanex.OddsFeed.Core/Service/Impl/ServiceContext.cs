// --------------------------------------------------------------------------
// <copyright file="ServiceContext.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.6">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Service.Impl
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Core.Service.Interface;

    /// <summary>
    /// Class ServiceContext.
    /// </summary>    
    public class ServiceContext : IServiceContext
    {
        /// <summary>
        /// Gets or sets who perform a business action.
        /// </summary>
        /// <value>Id of user login.</value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets which system user be in.
        /// </summary>
        /// <value>System id of user login.</value>
        public byte SystemId { get; set; }        

        public string Ip { get; set; }
    }
}