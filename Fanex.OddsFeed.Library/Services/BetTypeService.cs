namespace Fanex.OddsFeed.Library.Services
{ 
    using System.Collections.Generic;
    using System.Linq;
    using Schemas;

    public class BetTypeService: IBetTypeService
    {
        private IEnumerable<BetTypeSchema> _betTypeSchemas;

        public string GetBetTypeNameById(int betTypeId)
        {
            var betTypeSchema = GetBetTypeSchemas()
                .Where(schema => schema.BetTypeId == betTypeId)
                .FirstOrDefault();

            return betTypeSchema != null ? betTypeSchema.Tag : "b" + betTypeId;
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
