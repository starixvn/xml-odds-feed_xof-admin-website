USE [bodb_oddsfeed]
GO
/****** Object:  StoredProcedure [dbo].[[Odds_System_Sel]]    Script Date: 01/23/2013 15:11:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Odds_System_Sel]
AS  
  
/**    
 * Created : 20130306@Long.Luu
 * Task  : Get all system
 * DB  : bodb_oddsfeed,bodb02  
 *    
 * Revisions:       
 * Param's Explanation:   
 * Modify: 20130407@Thai: - add "where s.IsActive  = 1"
 */  
BEGIN  
	SET NOCOUNT ON;
	
	select s.SystemID, s.SystemName
	from System s with (nolock)
	where s.IsActive  = 1
	ORDER BY s.SystemName
				
END  

GO
GRANT EXECUTE ON [Odds_System_Sel] To [bodbAdminNet]