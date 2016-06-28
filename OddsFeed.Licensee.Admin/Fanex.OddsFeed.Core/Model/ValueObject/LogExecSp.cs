// --------------------------------------------------------------------------
// <copyright file="LogExecSp.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.11.14">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Model.ValueObject
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Class LogExecSP.
    /// </summary>
    public class LogExecSp
    {
        public long Id { get; set; }

        public DateTime LogDate { get; set; }

        public string Name { get; set; }

        public string Modifier { get; set; }

        public string ActionName { get; set; }

        public short ActionId { get; set; }

        public string OldData { get; set; }

        public string Data { get; set; }

        public int LogType { get; set; }

        public string Items { get; set; }

        public string SubItems { get; set; }
    }
}
