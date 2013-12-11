set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go



ALTER PROCEDURE [dbo].[Odds_Admin_Ins]	
	@UserID INT = 0 OUTPUT,
	@Username VARCHAR(50),
	@Password VARCHAR(50),
	@FirstName NVARCHAR(50),
	@LastName NVARCHAR(50),
	@RoleID TINYINT,
	@Email VARCHAR(50),
	@IsImport BIT = 0,
	@ListSystemId VARCHAR(1024),--Each SystemID is separated by comma ','
	@Creator INT,
	@Ip VARCHAR(25) = '',
	@ErrCode INT  = 0 OUTPUT,
	@ErrMsg NVARCHAR(200) = '' OUTPUT
AS
/*
 *	Created: 20130305@LongLuu
 *	Task : Add new Admin into Odds Feed - Licensee system
 *	DB: [bodb_oddsfeed]
 *  Description:
 *	Revisions:		
	
	Param's Explanation:
	- @UserID : OUTPUT admin is inserted
	- @Username : Username of admin
	- @Password : admin's password
	- @FirstName : admin's firstname
	- @LastName : admin's lastname
	- @RoleID : admin's role is: IT admin or Licensee Admin
	- @Email: admin's email
	- @ListSystemId: list id of system (1,2,3,4)
	- @Creator: the admin who is inserting new admin
	- @IP: creator's ip
*/
SET NOCOUNT ON

--Check Partner name
IF EXISTS(SELECT 1 FROM [dbo].[AdminUser] WITH(NOLOCK) WHERE [Username] = @Username)
BEGIN
	SELECT @ErrCode = 10043, @ErrMsg = 'Error! Cannot insert this admin. The username was existed.'
	RETURN
END

BEGIN TRANSACTION

INSERT INTO [dbo].[AdminUser](
	[Username]
	,[Password]
	,[RoleID]
	,[FirstName]
	,[LastName]
	,[Email]
	,[IsImport]
	,[Creator]
	,[CreateDate]
	,[LoginCount]
	,[IsActive]
)
VALUES(
	@Username	
	,@Password
	,@RoleID
	,@FirstName
	,@LastName
	,@Email
	,@IsImport
	,@Creator
	,GETDATE()
	,0
	,1
)

IF @@ERROR <> 0
BEGIN	
	SELECT @ErrMsg = N'Error! Insert admin unsuccessfully.', @ErrCode = 10050
	ROLLBACK TRANSACTION
	RETURN
END

SELECT @UserID = SCOPE_IDENTITY()


--Insert adminuser-system
INSERT INTO [dbo].[AdminUserSystem]([SystemID], [UserID], [IsActive], [IsHost])
SELECT Val, @UserID, 1, 1  From dbo.strSplit(@ListSystemId, ',')

if @@Error <> 0
BEGIN			
	SELECT @ErrMsg = 'Error! Insert admin unsuccessfully. Error when inserting system list.', @ErrCode = 10051			
	ROLLBACK TRANSACTION
	RETURN
END

COMMIT TRANSACTION

--Write the log 
DECLARE @SPName VARCHAR(50)
SELECT @SPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Add News Admin, AdminId:' + dbo.Is_Odds_ToString(@UserID) +
		', Username: ' + dbo.Is_Odds_ToString(@Username) +	
		', Password: ' + dbo.Is_Odds_ToString(@Password) +		
		', FistName: ' + dbo.Is_Odds_ToString(@FirstName) +
		', LastName : ' + dbo.Is_Odds_ToString(@LastName) +
		', RoleID: ' + dbo.Is_Odds_ToString(@RoleID) +
		', Email: ' + dbo.Is_Odds_ToString(@Email) +
		', IsImport: ' + dbo.Is_Odds_ToString(@IsImport) +
		', ListSystemID: ' + dbo.Is_Odds_ToString(@ListSystemId) +	
		', Creator: ' + dbo.Is_Odds_ToString(@Creator) +
		', IP: ' + dbo.Is_Odds_ToString(@IP) +
		', ExcutingSPName ' + dbo.Is_Odds_ToString(@SPName) 
EXEC [dbo].[Odds_WriteLog]
	@Creator=@Creator,	
	@PartnerID = @UserID,
	@TypeName= 'Admin',	
	@SPName=@SPName,
	@IP = @IP,
	@Detail=@LogData
		



