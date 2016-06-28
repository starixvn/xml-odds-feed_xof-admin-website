// --------------------------------------------------------------------------
// <copyright file="ICommand.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.6.6">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Command
{
    using System.Data;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Interface ICommand.
    /// </summary>
    public interface ICommand
    {
        string SettingKey { get; set; }

        IDataParameterCollection GetParameter();
    }
}