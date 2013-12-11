
USE [bodb_oddsfeed]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].Odds_Top10Domain_Sel	
	@SystemId Smallint,
	@DomainName VARCHAR(300)
AS  
  
/**    
 * Created : 20130318@Thai
 * Task  : Get existed domains
 * DB  : bodb_oddsfeed 
 *    
 * Revisions:       
 * Param's Explanation:   
 */  
SET NOCOUNT ON;

SELECT TOP 10 d.DomainID, d.DomainName
FROM dbo.Domain d with(nolock)	
WHERE d.DomainName like '%' + @DomainName + '%'
AND EXISTS (
	SELECT DomainID 
	FROM SystemDomain
	WHERE SystemID = @SystemId AND DomainID = d.DomainID
)
ORDER BY d.DomainName

GO
GRANT EXECUTE ON Odds_Top10Domain_Sel To [bodbAdminNet]
