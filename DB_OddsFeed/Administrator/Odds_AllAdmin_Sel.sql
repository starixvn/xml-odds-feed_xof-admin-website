USE [bodb_oddsfeed]
GO
/****** Object:  StoredProcedure [dbo].[Odds_SportSel]    Script Date: 01/23/2013 15:11:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Odds_AllAdmin_Sel]
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
	
	select a.username, a.roleid, a.firstname, a.lastname, a.email, c.username, a.createdate
	from AdminUser a with (nolock)
		inner join adminuser c with(nolock) on a.creator = c.userid
				
END  

GO
GRANT EXECUTE ON [Odds_AllAdmin_Sel] To [bodbAdminNet]