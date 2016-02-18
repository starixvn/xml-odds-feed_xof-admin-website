
USE [bodb_oddsfeed]
GO
/****** Object:  StoredProcedure [dbo].[Odds_SportSel]    Script Date: 01/23/2013 15:11:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Odds_Partner_SportTypeSel]
	@UserName VARCHAR(50)
AS  
  
/**    
 * Created : 20160117@Bobby
 * Task  : Get sport type for partner
 * DB  : DBODDSFEED.bodb_oddsfeed
 *    
 * Revisions:       
 * Param's Explanation:   
 */  
BEGIN  
	SET NOCOUNT ON;

	DECLARE @sporttypes NVARCHAR(1000)
		, @systemId		SMALLINT
	
	SELECT @sporttypes 	= SportIDList
		, @systemId		= systemId
	FROM dbo.PartnerProfile pr WITH (NOLOCK)
		INNER JOIN dbo.Partner p WITH (NOLOCK)
		ON pr.partnerid = p.partnerid
	WHERE p.UserName 	= @UserName
		
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
	
	-- final result
	SELECT s.SportType
		, s.SportName
		, s.SportNameCh
		, s.SportNameCs
	FROM bodb02.dbo.Sports s WITH (NOLOCK) 
	WHERE s.SportType IN (SELECT val FROM dbo.strSplit(@sporttypes,','))
	ORDER BY s.SportName
					
END  

GO
GRANT EXECUTE ON [Odds_Partner_SportTypeSel] To [bodbAdminNet]
GO
EXEC [dbo].[Odds_Partner_SportTypeSel] 'betdaq'