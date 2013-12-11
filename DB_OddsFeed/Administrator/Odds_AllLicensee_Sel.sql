USE [bodb_oddsfeed]
GO
/****** Object:  StoredProcedure [dbo].[Odds_AllLicensee_Sel]    Script Date: 03/07/2013 15:11:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Odds_AllLicensee_Sel]
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
	
	select s.systemid, s.systemname
	from System s with (nolock)
				
END  

GO
GRANT EXECUTE ON [Odds_AllLicensee_Sel] To [bodbAdminNet]