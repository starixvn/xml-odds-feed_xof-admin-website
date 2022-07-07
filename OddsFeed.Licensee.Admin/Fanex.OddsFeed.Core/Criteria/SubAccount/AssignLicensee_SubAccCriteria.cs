namespace Fanex.OddsFeed.Core.Criteria.SubAccount
{
    using System;
    using Data.Repository;

    public class AssignLicensee_SubAccCriteria : ResultCriteria
    {
        [SpParam(SqlParamName = "AdminID")]
        public int UserID { get; set; }

        [SpParam(SqlParamName = "ListSystemId")]
        public string ListSystemId { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        [SpParam(Ignored = true)]
        public string UserName { get; set; }

        [SpParam(Ignored = true)]
        public string[] SystemList
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ListSystemId))
                {
                    return null;
                }

                return ListSystemId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }

            set
            {
                ListSystemId = Utilities.StringArrayToString(value);
            }
        }

        public override string GetSettingKey() => "Odds_Admin_UpdSystem";

        public override bool IsValid() => true;
    }
}