namespace Fanex.OddsFeed.Core.Criteria.Administrator
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Data.Repository;
    using Fanex.Data;

    public class UpdateAdminSystemCriteria : CriteriaBase
    {
        [Required]
        [SpParam(SqlParamName = "AdminID")]
        public int UserID { get; set; }

        public string ListSystemId { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        // Ignored
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