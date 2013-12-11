USE [bodb_oddsfeed]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[Odds_Top10Partner_Sel]
	@SystemId Smallint,
	@Username VARCHAR(50)
AS  
  
/**    
 * Created : 20130318@Thai
 * Task  : Get existed affiliate
 * DB  : bodb_oddsfeed 
 *    
 * Revisions:       
 * Param's Explanation:   
 */  
SET NOCOUNT ON;

SELECT TOP 10 PartnerID, UserName
FROM dbo.Partner with(nolock)	
WHERE SystemID = @SystemId and username like '%' + @Username + '%'
ORDER BY UserName

GO
GRANT EXECUTE ON [Odds_Top10Partner_Sel] To [bodbAdminNet]
