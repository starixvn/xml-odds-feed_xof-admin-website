// --------------------------------------------------------------------------
// <copyright file="IDomainRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.2.25">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Repository.Interface
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed")]
    public interface IDomainRepository
    {
        DataCollection GetAllDomain(ICriteria criteria);

        DataCollection GetDomainSetting(ICriteria criteria);

        int CreateDomain(ICriteria criteria);

        int UpdateDomain(ICriteria criteria);

        int DeleteDomain(ICriteria criteria);
    }
}