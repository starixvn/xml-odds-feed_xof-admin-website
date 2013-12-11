IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Odds_SystemUpd')
	BEGIN
		DROP  PROCEDURE  [dbo].[Odds_SystemUpd]
	END

GO

CREATE PROCEDURE [dbo].[Odds_SystemUpd]
	@SystemID SMALLINT,
	@SystemName VARCHAR(50),
	@Creator INT,	
	@Ip VARCHAR(25) = '',
	@ErrCode INT  = 0 OUTPUT,
	@ErrMsg NVARCHAR(200) = '' OUTPUT	
AS
/*
Created: 20100322@Jacob
Task : Update system
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	
*/
SET NOCOUNT ON;
--Check System name
IF EXISTS(SELECT 1 FROM dbo.System WITH(NOLOCK) WHERE [SystemName] = @SystemName AND [SystemID]<>@SystemID)
BEGIN
	SELECT @ErrCode = 10106, @ErrMsg = 'Error! Cannot update this system. The system name was existed.'
	RETURN
END


UPDATE dbo.System with (updlock)
SET [SystemName]= @SystemName	
	,[LastUpdate]=GETDATE()
WHERE [SystemID]=@SystemID	

IF @@ERROR <> 0
BEGIN	
	SELECT @ErrMsg = N'Error! Update system unsuccessfully.', @ErrCode = 10107
	RETURN
END

--Write the log 
DECLARE @SPName VARCHAR(50)
SELECT @SPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Update System, SystemId:' + dbo.Is_Odds_ToString(@SystemId) +
		', Systemname: ' + dbo.Is_Odds_ToString(@SystemName) +
		', Creator: ' + dbo.Is_Odds_ToString(@Creator) +
		', ExcutingSPName ' + dbo.Is_Odds_ToString(@SPName) +	
		', Ip:' + dbo.Is_Odds_ToString(@Ip)

EXEC [dbo].[Is_Odds_WriteLog]
	@Creator=@Creator,		
	@TypeName= 'System',	
	@SPName=@SPName,
	@IP =@IP,
	@Detail=@LogData
	
GO
GRANT EXECUTE ON [dbo].[Odds_SystemUpd] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Odds_SystemUpd] TO [vnteam]
GO
