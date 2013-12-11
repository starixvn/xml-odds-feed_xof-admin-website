USE [bodb_oddsfeed]
GO
/****** Object:  StoredProcedure [dbo].[Odds_Partner_CheckExist]    Script Date: 01/31/2013 09:13:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Odds_Partner_CheckExist]      
	@SystemId Smallint,
	@UserName as Varchar(50),
	@PageSize int =30,  
	@PageIndex int =1,  
	@PageCount int=0 OUTPUT,  
	@RowCount int =0 OUTPUT  
AS  
  
/**    
 * Created : 20130107@Louis.Nguyen
 * Task  : check existed affiliates by Name
 * DB  : bodb_oddsfeed  
 *    
 * Revisions:       
 * Param's Explanation:   
 * Update: add paging
 */  
BEGIN  
 SET NOCOUNT ON  

SELECT @RowCount =count(1)   
FROM dbo.[Partner] p WITH (NOLOCK)
 LEFT JOIN dbo.AdminUser a WITH (NOLOCK) ON p.Creator = a.UserID
  WHERE --(p.IsDeleted is NULL or  p.IsDeleted = 'False' ) 
	--and 
	LTRIM(RTRIM(p.UserName)) =  @UserName 	

SELECT @PageCount=@RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;  
  
WITH News AS  
( 

 SELECT top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY PartnerId ASC) AS Row
		,PartnerId, p.UserName, Site, CreatedDate, LastUpdate, a.UserName as Creator, ContactPerson, ContactEmail, IsAllowed, p.Remark
 FROM dbo.[Partner] p WITH (NOLOCK)
 LEFT JOIN dbo.AdminUser a WITH (NOLOCK) ON p.Creator = a.UserID
  WHERE --(p.IsDeleted is NULL or  p.IsDeleted = 'False' ) 
	--and 
	LTRIM(RTRIM(p.UserName)) =  @UserName
)

SELECT  [Row]
		,PartnerId, UserName, Site, CreatedDate, LastUpdate, Creator, ContactPerson, ContactEmail, IsAllowed, Remark
FROM News  
WHERE  [Row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize  
ORDER BY PartnerId  

END  

GO
GRANT EXECUTE ON [Odds_Partner_CheckExist] To [bodbAdminNet]