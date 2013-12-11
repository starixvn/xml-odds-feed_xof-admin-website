CREATE PROCEDURE [dbo].[Odds_IPSettingDel]
	@PartnerID SMALLINT,
	@IPFrom BIGINT,	
	@Creator INT = 0,
	@IP VARCHAR(25) = '',
	@ErrCode INT = 0 OUTPUT,
	@ErrMsg NVARCHAR(200) = '' OUTPUT 	
AS
/*
Created: 20100322@Jacob
Task : Delete IP Setting
DB: DB052.bodb_oddsfeed
Revisions:
	- 

Param's Explanation:
	@Creator: ID of current admin
	@IP: IP Address of current admin
-
*/

SET NOCOUNT ON;


--Check PartnerID and IPFrom is existed or not
IF NOT EXISTS(SELECT 1 FROM [dbo].[PartnerIP] WITH(NOLOCK) WHERE [PartnerId] = @PartnerId AND IPFrom = @IPFrom)
BEGIN
	SELECT @ErrCode = 10061,@ErrMsg = 'Error! Cannot delete this IP setting. It was not exist.'
	RETURN
END

DELETE FROM [dbo].[PartnerIP] WHERE [PartnerID]=@PartnerID AND [IPFrom]=@IPFrom
IF @@ERROR <> 0
BEGIN	
	SELECT @ErrMsg = N'Error! Delete IPSetting unsuccessful.', @ErrCode = 10062
	RETURN
END


--Write the log 
DECLARE @SPName VARCHAR(50)
SELECT @SPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Delete IP Setting. PartnerID: ' + dbo.Is_Odds_ToString(@PartnerID) +		
		', IPFFrom ' + dbo.Is_Odds_ToString(@IPFrom)+				
		', Creator: ' + dbo.Is_Odds_ToString(@Creator) +			
		', ExcutingSPName ' + dbo.Is_Odds_ToString(@SPName) +			
		', Ip: ' + @Ip
		

EXEC [dbo].[Odds_WriteLog]
	@Creator=@Creator,		
	@TypeName= 'IP',	
	@SPName=@SPName,
	@IP =@IP,
	@Detail=@LogData
	
GO
GRANT EXECUTE ON [dbo].[Odds_IPSettingDel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Odds_IPSettingDel] TO [vnteam]
GO