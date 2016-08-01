USE bodb02
GO

ALTER PROCEDURE [dbo].[Is_Odds_GetDeadballOddsData]
AS
/*	
	Task : Get XML Odds
	DB: DBACC.bodb02
	Revisions:	
		- 20100426@Peter: Create SP
		- 20150909@bobby.nguyen: Re-format code and add more bettype[20]
		- 20151002@bobby.nguyen: Support all sporttype & bettype of sportbook product
		- 20160202@Don: Convert some information from Decimal to Float (data type)
		- 20160329@bobby: #54093 support new bettype PH AIO
		- 20160801@bobby: #61422 support new bettype PH AIO phase 2. Bet type 416
		
		
	**Bettypes: All sporttypes & bettypes of sportbook product
*/
BEGIN
	SET NOCOUNT ON

	DECLARE @date DATETIME

	SET @date = current_timestamp

	;WITH SportBookBettype 
	AS (
		SELECT DISTINCT bt.bettype
		FROM dbo.SportBettypeMatrix bt WITH(NOLOCK)
		WHERE bt.ProductID 		= 1 
			 AND bt.EventType 	= 1
			
	), OddsData 
	AS (
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
			, Convert(FLOAT, ROUND(o.Com1, 3)) com1
			, Convert(FLOAT, ROUND(o.ComX, 3)) comX
			, Convert(FLOAT, ROUND(o.Com2, 3)) com2
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
			, cs99
			, ISNULL(resourceID, '') resourceID
		FROM dbo.odds o WITH (NOLOCK)
			INNER JOIN dbo.match m  WITH (NOLOCK) 
			ON o.matchid 	= m.matchid
			INNER JOIN SportBookBettype bt 
			ON o.bettype 	= bt.bettype
		WHERE m.eventstatus = 'running'
			AND m.kickofftime > CURRENT_TIMESTAMP
			AND o.oddsstatus NOT IN (
				'suspend'
				, 'closed'
				, 'internal'
			)
			AND IsNull(o.liveindicator, 0) = 0
	)
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
		, cs99
		, r.[Timestamp]
		, r.ChangeTime
		, @date AS [VersionDate]
		, r.resourceID
	INTO #OddsDataMix
	FROM OddsData r
		INNER JOIN dbo.team h WITH (NOLOCK) 
			ON r.homeid = h.teamid
		INNER JOIN dbo.team a WITH (NOLOCK) 
			ON r.awayid = a.teamid
		INNER JOIN dbo.league l WITH (NOLOCK) 
			ON r.leagueid = l.leagueid
		INNER JOIN dbo.sports s WITH (NOLOCK) 
			ON l.sporttype = s.sporttype
	WHERE l.istest = 0
	
	-- All odds - bettype	
	SELECT *
	FROM #OddsDataMix r
	ORDER BY r.matchcode
		, r.matchid
		, r.bettype
		, r.oddsid
	
	--extra odds - bettype (152,416)
	;WITH ExtraOdds AS (
		SELECT oddsid
			, bettype
		FROM #OddsDataMix r
		WHERE r.bettype IN (152,416)
	)
	SELECT r.[oddsid]
		,ex.bettype
		,r.[betChoice]
		,r.[odds]
		,r.[OddsInfo]
	FROM dbo.odds_flex r WITH(NOLOCK)
		INNER JOIN ExtraOdds ex ON r.oddsid = ex.oddsid
		
	DROP TABLE #OddsDataMix
	RETURN
END
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_GetDeadballOddsData] TO [vnteam]
GO

GRANT EXECUTE ON [dbo].[Is_Odds_GetDeadballOddsData] TO [bodbAdminNet]
GO


