namespace Fanex.OddsFeed.WebServiceLibrary.Criteria
{
    using Fanex.Data.Repository;

    public class GetEventCriteria : CriteriaBase
    {
        public string Domain { get; set; }

        public string Ip { get; set; }

        public string Username { get; set; }

        public int? FromId { get; set; }

        public override string GetSettingKey()
        {
            return "SP.Ocs_Event";
        }
    }
}
