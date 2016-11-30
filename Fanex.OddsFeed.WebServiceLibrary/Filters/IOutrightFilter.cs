namespace Fanex.OddsFeed.WebServiceLibrary.Filters
{
    using System.Collections.Generic;
    using Common.DTO;

    public interface IOutrightFilter
    {
        IList<Outright> QueryOutrights(UserProfile userProfile, UserQuery userQuery, IEnumerable<Outright> outrights);
    }
}
