set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go
  
  
ALTER PROCEDURE [dbo].[Odds_Top10Admin_Sel]  
 @Username VARCHAR(50)  
AS    
    
/**      
 * Created : 20130312@Long  
 * Task  : Get existed administrator  
 * DB  : bodb_oddsfeed,bodb02, bodb_Licensee   
 *      
 * Revisions:         
 * Param's Explanation:     
 */    
SET NOCOUNT ON;  
  
SELECT TOP 10 RES.username  
FROM (  
 SELECT c.username   
 FROM bodb02.dbo.customer c with(nolock)  
 WHERE c.roleid in (6,10) and c.username like '%' + @Username + '%'  
 UNION  
 SELECT u.username  
 FROM bodb_Licensee.dbo.Users u with(nolock)  
 WHERE u.userrole='Licensee' and u.username like '%' + @Username + '%'  
) as RES  
WHERE RES.username NOT IN  
 (SELECT username  
 from AdminUser with(nolock))  
      
  
GO
GRANT EXECUTE ON [Odds_Top10Admin_Sel] To [bodbAdminNet]
