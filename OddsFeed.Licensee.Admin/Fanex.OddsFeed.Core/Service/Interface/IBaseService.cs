namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System.Collections.Generic;
    using Fanex.OddsFeed.Core.Helpers;
    using Data.Repository;
    using Fanex.OddsFeed.Core.Model;

    public interface IBaseService
    {
        DataCollection GetData(ICriteria criteria);

        DataGrid<T> GetDataGrid<T>(ICriteria criteria);

        T GetDataEntity<T>(ICriteria criteria);

        IEnumerable<T> GetData<T>(ICriteria criteria);

        CommandResult ExecuteNonQuery(ICriteria criteria);
    }
}