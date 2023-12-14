namespace Fanex.OddsFeed.Services.Interface
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using Fanex.OddsFeed.Criteria;
    using Fanex.OddsFeed.Models;

    public interface IBaseService
    {
        Task<IEnumerable<T>> GetDataAsync<T>(ICriteria criteria);

        Task<T> GetDataEntityAsync<T>(ICriteria criteria);

        Task<CommandResult> ExecuteNonQueryAsync(BaseNonQueryCommand command);

        Task<DataGrid<T>> GetDataGridAsync<T>(ICriteria criteria);
    }
}
