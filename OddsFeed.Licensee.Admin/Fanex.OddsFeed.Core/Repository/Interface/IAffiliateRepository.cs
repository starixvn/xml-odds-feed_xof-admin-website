namespace Fanex.OddsFeed.Core.Repository.Interface
{
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Framework.Data;

    public interface IAffiliateRepository
    {
        ICommandResult Insert(ref InsertAffiliateCriteria criteria);
    }
}