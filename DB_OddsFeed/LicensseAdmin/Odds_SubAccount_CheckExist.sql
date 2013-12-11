USE [bodb_oddsfeed]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Odds_SubAccount_CheckExist]  	
	@RoleId Tinyint,
	@UserName Varchar(50),
	@PageSize int =30,  
	@PageIndex int =1,  
	@PageCount int=0 OUTPUT,  
	@RowCount int =0 OUTPUT  
AS  
  
/**    
 * Created : 20130107@Louis.Nguyen
 * Task  : check existed users by name, systemid and roleid
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
		--INNER JOIN dbo.AdminUserSystem c WITH (NOLOCK) ON a.UserID = c.UserID
	WHERE a.RoleId = @RoleId and a.Username like @UserName

	SELECT @PageCount=@RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;  
	  
	WITH News AS  
	( 
		 SELECT top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY a.UserID ASC) AS Row
			  ,a.UserID
			  ,a.UserName                  
			  ,a.FirstName
			  ,a.LastName
			  ,a.Email		  
			  ,b.Username as Creator
			  ,a.CreateDate as CreatedDate
			  ,a.IsActive   
		FROM [dbo].[AdminUser] a WITH (NOLOCK)  
			INNER JOIN dbo.AdminUser b WITH (NOLOCK) ON a.Creator = b.UserID
			--INNER JOIN dbo.AdminUserSystem c WITH (NOLOCK) ON a.UserID = c.UserID
		WHERE a.RoleId = @RoleId and a.Username like @UserName
	)

	SELECT  [Row]
			,UserID
			,UserName                  
			,FirstName
			,LastName
			,Email		  
			,Creator
			,CreatedDate
			,IsActive   
	FROM News  
	WHERE  [Row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize  
	ORDER BY UserID 
	
END  

GO
GRANT EXECUTE ON [Odds_SubAccount_CheckExist] To [bodbAdminnet]