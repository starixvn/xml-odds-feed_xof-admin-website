// --------------------------------------------------------------------------
// <copyright file="OutrightsOdd.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Library.Outrights
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class OutrightsOdd.
    /// </summary>
    [Serializable]
    public class OutrightsOdd
    {
        public OutrightsOdd()
        {
            Team = new List<Language>();
        }
        
        public int Id { get; set; }

        public IList<Language> Team { get; set; }

        public string Value { get; set; }
    }
}
