// --------------------------------------------------------------------------
// <copyright file="IRepository.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.6.11">Created</change>
// </history>

using Fanex.Data.Repository;

namespace Fanex.OddsFeed.Framework.Data
{
    public interface IRepository<T> : IReadonlyRepository<T>
    {
        ICommandResult Insert(T entity);

        ICommandResult Update(T entity);

        ICommandResult Delete(T entity);
    }
}
