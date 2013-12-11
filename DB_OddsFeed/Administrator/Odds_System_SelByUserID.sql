set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[Odds_System_SelByUserID]
	@UserID INT,
	@IgnoreIsActive Bit = 1
AS  
  
/**    
 * Created : 20131002@Thai.Tran
 * Task  : Get System by UserId
 * DB  : bodb_oddsfeed
 *    
 * Revisions:       
 * Param's Explanation:   
 */  

BEGIN
	If @IgnoreIsActive = 1 
	Begin
		Select s.SystemID, s.SystemName
		From dbo.AdminUserSystem a with (nolock)
			Inner Join dbo.System s with (nolock)
			On a.SystemID = s.SystemID
		Where s.IsActive = 1 and a.UserID = @UserID
		ORDER BY s.SystemName  
	End
	Else	
	Begin
		Select s.SystemID, s.SystemName
		From dbo.AdminUserSystem a with (nolock)
			Inner Join dbo.System s with (nolock)
			On a.SystemID = s.SystemID
		Where s.IsActive = 1 and a.IsActive =1 and a.UserID = @UserID
		ORDER BY s.SystemName  
	End
	
END

GO
GRANT EXECUTE ON Odds_System_SelByUserID To [bodbAdminNet]