using Fanex.Data.Repository;

namespace Fanex.OddsFeed.WebService.BetTypes
{
    public class BetTypesCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "SP.BetType";
        }

        /// <summary>
        /// Check data's constrains.
        /// </summary>
        /// <returns>True: valid; False: invalid.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}