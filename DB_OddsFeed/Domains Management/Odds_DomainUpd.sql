IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Odds_DomainUpd')
	BEGIN
		DROP  PROCEDURE  [dbo].[Odds_DomainUpd]
	END

GO

CREATE PROCEDURE [dbo].[Odds_DomainUpd]
	@DomainID SMALLINT ,
	@DomainName NVARCHAR(256) = NULL,
	@IP VARCHAR(23) = NULL,
	@ISP NVARCHAR(256) = NULL,
	@Creator INT  = NULL,
	@CurrentIP VARCHAR(25)=NULL,
	@ErrCode INT = 0 OUTPUT,
	@ErrMsg NVARCHAR(200) = '' OUTPUT
AS
/*
Created: 20100122@Johnny
Task : Update Domain name
DB: DB052.bodb_MaINTenance
Revisions:
	- 

Param's Explanation:
	- @DomainName : URL of domain(Ex: Odds.portal.12bet,...)
	- @IP : IP of domain
	- @ISP : Internet service provider
	- @Creator : User Create new domain name
*/

--Check Domain name
--Check Domain name

SET NOCOUNT ON;

IF NOT EXISTS(SELECT 1 FROM Domain WITH(NOLOCK) WHERE [DomainID] = @DomainID)
BEGIN
	SELECT @ErrCode = 10004, @ErrMsg = 'This domain was not existed'
	RETURN
END

UPDATE Domain WITH (rowlock, updlock)
SET
      IP = ISNULL(@IP,IP)
      ,ISP = ISNULL(@ISP,ISP)      
      ,LastUpdate = GETDATE()
WHERE DomainID = @DomainID

--Write the log 
declare @SPName VARCHAR(50)
select @SPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Update Domain, DomainId:' + dbo.Is_Odds_ToString(@DomainID) +
		', DomainName ' + dbo.Is_Odds_ToString(@DomainName) +		
		', ISP ' + dbo.Is_Odds_ToString(@ISP) +		
		', Creator: ' + dbo.Is_Odds_ToString(@Creator) +		
		', Ip:' + @CurrentIP

EXEC [Is_Odds_WriteLog]
	@Creator=@Creator,		
	@TypeName= 'Domain',	
	@SPName=@SPName,
	@IP =@CurrentIP,
	@Detail=@LogData
	
GO
GRANT EXECUTE ON [dbo].[Odds_DomainUpd] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Odds_DomainUpd] TO [vnteam]
GO