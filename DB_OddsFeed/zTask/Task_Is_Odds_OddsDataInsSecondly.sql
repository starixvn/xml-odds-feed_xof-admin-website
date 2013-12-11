 Alter Procedure [dbo].[Task_Is_Odds_OddsDataInsSecondly]
	
AS
/*
Created: 20110210@Peter
Task : Get all Odds Data for OddsFeeed each 1s
DB: DB052.bodb_Maintenance
Original:

Revisions:		             

Param's Explanation:

*/
SET NOCOUNT ON
declare @date datetime
set @date=current_timestamp
--delete the oldest version if number of version >=30 for Deadball/Outright
--												 >=120 for LiveOdds
declare @numberOfVersionDeadball int,@numberOfVersionLive int,@delayTimeDeadball int, @delayTimeLive int 											
select @numberOfVersionDeadball=count (distinct VersionDate) from OutRightOdds (nolock)
select @numberOfVersionLive=count (distinct VersionDate) from LiveOdds (nolock)
select @delayTimeDeadball=isnull(cast(SValue as int),30) from SystemSettings (nolock) where [Type]='DelayTime' and SName='Deadball'
select @delayTimeLive=isnull(cast(SValue as int),30) from SystemSettings (nolock) where [Type]='DelayTime' and SName='Live'

declare @SupportedSports varchar(max),@SupportedBettypes varchar(max)
--select count (distinct VersionDate) from OutRightOdds (nolock)
select @SupportedSports=isnull(SValue,'') from SystemSettings (nolock) where [Type]='SportTypes' and SName='SupportedSports'
select @SupportedBettypes=isnull(SValue,'') from SystemSettings (nolock) where [Type]='BetTypes' and SName='SupportedBettypes'

if(@numberOfVersionDeadball>=@delayTimeDeadball)
begin
    
	declare @versionDate datetime
    select @VersionDate=(select top(1) VersionDate from OutRightOdds (nolock)) 
	--print @versionDate
	delete from OutRightOdds where VersionDate=@VersionDate
    delete from DeadballOdds where VersionDate=@VersionDate
end

if(@numberOfVersionLive>=@delayTimeLive)
begin
    declare @versionDateL datetime
	select @VersionDateL=(select top(1) VersionDate from LiveOdds (nolock))
  --  print @VersionDateL
	delete from LiveOdds where VersionDate=@VersionDateL
    
end


INSERT INTO [dbo].[OutRightOdds]
           ([VersionID]
           ,[OrID]
           ,[LeagueID]
           ,[TeamID]
           ,[Odds]
           ,[EventDate]
           ,[LeagueName]
           ,[LeagueNameC_UNI]
           ,[LeagueNameCN]
           ,[LeagueNameTC]
		   ,[CountryCode]
           ,[TeamName]
           ,[TeamNameC_UNI]
           ,[TeamNameCN]
           ,[LColor]
           ,[SportID]
           ,[VersionDate])

SELECT newid() as id,o.orid, o.leagueid, t.teamid, o.odds, o.eventdate,
    l.leaguename, l.leaguenamec_uni, l.leaguenamecn,l.leaguenametc,l.CountryCode,
    t.teamname, t.teamnamec_uni, t.teamnamecn, l.lcolor, l.sporttype,@date
FROM bodb02.dbo.outright o WITH (nolock)
    INNER JOIN bodb02.dbo.team t WITH (nolock) ON o.teamid = t.teamid
    INNER JOIN bodb02.dbo.league l WITH (nolock) ON o.leagueid = l.leagueid
WHERE
    o.oddsstatus = 'running' AND o.kickofftime > CURRENT_TIMESTAMP
    and l.sporttype in (select cast(val as tinyint) from dbo.strsplit(@SupportedSports, ','))
    
--ORDER BY  o.leagueid,o.matchcode, o.eventdate, l.leaguename, o.odds
     

INSERT INTO [dbo].DeadballOdds
           ([VersionID]
           ,[SportTypeName]
           ,[SportType]
           ,[MatchID]
           ,[BetType]
           ,[EventStatus]
           ,[KickOffTime]
           ,[MatchCode]
           ,[LeagueID]
           ,[Home]
           ,[HomeCN]
           ,[HomeC_UNI]
           ,[Away]
           ,[AwayCN]
           ,[AwayC_UNI]
           ,[LeagueName]
           ,[LeagueNameCN]
           ,[LeagueNameC_UNI]
		   ,CountryCode	
           ,[ComStatus]
           ,[OddsID]
           ,[OddsStatus]
           ,[Hdp1]
           ,[Hdp2]
           ,[Odds1A]
           ,[Odds2A]          
           ,[OddsSpreadA]
           ,[Com1]
           ,[ComX]
           ,[Com2]
           ,[TimeStamp]
           ,[ChangeTime]
           ,[VersionDate])

SELECT  newid(),
 s.Sportname as sporttypename,  
 l.sporttype,  
 r.matchid,
