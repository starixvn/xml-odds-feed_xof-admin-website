namespace Fanex.OddsFeed.WebServiceLibrary.Services
{
    using System.Collections.Generic;
    using Model;

    public interface IBetTypeService
    {
        string GetBetTypeNameById(int betTypeId);

        IDictionary<int, BetType> GetBetTypeList();
    }
}
