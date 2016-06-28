// --------------------------------------------------------------------------
// <copyright file="IAdministratorRepository.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013.03.05">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Repository.Interface
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed")]
    public interface IAdministratorRepository : IRepository<IAdministrator>, IBaseRepository
    {
        ICommandResult InsertAdmin(ICriteria criteria);
    }
}
