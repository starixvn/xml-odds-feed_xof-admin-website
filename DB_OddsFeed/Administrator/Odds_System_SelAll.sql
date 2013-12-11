USE [bodb_oddsfeed]
GO
/****** Object:  StoredProcedure [dbo].[[Odds_System_SelAll]]    Script Date: 01/23/2013 15:11:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Odds_System_SelAll]
 @UserRole TINYINT,
 @UserId int,
 @SystemId int
AS  
  
/**    
 * Created : 20130306@Louis.Nguyen
 * Task  : Get all system
 * DB  : bodb_oddsfeed,bodb02  
 *    
 * Revisions:       
 * Param's Explanation:   
 */  
BEGIN  
	SET NOCOUNT ON;
	
	select s.SystemID, s.SystemName
	FROM AdminUserSystem a WITH (NOLOCK)  
	  INNER JOIN [System] s WITH (NOLOCK) on a.SystemID = s.SystemID    
	WHERE a.UserID = @UserId and a.IsActive = 1  
		 and ((@UserRole = 1)  or  (@UserRole <> 1 and s.IsActive = 1 ))
	ORDER BY s.SystemName ASC  
				
END  

GO
GRANT EXECUTE ON [Odds_System_SelAll] To [bodbAdminNet]