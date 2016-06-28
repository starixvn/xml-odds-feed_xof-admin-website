// --------------------------------------------------------------------------
// <copyright file="CommandResultBase.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.6">Created</change>
// </history>

namespace Fanex.OddsFeed.Framework.Data
{
    using System;

    /// <summary>
    /// Class CommandResultBase.
    /// </summary>
    public abstract class CommandResultBase : ICommandResult
    {
        private dynamic _code;
        private dynamic _message;

        public CommandResultBase()
            : this(string.Empty, string.Empty)
        { 
        }

        public CommandResultBase(dynamic code, dynamic message)
        {
            this._code = code;
            this._message = message;
        }

        public dynamic Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public dynamic Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public abstract bool IsSuccessful { get; }      
    }
}
