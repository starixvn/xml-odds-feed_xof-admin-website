// --------------------------------------------------------------------------
// <copyright file="LogExecSpCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2012.8.16">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Misc
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class LogExecSPCriteria.
    /// </summary>
    public class LogExecSpCriteria : CriteriaBase
    {
        public int CurrentUserId { get; set; }

        public int CurrentSystemId { get; set; }

        public DateTime LogDate { get; set; }

        public string LoginIp { get; set; }

        public override bool IsValid()
        {
            return true;
        }
    }
}