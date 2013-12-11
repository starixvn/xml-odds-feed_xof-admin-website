USE [bodb_oddsfeed]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Odds_System_SelByUserIDAndSubAccountID')
	BEGIN
		DROP  PROCEDURE  [dbo].[Odds_System_SelByUserIDAndSubAccountID]
	END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Odds_System_SelByUserIDAndSubAccountID]
	@UserID INT,
	@SubAccountID INT	
AS  
  
/**    
 * Created : 20131004@Thai.Tran
 * Task  : Get all optional systems which userid login and sub-account belong to
 * DB  : bodb_oddsfeed
 *    
 * Revisions:       
 * Param's Explanation:   
 */  

BEGIN
	Declare @RoleID INT
	Declare @Creator INT
	Select @RoleID = RoleID From dbo.AdminUser Where UserID = @UserID
	Select @Creator = Creator From dbo.AdminUser Where UserID = @SubAccountID
	
	If @RoleID = 1 -- Account Manager
	Begin
		Select distinct s.SystemID, s.SystemName
		From dbo.AdminUserSystem a with (nolock)
		Inner Join dbo.System s with (nolock) On a.SystemID = s.SystemID	
		Where s.IsActive = 1 and a.IsActive = 1
			and a.SystemID in (Select SystemID 
								From dbo.AdminUserSystem 
								Where IsActive = 1 and UserID = @Creator)
		
		ORDER BY s.SystemName  		
	End
	Else If @RoleID = 2 -- Licensee Admin
	Begin
		Select distinct s.SystemID, s.SystemName
		From dbo.AdminUserSystem a with (nolock)
		Inner Join dbo.System s with (nolock) On a.SystemID = s.SystemID	
		Where s.IsActive = 1 and a.IsActive = 1
			and a.SystemID in (Select SystemID 
								From dbo.AdminUserSystem 
								Where IsActive = 1 and UserID = @UserID)
		
		ORDER BY s.SystemName  		
	End
	
END

GO
GRANT EXECUTE ON Odds_System_SelByUserIDAndSubAccountID To [bodbAdminNet]