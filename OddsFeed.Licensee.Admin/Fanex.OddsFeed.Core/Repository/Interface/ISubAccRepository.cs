namespace Fanex.OddsFeed.Core.Repository.Interface
{
    using Fanex.OddsFeed.Core.Criteria.Affiliate;
    using Fanex.OddsFeed.Framework.Data;

    public interface ISubAccRepository
    {
        ICommandResult Insert(ref InsertSubAccCriteria criteria);
    }
}