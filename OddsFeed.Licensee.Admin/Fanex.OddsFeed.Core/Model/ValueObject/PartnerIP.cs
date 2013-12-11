// --------------------------------------------------------------------------
// <copyright file="PartnerIP.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Model
{    
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Framework.Domain;

    /// <summary>
    /// Class PartnerIP.
    /// </summary>
    [Serializable]
    public class PartnerIP : IEntity<int, string>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string IPFrom { get; set; }

        public string IPTo { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string ISP { get; set; }     
    }
}