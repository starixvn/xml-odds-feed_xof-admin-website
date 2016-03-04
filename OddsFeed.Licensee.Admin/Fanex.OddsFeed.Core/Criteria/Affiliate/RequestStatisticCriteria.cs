// --------------------------------------------------------------------------
// <copyright file="RequestStatisticCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.02.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Criteria.Affiliate
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.OddsFeed.Core.Model;
    using Fanex.OddsFeed.Framework.Data;
    using Data.Repository;
    /// <summary>
    /// Class RequestStatisticCriteria.
    /// </summary>
    public class RequestStatisticCriteria : Framework.Data.CriteriaBase
    {        
        private DateTime _dateStart;
     
        private DateTime _dateEnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestStatisticCriteria" /> class.
        /// </summary>
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

        public string SystemName { get; set; }

        public string PartnerName { get; set; }

        public string DomainName { get; set; }

        [SpParam(Ignored = true)]
        public PeriodType PeriodOption { get; set; }
        
        [SpParam(Ignored = true)]
        public string Week { get; set; }
        
        [SpParam(Ignored = true)]
        public string ToWeek { get; set; }

        [SpParam(Ignored = true)]
        public int Month { get; set; }

        [SpParam(Ignored = true)]
        public int Year { get; set; }

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
                else if (this.PeriodOption == PeriodType.Year)
                {
                    // select year period
                    this._dateStart = new DateTime(this.Year, 1, 1);
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
                else if (this.PeriodOption == PeriodType.Year)
                {
                    // select year period
                    this._dateEnd = new DateTime(this.Year, 12, 31);
                }

                return this._dateEnd;
            }

            set
            {
                this._dateEnd = value;
            }
        }

        public override string GetSettingKey()
        {
            return "Odds_Partner_Statistic_Sel";
        }

        /// <summary>
        /// Check data's constrains.
        /// </summary>
        /// <returns>True: valid; False: invalid.</returns>
        public override bool IsValid()
        {
            return this.DateStart <= this.DateEnd;
        }
    }
}
