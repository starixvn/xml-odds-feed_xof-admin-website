namespace Fanex.OddsFeed.Library.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Helper;
    using Model;
    using Schemas;

    public class BetTypeService : IBetTypeService
    {
        private IEnumerable<BetTypeSchema> _betTypeSchemas;

        public string GetBetTypeNameById(int betTypeId)
        {
            var betTypeList = GetBetTypeList();

            return BetTypeHelper.GetBetTypeName(betTypeList, betTypeId);
        }

        public IDictionary<int, BetType> GetBetTypeList()
        {
            return GetBetTypeSchemas().ToDictionary(
                schema => schema.BetTypeId,
                schema => new BetType { BetTypeName = schema.Tag, IsChildBetType = schema.IsChildBetType });
        }

        private IEnumerable<BetTypeSchema> GetBetTypeSchemas()
        {
            if (_betTypeSchemas == null)
            {
                _betTypeSchemas = Caching.GetBetTypeSchemas();
            }

            return _betTypeSchemas;
        }
    }
}
