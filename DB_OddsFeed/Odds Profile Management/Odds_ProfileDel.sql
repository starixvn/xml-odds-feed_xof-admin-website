IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_ProfileDel')
	BEGIN
		DROP  PROCEDURE  [dbo].[Is_Odds_ProfileDel]
	END

GO

CREATE PROCEDURE [dbo].[Is_Odds_ProfileDel]
	@ProfileID SMALLINT,
	@Creator INT = 0,
	@IP VARCHAR(25) = '',
	@ErrCode int = 0 Output,
	@ErrMsg NVARCHAR(200) = '' output
AS
/*
Created: 20100322@Jacob
Task : Delete profile
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	- @SystemID : get all if @SystemID isnull
*/
SET NOCOUNT ON;
-- Check if profile is assigned to any partner
IF EXISTS(SELECT 1 FROM [dbo].[Odds_Partner] WITH(NOLOCK) WHERE [ProfileID] = @ProfileID)
BEGIN
	SELECT @ErrCode = 10125, @ErrMsg = 'Error! Cannot delete this profile. It was assigned to some partners.'
	RETURN
END


UPDATE [dbo].[Odds_OddsProfile]
SET [IsDeleted]=1
WHERE [ProfileID]=@ProfileID

IF @@ERROR <> 0
BEGIN	
	SELECT @ErrMsg = N'Error! Delete profile unsuccessfully.', @ErrCode = 10126
	RETURN
END

--Write the log 
DECLARE @ExecutingSPName VARCHAR(50)
SELECT @ExecutingSPName = OBJECT_NAME(@@ProcID) 


DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Delete Profile, ProfileId:' + dbo.Is_Odds_ToString(@ProfileID) +
		', ExcutingSPName ' + dbo.Is_Odds_ToString(@ExecutingSPName) +	
		', Creator: ' + dbo.Is_Odds_ToString(@Creator) +			
		', Ip: ' + @Ip
		

EXEC [Is_Odds_WriteLog]
	@Creator=@Creator,		
	@TypeName= 'Profile',	
	@SPName=@ExecutingSPName,
	@IP =@IP,
	@Detail=@LogData
	
	
GO
GRANT EXECUTE ON [dbo].[Is_Odds_ProfileDel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_ProfileDel] TO [vnteam]
GO