namespace Fanex.OddsFeed.Library.Services
{
    using System.Collections.Generic;

    public interface IBetTypeService
    {
        string GetBetTypeNameById(int betTypeId);

        IDictionary<int, string> GetBetTypeList();
    }
}
