namespace Fanex.OddsFeed.Core.Criteria
{
    using Data.Repository;

    public class ResultCriteria : CriteriaBase
    {
        public int ErrCode { get; set; }

        public string ErrMsg { get; set; } = string.Empty;
    }
}