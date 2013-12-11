// --------------------------------------------------------------------------
// <copyright file="ISubAccRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.02.05">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Repository.Interface
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Domain;
    using Fanex.OddsFeed.Core.Model;    
    using Fanex.OddsFeed.Core.Model.ValueObject;            
    using Fanex.OddsFeed.Framework.Data;
    
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed")]
    public interface ISubAccRepository : IRepository<IUser>, IBaseRepository
    {                
        ICommandResult Insert(ref InsertSubAccCriteria criteria, ICriteria logCriteria);               
    }
}