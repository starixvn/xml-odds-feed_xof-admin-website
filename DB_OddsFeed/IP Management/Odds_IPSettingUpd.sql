CREATE PROCEDURE [dbo].[Odds_IPSettingUpd]
	@PartnerID INT,
	@IPFrom BIGINT,	
	@NewIPFrom BIGINT,
	@NewIPTo BIGINT,	
	@Creator INT = 0,
	@IP VARCHAR(25) = '',
	@ErrCode INT = 0 Output,
	@ErrMsg NVARCHAR(200) = '' output 
AS
/*
Created: 20121218@Johnny
Task : Update IP Setting
DB: DB052.
Revisions:
	- 

Param's Explanation:
	@Creator: ID of current admin
	@IP: IP Address of current admin	
*/

SET NOCOUNT ON;
--Check PartnerID and IPFrom is existed or not
IF EXISTS(SELECT 1 FROM [dbo].[PartnerIP] WITH(NOLOCK) WHERE [PartnerId] = @PartnerId AND [IPFrom] = @NewIPFrom AND [IPFrom]<> @IPFrom)
BEGIN
	SELECT @ErrCode = 10065,@ErrMsg = 'Error! Cannot update this IP setting. This value was existed.'
	RETURN
END

IF @NewIPTo >= @IPFrom
BEGIN
	UPDATE [dbo].[PartnerIP]
		SET [IPFrom]=@NewIPFrom	,[IPTo] = @NewIPTo
	WHERE [PartnerID]=@PartnerID AND [IPFrom]=@IPFrom
	
	IF @@ERROR <> 0
	BEGIN	
		Select @ErrMsg = N'Error! Update IP setting unsuccessful.', @ErrCode = 10066
		RETURN
	END
END
ELSE
BEGIN
	SELECT @ErrMsg = N'Error! Update IP setting unsuccessful. IPFrom is larger than IPTo.', @ErrCode = 10067
END


--Write the log 
DECLARE @StoreName VARCHAR(50)
SELECT @StoreName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Update IP Setting. PartnerID: ' + dbo.Is_Odds_ToString(@PartnerID) +		
		', IPFFrom ' + dbo.Is_Odds_ToString(@IPFrom)+				
		', NewIPFfrom ' + dbo.Is_Odds_ToString(@NewIPFrom)+
		', NewIPTo ' + dbo.Is_Odds_ToString(@NewIPTo)+
		', Creator: ' + dbo.Is_Odds_ToString(@Creator) +			
		', SPName ' + dbo.Is_Odds_ToString(@StoreName) +	
		', Ip: ' + @Ip
		

EXEC [dbo].[Odds_WriteLog]
	@Creator=@Creator,		
	@TypeName= 'IP',	
	@SPName=@StoreName,
	@IP =@IP,
	@Detail=@LogData

GO
GRANT EXECUTE ON [dbo].[Odds_IPSettingUpd] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Odds_IPSettingUpd] TO [vnteam]
GO