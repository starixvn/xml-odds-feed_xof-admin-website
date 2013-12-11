// --------------------------------------------------------------------------
// <copyright file="TypeSetting.cs" company="Nexcel Solutions Vietnam">
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
    /// Class TypeSetting.
    /// </summary>
    [Serializable]
    public class TypeSetting : IEntity<short, string>
    {
        public short Id { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public bool IsChecked { get; set; }
    }
}