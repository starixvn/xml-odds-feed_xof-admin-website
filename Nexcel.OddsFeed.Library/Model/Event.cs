// --------------------------------------------------------------------------
// <copyright file="Event.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Nexcel.OddsFeed.Library.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class Event.
    /// </summary>
    [Serializable]
    public class Event : BaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        public Event() : base("Events")
        {
        }

        public int EventId { get; set; }

        public string EventName { get; set; }

        public string CountryId { get; set; }

        public string SportName { get; set; }

        public string EventDate { get; set; }

        public string KickOffTime { get; set; }

        public bool Status { get; set; }        
    }
}
