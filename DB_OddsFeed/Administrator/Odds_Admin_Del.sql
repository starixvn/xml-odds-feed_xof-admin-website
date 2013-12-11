set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER PROCEDURE [dbo].[Odds_Admin_Del]	
	@AdminID INT = 0,
	@Isactive BIT = 0,
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

IF NOT EXISTS(SELECT 1 From [dbo].AdminUser WITH(NOLOCK) WHERE [UserID] = @AdminID)
BEGIN
	SELECT @ErrCode = 10046, @ErrMsg = 'Error! Cannot update this administrator. It was not existed.'
	RETURN
END

BEGIN TRANSACTION

UPDATE [dbo].[AdminUser] WITH(Rowlock, UpdLock)
SET 
	[IsActive] = ISNULL(@Isactive, [IsActive]),	
	LoginCount = 0,
	LastLogin = getdate()
WHERE [UserID] = @AdminID

IF @@ERROR <> 0
BEGIN	
	SELECT @ErrMsg = N'Error! Update active of administrator unsuccessfully.', @ErrCode = 10049
	ROLLBACK TRANSACTION
	RETURN
END

COMMIT TRANSACTION


DECLARE @SPName VARCHAR(50)
SELECT @SPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Update Administrator active info, UserID:' + dbo.Is_Odds_ToString(@AdminID) +		
		', IsAllow: ' + dbo.Is_Odds_ToString(@Isactive) +
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
GRANT EXECUTE ON [Odds_Admin_Del] To [bodbAdminNet]