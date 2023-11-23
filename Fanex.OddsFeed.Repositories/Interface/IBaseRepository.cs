namespace Fanex.OddsFeed.Repositories.Interface
{
    using Data.Repository;
    using Fanex.OddsFeed.Models;

    public interface IBaseRepository
    {
        Task<CommandResult> ExecuteNonQueryAsync(INonQueryCommand command);

        Task<CommandResult> ExecuteNonQueryAsync(ICriteria criteria);

        CommandResult ExecuteNonQuery(ICriteria criteria);

        Task<DataGrid<T>> GetDataGrid<T>(ICriteria criteria);
    }
}