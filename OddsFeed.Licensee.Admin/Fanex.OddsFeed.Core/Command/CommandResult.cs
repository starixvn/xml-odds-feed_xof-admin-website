// --------------------------------------------------------------------------
// <copyright file="CommandResult.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2012.8.14">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Command
{
    using Fanex.OddsFeed.Framework.Data;

    /// <summary>
    /// Class CommandResult.
    /// </summary>
    public class CommandResult : CommandResultBase
    {
        public override bool IsSuccessful
        {
            get { return Code == 0; }
        }
    }
}
