// --------------------------------------------------------------------------
// <copyright file="Licensee.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.2.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Core.Domain;

    /// <summary>
    /// Class Licensee.
    /// </summary>
    [Serializable]
    public class Licensee : ILicensee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public short SystemID { get; set; }

        public string SystemName { get; set; }

        public int Creator { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdate { get; set; }

        public int LicenseeID { get; set; }

        public bool IsActive { get; set; }
    }
}