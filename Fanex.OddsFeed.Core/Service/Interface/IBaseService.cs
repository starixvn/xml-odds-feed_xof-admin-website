namespace Fanex.OddsFeed.Core.Service.Interface
{
    using System.Collections.Generic;
    using Data.Repository;
    using Fanex.OddsFeed.Core.Model;
    public interface IBaseService
    {
        IList<T> GetData<T>(ICriteria criteria);

        Task<IEnumerable<T>> GetDataAsync<T>(ICriteria criteria);

        T GetDataEntity<T>(ICriteria criteria);

        Task<T> GetDataEntityAsync<T>(ICriteria criteria);

        Task<ValueTuple<IEnumerable<TFirst>, IEnumerable<TSecond>>> FetchMultipleAsync<TFirst, TSecond>(ICriteria criteria);

        Task<ValueTuple<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>>> FetchMultipleAsync<TFirst, TSecond, TThird>(ICriteria criteria);

        Task<CommandResult> ExecuteNonQueryAsync(INonQueryCommand criteria);
    }
}
