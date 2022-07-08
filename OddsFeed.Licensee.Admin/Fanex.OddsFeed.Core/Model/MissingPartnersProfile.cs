namespace Fanex.OddsFeed.Core.Model
{
    using System;

    [Serializable]
    public class MissingPartnersProfile
    {
        public MissingPartnersProfile()
        {
            this.ErrCode = 0;

            this.AffsOddsType = string.Empty;
            this.AffsLanguageList = string.Empty;
            this.AffsLanguageList = string.Empty;
            this.AffsSportIDList = string.Empty;
            this.AffsCountryCodeList = string.Empty;
            this.AffsLeagueIDList = string.Empty;
            this.AffsOutrightIDList = string.Empty;
            this.AffsBettypeIDList = string.Empty;
        }

        public int ErrCode { get; set; }

        public string AffsOddsType { get; set; }

        public string AffsLanguageList { get; set; }

        public string AffsSportIDList { get; set; }

        public string AffsCountryCodeList { get; set; }

        public string AffsLeagueIDList { get; set; }

        public string AffsOutrightIDList { get; set; }

        public string AffsBettypeIDList { get; set; }
    }
}