ALTER PROCEDURE [dbo].[OddsFeed_AffFull_Outright_OddsSel]  
@timestamp int = 0
AS  
/*	
	Task : Get XML Odds (bettype 1,3,5,7,8,15) and Outright
	DB: DB06.bodb02
	Revisions:	
		- 20100123@Viet: Create SP
*/

SET NOCOUNT ON  
-- OUTRIGHT  
SELECT o.orid, o.leagueid, t.teamid, o.odds, o.eventdate,
    l.leaguename, l.leaguenamec_uni, l.leaguenamecn,
    t.teamname, t.teamnamec_uni, t.teamnamecn, l.lcolor, l.leaguenametc sportname
FROM dbo.outright o WITH (nolock)
    INNER JOIN dbo.team t WITH (nolock) ON o.teamid = t.teamid
    INNER JOIN dbo.league l WITH (nolock) ON o.leagueid = l.leagueid
WHERE
    o.oddsstatus = 'running' AND o.kickofftime > CURRENT_TIMESTAMP
    and l.sporttype = 1
    and l.IsTest=0
ORDER BY o.matchcode, o.eventdate, l.leaguename, o.odds

SELECT  
	l.leaguenametc as sporttypename,  
	l.sporttype,  
	r.matchid, eventstatus, dateadd(hh, 12, kickofftime) as kickofftime, matchcode, l.leagueid,    
	h.teamname AS Home,h.teamnamecn AS Homecn,h.teamnamec_uni AS Homec_uni,  
	a.teamname AS Away,a.teamnamecn AS Awaycn,a.teamnamec_uni AS Awayc_uni, l.leaguename AS LeagueName, l.leaguenamecn as leaguenamecn, l.leaguenamec_uni,  
	dbo.f_DateTime_UnixTime(changetime) as [timestamp],
	----------------------------------------------------------------------------------------------------  
	--bettype = 1
	Max(case when o.bettype = 1 then o.oddsid else NULL end) as OddsID_1, 
	Max(case when o.bettype = 1 then o.oddsstatus else NULL end) as OddsStatus_1, 

	Max(case when o.bettype = 1 then o.hdp1 else NULL end) as Hdp1_1, 
	Max(case when o.bettype = 1 then o.hdp2 else NULL end) as Hdp2_1, 

	Convert(decimal(7,3), Max(case when o.bettype = 1 then dbo.f_CalOdds_Malay_HK(o.odds1a) Else NULL end)) as  odds1hka_1,
	Convert(decimal(7,3), Max(case when o.bettype = 1 then dbo.f_CalOdds_Malay_HK(o.odds2a) Else NULL end)) as  odds2hka_1,

	Convert(decimal(7,3), Max(case when o.bettype = 1 then o.oddsspreada Else NULL end)) as  oddsspreada_1,
	dateadd(hh, 12, Max(case when o.bettype = 1 then IsNull(changetime, DATEADD(minute,-10, CURRENT_TIMESTAMP)) Else NULL end)) as  changetime_1,
	----------------------------------------------------------------------------------------------------
	--bettype = 3
	Max(case when o.bettype = 3 then o.oddsid else NULL end) as OddsID_3, 
	Max(case when o.bettype = 3 then o.oddsstatus else NULL end) as OddsStatus_3, 
	Convert(decimal(7,3), Max(case when o.bettype = 3 then o.hdp1 else NULL end)) as Hdp1_3, 
	Convert(decimal(7,3), Max(case when o.bettype = 3 then o.hdp2 else NULL end)) as Hdp2_3, 

	Convert(decimal(7,3), Max(case when o.bettype = 3 then dbo.f_CalOdds_Malay_HK(o.odds1a) Else NULL end)) as  odds1hka_3,
	Convert(decimal(7,3), Max(case when o.bettype = 3 then dbo.f_CalOdds_Malay_HK(o.odds2a) Else NULL end)) as  odds2hka_3,

	Convert(decimal(7,3), Max(case when o.bettype = 3 then o.oddsspreada Else NULL end)) as  oddsspreada_3,
	dateadd(hh, 12, Max(case when o.bettype = 3 then IsNull(changetime, DATEADD(minute,-10, CURRENT_TIMESTAMP)) Else NULL end)) as  changetime_3,
	----------------------------------------------------------------------------------------------------		
	--bettype = 5
	Max(case when o.bettype = 5 then o.oddsid else NULL end) as OddsID_5, 
	Max(case when o.bettype = 5 then o.oddsstatus else NULL end) as OddsStatus_5, 

	Convert(decimal(7,3), Max(case when o.bettype = 5 then o.Com1 Else NULL end)) as  Com1_5,
	Convert(decimal(7,3), Max(case when o.bettype = 5 then o.Comx Else NULL end)) as  Comx_5,
	Convert(decimal(7,3), Max(case when o.bettype = 5 then o.Com2 Else NULL end)) as  Com2_5,
	dateadd(hh, 12, Max(case when o.bettype = 5 then IsNull(changetime, DATEADD(minute,-10, CURRENT_TIMESTAMP)) Else NULL end)) as  changetime_5,
	----------------------------------------------------------------------------------------------------
	--bettype = 7  
	Max(case when o.bettype = 7 then o.oddsid else NULL end) as OddsID_7,   
	Max(case when o.bettype = 7 then o.oddsstatus else NULL end) as OddsStatus_7,   

	Max(case when o.bettype = 7 then o.hdp1 else NULL end) as Hdp1_7,   
	Max(case when o.bettype = 7 then o.hdp2 else NULL end) as Hdp2_7,   

	Convert(decimal(7,3), Max(case when o.bettype = 7 then dbo.f_CalOdds_Malay_HK(o.odds1a) Else NULL end)) as  odds1hka_7,  
	Convert(decimal(7,3), Max(case when o.bettype = 7 then dbo.f_CalOdds_Malay_HK(o.odds2a) Else NULL end)) as  odds2hka_7,  

	Convert(decimal(7,3), Max(case when o.bettype = 7 then o.oddsspreada Else NULL end)) as  oddsspreada_7,  
	dateadd(hh, 12, Max(case when o.bettype = 7 then IsNull(changetime, DATEADD(minute,-10, CURRENT_TIMESTAMP)) Else NULL end)) as  changetime_7,  
	----------------------------------------------------------------------------------------------------  
	--bettype = 8  
	Max(case when o.bettype = 8 then o.oddsid else NULL end) as OddsID_8,   
	Max(case when o.bettype = 8 then o.oddsstatus else NULL end) as OddsStatus_8,   
	Convert(decimal(7,3), Max(case when o.bettype = 8 then o.hdp1 else NULL end)) as Hdp1_8,   
	Convert(decimal(7,3), Max(case when o.bettype = 8 then o.hdp2 else NULL end)) as Hdp2_8,   

	Convert(decimal(7,3), Max(case when o.bettype = 8 then dbo.f_CalOdds_Malay_HK(o.odds1a) Else NULL end)) as  odds1hka_8,  
	Convert(decimal(7,3), Max(case when o.bettype = 8 then dbo.f_CalOdds_Malay_HK(o.odds2a) Else NULL end)) as  odds2hka_8,  

	Convert(decimal(7,3), Max(case when o.bettype = 8 then o.oddsspreada Else NULL end)) as  oddsspreada_8,  
	dateadd(hh, 12, Max(case when o.bettype = 8 then IsNull(changetime, DATEADD(minute,-10, CURRENT_TIMESTAMP)) Else NULL end)) as  changetime_8,  
	----------------------------------------------------------------------------------------------------    
	--bettype = 15  
	Max(case when o.bettype = 15 then o.oddsid else NULL end) as OddsID_15,   
	Max(case when o.bettype = 15 then o.oddsstatus else NULL end) as OddsStatus_15,   

	Convert(decimal(7,3), Max(case when o.bettype = 15 then o.Com1 Else NULL end)) as  Com1_15,  
	Convert(decimal(7,3), Max(case when o.bettype = 15 then o.Comx Else NULL end)) as  Comx_15,  
	Convert(decimal(7,3), Max(case when o.bettype = 15 then o.Com2 Else NULL end)) as  Com2_15,  
	dateadd(hh, 12, Max(case when o.bettype = 15 then IsNull(changetime, DATEADD(minute,-10, CURRENT_TIMESTAMP)) Else NULL end)) as  changetime_15,  
	----------------------------------------------------------------------------------------------------  
	case when sum(case when (IsNull(o.liveindicator,0) = 1) then 1 else 0 end) >0 then 1 else 0 end as Live  
