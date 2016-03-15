
USE [bodb_oddsfeed]
GO
/****** Object:  StoredProcedure [dbo].[Odds_SportSel]    Script Date: 01/23/2013 15:11:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Odds_Partner_LeagueSel]
	@UserName VARCHAR(50)
AS  
  
/**    
 * Created : 20160117@Bobby
 * Task  : Get sport type for partner
 * DB  : DBODDSFEED.bodb_oddsfeed
 *    
 * Revisions: 
 *	- 20160307@Bobby: #53539 - Correct country code when value is all & league keyword & odds type & outright list
 *
 * Param's Explanation:   
 */  
BEGIN  
	SET NOCOUNT ON;

	DECLARE @sporttypes 	VARCHAR(MAX)	
		,@countryCodes 		VARCHAR(MAX)
		,@leagueIds 		VARCHAR(MAX)
		,@systemId			SMALLINT
		,@leagueKeyword		VARCHAR(100)
		,@leagueKeyword_O	VARCHAR(100)
		,@oddsTypes			VARCHAR(50)
		,@outrightIds 		VARCHAR(MAX)
	
	CREATE TABLE #leagueEvent ( leagueid INT, isOutright BIT DEFAULT(0) )
	
	SELECT @leagueIds		= pr.LeagueIDList 
		, @sporttypes 		= pr.SportIDList
		, @countryCodes		= pr.CountryCodeList
		, @systemId			= p.systemId
		, @leagueKeyword	= ISNULL(pr.LeagueKeyword, '')
		, @leagueKeyword_O	= ISNULL(pr.LeagueKeyword_O, '')
		, @oddsTypes		= pr.OddsType
		, @outrightIds		= pr.OutrightIDList
		
	FROM dbo.PartnerProfile pr WITH (NOLOCK)
		INNER JOIN dbo.Partner p WITH (NOLOCK)
		ON pr.partnerid = p.partnerid
	WHERE p.UserName 	= @UserName
	
	IF(@leagueIds = 'All')
	BEGIN
		SELECT @leagueIds 	= LeagueIDList
		FROM dbo.OddsSystemProfile WITH (NOLOCK)
		WHERE SystemID 	= @SystemID
	END
	
	IF(@outrightIds = 'All')
	BEGIN
		SELECT @outrightIds = OutrightIDList
		FROM dbo.OddsSystemProfile WITH (NOLOCK)
		WHERE SystemID 	= @SystemID
	END
	
	-- filter by league keyword
	IF(@leagueKeyword = ''
		OR @leagueKeyword_O = ''
	)
	BEGIN
		
		SELECT @leagueKeyword	= CASE WHEN @leagueKeyword = '' THEN s.LeagueKeyword ELSE @leagueKeyword END
			, @leagueKeyword_O	= CASE WHEN @leagueKeyword_O = '' THEN s.LeagueKeyword_O ELSE @leagueKeyword_O END
		FROM dbo.OddsSystemProfile s WITH (NOLOCK)
		WHERE SystemID 	= @SystemID
			AND (@leagueKeyword = '' OR @leagueKeyword_O = '')
	END	
	
	-- filter by odds type
	IF(ISNULL(@oddsTypes, '') = '')
	BEGIN
		
		SELECT @oddsTypes		= OddsType
		FROM dbo.OddsSystemProfile WITH (NOLOCK)
		WHERE SystemID 	= @SystemID
	END	
	
	IF(@leagueIds = 'All' OR @outrightIds = 'All')
	BEGIN
				
		-- filter by sporttypes
		IF(@sporttypes 	= 'all')
		BEGIN
			SELECT @sporttypes 	= SportIDList
			FROM dbo.OddsSystemProfile WITH (NOLOCK)
			WHERE SystemID 	= @SystemID
			
			IF(@sporttypes = 'all')
			BEGIN
				SELECT @sporttypes = SValue  
				FROM dbo.SystemSetting WITH (NOLOCK)  
				WHERE [Type] = 'SportTypes'
			END
		END
		
		-- filter by countryCodes
		IF(@countryCodes = 'all')
		BEGIN
			SELECT @countryCodes 	= CountryCodeList
			FROM dbo.OddsSystemProfile WITH (NOLOCK)
			WHERE SystemID 	= @SystemID
			
		END
	END
	
	IF( @leagueIds = 'All')	
	BEGIN
		WITH CTE_Country AS (
			SELECT DISTINCT val AS CountryCode
		FROM dbo.strSplit(@countryCodes,',')
		)
		,CTE_Sport AS (
			SELECT DISTINCT val AS sporttype
			FROM dbo.strSplit(@sporttypes,',')
		)
		,CTE_OddsType AS (
			SELECT DISTINCT val AS OddsType
			FROM dbo.strSplit(@oddsTypes,',')
		)
		INSERT INTO #leagueEvent (leagueid)
		SELECT l.leagueid
		FROM bodb02.dbo.match m WITH (NOLOCK) 
			INNER JOIN bodb02.dbo.league l WITH (NOLOCK) 
			ON l.leagueid=m.leagueid
			INNER JOIN CTE_Sport s ON l.sporttype = s.sporttype
		WHERE m.eventstatus != 'completed' 
			AND l.isTest	= 0 
			AND l.IsActive	= 1
			AND ( @countryCodes = 'All' OR l.CountryCode IN  (SELECT CountryCode FROM CTE_Country) )
			AND EXISTS (SELECT 1 FROM CTE_OddsType WHERE OddsType <> 8 )
	END
	ELSE 
	BEGIN
		WITH CTE_OddsType AS (
			SELECT DISTINCT val AS OddsType
			FROM dbo.strSplit(@oddsTypes,',')
		)
		INSERT INTO #leagueEvent (leagueid)
		SELECT DISTINCT val AS leagueid
		FROM dbo.strSplit(@leagueIds,',')
		WHERE EXISTS (SELECT 1 FROM CTE_OddsType WHERE OddsType <> 8 )
	END
	
	IF( @outrightIds = 'All')	
	BEGIN
		WITH CTE_Country AS (
			SELECT DISTINCT val AS CountryCode
		FROM dbo.strSplit(@countryCodes,',')
		)
		,CTE_Sport AS (
			SELECT DISTINCT val AS sporttype
			FROM dbo.strSplit(@sporttypes,',')
		)
		,CTE_OddsType AS (
			SELECT DISTINCT val AS OddsType
			FROM dbo.strSplit(@oddsTypes,',')
		)
		INSERT INTO #leagueEvent (leagueid, isOutright)
		SELECT l.leagueid, 1
		FROM bodb02.dbo.outright o WITH(NOLOCK)
			INNER JOIN bodb02.dbo.league l WITH (NOLOCK) 
			ON l.leagueid=o.leagueid
			INNER JOIN CTE_Sport s ON l.sporttype = s.sporttype
		WHERE o.oddsstatus 	!= 'completed'
			AND l.isTest	= 0 
			AND l.IsActive	= 1
			AND ( @countryCodes = 'All' OR l.CountryCode IN  (SELECT CountryCode FROM CTE_Country) )
			AND EXISTS (SELECT 1 FROM CTE_OddsType WHERE OddsType = 8 )
	END
	ELSE
	BEGIN
		WITH CTE_OddsType AS (
			SELECT DISTINCT val AS OddsType
			FROM dbo.strSplit(@oddsTypes,',')
		)
		INSERT INTO #leagueEvent (leagueid, isOutright)
		SELECT DISTINCT val, 1
		FROM dbo.strSplit(@outrightIds,',')
		WHERE EXISTS (SELECT 1 FROM CTE_OddsType WHERE OddsType = 8 )
	END
	
	-- final result
	SELECT DISTINCT l.leagueid
		, l.leaguename
		, l.leaguenamecn AS leaguenameCh
		, l.[leaguenamec_uni] AS leaguenameCs
		, l.sporttype
		, s.SportName
	FROM bodb02.dbo.league l WITH (NOLOCK)
		INNER JOIN #leagueEvent le ON l.leagueid = le.leagueid
		INNER JOIN bodb02.dbo.Sports s WITH (NOLOCK) ON s.SportType = l.sporttype
	
	WHERE 1 = CASE 
				WHEN ( le.isOutright = 1 AND (@leagueKeyword_O IS NULL OR l.leaguename  LIKE '%' + @leagueKeyword_O + '%')) THEN 1
				WHEN ( le.isOutright = 0 AND (@leagueKeyword IS NULL OR l.leaguename  LIKE '%' + @leagueKeyword + '%')) THEN 1
				ELSE 0 
			END

	ORDER BY l.leaguename
	
	DROP TABLE #leagueEvent

	RETURN
END  

GO
GRANT EXECUTE ON [Odds_Partner_LeagueSel] To [bodbAdminNet]
GO
EXEC [Odds_Partner_LeagueSel] 'aff001'