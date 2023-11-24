namespace Fanex.OddsFeed.Repositories.Interface
{
    using Fanex.Data.Repository;
    using Fanex.OddsFeed.Models;

    public interface ISubAccRepository
    {
        Task<CommandResult> Insert(ICriteria criteria);
    }
}