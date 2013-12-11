// --------------------------------------------------------------------------
// <copyright file="OddsFeedActionResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Model
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Class OddsFeedActionResult.
    /// </summary>
    public class OddsFeedActionResult
    {
        public OddsFeedActionResult()
        {
            this.ErrorCode = 0;
            this.IsSuccess = true;            
            this.ResultMessage = string.Empty;
        }
        
        public int ErrorCode { get; set; }
        
        public bool IsSuccess { get; set; }

        public string ResultMessage { get; set; }
    }
}
