
USE [bodb_oddsfeed]
GO
/****** Object:  StoredProcedure [dbo].[Odds_SportSel]    Script Date: 01/23/2013 15:11:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Odds_Partner_BetTypeSel]
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

	DECLARE @bettypes	NVARCHAR(1000)
		, @systemId		SMALLINT
	
	SELECT @bettypes 	= BetTypeIDList
		, @systemId		= systemId
	FROM dbo.PartnerProfile pr WITH (NOLOCK)
		INNER JOIN dbo.Partner p WITH (NOLOCK)
		ON pr.partnerid = p.partnerid
	WHERE p.UserName 	= @UserName
		
	IF(@bettypes = 'all')
	BEGIN
		SELECT @bettypes 	= BetTypeIDList
		FROM dbo.OddsSystemProfile WITH (NOLOCK)
		WHERE SystemID 	= @SystemID
		
		IF(@bettypes = 'all')
		BEGIN
			SELECT @bettypes = SValue  
			FROM dbo.SystemSetting WITH (NOLOCK)  
			WHERE [Type] = 'bettypes'
		END
	END
	
	-- final result
	SELECT bt.typeid
		, bt.typenamee 
		, bt.typenamecn AS typenameCh
		, bt.typenamec_uni AS typenameCs
		
	FROM bodb02.dbo.bettype bt WITH (NOLOCK)
	WHERE bt.typeid IN (SELECT val FROM dbo.strSplit(@bettypes,','))
	ORDER BY bt.typenamee
					
END  

GO
GRANT EXECUTE ON [Odds_Partner_BetTypeSel] To [bodbAdminNet]
GO
EXEC [dbo].[Odds_Partner_BetTypeSel] 'betdaq'