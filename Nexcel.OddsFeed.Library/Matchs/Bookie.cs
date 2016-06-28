// --------------------------------------------------------------------------
// <copyright file="Bookie.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Nexcel.OddsFeed.Library.Matches
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class Bookie.
    /// </summary>
    [Serializable]
    public class Bookie
    {    
        public Bookie()
        {
            Name = new List<Language>();
            Odds = new List<MatchOdd>();
        }

        public IList<Language> Name { get; set; }

        public IList<MatchOdd> Odds { get; set; }
    }
}
