// --------------------------------------------------------------------------
// <copyright file="IBaseRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.02.05">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Repository.Interface
{
    using System.Collections;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Core.Command;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed")]
    public interface IBaseRepository
    {
        CommandResult GetCommandResult(Hashtable hash);

        ICommandResult ReturnICommandResult(ICriteria criteria, ICriteria logCriteria);
        ICommandResult ReturnICommandResult(ICriteria criteria);

        IDataReader ReturnDataReader(ICriteria criteria, ICriteria logCriteria);
        IDataReader ReturnDataReader(ICriteria criteria);

        DataSet ReturnDataSet(ICriteria criteria, ICriteria logCriteria);

        DataSet ReturnDataSet(ICriteria criteria);

        DataCollection ReturnDataCollection(ICriteria criteria, ICriteria logCriteria);

        DataCollection ReturnDataCollection(ICriteria criteria);

        ICommandResult Update(ICriteria criteria, ICriteria logCriteria);

        ICommandResult Update(ICriteria criteria);

        ICommandResult Delete(ICriteria criteria, ICriteria logCriteria);
        ICommandResult Delete(ICriteria criteria);
    }
}