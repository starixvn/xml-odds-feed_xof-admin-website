namespace Fanex.OddsFeed.Core.Repository.Interface
{
    using System.Collections;
    using System.Data;
    using Fanex.OddsFeed.Core.Command;
    using Fanex.OddsFeed.Core.Helpers;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;

    public interface IBaseRepository
    {
        object ReturnSingleValue(ICriteria criteria);

        IDataReader ReturnDataReader(ICriteria criteria, ICriteria logCriteria);

        IDataReader ReturnDataReader(ICriteria criteria);

        DataSet ReturnDataSet(ICriteria criteria, ICriteria logCriteria);

        DataSet ReturnDataSet(ICriteria criteria);

        DataCollection ReturnDataCollection(ICriteria criteria, ICriteria logCriteria);

        DataCollection ReturnDataCollection(ICriteria criteria);

        ICommandResult ExecuteNonQuery(ICriteria criteria, ICriteria logCriteria);

        ICommandResult ExecuteNonQuery(ICriteria criteria);

        CommandResult GetCommandResult(Hashtable hash);
    }
}