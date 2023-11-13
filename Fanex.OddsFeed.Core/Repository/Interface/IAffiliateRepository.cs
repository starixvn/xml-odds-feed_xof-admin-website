namespace Fanex.OddsFeed.Core.Repository.Interface
{
    using Fanex.Data.Repository;
    using Fanex.OddsFeed.Core.Model;

    public interface IAffiliateRepository
    {
        Task<CommandResult> Insert(ICriteria criteria);
    }
}