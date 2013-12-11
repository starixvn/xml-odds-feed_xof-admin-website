IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_ProfileIns')
	BEGIN
		DROP  PROCEDURE  [dbo].[Is_Odds_ProfileIns]
	END

GO

CREATE PROCEDURE [dbo].[Is_Odds_ProfileIns]
	@ProfileID SMALLINT = 0 Output,
	@ProfileName VARCHAR(50),
	@Description NVARCHAR(100) = '',
	@Creator INT = 0,
	@TimeRange SMALLINT,
	@SPName VARCHAR(100),
	@SPParams VARCHAR(100),
	@OddsType varchar(50),
	@LanguageList varchar(50),
	@OddsRegion tinyint,
	@SportIDList varchar(MAX),	
	@BettypeIDList varchar(MAX),
	@LeagueIDList varchar(MAX),
	@CountryCodeList varchar(MAX)=null,
	@LeagueKeyword varchar(100)=null,
	@LeagueKeyword_O varchar(100)=null,
	@IP VARCHAR(25) = '',
	@ErrCode int = 0 Output,
	@ErrMsg NVARCHAR(200) = '' output
AS
/*
Created: 20100322@Jacob
Task : Select list system
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	- @SystemID : get all if @SystemID isnull
*/
SET NOCOUNT ON;
--Check System name
IF EXISTS(SELECT 1 FROM [dbo].[Odds_OddsProfile] WITH(NOLOCK) WHERE [ProfileName] = @ProfileName)
BEGIN
	SELECT @ErrCode = 10121, @ErrMsg = 'Error! Cannot insert this profile The profile name was existed.'
	RETURN
END


INSERT INTO [dbo].[Odds_OddsProfile](	
	[ProfileName]
	,[Description]
	,[CreatedDate]
	,[Creator]
	,[IsDeleted]
	,[LastUpdate]
	,[TimeRange]
	,[SPName]
	,[SPParams]
	,[OddsType]
    ,[LanguageList]
    ,[OddsRegion]
    ,[SportIDList]
    ,[BettypeIDList]
    ,[LeagueIDList]
    ,[CountryCodeList]
    ,[LeagueKeyword]
    ,[LeagueKeyword_O]
)
VALUES(
	@ProfileName
	,@Description
	,GETDATE()
	,@Creator
	,0
	,GETDATE()
	,@TimeRange
	,@SPName
	,@SPParams
	,@OddsType
    ,@LanguageList
    ,@OddsRegion
    ,@SportIDList
    ,@BettypeIDList
    ,@LeagueIDList
    ,@CountryCodeList
    ,@LeagueKeyword
    ,@LeagueKeyword_O
)

IF @@ERROR <> 0
BEGIN	
	SELECT @ErrMsg = N'Error! Insert profile unsuccessfully.', @ErrCode = 10122
	RETURN
END

SELECT @ProfileID = SCOPE_IDENTITY()


--Write the log 
DECLARE @ExecutingSPName VARCHAR(50)
SELECT @ExecutingSPName = OBJECT_NAME(@@ProcID) 


DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Add new Profile, ProfileId:' + dbo.Is_Odds_ToString(@ProfileID) +
		', ProfileName ' + dbo.Is_Odds_ToString(@ProfileName) +		
		', Description ' + dbo.Is_Odds_ToString(@Description) +	
		', TimeRange ' + dbo.Is_Odds_ToString(@TimeRange) +	
		', SPName ' + dbo.Is_Odds_ToString(@SPName) +	
		', SPParams ' + dbo.Is_Odds_ToString(@SPParams) +	
		', OddsType ' + dbo.Is_Odds_ToString(@OddsType) +	
		', LanguageList ' + dbo.Is_Odds_ToString(@LanguageList) +	
		', OddsRegion ' + dbo.Is_Odds_ToString(@OddsRegion) +	
		', SportIDList ' + isnull(@SportIDList,'') +			
		', BettypeIDList ' + isnull(@BettypeIDList,'') +			
		', LeagueIDList ' + isnull(@LeagueIDList,'')  +	
		', CountryCodeList ' + isnull(@CountryCodeList,'')  +			
		', LeagueKeyword ' + dbo.Is_Odds_ToString(@LeagueKeyword) +			
		', LeagueKeyword_O ' + dbo.Is_Odds_ToString(@LeagueKeyword_O) +			
		', ExcutingSPName ' + dbo.Is_Odds_ToString(@ExecutingSPName) +	
		', Creator: ' + dbo.Is_Odds_ToString(@Creator) +			
		', Ip: ' + @Ip
		

EXEC [Is_Odds_WriteLog]
	@Creator=@Creator,		
	@TypeName= 'Profile',	
	@SPName=@ExecutingSPName,
	@IP =@IP,
	@Detail=@LogData
	
	
GO
GRANT EXECUTE ON [dbo].[Is_Odds_ProfileIns] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_ProfileIns] TO [vnteam]
GO