namespace Fanex.OddsFeed.Criteria.Licensee
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Data.Repository;
    using Fanex.Data;
    using Fanex.OddsFeed.Models.ValueObject;

    public class LicenseeRequestCriteria : CriteriaBase
    {
        private const int TimeZone = 7;
        private DateTime _dateStart;
        private DateTime _dateEnd;

        public LicenseeRequestCriteria()
            : this(DateTimeOffset.Now.DateTime)
        {
        }

        public LicenseeRequestCriteria(DateTime dateEnd)
        {
            this.DateStart = dateEnd;
            this.DateEnd = dateEnd;

            this.Month = dateEnd.Month;
            this.Year = dateEnd.Year;
        }

        public int? LicenseeId { get; set; }

        public string SystemName { get; set; }



        [Required]
        public DateTime DateStart
        {
            get
            {
                if (this.PeriodOption == PeriodType.Month.ToString())
                {
                    // select month period
                    this._dateStart = new DateTimeOffset(this.Year, this.Month, 1, 0, 0, 0, TimeSpan.FromHours(-TimeZone)).DateTime;
                }
                else
                {
                    if (this.PeriodOption == PeriodType.Year.ToString())
                    {
                        // select year period
                        this._dateStart = new DateTimeOffset(this.Year, 1, 1, 0, 0, 0, TimeSpan.FromHours(-TimeZone)).DateTime;
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
                if (this.PeriodOption == PeriodType.Month.ToString())
                {
                    // select month period
                    this._dateEnd = new DateTimeOffset(this.Year, this.Month, 1, 0, 0, 0, TimeSpan.FromHours(-TimeZone)).DateTime.AddMonths(1).AddDays(-1);
                }
                else
                {
                    if (this.PeriodOption == PeriodType.Year.ToString())
                    {
                        // select year period
                        const byte December = 12;
                        const byte LastDayOfMonth = 31;
                        this._dateEnd = new DateTimeOffset(this.Year, December, LastDayOfMonth, 0, 0, 0, TimeSpan.FromHours(-TimeZone)).DateTime;
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

        public string PeriodOption { get; set; }

        // Ignored
        [SpParam(Ignored = true)]
        public string Week { get; set; }

        [SpParam(Ignored = true)]
        public string ToWeek { get; set; }

        [SpParam(Ignored = true)]
        public int Month { get; set; }

        [SpParam(Ignored = true)]
        public int Year { get; set; }

        public override string GetSettingKey()
        {
            return "Odds_Licensee_Statistic_Sel";
        }

        public override bool IsValid()
        {
            return this.DateStart <= this.DateEnd;
        }
    }
}