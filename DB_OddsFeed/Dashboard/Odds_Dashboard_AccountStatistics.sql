USE [bodb_oddsfeed]
GO
/****** Object:  StoredProcedure [dbo].[Odds_Dashboard_AccountStatistics]    Script Date: 07/04/2013 13:43:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

  
  
  
ALTER PROCEDURE [dbo].[Odds_Dashboard_AccountStatistics]  
 @SystemId INT  
AS    
    
/**      
 * Created : 20130327@Long.Luu  
 * Task  : Get all recent activities  
 * DB  : bodb_oddsfeed    
 *      
 * Revisions:         
 * Param's Explanation:     
 * Modify: 
	- Thai@20130903:
		+ add "IsDeleted = 0" in case getting totalaff
 */    
  
BEGIN  
 SET NOCOUNT ON  
 SELECT  
 (select count(*)   
    from partner with(nolock)  
    where IsDeleted = 0 and systemid = @SystemId)as totalaff ,  
 (select count(*)   
    from adminuser a with(nolock)   
     INNER JOIN dbo.AdminUser b WITH (NOLOCK) ON a.Creator = b.UserID  
     INNER JOIN dbo.AdminUserSystem c WITH (NOLOCK) ON a.UserID = c.UserID    
    where a.RoleId = 3 and c.IsActive = 1 and c.SystemId = @SystemId)  as totalsubacc ,  
 isnull((select sum(TotalRequest)   
    from partnerstatistics ps with(nolock)   
   left join partner p with(nolock) on ps.partnerid = p.partnerid  
    where p.systemid = @SystemId and datediff(d, date,current_timestamp)=0),0) as totaltodayrequest,  
 isnull((select sum(TotalRequest)   
  from partnerstatistics ps with(nolock)   
   left join partner p with(nolock) on ps.partnerid = p.partnerid  
  where p.systemid = @SystemId and datediff(m, date,current_timestamp)=0),0) as totalmonthrequest,  
 (select top 1 p.username  
  from partnerstatistics ps with(nolock)   
   left join partner p with(nolock) on ps.partnerid = p.partnerid  
  where p.systemid = @SystemId  
  order by LastCallTime desc) as lastrequestusername,  
 (select top 1  CONVERT(VARCHAR(50),ps.lastcalltime,13) 
  from partnerstatistics ps with(nolock)   
   left join partner p with(nolock) on ps.partnerid = p.partnerid  
  where p.systemid = @SystemId  
  order by LastCallTime desc) as lastrequesttime,  
 (select top 1 ps.IP   
  from partnerstatistics ps with(nolock)   
   left join partner p with(nolock) on ps.partnerid = p.partnerid  
  where p.systemid = @SystemId  
  order by LastCallTime desc) as lastrequestip  
END
