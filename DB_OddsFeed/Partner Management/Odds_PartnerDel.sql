IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_PartnerDel')
	BEGIN
		DROP  Procedure  [dbo].[Is_Odds_PartnerDel]
	END

GO

GO
CREATE PROCEDURE [dbo].[Is_Odds_PartnerDel]
	@PartnerID INT,
	@Creator INT = NULL,
	@IP VARCHAR(25) = '',
	@ErrCode INT = 0 OUTPUT,
	@ErrMsg NVARCHAR(200) = '' OUTPUT
AS
/*
Created: 20100322@Jacob
Task : Delete Partner
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	@Creator: ID of current admin
	@IP: IP Address of current admin
		
*/
SET NOCOUNT ON;
--Check Partner Setting is existed or not
IF NOT EXISTS(SELECT 1 FROM [dbo].[Odds_Partner] WITH(NOLOCK) WHERE [PartnerID]=@PartnerID)
BEGIN
	SELECT @ErrCode = 10041,@ErrMsg = 'Error! Cannot delete this parter. It was not existed.'
	RETURN
END


BEGIN TRANSACTION

DELETE FROM [dbo].[Odds_DomainSetting] WHERE [PartnerID]=@PartnerID
DELETE FROM [dbo].[Odds_IPSetting] WHERE [PartnerID]=@PartnerID
DELETE FROM [dbo].[Odds_Partner] WHERE [PartnerID]=@PartnerID

IF @@ERROR <> 0
BEGIN	
	Select @ErrMsg = N'Error! Delete partner unsuccessfully.', @ErrCode = 10042
	ROLLBACK TRANSACTION
	RETURN
END

COMMIT TRANSACTION

DECLARE @SPName VARCHAR(50)
SELECT @SPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Delete Partner , PartnerId:' + dbo.Is_Odds_ToString(@PartnerId) +
		', Creator: ' + dbo.Is_Odds_ToString(@Creator) +
		', Ip:' + dbo.Is_Odds_ToString(@Ip)+
		', ExcutingSPName ' + dbo.Is_Odds_ToString(@SPName) 

EXEC [dbo].[Is_Odds_WriteLog]
	@Creator=@Creator,	
	@PartnerID = @PartnerID,
	@TypeName= 'Partner',	
	@SPName=@SPName,
	@IP =@IP,
	@Detail=@LogData

GO
GRANT EXECUTE ON [dbo].[Is_Odds_PartnerDel] TO [bodbAdminNet]
GO
GRANT EXECUTE ON [dbo].[Is_Odds_PartnerDel] TO [vnteam]
GO