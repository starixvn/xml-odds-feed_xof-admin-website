USE [bodb_oddsfeed]
GO
/****** Object:  StoredProcedure [dbo].[Odds_User_EditPermission]    Script Date: 02/18/2013 13:47:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Odds_User_EditPermission]
	@UserID INT,	
	@OddsTypeFlag BIT,
	@OddsStyleFlag BIT,
	@LanguageListFlag BIT,
	@SportIDListFlag BIT,
	@CountryCodeListFlag BIT,
	@BetTypeIDListFlag BIT,
	@UpdatedBy INT = 0,
	@IP VARCHAR(25) = '',	
	@ErrCode INT = 0 OUTPUT,
	@ErrMsg NVARCHAR(4000) = '' OUTPUT
	
AS
/*
 *	Created: 20130128@Thai
 *	Task : Update user's permission
 *	DB: [bodb_oddsfeed]
 *  Description:
 *	Revisions:		
	
*/
SET NOCOUNT ON

DECLARE @RoleID TINYINT
DECLARE @IsActive BIT

--Check user name
IF NOT EXISTS(SELECT 1 FROM dbo.AdminUser WITH(NOLOCK) WHERE [UserID] = @UserID)
BEGIN
	SELECT @ErrCode = 101, @ErrMsg = 'This user was not existed.'
	RETURN
END

SELECT @RoleID = RoleID, @IsActive=IsActive FROM AdminUser WITH (NOLOCK) WHERE UserID = @UserID

--IF (@IsActive IS NULL or @IsActive <> 'True')
--BEGIN
--	SELECT @ErrMsg = 'This user is not active.', @ErrCode=102         
--END
--ELSE
If @RoleID <> 3
BEGIN
	SELECT @ErrMsg = 'This user is not a sub-account.', @ErrCode=103     		
END
ELSE
BEGIN			
	--Set permission flags
	IF EXISTS(SELECT 1 FROM dbo.UserPermission WITH(NOLOCK) WHERE [UserID] = @UserID)
	Begin
		UPDATE [dbo].[UserPermission]
		SET 
		   [UpdatedBy] = @UpdatedBy
		  ,[LastUpdate] = getdate()
		  ,[OddsTypeFlag] = @OddsTypeFlag
		  ,[LanguageListFlag] = @LanguageListFlag
		  ,[OddsRegionFlag] = @OddsStyleFlag
		  ,[SportIDListFlag] = @SportIDListFlag
		  ,[BetTypeIDListFlag] = @BetTypeIDListFlag
		  ,[CountryCodeListFlag] = @CountryCodeListFlag      
		WHERE [UserID] = @UserID
	End
	Else
	Begin
		-- insert flags into permission table
		INSERT INTO [dbo].[UserPermission]
		   ([UserID]
           ,[UpdatedBy]
           ,[LastUpdate]
           ,[OddsTypeFlag]
           ,[LanguageListFlag]
           ,[OddsRegionFlag]
           ,[SportIDListFlag]
           ,[BetTypeIDListFlag]
           --,[LeagueIDListFlag]
           ,[CountryCodeListFlag]
           --,[LeagueKeywordFlag]
           --,[LeagueKeyword_OFlag]
           --,[DelayTimesFlag]
           --,[AddEditAffiliateFlag]
           --,[DeleteAffiliate]
           )
		VALUES
           (@UserID
           ,@UpdatedBy
           ,getdate()
           ,@OddsTypeFlag
           ,@LanguageListFlag
           ,@OddsStyleFlag
           ,@SportIDListFlag
           ,@BetTypeIDListFlag
           --,<LeagueIDListFlag, bit,>
           ,@CountryCodeListFlag
           --,<LeagueKeywordFlag, bit,>
           --,<LeagueKeyword_OFlag, bit,>
           --,<DelayTimesFlag, bit,>
           --,<AddEditAffiliateFlag, bit,>
           --,<DeleteAffiliate, bit,>
           )
	End
END

--Write the log 
DECLARE @SPName VARCHAR(50)
SELECT @SPName = OBJECT_NAME(@@ProcID) 


DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Update User Permission, UserId:' + dbo.Is_Odds_ToString(@UserID) +				
		', Updated By:' + dbo.Is_Odds_ToString(@UpdatedBy)+
		', Ip:' + dbo.Is_Odds_ToString(@Ip)+
		', ExcutingSPName ' + dbo.Is_Odds_ToString(@SPName)
		
EXEC Odds_WriteLog
		 @Creator = @UpdatedBy,
		 @PartnerId = @UserID,
		 @Typename = 'User',
		 @SPName = @SPName, 
		 @IP = @IP,
		 @Detail = @LogData
	
GO
GRANT EXECUTE ON [Odds_User_EditPermission] To [bodbAdminnet]