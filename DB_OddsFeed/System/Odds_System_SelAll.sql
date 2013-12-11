USE [bodb_oddsfeed]
GO
/****** Object:  StoredProcedure [dbo].[[Odds_System_Sel]]    Script Date: 01/23/2013 15:11:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Odds_System_SelAll]
AS  
  
/**    
 * Created : 20130306@Thai.Tran
 * Task  : Get all system
 * DB  : bodb_oddsfeed,bodb02  
 *    
 * Revisions:       
 * Param's Explanation:    
 */  
BEGIN  
	SET NOCOUNT ON;
	
	select s.SystemID, s.SystemName
	from System s with (nolock)
	ORDER BY s.SystemName
				
END  

GO
GRANT EXECUTE ON [Odds_System_SelAll] To [bodbAdminNet]