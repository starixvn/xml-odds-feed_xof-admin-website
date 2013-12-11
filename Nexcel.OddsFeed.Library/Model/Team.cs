// --------------------------------------------------------------------------
// <copyright file="Team.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
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
    /// Class Team.
    /// </summary>
    [Serializable]
    public class Team : BaseModel
    {
        public int TeamId { get; set; }

        public string Name_en { get; set; }

        public string Name_cs { get; set; }

        public string Name_ct { get; set; }

        public string SportName { get; set; }
    }
}
