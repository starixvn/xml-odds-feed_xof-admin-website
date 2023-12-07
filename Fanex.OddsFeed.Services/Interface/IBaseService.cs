namespace Fanex.OddsFeed.Services.Interface
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.OddsFeed.Criteria;
    using Fanex.OddsFeed.Models;

    public interface IBaseService
    {
        Task<IEnumerable<T>> GetDataAsync<T>(ICriteria criteria);

        Task<T> GetDataEntityAsync<T>(ICriteria criteria);

        Task<DataGrid<T>> GetDataGrid<T>(ICriteria criteria);

        Task<ValueTuple<IEnumerable<TFirst>, IEnumerable<TSecond>>> FetchMultipleAsync<TFirst, TSecond>(ICriteria criteria);

        Task<ValueTuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>>> FetchMultipleAsync<TFirst, TSecond, TThird>(ICriteria criteria);

        Task<CommandResult> ExecuteNonQueryAsync(BaseNonQueryCommand command);
    }
}
