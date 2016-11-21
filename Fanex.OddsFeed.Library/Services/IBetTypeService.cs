namespace Fanex.OddsFeed.Library.Services
{
    using System.Collections.Generic;
    using Model;

    public interface IBetTypeService
    {
        string GetBetTypeNameById(int betTypeId);

        IDictionary<int, BetType> GetBetTypeList();
    }
}
