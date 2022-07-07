namespace Fanex.OddsFeed.Core.Repository.Interface
{
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Core.Model;

    public interface IAffiliateRepository
    {
        CommandResult Insert(ref InsertAffiliateCriteria criteria);
    }
}