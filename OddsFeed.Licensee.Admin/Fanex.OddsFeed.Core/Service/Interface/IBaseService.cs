namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System.Collections.Generic;
    using Data.Repository;
    using Fanex.OddsFeed.Core.Model;

    public interface IBaseService
    {
        DataGrid<T> GetDataGrid<T>(ICriteria criteria);

        T GetDataEntity<T>(ICriteria criteria);

        IEnumerable<T> GetData<T>(ICriteria criteria);

        CommandResult ExecuteNonQuery(ICriteria criteria);
    }
}