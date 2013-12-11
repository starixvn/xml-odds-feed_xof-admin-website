set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go



/****** Object:  Stored Procedure dbo.[Odds_DomainIns]    Script Date: 6/30/2010 4:28:20 AM ******/

CREATE PROCEDURE [dbo].[Odds_DomainIns]
	@DomainID SMALLINT = 0 OUTPUT,
	@DomainName NVARCHAR(300) = '',	
	@IP VARCHAR(23) = '',
	@ISP NVARCHAR(256) = '',
	@Creator INT = 0,
	@CurrentIP VARCHAR(25)='',
	@ErrCode INT = 0 OUTPUT,
	@ErrMsg NVARCHAR(200) = '' OUTPUT
AS
/*
Created: 20100122@Johnny
Task : Insert Domain name
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	- @DomainName : URL of domain(Ex: Odds.portal.12bet,...)
	- @IP : IP of domain
	- @ISP : Internet service provider
	- @Creator : User Create new domain name
*/

SET NOCOUNT ON;
--Check Domain name
IF EXISTS(SELECT 1 FROM Domain WITH(NOLOCK) WHERE [DomainName] = @DomainName)
BEGIN
	SELECT @ErrCode = 10001, @ErrMsg = 'This domain name was existed'
	return
END

IF( @DomainName='')
BEGIN
	SELECT @ErrCode = 10002, @ErrMsg = 'The domain name was empty'
	RETURN 
END
INSERT INTO Domain(
	[DomainName]
	,[CreatedDate]
	,[IP]
	,[ISP]
	,[Creator]
	,[LastUpdate])
VALUES
(
	@DomainName,
	GETDATE(),
	@IP,
	@ISP,
	@Creator,
	GETDATE()
)

SELECT @DomainID = SCOPE_IDENTITY()

IF @@ERROR <> 0
BEGIN	
	SELECT @ErrMsg = N'Error! Create domain unsuccessfully.', @ErrCode = 10003
	RETURN
END

--Write the log 
DECLARE @ExecutingSPName VARCHAR(50)
SELECT @ExecutingSPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Add News Domain, DomainID:' + dbo.Is_Odds_ToString(@DomainID) +
		', DomainName: ' + dbo.Is_Odds_ToString(@DomainName) +
		', IP: ' + dbo.Is_Odds_ToString(@IP) +
		', ISP: ' + dbo.Is_Odds_ToString(@ISP) +
		', Ip:' + dbo.Is_Odds_ToString(@CurrentIP)+
		', ExcutingSPName ' + dbo.Is_Odds_ToString(@ExecutingSPName) 

EXEC [Is_Odds_WriteLog]
	@Creator=@Creator,		
	@TypeName= 'Domain',	
	@SPName=@ExecutingSPName,
	@IP =@CurrentIP,
	@Detail=@LogData

GO
GRANT EXECUTE ON [dbo].[Odds_DomainIns] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Odds_DomainIns] TO [vnteam]
GO
