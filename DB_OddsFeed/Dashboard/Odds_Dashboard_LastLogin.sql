set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go



ALTER PROCEDURE [dbo].[Odds_Dashboard_LastLogin]
	@UserID INT,
	@PageIndex int = 1,  
	@PageCount int = 1 OUTPUT,  
	@RowCount int = 0 OUTPUT
AS  
  
/**    
 * Created : 20130327@Long.Luu
 * Task  : Get top 8 last login of an adminuser
 * DB  : bodb_oddsfeed  
 *    
 * Revisions:       
 * Param's Explanation:   
 * Modify: 20130407@Thai: Order by LoggedDate, remove @RowCount
 */  

BEGIN
	SET NOCOUNT ON
	--select @RowCount = count(1)
	--from userlog with(nolock)
	--where SPName = 'Odds_User_Login' and Creator = @UserID;

	select top 8  (CONVERT(VARCHAR(11),LoggedDate,106) + ' ' + CONVERT(VARCHAR(10),LoggedDate,108)) as LastLoginTime, IP
	from userlog with(nolock)
	where SPName = 'Odds_User_Login' and Creator = @UserID
	order by LoggedDate desc --  LastLoginTime desc
END


