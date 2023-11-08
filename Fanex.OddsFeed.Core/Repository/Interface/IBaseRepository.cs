namespace Fanex.OddsFeed.Core.Repository.Interface
{
    using Data.Repository;
    using Fanex.OddsFeed.Core.Model;

    public interface IBaseRepository
    {
        Task<CommandResult> ExecuteNonQueryAsync(INonQueryCommand command);

        Task<CommandResult> ExecuteNonQueryAsync(ICriteria criteria);

        CommandResult ExecuteNonQuery(ICriteria criteria);
        DataGrid<T> GetDataGrid<T>(ICriteria criteria);

    }
}