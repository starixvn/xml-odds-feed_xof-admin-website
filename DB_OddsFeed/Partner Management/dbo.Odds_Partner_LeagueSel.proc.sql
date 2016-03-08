
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
 *	- 20160307@Bobby: #53539 - Correct country code when value is all & league keyword
 *
 * Param's Explanation:   
 */  
BEGIN  
	SET NOCOUNT ON;

	DECLARE @sporttypes 	VARCHAR(MAX)	
		,@countryCodes 		VARCHAR(MAX)
		,@systemId			SMALLINT
		,@leagueKeyword		VARCHAR(100)
		,@leagueKeyword_O	VARCHAR(100)
	
	SELECT @sporttypes 		= SportIDList
		, @countryCodes		= CountryCodeList
		, @systemId			= systemId
		, @leagueKeyword	= LeagueKeyword
		, @leagueKeyword_O	= LeagueKeyword_O
	FROM dbo.PartnerProfile pr WITH (NOLOCK)
		INNER JOIN dbo.Partner p WITH (NOLOCK)
		ON pr.partnerid = p.partnerid
	WHERE p.UserName 	= @UserName
	
	-- filter by league keyword
	IF(@leagueKeyword IS NULL 
		OR @leagueKeyword_O IS NULL
	)
	BEGIN
		
		SELECT @leagueKeyword	= ISNULL(@leagueKeyword, LeagueKeyword)
			, @leagueKeyword_O	= ISNULL(@leagueKeyword_O, LeagueKeyword_O)
		FROM dbo.OddsSystemProfile WITH (NOLOCK)
		WHERE SystemID 	= @SystemID
			AND (@leagueKeyword IS NULL OR @leagueKeyword_O IS NULL)
	END	
	
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
	
	-- final result
	;WITH CTE_League AS
	(
		SELECT l.leagueid
		FROM bodb02.dbo.match m WITH (NOLOCK) 
			INNER JOIN bodb02.dbo.league l WITH (NOLOCK) 
			ON l.leagueid=m.leagueid
		WHERE m.eventstatus != 'completed' 
			AND l.isTest	= 0 
			AND l.IsActive	= 1
			AND m.sporttype IN (SELECT val FROM dbo.strSplit(@sporttypes,',')) 
			AND ( @countryCodes = 'All' OR l.CountryCode IN  (SELECT val FROM dbo.strSplit(@countryCodes,',')) )
			AND ( @leagueKeyword IS NULL OR l.leaguename  LIKE '%' + @leagueKeyword + '%' )
				
        UNION ALL
		SELECT l.leagueid
		FROM bodb02.dbo.outright o WITH(NOLOCK)
			INNER JOIN bodb02.dbo.league l WITH (NOLOCK) 
			ON l.leagueid=o.leagueid
		WHERE o.oddsstatus 	!= 'completed'
			AND l.isTest	= 0 
			AND l.IsActive	= 1
			AND l.sporttype IN (SELECT val FROM dbo.strSplit(@sporttypes,',') )
			AND ( @countryCodes = 'All' OR l.CountryCode IN  (SELECT val FROM dbo.strSplit(@countryCodes,',')) )
			AND ( @leagueKeyword_O IS NULL OR l.leaguename  LIKE '%' + @leagueKeyword_O + '%' )
		
		
	) 
	SELECT DISTINCT l.leagueid
		, l.leaguename
		, l.leaguenamecn AS leaguenameCh
		, l.[leaguenamec_uni] AS leaguenameCs
		, l.sporttype
		, s.SportName
	FROM bodb02.dbo.league l WITH (NOLOCK)
		INNER JOIN CTE_League cl ON l.leagueid = cl.leagueid
		INNER JOIN bodb02.dbo.Sports s WITH (NOLOCK) ON s.SportType = l.sporttype
	
	ORDER BY l.leaguename
	

	RETURN
END  

GO
GRANT EXECUTE ON [Odds_Partner_LeagueSel] To [bodbAdminNet]
GO
EXEC [Odds_Partner_LeagueSel] 'betdaq'