// --------------------------------------------------------------------------
// <copyright file="League.cs" company=" Solutions Vietnam">
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
    /// Class League.
    /// </summary>
    [Serializable]
    public class League : BaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="League"/> class.
        /// </summary>
        public League() : base("Leagues")
        {                        
        }

        public int LeagueId { get; set; }

        public string Name_en { get; set; }
        
        public string Name_cs { get; set; }

        public string Name_ct { get; set; }

        public string SportName { get; set; }

        public string CountryId { get; set; }

        public string StartDate { get; set; }

        public bool IsActive { get; set; }
    }
}
