namespace Fanex.OddsFeed.Core.Repository.Interface
{
    using System.Data;
    using Fanex.OddsFeed.Core.Helpers;
    using Data.Repository;
    using Fanex.OddsFeed.Core.Model;

    public interface IBaseRepository
    {
        object ReturnSingleValue(ICriteria criteria);

        IDataReader ReturnDataReader(ICriteria criteria, ICriteria logCriteria);

        IDataReader ReturnDataReader(ICriteria criteria);

        DataSet ReturnDataSet(ICriteria criteria, ICriteria logCriteria);

        DataSet ReturnDataSet(ICriteria criteria);

        DataCollection ReturnDataCollection(ICriteria criteria, ICriteria logCriteria);

        DataCollection ReturnDataCollection(ICriteria criteria);

        CommandResult ExecuteNonQuery(ICriteria criteria);
    }
}