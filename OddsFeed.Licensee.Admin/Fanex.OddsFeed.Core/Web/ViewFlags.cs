// --------------------------------------------------------------------------
// <copyright file="ViewFlags.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Web
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Core.Business;

    /// <summary>
    /// Class ViewFlags.
    /// </summary>
    public class ViewFlags
    {      
        public ViewFlags()
        {
            this.IsLive = false;
            this.HasEditRight = false;
            this.IsNapAdminSite = false;
            this.HasAssignFunctionRight = false;
            this.HasAssignFunctionToUserRight = false;
            this.HasAssignFunctionToGroupRight = false;
            this.HasAssignSiteRight = false;
            this.IsShowLevelColumn = false;
        }

        public bool IsLive { get; set; }

        public bool HasEditRight { get; set; }

        public bool IsNapAdminSite { get; set; }

        public bool HasAssignSiteRight { get; set; }

        public bool HasAssignFunctionRight { get; set; }

        public bool HasAssignFunctionToUserRight { get; set; }

        public bool HasAssignFunctionToGroupRight { get; set; }

        public bool HasManageUserRight { get; set; }

        public bool IsShowLevelColumn { get; set; }

        public void SetEditRight(int policy)
        {
            if (policy >= (int)Constant.Policy.Host)
            {
                HasEditRight = true;
            }
        }
    }
}
