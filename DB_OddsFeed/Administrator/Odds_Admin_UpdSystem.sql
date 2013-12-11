set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER PROCEDURE [dbo].[Odds_Admin_UpdSystem]	
	@AdminID INT = 0,
	@ListSystemId VARCHAR(1024) = '',--Each SystemID is separated by comma ','
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
 *  Modify:
	- Thai@20131004: just remove system which creator's role is licensee-admin has permission.
*/
SET NOCOUNT ON

IF NOT EXISTS(SELECT 1 From [dbo].AdminUser WITH(NOLOCK) WHERE [UserID] = @AdminID)
BEGIN
	SELECT @ErrCode = 10046, @ErrMsg = 'Error! Cannot update this administrator. It was not existed.'
	RETURN
END

BEGIN TRANSACTION

--
Declare @RoleIdAdmin int
Select @RoleIdAdmin  = roleID from AdminUser (nolock) where UserID = @AdminID

Declare @RoleCreator int
Select @RoleCreator  = roleID from AdminUser (nolock) where UserID = @Creator

-- remove all related rows of this admin before insert new ones
DELETE FROM AdminUserSystem
WHERE [UserID] = @AdminID
	  and
	   (
			@RoleCreator = 1
			or
			-- just remove system which creator's role is licensee-admin has permission.
			(@RoleCreator = 2 and  SystemID in (Select SystemID
												From dbo.AdminUserSystem
												Where UserID = @Creator and IsActive = 1)
			)
		)
		
DELETE FROM AdminUserSystem
WHERE UserID in (select UserID from AdminUser (nolock) where roleid=3 and creator = @AdminID)
	  and @RoleIdAdmin = 2 and SystemID not in (
	  
	  select val From dbo.strSplit(@ListSystemId, ',') 
	  
	  )

if @@Error <> 0
BEGIN			
	SELECT @ErrMsg = 'Error! Remove Admin-System unsuccessfully.', @ErrCode = 10051			
	ROLLBACK TRANSACTION
	RETURN
END

--Insert adminuser-system
INSERT INTO [dbo].[AdminUserSystem]([SystemID], [UserID], [IsActive], [IsHost])
SELECT Val, @AdminID, 1, 1  From dbo.strSplit(@ListSystemId, ',') Where val <> ''

IF @@ERROR <> 0
BEGIN	
	SELECT @ErrMsg = N'Error! Insert admin unsuccessfully. Error when inserting system list.', @ErrCode = 10049
	ROLLBACK TRANSACTION
	RETURN
END

COMMIT TRANSACTION


DECLARE @SPName VARCHAR(50)
SELECT @SPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Update Administrator info, UserID:' + dbo.Is_Odds_ToString(@AdminID) +		
		', System List: ' + dbo.Is_Odds_ToString(@ListSystemId) +
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
GRANT EXECUTE ON [Odds_Admin_UpdSystem] To [bodbAdminNet]