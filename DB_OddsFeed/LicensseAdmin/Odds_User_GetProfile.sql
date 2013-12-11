USE [bodb_oddsfeed]
GO
/****** Object:  StoredProcedure [dbo].[Odds_User_GetProfile]    Script Date: 02/06/2013 09:43:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Odds_User_GetProfile]
	@UserID INT,
	@SystemId SMALLINT,		
	@IP VARCHAR(25) = '',
	@NetSessionID NVARCHAR(30),		
	@OnUserId INT OUTPUT,
	@ErrCode INT = 0 OUTPUT,
	@ErrMsg NVARCHAR(4000) = '' OUTPUT
	
AS
/*
 *	Created: 20130128@Thai
 *	Task : Get user's profile
 *	DB: [bodb_oddsfeed],[bodb_licensee],[bodb02]
 *  Description:
 *	Revisions:		
	
*/
SET NOCOUNT ON

DECLARE @NumberOfIPs SMALLINT
DECLARE @OddsTypeFlag BIT
DECLARE @OddsStyleFlag BIT
DECLARE @LanguageListFlag BIT
DECLARE @SportIDListFlag BIT
DECLARE @CountryCodeListFlag BIT
DECLARE @BetTypeIDListFlag BIT

DECLARE @RoleID TINYINT
DECLARE @IsActive BIT
DECLARE @LogData NVARCHAR(500)


SELECT @RoleID = RoleID, @IsActive=IsActive FROM AdminUser WITH (NOLOCK) WHERE UserID = @UserID

IF (@IsActive IS NULL or @IsActive <> 'True')
BEGIN
	SELECT @ErrMsg = 'This user is not active.', @ErrCode=102     
    SET @LogData = 'UserId: ' + @UserID + ' - ' + 'Failed(User_Not_Active)'   
END
ELSE
BEGIN	
	
	UPDATE On_User	WITH (UPDLOCK)
	SET IsKickedOut = 1 
	WHERE UserID = @UserID and SystemID = @SystemId
	
	INSERT INTO On_User
	Values (@UserID, getdate(), @SystemId, @IP, 0, @NetSessionID)			
	
	SET  @OnUserId = SCOPE_IDENTITY()
	
	UPDATE AdminUser WITH (UPDLOCK)
	SET LastLogin = getdate()
	WHERE UserId = @UserId
	
	-- Get permission flags
	If @RoleID = 1 or @RoleID = 2 -- IT Admin or Licensee
	Begin
		Select  @NumberOfIPs = NumberOfIPs 
				,@OddsTypeFlag = OddsTypeFlag
				,@OddsStyleFlag = OddsRegionFlag
				,@LanguageListFlag = LanguageListFlag
				,@SportIDListFlag = SportIDListFlag 
				,@CountryCodeListFlag = CountryCodeListFlag
				,@BetTypeIDListFlag = BetTypeIDListFlag
		from dbo.OddsSystemProfile with(nolock) where SystemID = @SystemId	
	End
	Else If @RoleID = 3 -- licensee sub-acc
	Begin
		Select  @NumberOfIPs = NumberOfIPs 		
		from dbo.OddsSystemProfile with(nolock) where SystemID = @SystemId	
		
		Select  @OddsTypeFlag = OddsTypeFlag
				,@OddsStyleFlag = OddsRegionFlag
				,@LanguageListFlag = LanguageListFlag
				,@SportIDListFlag = SportIDListFlag 
				,@CountryCodeListFlag = CountryCodeListFlag
				,@BetTypeIDListFlag = BetTypeIDListFlag
		from dbo.UserPermission with(nolock) where UserID = @UserID
		
		Select  @OddsTypeFlag = @OddsTypeFlag & OddsTypeFlag
				,@OddsStyleFlag =  @OddsStyleFlag & OddsRegionFlag
				,@LanguageListFlag = @LanguageListFlag & LanguageListFlag
				,@SportIDListFlag = @SportIDListFlag & SportIDListFlag 
				,@CountryCodeListFlag = @CountryCodeListFlag & CountryCodeListFlag
				,@BetTypeIDListFlag = @BetTypeIDListFlag & BetTypeIDListFlag
		from dbo.[OddsSystemProfile] with(nolock) where SystemID = @SystemId 						
	End	
END

SELECT  @NumberOfIPs as NumberOfIPs 
		,@OddsTypeFlag as OddsTypeFlag
		,@OddsStyleFlag as OddsStyleFlag
		,@LanguageListFlag as LanguageListFlag
		,@SportIDListFlag as SportIDListFlag 
		,@CountryCodeListFlag as CountryCodeListFlag
		,@BetTypeIDListFlag as BetTypeIDListFlag

--Write the log 
DECLARE @ExecutingSPName VARCHAR(50)
SELECT @ExecutingSPName = OBJECT_NAME(@@ProcID) 

SET @UserID = ISNULL(@UserID,0)

EXEC Odds_WriteLog
		 @Creator = @UserID,
		 @PartnerId = @UserID,
		 @Typename = 'Statistics',
		 @SPName = @ExecutingSPName, 
		 @IP = @IP,
		 @Detail = @LogData
	

GO
GRANT EXECUTE ON [Odds_User_GetProfile] To [bodbAdminNet]
