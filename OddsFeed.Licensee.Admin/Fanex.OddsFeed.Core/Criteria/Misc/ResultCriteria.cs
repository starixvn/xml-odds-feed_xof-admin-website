// --------------------------------------------------------------------------
// <copyright file="ResultCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2012.8.16">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Misc
{
    using System.Diagnostics.CodeAnalysis;
    using Framework.Data;

    /// <summary>
    /// Class ResultCriteria.
    /// </summary>
    public class ResultCriteria : CriteriaBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultCriteria"/> class.
        /// </summary>
        public ResultCriteria()
        {
            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}
