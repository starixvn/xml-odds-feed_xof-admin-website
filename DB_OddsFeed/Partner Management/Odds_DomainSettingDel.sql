IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_DomainSettingDel')
	BEGIN
		DROP  PROCEDURE  [dbo].[Is_Odds_DomainSettingDel]
	END

GO

CREATE PROCEDURE [dbo].[Is_Odds_DomainSettingDel]
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
	@Creator: ID of current admin
	@IP: IP Address of current admin
*/
SET NOCOUNT ON;


--Check PartnerID and IPFrom is existed or not
IF NOT EXISTS(SELECT 1 FROM [dbo].[Odds_DomainSetting] WITH(NOLOCK) WHERE [DomainID]=@DomainID AND PartnerID=@PartnerID)
BEGIN
	SELECT @ErrCode = 10082,@ErrMsg = 'Error! Cannot delete this Domain setting. It was not existed.'
	RETURN
END

DELETE FROM [dbo].[Odds_DomainSetting]
WHERE [DomainID]=@DomainID AND PartnerID=@PartnerID


IF @@Error <> 0
BEGIN			
	SELECT @ErrMsg = 'Error! Delete domain setting unsuccessful.Error when delete domain setting.', @ErrCode = 10083			
	RETURN
END

--Write the log 
DECLARE @StoreName VARCHAR(50)
SELECT @StoreName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Delete Domain Setting. DomainID: ' + dbo.Is_Odds_ToString(@DomainID) +		
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

GRANT EXECUTE ON [dbo].[Is_Odds_DomainSettingDel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_DomainSettingDel] TO [vnteam]
GO
