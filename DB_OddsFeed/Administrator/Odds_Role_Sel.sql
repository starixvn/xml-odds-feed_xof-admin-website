USE [bodb_oddsfeed]
GO
/****** Object:  StoredProcedure [dbo].[[Odds_System_Sel]]    Script Date: 01/23/2013 15:11:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Odds_Role_Sel]
AS  
  
/**    
 * Created : 20130306@Long.Luu
 * Task  : Get all system
 * DB  : bodb_oddsfeed,bodb02  
 *    
 * Revisions:       
 * Param's Explanation:   
 */  
BEGIN  
	SET NOCOUNT ON;
	
	select u.RoleID, u.RoleName
		from UserRole u with (nolock)
				
END  

GO
GRANT EXECUTE ON [Odds_Role_Sel] To [bodbAdminNet]