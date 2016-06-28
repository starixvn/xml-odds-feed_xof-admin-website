// --------------------------------------------------------------------------
// <copyright file="Sport.cs" company=" Solutions Vietnam">
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
    /// Class Sport.
    /// </summary>
    [Serializable]
    public class Sport : BaseModel
    {
        public int SportId { get; set; }

        public string Name_en { get; set; }

        public string Name_cs { get; set; }

        public string Name_ct { get; set; }
    }
}
