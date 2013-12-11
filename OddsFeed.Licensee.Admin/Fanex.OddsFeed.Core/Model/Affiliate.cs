// --------------------------------------------------------------------------
// <copyright file="Affiliate.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2012.12.11">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Web;

    /// <summary>
    /// Class Affiliate.
    /// </summary>
    [Serializable]
    public class Affiliate : IAffiliate
    {
        public int Id { get; set; }

        public short SystemID { get; set; }

        public short ProfileID { get; set; }

        public string Site { get; set; }

        public bool IsAllowed { get; set; }

        public string Remark { get; set; }

        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastUpdate { get; set; }

        public DateTime LastRequest { get; set; }

        public string Creator { get; set; }

        public string ContactPerson { get; set; }

        public string ContactEmail { get; set; }

        public string Name { get; set; }
    }
}