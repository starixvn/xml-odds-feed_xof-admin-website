set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[Odds_Admin_UpdPwd]	
	@UserID INT = 0,
	@Password VARCHAR(50),
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
*/
SET NOCOUNT ON

IF NOT EXISTS(SELECT 1 From [dbo].AdminUser WITH(NOLOCK) WHERE [UserID] = @UserID)
BEGIN
	SELECT @ErrCode = 10046, @ErrMsg = 'Error! Cannot reset password for this administrator. It was not existed.'
	RETURN
END

BEGIN TRANSACTION

UPDATE [dbo].[AdminUser] WITH(Rowlock, UpdLock)
SET [Password] = ISNULL(@Password, [Password]),
	[IsImport] = 0
WHERE [UserID] = @UserID

IF @@ERROR <> 0
BEGIN	
	SELECT @ErrMsg = N'Error! Reset password of administrator unsuccessfully.', @ErrCode = 10049
	ROLLBACK TRANSACTION
	RETURN
END

COMMIT TRANSACTION


DECLARE @SPName VARCHAR(50)
SELECT @SPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Reset Administrator password, UserID:' + dbo.Is_Odds_ToString(@UserID) +		
		', Password: ' + dbo.Is_Odds_ToString(@Password) +
		', IsImport: false' +
		', Creator: ' + dbo.Is_Odds_ToString(@Creator) +
		', Ip:' + dbo.Is_Odds_ToString(@Ip)+
		', ExcutingSPName ' + dbo.Is_Odds_ToString(@SPName) 

EXEC [dbo].[Odds_WriteLog]
	@Creator=@Creator,	
	@PartnerID = @UserID,
	@TypeName= 'Administrator',	
	@SPName=@SPName,
	@IP =@IP,
	@Detail=@LogData

GO
GRANT EXECUTE ON [Odds_Top10Admin_Sel] To [bodbAdminNet]
