// --------------------------------------------------------------------------
// <copyright file="OddsProfile.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Nexcel.OddsFeed.Library
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Data.DNAide;

    /// <summary>
    /// Class OddsProfile.
    /// </summary>
    public class OddsProfile
    {                
        /// <summary>
        /// Initializes a new instance of the <see cref="OddsProfile"/> class.
        /// </summary>
        /// <param name="databaseResult">The database result.</param>
        public OddsProfile(DbResult databaseResult)
        {
            var data = databaseResult.ResultSet.Tables[0].Rows[0];

            OddsType = Utilities.GetIntList(data["OddsType"].ToString());
            OddRegion = string.IsNullOrEmpty(data["OddsRegion"].ToString()) ? 0 : int.Parse(data["OddsRegion"].ToString());
            DelayTimes = Utilities.GetIntList(data["DelayTimes"].ToString());
            Languages = Utilities.GetStringIList(data["LanguageList"].ToString());

            Sports = data["SportIDList"].ToString().Equals("all") ? null : Utilities.GetIntList(data["SportIDList"].ToString());
            Countries = data["CountryCodeList"].ToString().Equals("all") ? null : Utilities.GetStringIList(data["CountryCodeList"].ToString().ToLower());
            
            ////leagues
            Leagues = data["LeagueIDList"].ToString().Equals("all") ? null : Utilities.GetIntList(data["LeagueIDList"].ToString());
            
            ////outrights
            Outrights = data["OutrightIDList"].ToString().Equals("all") ? null : Utilities.GetIntList(data["OutrightIDList"].ToString());
            
            BetTypes = data["BetTypeIDList"].ToString().Equals("all") ? null : Utilities.GetIntList(data["BetTypeIDList"].ToString());

            LeagueKeyword = data["LeagueKeyword"].ToString().ToLower();
            OutrightsKeyword = data["LeagueKeyword_O"].ToString().ToLower();

            ////TimeRange = string.IsNullOrEmpty(data["TimeRange"].ToString()) ? 0 : int.Parse(data["TimeRange"].ToString());
            ////SpParams = data["SPParams"].ToString();

            if (data.Table.Columns.Contains("Domain"))
            {
                Domain = data["Domain"].ToString();
            }
        }

        #region Properties

        /// <summary>
        /// Gets or sets the Odds type.
        /// </summary>
        /// <value> 2: dead ball. </value>
        /// <value> 4: live.</value>
        /// <value> 8: outrights.</value>
        public IList<int> OddsType { get; set; }

        public int OddRegion { get; set; }

        public IList<string> Languages { get; set; }

        public IList<int> Sports { get; set; }

        public IList<string> Countries { get; set; }

        /// <summary>
        /// Gets or sets Leagues
        /// Separate ; between match and outright
        /// Separate , for match/outright        
        /// All:  = null, get all match/outright
        /// Count = 0: : select no league.
        /// </summary>
        /// <value> All:  = null, get all match / outright. </value>        
        public IList<int> Leagues { get; set; }

        public IList<int> Outrights { get; set; }

        public IList<int> BetTypes { get; set; }

        /// <summary>
        /// Gets or sets DelayTimes
        /// Delay Time for Dead ball, Live, OutRight.
        /// </summary>
        /// <value> 60, 60, 60 : Dead ball, live, Outright. </value>
        public IList<int> DelayTimes { get; set; }

        public string LeagueKeyword { get; set; }

        public string OutrightsKeyword { get; set; }

        public int TimeRange { get; set; }

        public string SpParams { get; set; }

        public string Domain { get; set; }

        #endregion
    }
}
