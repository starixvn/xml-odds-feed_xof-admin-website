 ALTER Procedure [dbo].[Is_Odds_GetLiveOddsData]
	
AS
  
/*	
	Task : Get XML Odds
	DB: DB06.bodb02
	Revisions:	
		- 20100426@Peter: Create SP
	
	**Bettypes:
		Outright
		---------------
		Handicap
		Over/Under
		FT.1X2
		1st Handicap
		1st Over/Under
		1st 1X2		
		Odd/Even
		1st Odd/Even
		First Goal/Last Goal
		Correct Score
		Total Goal
		HT/FT
*/
SET NOCOUNT ON
declare @date datetime
set @date=current_timestamp

declare @SupportedSports varchar(max),@SupportedBettypes varchar(max)
--select count (distinct VersionDate) from OutRightOdds (nolock)
--select @SupportedSports=isnull(SValue,'') from bodb_Maintenance.dbo.SystemSettings with (nolock) where [Type]='SportTypes' and SName='SupportedSports'
--select @SupportedBettypes=isnull(SValue,'') from bodb_Maintenance.dbo.SystemSettings with (nolock) where [Type]='BetTypes' and SName='SupportedBettypes'

Select newid() as [VersionID],
 s.Sportname as [SportTypeName],  
 l.sporttype,  
 r.matchid,
 r.bettype,
 r.eventstatus, 
 dateadd(hh, 12, r.kickofftime) as kickofftime,  
 r.matchcode, 
 r.leagueid,    
 h.teamname AS Home,h.teamnamecn AS Homecn,h.teamnamec_uni AS Homec_uni,  
 a.teamname AS Away,a.teamnamecn AS Awaycn,a.teamnamec_uni AS Awayc_uni,
 l.leaguename AS LeagueName, l.leaguenamecn, l.leaguenamec_uni,l.CountryCode, 
 r.oddsid,r.oddsstatus,r.hdp1,r.hdp2,r.odds1a,r.odds2a,
r.oddsspreada,r.Com1,r.Comx,r.Com2,
cs00, cs11, cs22, cs33, cs44,
cs01, cs02, cs03, cs04, cs05,
	cs10, cs20, cs30, cs40, cs50,
	cs12, cs13, cs23, cs14, cs24, cs34,
	cs21, cs31, cs32, cs41, cs42, cs43,
r.[Timestamp] ,r.ChangeTime,
 @date as [VersionDate]
 
 FROM 
 (
SELECT
	m.matchid, m.matchcode, m.eventstatus, m.showtime, m.kickofftime,
	m.homeid, m.awayid, m.leagueid,
	o.oddsid, o.OddsStatus, o.bettype,
	o.hdp1, o.hdp2,
	o.odds1a,
	o.odds2a,
	o.oddsspreada,
	Convert(decimal(7,3) , o.Com1) com1,
	Convert(decimal(7,3) , o.ComX) comX,
	Convert(decimal(7,3) , o.Com2) com2,
	DateAdd(hh, 12, IsNull(o.changetime, DATEADD(minute,-10, CURRENT_TIMESTAMP))) as  ChangeTime,
	dbo.f_DateTime_UnixTime(o.changetime) [Timestamp],	
	cs00, cs11, cs22, cs33, cs44,
	cs01, cs02, cs03, cs04, cs05,
	cs10, cs20, cs30, cs40, cs50,
	cs12, cs13, cs23, cs14, cs24, cs34,
	cs21, cs31, cs32, cs41, cs42, cs43
FROM dbo.match m with (nolock)
INNER JOIN dbo.odds o with (nolock)	ON m.matchid = o.matchid
WHERE
	m.eventstatus = 'running' 
	--AND m.kickofftime > CURRENT_TIMESTAMP
	and o.oddsstatus NOT IN ('suspend','closed','internal') 
	--and m.sporttype in (select cast(val as tinyint) from dbo.strsplit(@SupportedSports, ','))   
    --and  o.bettype in (select cast(val as tinyint) from dbo.strsplit(@SupportedBettypes, ','))
    and o.bettype in (1,3,5)
	and IsNull(o.liveindicator,0) =1
	
)  r
INNER JOIN dbo.team h with (nolock) on r.homeid = h.teamid  
INNER JOIN dbo.team a with (nolock) ON r.awayid = a.teamid  
INNER JOIN dbo.league l with(nolock) ON r.leagueid = l.leagueid  
inner join dbo.sports s with(nolock) on l.sporttype = s.sporttype   
WHERE l.istest = 0

ORDER BY
	r.matchcode, r.matchid,
	r.bettype, r.oddsid

GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_GetLiveOddsData] TO [vnteam]
GO
GRANT EXECUTE ON [dbo].[Is_Odds_GetLiveOddsData] TO [bodbAdminNet]
GO