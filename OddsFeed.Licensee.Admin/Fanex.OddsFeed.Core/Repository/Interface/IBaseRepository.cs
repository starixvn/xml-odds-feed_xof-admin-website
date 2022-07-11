namespace Fanex.OddsFeed.Core.Repository.Interface
{
    using Fanex.OddsFeed.Core.Helpers;
    using Data.Repository;
    using Fanex.OddsFeed.Core.Model;

    public interface IBaseRepository
    {
        DataCollection ReturnDataCollection(ICriteria criteria);

        CommandResult ExecuteNonQuery(ICriteria criteria);
    }
}