CREATE PROCEDURE [dbo].[Odds_IPSettingIns]
	@PartnerID INT,
	@IPFrom BIGINT,	
	@IPTo BIGINT,	
	@Creator INT = 0,
	@IP VARCHAR(25) = '',
	@ErrCode INT = 0 OUTPUT,
	@ErrMsg NVARCHAR(200) = '' OUTPUT 
AS
/*
Created: 20100122@Jacob
Task : Insert IP Setting
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	@Creator: ID of current admin
	@IP: IP Address of current admin 	
*/

SET NOCOUNT ON;
--Check PartnerID and IPFrom is existed or not
IF EXISTS(SELECT 1 FROM [dbo].[PartnerIP] WITH(NOLOCK) WHERE [PartnerId] = @PartnerId AND IPFrom = @IPFrom)
BEGIN
	SELECT @ErrCode = 10063,@ErrMsg = 'Error! Cannot insert this IP setting. It was existed.'
	RETURN
END

IF @IPTo >= @IPFrom
BEGIN
	INSERT INTO [dbo].[PartnerIP](
		[PartnerID]
		,[IPFrom]
		,[IPTo]
	)
	VALUES(
		@PartnerID
		,@IPFrom
		,@IPTo
	)
	IF @@ERROR <> 0
	BEGIN	
		SELECT @ErrMsg = N'Error! Insert IP setting unsuccessful.', @ErrCode = 10064
		RETURN
	END
END
ELSE
BEGIN	
	SELECT @ErrMsg = N'Error! Insert IP setting unsuccessful! IPFrom is larger than IPTo.', @ErrCode = 10068
	RETURN
END

--Write the log 
DECLARE @SPName VARCHAR(50)
SELECT @SPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Insert IP Setting. PartnerID: ' + dbo.Is_Odds_ToString(@PartnerID) +		
		', IPFFrom ' + dbo.Is_Odds_ToString(@IPFrom)+		
		', IPFTo ' + dbo.Is_Odds_ToString(@IPTo)+	
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

GRANT EXECUTE ON [dbo].[Odds_IPSettingIns] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Odds_IPSettingIns] TO [vnteam]
GO