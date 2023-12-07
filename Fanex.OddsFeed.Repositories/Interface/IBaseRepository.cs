namespace Fanex.OddsFeed.Repositories.Interface
{
    using Fanex.Data.Repository;
    using Fanex.OddsFeed.Models;

    public interface IBaseRepository
    {
        Task<DataGrid<T>> GetDataGrid<T>(ICriteria criteria);
    }
}