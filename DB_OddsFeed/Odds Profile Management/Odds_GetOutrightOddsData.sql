set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

/****** Object:  Stored Procedure dbo.Is_Odds_GetOutrightOddsData    Script Date: 5/12/2011 11:39:05 AM ******/
  ALTER Procedure [dbo].[Is_Odds_GetOutrightOddsData]
 
AS
/*
Created: 20110210@Peter
Task : Get all OutRight Odds Data for OddsFeeed each 5s
DB: DB06.bodb02
Original:

Revisions:  
  - This SP just get sportsbook outright only. For other outright (Racing), we must join to another table
   For Virtual Racing,we have VSOutright table             

Param's Explanation:

*/
SET NOCOUNT ON
declare @date datetime
set @date=current_timestamp

SELECT newid() [VersionID],o.orid [OrID], o.leagueid [LeagueID], t.teamid [TeamID], o.odds [Odds], o.eventdate [EventDate],
    l.leaguename [LeagueName], l.leaguenamec_uni [LeagueNameC_UNI], l.leaguenamecn [LeagueNameCN],l.leaguenametc [LeagueNameTC],l.CountryCode [CountryCode],
    t.teamname [TeamName], t.teamnamec_uni [TeamNameC_UNI], t.teamnamecn [TeamNameCN], l.lcolor [LColor], l.sporttype [SportID],@date [VersionDate]
FROM outright o WITH (nolock)
    INNER JOIN team t WITH (nolock) ON o.teamid = t.teamid
    INNER JOIN league l WITH (nolock) ON o.leagueid = l.leagueid
WHERE
    o.oddsstatus = 'running' AND o.kickofftime > CURRENT_TIMESTAMP
 AND l.istest = 0
    and (l.sporttype<=99 or l.sporttype  in (180,183,184,185,186))
    
ORDER BY  o.leagueid,o.matchcode, o.eventdate, l.leaguename, o.odds

GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_GetOutrightOddsData] TO [vnteam]
GO
GRANT EXECUTE ON [dbo].[Is_Odds_GetOutrightOddsData] TO [bodbAdminNet]
GO