o.bettype,
 eventstatus, 
dateadd(hh, 12, kickofftime) as kickofftime, matchcode, l.leagueid,    
 h.teamname AS Home,h.teamnamecn AS Homecn,h.teamnamec_uni AS Homec_uni,  
 a.teamname AS Away,a.teamnamecn AS Awaycn,a.teamnamec_uni AS Awayc_uni,
 l.leaguename AS LeagueName, l.leaguenamecn, l.leaguenamec_uni,l.CountryCode,
 comstatus as comstatus,
 o.oddsid,o.oddsstatus,o.hdp1,o.hdp2,o.odds1a,o.odds2a,
o.oddsspreada,o.Com1,o.Comx,o.Com2,bodb02.dbo.f_DateTime_UnixTime(changetime) ,
IsNull(changetime, DATEADD(minute,-10, CURRENT_TIMESTAMP)),
 @date
FROM   
 bodb02.dbo.odds o with (nolock)   
 INNER JOIN (  
   select o1.matchcode, o1.kickofftime, o1.eventstatus, o1.showtime, o1.homeid, o1.awayid, o1.leagueid, o1.matchid, o1.bettype, o1.oddsid, o1.hdp1, o1.hdp2, o1.odds1a, o1.liveindicator, count(*) as oddsorder  
   from  
    (select m.matchcode, m.kickofftime, m.eventstatus, m.showtime, m.homeid, m.awayid, m.leagueid,o.matchid, o.bettype, o.oddsid, o.hdp1, o.hdp2, o.odds1a, o.liveindicator 
     from bodb02.dbo.odds as o with (nolock) 
         inner join bodb02.dbo.match as m with (nolock) on o.matchid = m.matchid  
    where m.eventstatus = 'running' AND kickofftime > DATEADD(hour,-4,CURRENT_TIMESTAMP) and o.oddsstatus NOT IN ('suspend','closed','internal') 
          and m.sporttype in (select cast(val as tinyint) from dbo.strsplit(@SupportedSports, ','))   
          and o.bettype in (select cast(val as tinyint) from dbo.strsplit(@SupportedBettypes, ','))
   ) as o1  
   inner join  
    (select o.matchid, o.bettype, o.oddsid, o.liveindicator, o.hdp1, o.hdp2 
     from  bodb02.dbo.odds as o with (nolock) 
	inner join bodb02.dbo.match as m with (nolock) on o.matchid = m.matchid  
    where m.eventstatus = 'running' AND kickofftime > DATEADD(hour,-4,CURRENT_TIMESTAMP) and o.oddsstatus NOT IN ('suspend','closed','internal')
     and m.sporttype in (select cast(val as tinyint) from dbo.strsplit(@SupportedSports, ','))   
     and  o.bettype in (select cast(val as tinyint) from dbo.strsplit(@SupportedBettypes, ','))
  ) as o2  
   on o1.matchid = o2.matchid and o1.bettype=o2.bettype and isnull(o1.liveindicator,0) + isnull(o1.hdp1,0) - isnull(o1.hdp2,0)>=isnull(o2.liveindicator,0) + isnull(o2.hdp1,0) - isnull(o2.hdp2,0)--and o1.oddsid>=o2.oddsid  
   group by o1.matchcode, o1.kickofftime, o1.eventstatus, o1.showtime, o1.homeid, o1.awayid, o1.leagueid,o1.matchid, o1.bettype, o1.oddsid, o1.hdp1, o1.hdp2, o1.odds1a, o1.liveindicator  
   ) AS r ON o.oddsid = r.oddsid  
 INNER JOIN bodb02.dbo.team h with (nolock) on r.homeid = h.teamid  
 INNER JOIN bodb02.dbo.team a with (nolock) ON r.awayid = a.teamid  
 INNER JOIN bodb02.dbo.league l with(nolock) ON r.leagueid = l.leagueid  
 inner join bodb02.dbo.sports s with(nolock) on l.sporttype = s.sporttype   
WHERE  
r.kickofftime > CURRENT_TIMESTAMP 
 AND IsNull(o.liveindicator,0) = 0
/*group by l.sporttype, oddsorder, r.matchcode, r.matchid,o.bettype, eventstatus, kickofftime, showtime, matchcode, l.leagueid,   
 h.teamname,h.teamnamecn,h.teamnamec_uni,  
 a.teamname,a.teamnamecn,a.teamnamec_uni, l.leaguename, l.leaguenamecn, l.leaguenamec_uni,  
 s.Sportname, comstatus  */
--ORDER BY r.matchid, oddsorder,r.matchcode


