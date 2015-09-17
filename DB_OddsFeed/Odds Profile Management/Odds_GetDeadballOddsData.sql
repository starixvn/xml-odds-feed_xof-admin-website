USE bodb02
GO

ALTER PROCEDURE [dbo].[Is_Odds_GetDeadballOddsData]
AS
/*	
	Task : Get XML Odds
	DB: DB06.bodb02
	Revisions:	
		- 20100426@Peter: Create SP
		- 20150909@bobby.nguyen: Re-format code and add more bettype[20]
		
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
BEGIN
	SET NOCOUNT ON

	DECLARE @date DATETIME

	SET @date = current_timestamp

	DECLARE @SupportedSports VARCHAR(max)
		, @SupportedBettypes VARCHAR(max)

	--select count (distinct VersionDate) from OutRightOdds (nolock)
	--select @SupportedSports=isnull(SValue,'') from bodb_Maintenance.dbo.SystemSettings with (nolock) where [Type]='SportTypes' and SName='SupportedSports'
	--select @SupportedBettypes=isnull(SValue,'') from bodb_Maintenance.dbo.SystemSettings with (nolock) where [Type]='BetTypes' and SName='SupportedBettypes'
	SELECT newid() AS [VersionID]
		, s.Sportname AS [SportTypeName]
		, l.sporttype
		, r.matchid
		, r.bettype
		, r.eventstatus
		, dateadd(hh, 12, r.kickofftime) AS kickofftime
		, r.matchcode
		, r.leagueid
		, h.teamname AS Home
		, h.teamnamecn AS Homecn
		, h.teamnamec_uni AS Homec_uni
		, a.teamname AS Away
		, a.teamnamecn AS Awaycn
		, a.teamnamec_uni AS Awayc_uni
		, l.leaguename AS LeagueName
		, l.leaguenamecn
		, l.leaguenamec_uni
		, l.CountryCode
		, r.oddsid
		, r.oddsstatus
		, r.hdp1
		, r.hdp2
		, r.odds1a
		, r.odds2a
		, r.oddsspreada
		, r.Com1
		, r.Comx
		, r.Com2
		, cs00
		, cs11
		, cs22
		, cs33
		, cs44
		, cs01
		, cs02
		, cs03
		, cs04
		, cs05
		, cs10
		, cs20
		, cs30
		, cs40
		, cs50
		, cs12
		, cs13
		, cs23
		, cs14
		, cs24
		, cs34
		, cs21
		, cs31
		, cs32
		, cs41
		, cs42
		, cs43
		, r.[Timestamp]
		, r.ChangeTime
		, @date AS [VersionDate]
	FROM (
		SELECT m.matchid
			, m.matchcode
			, m.eventstatus
			, m.showtime
			, m.kickofftime
			, m.homeid
			, m.awayid
			, m.leagueid
			, o.oddsid
			, o.OddsStatus
			, o.bettype
			, o.hdp1
			, o.hdp2
			, o.odds1a
			, o.odds2a
			, o.oddsspreada
			, Convert(DECIMAL(7, 3), o.Com1) com1
			, Convert(DECIMAL(7, 3), o.ComX) comX
			, Convert(DECIMAL(7, 3), o.Com2) com2
			, DateAdd(hh, 12, IsNull(o.changetime, DATEADD(minute, - 10, CURRENT_TIMESTAMP))) AS ChangeTime
			, dbo.f_DateTime_UnixTime(o.changetime) [Timestamp]
			, cs00
			, cs11
			, cs22
			, cs33
			, cs44
			, cs01
			, cs02
			, cs03
			, cs04
			, cs05
			, cs10
			, cs20
			, cs30
			, cs40
			, cs50
			, cs12
			, cs13
			, cs23
			, cs14
			, cs24
			, cs34
			, cs21
			, cs31
			, cs32
			, cs41
			, cs42
			, cs43
		FROM dbo.match m WITH (NOLOCK)
		INNER JOIN dbo.odds o WITH (NOLOCK) ON m.matchid = o.matchid
		WHERE m.eventstatus = 'running'
			AND m.kickofftime > CURRENT_TIMESTAMP
			AND o.oddsstatus NOT IN (
				'suspend'
				, 'closed'
				, 'internal'
				)
			--and m.sporttype in (select cast(val as tinyint) from dbo.strsplit(@SupportedSports, ','))   
			--and  o.bettype in (select cast(val as tinyint) from dbo.strsplit(@SupportedBettypes, ','))
			AND o.bettype IN (
				1
				, 2
				, 3
				, 4
				, 5
				, 6
				, 7
				, 8
				, 10
				, 12
				, 15
				, 16
				, 17
				, 18
				, 20
				)
			AND IsNull(o.liveindicator, 0) = 0
		) r
	INNER JOIN dbo.team h WITH (NOLOCK) ON r.homeid = h.teamid
	INNER JOIN dbo.team a WITH (NOLOCK) ON r.awayid = a.teamid
	INNER JOIN dbo.league l WITH (NOLOCK) ON r.leagueid = l.leagueid
	INNER JOIN dbo.sports s WITH (NOLOCK) ON l.sporttype = s.sporttype
	WHERE l.istest = 0
	ORDER BY r.matchcode
		, r.matchid
		, r.bettype
		, r.oddsid
END
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_GetDeadballOddsData] TO [vnteam]
GO

GRANT EXECUTE ON [dbo].[Is_Odds_GetDeadballOddsData] TO [bodbAdminNet]
GO


