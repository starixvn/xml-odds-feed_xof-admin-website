using Fanex.Data.Repository;

namespace Nexcel.OddsFeed.WebService.Leagues
{
    public class LeaguesCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "SP.League";
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