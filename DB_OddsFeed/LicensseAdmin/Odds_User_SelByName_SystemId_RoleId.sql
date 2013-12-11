USE [bodb_oddsfeed]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Odds_User_SelByName_SystemId_RoleId]  
	@SystemId Smallint,
	@RoleId Tinyint,
	@UserName Varchar(50),
	@PageSize int =30,  
	@PageIndex int =1,  
	@PageCount int=0 OUTPUT,  
	@RowCount int =0 OUTPUT  
AS  
  
/**    
 * Created : 20130107@Thai
 * Task  : Get users by name, systemid and roleid
 * DB  : bodb_oddsfeed  
 *    
 * Revisions:       
 * Param's Explanation:   
 */  
BEGIN  
	SET NOCOUNT ON		
	
	SELECT @RowCount =count(1)   
	FROM [dbo].[AdminUser] a WITH (NOLOCK)  
		INNER JOIN dbo.AdminUser b WITH (NOLOCK) ON a.Creator = b.UserID
		INNER JOIN dbo.AdminUserSystem c WITH (NOLOCK) ON a.UserID = c.UserID
	WHERE c.IsActive = 1 and a.RoleId = @RoleId and c.SystemId = @SystemId and a.Username like '%' + ISNULL(@UserName ,'') + '%'

	SELECT @PageCount=@RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;  
	  
	WITH News AS  
	( 
		 SELECT top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY a.UserID ASC) AS Row
			  ,a.UserID
			  ,a.UserName  
			  ,a.IsActive                   
			  ,a.FirstName
			  ,a.LastName
			  ,a.Email		  
			  ,b.Username as Creator
			  ,a.CreateDate as CreatedDate
		FROM [dbo].[AdminUser] a WITH (NOLOCK)  
			INNER JOIN dbo.AdminUser b WITH (NOLOCK) ON a.Creator = b.UserID
			INNER JOIN dbo.AdminUserSystem c WITH (NOLOCK) ON a.UserID = c.UserID
		WHERE  c.IsActive = 1 and a.RoleId = @RoleId and c.SystemId = @SystemId and a.Username like @UserName + '%'
	)

	SELECT  [Row]
			,UserID
			,UserName 
			,IsActive                    
			,FirstName
			,LastName
			,Email		  
			,Creator
			,CreatedDate
	FROM News  
	WHERE  [Row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize  
	ORDER BY UserName, UserID 
	
END  

GO
GRANT EXECUTE ON [Odds_User_SelByName_SystemId_RoleId] To [bodbAdminnet]