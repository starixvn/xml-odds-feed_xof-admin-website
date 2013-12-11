// --------------------------------------------------------------------------
// <copyright file="PeriodType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Peter.Tong" date="2012.07.03">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Model
{
    using System.ComponentModel;

    /// <summary>
    /// Enumerate of Excel file name.
    /// </summary>
    public enum PeriodType
    {
        /// <summary>
        /// Enumerate Date.
        /// </summary>
        [Description("Date")]        
        Date = 0,

        /// <summary>
        /// Enumerate Week.
        /// </summary>
        [Description("Week")]        
        Week = 1,

        /// <summary>
        /// Enumerate Month.
        /// </summary>
        [Description("Month")]        
        Month = 2,
        
        /// <summary>
        /// Enumerate Year.
        /// </summary>
        [Description("Year")]
        Year = 4,        

        /// <summary>
        /// Enumerate NULL.
        /// </summary>
        NULL = -1
    }
}
