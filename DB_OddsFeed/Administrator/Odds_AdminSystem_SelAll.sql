USE [bodb_oddsfeed]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Odds_AdminSystem_SelAll')
	BEGIN
		DROP  PROCEDURE  [dbo].[Odds_AdminSystem_SelAll]
	END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
CREATE PROCEDURE [dbo].[Odds_AdminSystem_SelAll]  
 @UserRole TINYINT,  
 @UserId int 
AS    
    
/**      
 * Created : 20130306@Louis.Nguyen  
 * Task  : Get all system  by user id
 * DB  : bodb_oddsfeed,bodb02    
 *      
 * Revisions:         
 * Param's Explanation:     
 * Modify:
	- Thai@20131003: "s.IsActive = 1 and a.IsActive = 1" if UserId is not Account Manger.
	- Thai@20140520: Remove "@SystemId"
 */    
BEGIN    
 SET NOCOUNT ON;  
   
 select s.SystemID, s.SystemName  
 FROM AdminUserSystem a WITH (NOLOCK)    
   INNER JOIN [System] s WITH (NOLOCK) on a.SystemID = s.SystemID      
 WHERE a.UserID = @UserId
   and ((@UserRole = 1)  or  (@UserRole <> 1 and s.IsActive = 1 and a.IsActive = 1))  
 ORDER BY s.SystemName ASC    
      
END    

GO
GRANT EXECUTE ON [dbo].[Odds_AdminSystem_SelAll] TO [bodbAdminNet]
GO
GRANT EXECUTE ON [dbo].[Odds_AdminSystem_SelAll] TO [vnteam]
GO