INSERT INTO [dbo].LiveOdds
           ([VersionID]
           ,[SportTypeName]
           ,[SportType]
           ,[MatchID]
           ,[BetType]
           ,[EventStatus]
           ,[KickOffTime]
           ,[MatchCode]
           ,[LeagueID]
           ,[Home]
           ,[HomeCN]
           ,[HomeC_UNI]
           ,[Away]
           ,[AwayCN]
           ,[AwayC_UNI]
           ,[LeagueName]
           ,[LeagueNameCN]
           ,[LeagueNameC_UNI]
		   ,CountryCode	
           ,[ComStatus]
           ,[OddsID]
           ,[OddsStatus]
           ,[Hdp1]
           ,[Hdp2]
           ,[Odds1A]
           ,[Odds2A]          
           ,[OddsSpreadA]
           ,[Com1]
           ,[ComX]
           ,[Com2]
           ,[TimeStamp]
           ,[ChangeTime]
           ,[VersionDate])

SELECT  newid(),
 s.Sportname as sporttypename,  
 l.sporttype,  
 r.matchid,
o.bettype,
 eventstatus, 
dateadd(hh, 12, kickofftime) as kickofftime, matchcode, l.leagueid,    
 h.teamname AS Home,h.teamnamecn AS Homecn,h.teamnamec_uni AS Homec_uni,  
 a.teamname AS Away,a.teamnamecn AS Awaycn,a.teamnamec_uni AS Awayc_uni,
 l.leaguename AS LeagueName, l.leaguenamecn, l.leaguenamec_uni,l.CountryCode,
 comstatus as comstatus,
 o.oddsid,o.oddsstatus,o.hdp1,o.hdp2,o.odds1a,o.odds2a,
o.oddsspreada,o.Com1,o.Comx,o.Com2,bodb02.dbo.f_DateTime_UnixTime(changetime) ,
IsNull(changetime, DATEADD(minute,-10, CURRENT_TIMESTAMP)),
 @date
FROM   
 bodb02.dbo.odds o with (nolock)   
 INNER JOIN (  
   select o1.matchcode, o1.kickofftime, o1.eventstatus, o1.showtime, o1.homeid, o1.awayid, o1.leagueid, o1.matchid, o1.bettype, o1.oddsid, o1.hdp1, o1.hdp2, o1.odds1a, o1.liveindicator, count(*) as oddsorder  
   from  
    (select m.matchcode, m.kickofftime, m.eventstatus, m.showtime, m.homeid, m.awayid, m.leagueid,o.matchid, o.bettype, o.oddsid, o.hdp1, o.hdp2, o.odds1a, o.liveindicator 
     from bodb02.dbo.odds as o with (nolock) 
         inner join bodb02.dbo.match as m with (nolock) on o.matchid = m.matchid  
    where m.eventstatus = 'running' AND kickofftime > DATEADD(hour,-4,CURRENT_TIMESTAMP) and o.oddsstatus NOT IN ('suspend','closed','internal') 
		 and m.sporttype in (select cast(val as tinyint) from dbo.strsplit(@SupportedSports, ','))   
         and o.bettype in (select cast(val as tinyint) from dbo.strsplit(@SupportedBettypes, ','))
   ) as o1  
   inner join  
    (select o.matchid, o.bettype, o.oddsid, o.liveindicator, o.hdp1, o.hdp2 
     from  bodb02.dbo.odds as o with (nolock) 
	inner join bodb02.dbo.match as m with (nolock) on o.matchid = m.matchid  
    where m.eventstatus = 'running' AND kickofftime > DATEADD(hour,-4,CURRENT_TIMESTAMP) and o.oddsstatus NOT IN ('suspend','closed','internal')
	 and m.sporttype in (select cast(val as tinyint) from dbo.strsplit(@SupportedSports, ','))   
       and o.bettype in (select cast(val as tinyint) from dbo.strsplit(@SupportedBettypes, ','))
  ) as o2  
   on o1.matchid = o2.matchid and o1.bettype=o2.bettype and isnull(o1.liveindicator,0) + isnull(o1.hdp1,0) - isnull(o1.hdp2,0)>=isnull(o2.liveindicator,0) + isnull(o2.hdp1,0) - isnull(o2.hdp2,0)--and o1.oddsid>=o2.oddsid  
   group by o1.matchcode, o1.kickofftime, o1.eventstatus, o1.showtime, o1.homeid, o1.awayid, o1.leagueid,o1.matchid, o1.bettype, o1.oddsid, o1.hdp1, o1.hdp2, o1.odds1a, o1.liveindicator  
   ) AS r ON o.oddsid = r.oddsid  
 INNER JOIN bodb02.dbo.team h with (nolock) on r.homeid = h.teamid  
 INNER JOIN bodb02.dbo.team a with (nolock) ON r.awayid = a.teamid  
 INNER JOIN bodb02.dbo.league l with(nolock) ON r.leagueid = l.leagueid  
 inner join bodb02.dbo.sports s with(nolock) on l.sporttype = s.sporttype   
WHERE  
r.kickofftime > CURRENT_TIMESTAMP 
 AND IsNull(o.liveindicator,0) = 1

GO
GRANT VIEW DEFINITION ON [dbo].[Task_Is_Odds_OddsDataInsSecondly] TO [vnteam]