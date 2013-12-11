// --------------------------------------------------------------------------
// <copyright file="Bettype.cs" company="Nexcel Solutions Vietnam">
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
    /// Class BETTYPE.
    /// </summary>
    [Serializable]
    public class Bettype : IEntity<int, string>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsChecked { get; set; }
    }
}