set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER PROCEDURE [dbo].[Odds_Admin_UpdInfo]	
	@AdminID INT = 0,
	@Username VARCHAR(50) = '',
	@FirstName NVARCHAR(50),
	@LastName NVARCHAR(50),
	@RoleID TINYINT,
	@Email VARCHAR(50),
	@Creator INT = 0,
	@IP VARCHAR(25) = '',
	@ErrCode INT  = 0 OUTPUT,
	@ErrMsg NVARCHAR(200) = '' OUTPUT
AS
/*
 *	Created: 20130307@LongLuu
 *	Task : select Administrator according to filter options Odds Feed - Licensee system
 *	DB: [bodb_oddsfeed]
 *  Description:
 *	Revisions:		
	
	Param's Explanation:
	- @SystemID : system id
	- @Username : Username of admin
 *  Update: 
	@20130625@Thai: Assign all systems for the user if it's role is ITAdmin role (id = 1)
*/
SET NOCOUNT ON

IF NOT EXISTS(SELECT 1 From [dbo].AdminUser WITH(NOLOCK) WHERE [UserID] = @AdminID)
BEGIN
	SELECT @ErrCode = 10046, @ErrMsg = 'Error! Cannot update this administrator. It was not existed.'
	RETURN
END

BEGIN TRANSACTION

UPDATE [dbo].[AdminUser] WITH(Rowlock, UpdLock)
SET [Username] = ISNULL(@Username, [Username]),
	[RoleID] = ISNULL(@RoleID, [RoleID]),
	[FirstName] = ISNULL(@FirstName, [FirstName]),
	[LastName] = ISNULL(@LastName, [LastName]),
	[Email] = ISNULL(@Email, [Email]),
	[ParentID] = NULL
WHERE [UserID] = @AdminID

IF @@ERROR <> 0
BEGIN	
	SELECT @ErrMsg = N'Error! Update active of administrator unsuccessfully.', @ErrCode = 10049
	ROLLBACK TRANSACTION
	RETURN
END

-- Assign all systems for the user if it's role is ITAdmin
IF @RoleID = 1
Begin

	INSERT INTO dbo.AdminUserSystem
	Select SystemID, @AdminID, 1,1  from dbo.System
	Where SystemID not in 
						(
							Select SystemID from dbo.AdminUserSystem
							Where UserID = @AdminID
						)	
End

IF @@ERROR <> 0
BEGIN	
	SELECT @ErrMsg = N'Error! Assign system for user administrator unsuccessfully.', @ErrCode = 10049
	ROLLBACK TRANSACTION
	RETURN
END


COMMIT TRANSACTION


DECLARE @SPName VARCHAR(50)
SELECT @SPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Update Administrator info, UserID:' + dbo.Is_Odds_ToString(@AdminID) +		
		', Username: ' + dbo.Is_Odds_ToString(@Username) +
		', RoleID: ' + dbo.Is_Odds_ToString(@RoleID) +
		', FirstName: ' + dbo.Is_Odds_ToString(@FirstName) +
		', LastName: ' + dbo.Is_Odds_ToString(@LastName) +
		', Email: ' + dbo.Is_Odds_ToString(@Email) +
		', Creator: ' + dbo.Is_Odds_ToString(@Creator) +
		', Ip:' + dbo.Is_Odds_ToString(@Ip)+
		', ExcutingSPName ' + dbo.Is_Odds_ToString(@SPName) 

EXEC [dbo].[Odds_WriteLog]
	@Creator=@Creator,	
	@PartnerID = @AdminID,
	@TypeName= 'Administrator',	
	@SPName=@SPName,
	@IP =@IP,
	@Detail=@LogData

GO
GRANT EXECUTE ON [Odds_Admin_UpdInfo] To [bodbAdminNet]