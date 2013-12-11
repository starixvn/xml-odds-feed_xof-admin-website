// --------------------------------------------------------------------------
// <copyright file="RequestStatistic.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.02.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Model
{    
    using System;
    using System.ComponentModel.DataAnnotations;    
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Class RequestStatistic.
    /// </summary>
    [Serializable]
    public class RequestStatistic
    {
        public string Partner { get; set; }

        public string Domain { get; set; }

        public string IP { get; set; }

        public DateTime Date { get; set; }

        public DateTime LastCallTime { get; set; }

        public int TotalRequest { get; set; }     
    }
}