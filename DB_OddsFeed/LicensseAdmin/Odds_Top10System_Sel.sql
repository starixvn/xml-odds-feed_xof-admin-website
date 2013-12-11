USE [bodb_oddsfeed]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].Odds_Top10System_Sel	
	@Systemname VARCHAR(50)
AS  
  
/**    
 * Created : 20130318@Thai
 * Task  : Get existed systems
 * DB  : bodb_oddsfeed 
 *    
 * Revisions:       
 * Param's Explanation:   
 */  
SET NOCOUNT ON;

SELECT TOP 10 SystemID, SystemName
FROM dbo.System with(nolock)	
WHERE SystemName like '%' + @Systemname + '%'
ORDER BY SystemName
GO
GRANT EXECUTE ON Odds_Top10System_Sel To [bodbAdminNet]
