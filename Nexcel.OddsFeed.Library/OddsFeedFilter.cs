// --------------------------------------------------------------------------
// <copyright file="OddsFeedFilter.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.09.18">Created</change>
// </history>
namespace Nexcel.OddsFeed.Library
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Class Filter.
    /// </summary>
    public class OddsFeedFilter
    {
        public IList<int> Sports { get; set; }

        public IList<string> Countries { get; set; }

        public IList<int> BetTypes { get; set; }
        
        public IList<string> Leagues { get; set; }
    }
}
