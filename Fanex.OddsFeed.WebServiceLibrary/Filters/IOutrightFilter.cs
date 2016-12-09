namespace Fanex.OddsFeed.WebServiceLibrary.Filters
{
    using System.Collections.Generic;
    using Common.DTO;

    public interface IOutrightFilter
    {
        IList<Outright> QueryOutrights(IEnumerable<Outright> outrights, UserProfile userProfile, UserQuery userQuery);
    }
}
