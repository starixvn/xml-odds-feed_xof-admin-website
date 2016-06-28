// --------------------------------------------------------------------------
// <copyright file="IPagedCriteria.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.6.19">Created</change>
// </history>

using Fanex.Data.Repository;

namespace Fanex.OddsFeed.Framework.Data
{
    public interface IPagedCriteria : ICriteria
    {
        int PageIndex { get; set; }

        int PageSize { get; set; }

        int TotalRecords { get; set; }
    }
}
