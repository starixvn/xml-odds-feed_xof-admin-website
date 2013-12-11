
USE [bodb_oddsfeed]
GO
set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[Odds_Top10SubAccount_Sel]
	@SystemId Smallint,
	@RoleId Tinyint,
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

SELECT TOP 10 a.UserName
FROM [dbo].[AdminUser] a WITH (NOLOCK)  
		INNER JOIN dbo.AdminUser b WITH (NOLOCK) ON a.Creator = b.UserID
		INNER JOIN dbo.AdminUserSystem c WITH (NOLOCK) ON a.UserID = c.UserID
	WHERE a.RoleId = @RoleId and c.SystemId = @SystemId and a.Username like '%' + ISNULL(@UserName ,'') + '%'
ORDER BY  a.UserName

GO
GRANT EXECUTE ON [Odds_Top10SubAccount_Sel] To [bodbAdminNet]
