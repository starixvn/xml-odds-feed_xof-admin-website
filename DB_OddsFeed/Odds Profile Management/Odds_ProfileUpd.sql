IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_ProfileUpd')
	BEGIN
		DROP  PROCEDURE  [dbo].[Is_Odds_ProfileUpd]
	END

GO

CREATE PROCEDURE [dbo].[Is_Odds_ProfileUpd]
	@ProfileID SMALLINT,
	@ProfileName VARCHAR(50),
	@Description NVARCHAR(100) = '',	
	@TimeRange SMALLINT,
	@SPName VARCHAR(100),
	@SPParams VARCHAR(100),	
	@OddsType varchar(50),
	@LanguageList varchar(50),
	@OddsRegion tinyint,
	@SportIDList varchar(2048)=null,	
	@BettypeIDList varchar(2048)=null,
	@LeagueIDList varchar(2048)=null,
	@CountryCodeList varchar(2048)=null,
	@LeagueKeyword varchar(100)=null,
	@LeagueKeyword_O varchar(100)=null,
	@Creator INT = 0,
	@IP VARCHAR(25) = '',
	@ErrCode int = 0 Output,
	@ErrMsg NVARCHAR(200) = '' output
AS
/*
Created: 20100322@Jacob
Task : Update profile
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	- @SystemID : get all if @SystemID isnull
*/
SET NOCOUNT ON;
--Check System name
IF EXISTS(SELECT 1 FROM [dbo].[Odds_OddsProfile] WITH(NOLOCK) WHERE [ProfileName] = @ProfileName AND [ProfileID]<>@ProfileID)
BEGIN
	SELECT @ErrCode = 10123, @ErrMsg = 'Error! Cannot update this profile The profile name was existed.'
	RETURN
END


UPDATE [dbo].[Odds_OddsProfile]
SET [ProfileName]=ISNULL(@ProfileName,[ProfileName])
	,[Description]=ISNULL(@Description,[Description])	
	,[LastUpdate]=GETDATE()
	,[TimeRange]=ISNULL(@TimeRange,[TimeRange])
	,[SPName]=ISNULL(@SPName,[SPName])
	,[SPParams]=ISNULL(@SPParams,[SPParams])
	,OddsType =ISNULL(@OddsType,[OddsType])
	,LanguageList =ISNULL(@LanguageList,[LanguageList])
	,OddsRegion=ISNULL(@OddsRegion,[OddsRegion])
	,SportIDList =ISNULL(@SportIDList,[SportIDList])
	,BettypeIDList =ISNULL(@BettypeIDList,[BettypeIDList])
	,LeagueIDList =ISNULL(@LeagueIDList,[LeagueIDList])
	,CountryCodeList =ISNULL(@CountryCodeList,[CountryCodeList])
	,LeagueKeyword =ISNULL(@LeagueKeyword,[LeagueKeyword])
	,LeagueKeyword_O =ISNULL(@LeagueKeyword_O,[LeagueKeyword_O])
	
WHERE [ProfileID]=@ProfileID

IF @@ERROR <> 0
BEGIN	
	SELECT @ErrMsg = N'Error! Update profile unsuccessfully.', @ErrCode = 10124
	RETURN
END

--Write the log 
DECLARE @ExecutingSPName VARCHAR(50)
SELECT @ExecutingSPName = OBJECT_NAME(@@ProcID) 


DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Update Profile, ProfileId:' + dbo.Is_Odds_ToString(@ProfileID) +
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
GRANT EXECUTE ON [dbo].[Is_Odds_ProfileUpd] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_ProfileUpd] TO [vnteam]
GO
