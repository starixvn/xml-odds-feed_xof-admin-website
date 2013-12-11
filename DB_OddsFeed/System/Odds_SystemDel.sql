USE [bodb_oddsfeed]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_SystemDel')
	BEGIN
		DROP  PROCEDURE  [dbo].[Is_Odds_SystemDel]
	END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Is_Odds_SystemDel]
	@SystemID SMALLINT,
	@Creator INT,	
	@Ip VARCHAR(25) = '',
	@ErrCode INT  = 0 OUTPUT,
	@ErrMsg NVARCHAR(200) = '' OUTPUT	
AS
/*
Created: 20100322@Jacob
Task : Delete system
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	
*/
SET NOCOUNT ON;
--Check if system is assigned to any partner
IF EXISTS(SELECT 1 FROM [dbo].[Partner] WITH(NOLOCK) WHERE [SystemID] = @SystemID)
BEGIN
	SELECT @ErrCode = 10104, @ErrMsg = 'Error! Cannot delete this system. It was assigned to some partner.'
	RETURN
END

--Check if system is assigned to any domain
IF EXISTS(SELECT 1 FROM [dbo].[SystemDomain] WITH(NOLOCK) WHERE [SystemID] = @SystemID)
BEGIN
	SELECT @ErrCode = 10105, @ErrMsg = 'Error! Cannot delete this system. It was assigned to some domain.'
	RETURN
END


DELETE FROM [dbo].[System]WHERE  [SystemID]=@SystemID

IF @@ERROR <> 0
BEGIN	
	SELECT @ErrMsg = N'Error! Delete system unsuccessfully.', @ErrCode = 10103
	RETURN
END


--Write the log 
DECLARE @SPName VARCHAR(50)
SELECT @SPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Delete News System, SystemId:' + dbo.Is_Odds_ToString(@SystemId) +		
		', Creator: ' + dbo.Is_Odds_ToString(@Creator) +
		', Ip:' + dbo.Is_Odds_ToString(@Ip)

EXEC [dbo].[Is_Odds_WriteLog]
	@Creator=@Creator,		
	@TypeName= 'System',	
	@SPName=@SPName,
	@IP =@IP,
	@Detail=@LogData
	
GO
GRANT EXECUTE ON [dbo].[Is_Odds_SystemDel] TO [bodbAdminNet]
GO
GRANT EXECUTE ON [dbo].[Is_Odds_SystemDel] TO [vnteam]
GO