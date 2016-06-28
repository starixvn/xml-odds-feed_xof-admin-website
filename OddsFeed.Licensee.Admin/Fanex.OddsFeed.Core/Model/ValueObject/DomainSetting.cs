// --------------------------------------------------------------------------
// <copyright file="DomainSetting.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Model
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Core.Domain;    
    using Fanex.OddsFeed.Framework.Domain;

    /// <summary>
    /// Class DomainSetting.
    /// </summary>
    [Serializable]
    public class DomainSetting : IEntity<short, string>
    {
        public short Id { get; set; }

        public string Name { get; set; }

        public string IP { get; set; }

        public string ISP { get; set; }

        public bool IsActive { get; set; }

        public bool IsOddsDomain { get; set; }    
    }
}