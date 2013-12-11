
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_DomainSettingIns')
	BEGIN
		DROP  PROCEDURE  [dbo].[Is_Odds_DomainSettingIns]
	END

GO

CREATE PROCEDURE [dbo].[Is_Odds_DomainSettingIns]
	@DomainID SMALLINT,
	@PartnerID INT,
	@Creator INT = 0,
	@IP VARCHAR(25) = '',
	@ErrCode INT = 0 OUTPUT,
	@ErrMsg NVARCHAR(200) = '' OUTPUT 
AS
/*
Created: 20100322@jacob
Task : Delete Domain Setting
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:	
	- @IP : IP for logging	
	- @Creator : User delete domain setting
*/
SET NOCOUNT ON;

IF EXISTS(SELECT 1 FROM [dbo].[Odds_DomainSetting] WITH(NOLOCK) WHERE  [DomainID]=@DomainID AND [PartnerID]=@PartnerID)
BEGIN
	SELECT @ErrMsg = 'Error! Cannot insert this dmain setting. It was existed.', @ErrCode = 10084
	RETURN
END

INSERT INTO [dbo].[Odds_DomainSetting]([PartnerID], [DomainID])
VALUES(@PartnerID, @DomainID)

IF @@Error <> 0
BEGIN			
	SELECT @ErrMsg = 'Error! insert domain setting unsuccessful.', @ErrCode = 10085			
	RETURN
END

--Write the log 
DECLARE @StoreName varchar(50)
SELECT @StoreName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData nvarchar(4000)
SELECT @LogData = 'Insert Domain Setting. DomainID: ' + dbo.Is_Odds_ToString(@DomainID) +		
		', PartnerID ' + dbo.Is_Odds_ToString(@PartnerID)+		
		', Creator: ' + dbo.Is_Odds_ToString(@Creator) +			
		', SPName ' + dbo.Is_Odds_ToString(@StoreName) +	
		', Ip: ' + @Ip
		

EXEC [dbo].[Is_Odds_WriteLog]
	@Creator=@Creator,		
	@TypeName= 'Domain',	
	@SPName=@StoreName,
	@IP =@IP,
	@Detail=@LogData

GO

GRANT EXECUTE ON [dbo].[Is_Odds_DomainSettingIns] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_DomainSettingIns] TO [vnteam]
GO
