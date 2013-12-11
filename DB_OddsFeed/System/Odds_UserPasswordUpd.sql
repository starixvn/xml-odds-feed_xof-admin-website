USE [bodb_oddsfeed]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Odds_UserPasswordUpd')
	BEGIN
		DROP  PROCEDURE  [dbo].[Odds_UserPasswordUpd]
	END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Odds_UserPasswordUpd]
	@UserID INT = 0,
	@OldPassword varchar(50),
	@Password VARCHAR(50),
	@ConfirmPassword VARCHAR(50),
	@Creator INT = 0,
	@IP VARCHAR(25) = '',
	@ErrCode INT  = 0 OUTPUT,
	@ErrMsg NVARCHAR(200) = '' OUTPUT
AS
/*
 *	Created: 20130314@Phillip
 *	DB: [bodb_oddsfeed]
 *  Description:
 *	Revisions:
*/
SET NOCOUNT ON

IF NOT EXISTS(SELECT 1 From [dbo].AdminUser WITH(NOLOCK) WHERE [UserID] = @UserID)
BEGIN
	SELECT @ErrCode = 10601, @ErrMsg = 'Error! This User was not existed.'
	RETURN
END

DECLARE @OldPasswordDB VARCHAR(50)
SELECT @OldPasswordDB = [Password] FROM [dbo].[AdminUser] with (nolock) WHERE [UserID] = @UserID
IF @OldPassword <> @OldPasswordDB
BEGIN
	SELECT @ErrCode = 10602, @ErrMsg = 'Error! Old password does not match.'
	RETURN
END

IF @Password <> @ConfirmPassword
BEGIN
	SELECT @ErrCode = 10603, @ErrMsg = 'Error! Confirmed password does not match.'
	RETURN
END

BEGIN TRANSACTION

UPDATE [dbo].[AdminUser] WITH(Rowlock, UpdLock)
SET [Password] = ISNULL(@Password, [Password]),
	[IsImport] = 0
WHERE [UserID] = @UserID

IF @@ERROR <> 0
BEGIN	
	SELECT @ErrMsg = N'Error! Update password unsuccessfully.', @ErrCode = 10604
	ROLLBACK TRANSACTION
	RETURN
END

COMMIT TRANSACTION


DECLARE @SPName VARCHAR(50)
SELECT @SPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Update password, UserID:' + dbo.Is_Odds_ToString(@UserID) +		
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

GRANT EXECUTE ON [dbo].[Odds_UserPasswordUpd] TO [bodbAdminNet]
GO
GRANT EXECUTE ON [dbo].[Odds_UserPasswordUpd] TO [vnteam]
GO