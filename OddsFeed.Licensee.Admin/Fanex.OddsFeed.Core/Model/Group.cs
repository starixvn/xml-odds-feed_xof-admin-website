// --------------------------------------------------------------------------
// <copyright file="Group.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.8">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Model
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Core.Domain;

    /// <summary>
    /// Class Group.
    /// </summary>
    [Serializable]
    public class Group : IGroup
    {
        public short Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public IUser Creator { get; set; }

        public DateTime CreatedDate { get; set; }

        public IUser Modifier { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int NumOfUser { get; set; }
    }
}