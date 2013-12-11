IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Odds_DomainDel')
	BEGIN
		DROP  PROCEDURE  [dbo].[Odds_DomainDel]
	END

GO

CREATE PROCEDURE [dbo].[Odds_DomainDel]
	-- Add the parameters for the stored procedure here
	@DomainID smallint=0,
	@Creator INT=0,
	@Ip VARCHAR(25) = '',
	@ErrCode INT = 0 OUTPUT,
	@ErrMsg NVARCHAR(200) = '' OUTPUT
	
AS
	SET NOCOUNT ON;

	-- Check if domain is existed
	IF NOT EXISTS(SELECT 1 FROM [dbo].[Domain] WITH(NOLOCK) WHERE [DomainID]=@DomainID)
	BEGIN
		SELECT @ErrCode = 10031,@ErrMsg = 'Cannot delete. This domain was not existed.'
		RETURN
	END

	-- Check if domain is used by Affiliate or Licensee
	IF  EXISTS (SELECT 1 FROM [dbo].[PartnerDomain] WITH(NOLOCK) WHERE [DomainID]=@DomainID)
		OR EXISTS (SELECT 1 FROM [dbo].[SystemDomain] WITH(NOLOCK) WHERE [DomainID]=@DomainID)
	BEGIN
		SELECT @ErrCode=10032, @ErrMsg='Error! Cannot delete this domain because it is currently in use.'
		RETURN
	END

BEGIN TRANSACTION
	DELETE FROM [dbo].[Domain]  WHERE [DomainID]=@DomainID
	IF @@ERROR <> 0
	BEGIN	
		Select @ErrMsg = N'Delete domain unsuccessfully.', @ErrCode = 10033
		ROLLBACK TRANSACTION
		RETURN
	END
COMMIT TRANSACTION


--Write the log 
DECLARE @ExecutingSPName VARCHAR(50)
SELECT @ExecutingSPName = OBJECT_NAME(@@ProcID) 


DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Delete Domain, DomainId:' + dbo.Is_Odds_ToString(@DomainID) +
		', ExcutingSPName ' + dbo.Is_Odds_ToString(@ExecutingSPName) +	
		', Creator: ' + dbo.Is_Odds_ToString(@Creator) +			
		', Ip: ' + @Ip
		

EXEC [Is_Odds_WriteLog]
	@Creator=@Creator,		
	@TypeName= 'Domain',	
	@SPName=@ExecutingSPName,
	@IP =@IP,
	@Detail=@LogData
	
GO
GRANT EXECUTE ON [dbo].[Odds_DomainDel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Odds_DomainDel] TO [vnteam]
GO
