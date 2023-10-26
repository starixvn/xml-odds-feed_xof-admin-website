namespace Fanex.OddsFeed.Core.Model
{
    using System.ComponentModel;

    public enum PeriodType
    {
        [Description("Date")]
        Date = 0,

        [Description("Week")]
        Week = 1,

        [Description("Month")]
        Month = 2,

        [Description("Year")]
        Year = 4,

        NULL = -1
    }
}