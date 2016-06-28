// --------------------------------------------------------------------------
// <copyright file="Language.cs" company=" Solutions Vietnam">
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
    using Fanex.OddsFeed.Framework.Domain;

    /// <summary>
    /// Class Language.
    /// </summary>
    [Serializable]
    public class Language : IEntity<string, string>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsChecked { get; set; }
    }
}