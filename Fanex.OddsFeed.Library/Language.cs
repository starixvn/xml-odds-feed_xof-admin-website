// --------------------------------------------------------------------------
// <copyright file="Language.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.04.03">Created</change>
// </history>

namespace Fanex.OddsFeed.Library
{
    using System;

    /// <summary>
    /// Class Language.
    /// </summary>
    [Serializable]
    public class Language
    {
        public string Content { get; set; }

        public string Code { get; set; }
    }
}