FROM   
	odds o (nolock)   
	INNER JOIN (  
		select o1.matchcode, o1.kickofftime, o1.eventstatus, o1.showtime, o1.homeid, o1.awayid, o1.leagueid, o1.matchid, o1.bettype, o1.oddsid, o1.hdp1, o1.hdp2, o1.odds1a, o1.liveindicator, count(*) as oddsorder  
		from  
		(select m.matchcode, m.kickofftime, m.eventstatus, m.showtime, m.homeid, m.awayid, m.leagueid,o.matchid, o.bettype, o.oddsid, o.hdp1, o.hdp2, o.odds1a, o.liveindicator from   
		odds as o (nolock) inner join match as m (nolock) on o.matchid = m.matchid  
		where m.eventstatus = 'running' AND kickofftime > DATEADD(hour,-4,CURRENT_TIMESTAMP) and o.oddsstatus NOT IN ('suspend','closed','internal') and o.bettype in (1,3,5,7,8,15) ) as o1  
		inner join  
		(select o.matchid, o.bettype, o.oddsid, o.liveindicator, o.hdp1, o.hdp2 from   
		odds as o (nolock) inner join match as m (nolock) on o.matchid = m.matchid  
		where m.eventstatus = 'running' AND kickofftime > DATEADD(hour,-4,CURRENT_TIMESTAMP) and o.oddsstatus NOT IN ('suspend','closed','internal') and o.bettype in (1,3,5,7,8,15)  ) as o2  
		on o1.matchid = o2.matchid and o1.bettype=o2.bettype and isnull(o1.liveindicator,0) + isnull(o1.hdp1,0) - isnull(o1.hdp2,0)>=isnull(o2.liveindicator,0) + isnull(o2.hdp1,0) - isnull(o2.hdp2,0)--and o1.oddsid>=o2.oddsid  
		group by o1.matchcode, o1.kickofftime, o1.eventstatus, o1.showtime, o1.homeid, o1.awayid, o1.leagueid,o1.matchid, o1.bettype, o1.oddsid, o1.hdp1, o1.hdp2, o1.odds1a, o1.liveindicator  
		) AS r ON o.oddsid = r.oddsid  
	INNER JOIN team h (nolock) on r.homeid = h.teamid  
	INNER JOIN team a (nolock) ON r.awayid = a.teamid  
	INNER JOIN league l (nolock) ON r.leagueid = l.leagueid   
WHERE o.bettype IN (1,3,5,7,8,15)  
	AND r.kickofftime > CURRENT_TIMESTAMP 
	AND IsNull(o.liveindicator,0) = 0  
	AND l.istest = 0
	 and dbo.f_DateTime_UnixTime(changetime) > @timestamp 
group by l.sporttype, oddsorder, r.matchcode, r.matchid, eventstatus, kickofftime, showtime, matchcode, l.leagueid,   
	h.teamname,h.teamnamecn,h.teamnamec_uni,  
	a.teamname,a.teamnamecn,a.teamnamec_uni, l.leaguename, l.leaguenamecn, l.leaguenamec_uni,  
	l.leaguenametc,o.changetime  
ORDER BY r.matchcode, r.matchid, oddsorder

GO
GRANT EXECUTE ON [dbo].[OddsFeed_AffFull_Outright_OddsSel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[OddsFeed_AffFull_Outright_OddsSel] TO [vnteam]