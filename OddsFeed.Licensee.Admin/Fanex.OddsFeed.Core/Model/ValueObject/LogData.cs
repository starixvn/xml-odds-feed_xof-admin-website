// --------------------------------------------------------------------------
// <copyright file="LogData.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.11.15">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Model.ValueObject
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Class LogData.
    /// </summary>
    public class LogData
    {
        public string FieldName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }
}
