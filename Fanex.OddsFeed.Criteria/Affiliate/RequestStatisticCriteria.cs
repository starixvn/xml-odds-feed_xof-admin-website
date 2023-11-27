namespace Fanex.OddsFeed.Criteria.Affiliate
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Data.Repository;
    using Fanex.Data;
    using Fanex.OddsFeed.Models.ValueObject;

    public class RequestStatisticCriteria : CriteriaBase
    {
        private DateTime _dateStart;
     
        private DateTime _dateEnd;

        public RequestStatisticCriteria()
            : this(DateTime.Now)
        {
        }

        public RequestStatisticCriteria(DateTime dateEnd)
        {
            this.DateStart = dateEnd;
            this.DateEnd = dateEnd;

            this.Month = dateEnd.Month;
            this.Year = dateEnd.Year;
        }

        public int SystemId { get; set; }

        public string PartnerName { get; set; }

        public string DomainName { get; set; }

        [Required]
        public DateTime DateStart
        {
            get
            {
                if (this.PeriodOption == PeriodType.Month)
                {
                    // select month period
                    this._dateStart = new DateTime(this.Year, this.Month, 1);
                }
                else
                {
                    if (this.PeriodOption == PeriodType.Year)
                    {
                        // select year period
                        this._dateStart = new DateTime(this.Year, 1, 1);
                    }
                }

                return this._dateStart;
            }

            set
            {
                this._dateStart = value;
            }
        }
        
        public DateTime DateEnd
        {
            get
            {
                if (this.PeriodOption == PeriodType.Month)
                {
                    // select month period
                    this._dateEnd = new DateTime(this.Year, this.Month, 1).AddMonths(1).AddDays(-1);
                }
                else
                {
                    if (this.PeriodOption == PeriodType.Year)
                    {
                        const byte December = 12;
                        const byte LastDayOfMonth = 31;
                        this._dateEnd = new DateTime(this.Year, December, LastDayOfMonth);
                    }
                }

                return this._dateEnd;
            }

            set
            {
                this._dateEnd = value;
            }
        }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        // Ignored
        [SpParam(Ignored = true)]
        public PeriodType PeriodOption { get; set; }

        [SpParam(Ignored = true)]
        public string Week { get; set; }

        [SpParam(Ignored = true)]
        public string Date { get; set; }

        [SpParam(Ignored = true)]
        public string ToWeek { get; set; }

        [SpParam(Ignored = true)]
        public int Month { get; set; }

        [SpParam(Ignored = true)]
        public int Year { get; set; }

        [SpParam(Ignored = true)]
        public string SystemName { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_Partner_Statistic_Sel";
        }

        public override bool IsValid()
        {
            return this.DateStart <= this.DateEnd;
        }
    }
}