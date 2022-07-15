namespace Fanex.OddsFeed.Core.Repository.Interface
{
    using Data.Repository;
    using Fanex.OddsFeed.Core.Model;

    public interface IBaseRepository
    {
        DataGrid<T> GetDataGrid<T>(ICriteria criteria);

        CommandResult ExecuteNonQuery(ICriteria criteria);
    }
}