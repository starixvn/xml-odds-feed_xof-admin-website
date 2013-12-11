IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_SystemIns')
	BEGIN
		DROP  PROCEDURE  [dbo].[Is_Odds_SystemIns]
	END

GO

CREATE PROCEDURE [dbo].[Is_Odds_SystemIns]
	@SystemName VARCHAR(50),
	@Creator INT,	
	@Ip VARCHAR(25) = '',
	@ErrCode INT  = 0 OUTPUT,
	@ErrMsg NVARCHAR(200) = '' OUTPUT,
	@SystemID SMALLINT = 0 OUTPUT
AS
/*
Created: 20100322@Jacob
Task : Insert system
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	- @SystemID : get all if @SystemID isnull
*/
SET NOCOUNT ON;
--Check System name
IF EXISTS(SELECT 1 FROM [dbo].[System] WITH(NOLOCK) WHERE [SystemName] = @SystemName)
BEGIN
	SELECT @ErrCode = 10101, @ErrMsg = 'Error! Cannot insert this system. The system name was existed.'
	RETURN
END


INSERT INTO [dbo].[System](
	[SystemName]	
	,[CreatedDate]
	,[LastUpdate]
	,[Creator]
	
)
VALUES(
	@SystemName	
	,GETDATE()
	,GETDATE()
	,@Creator	
)

IF @@ERROR <> 0
BEGIN	
	SELECT @ErrMsg = N'Error! Insert system unsuccessfully.', @ErrCode = 10102
	RETURN
END

SELECT @SystemID = SCOPE_IDENTITY()


--Write the log 
DECLARE @SPName VARCHAR(50)
SELECT @SPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Add News System, SystemId:' + dbo.Is_Odds_ToString(@SystemId) +
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
GRANT EXECUTE ON [dbo].[Is_Odds_SystemIns] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_SystemIns] TO [vnteam]
GO
