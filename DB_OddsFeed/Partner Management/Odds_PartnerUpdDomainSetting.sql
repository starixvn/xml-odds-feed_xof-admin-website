Alter PROCEDURE [dbo].[Is_Odds_PartnerUpdDomainSetting]
	@PartnerID INT,
	@Creator INT = 0,
	@ListDomainId varchar(100),--Each DomainID is separate by comma ','
	@Ip varchar(25) = '',
	@ErrCode int = 0 Output,
	@ErrMsg nvarchar(200) = '' Output
AS
/*
Created: 20100122@Johnny
Task : Update Domain setting for partner
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	- @PartnerID : output partner is inserted
	- @Creator
	- @ListDomainId 
*/

--Check Domain name
SET NOCOUNT ON;
Begin Transaction

--DomainSetting
Begin Try
	Delete Odds_DomainSetting where PartnerId = @PartnerId

	Insert into Odds_DomainSetting(PartnerId, DomainId)
	Select @PartnerId, Val From dbo.strSplit(@ListDomainId, ',')
End Try
Begin Catch
	Select @ErrMsg = ERROR_MESSAGE(), @ErrCode = 10013			
	RollBack Transaction
	Return
End Catch

Commit Transaction

--Write the log 
declare @SPName VARCHAR(50)
select @SPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Update domain setting for partner, PartnerId:' + dbo.Is_Odds_ToString(@PartnerId) +
		', Creator: ' + dbo.Is_Odds_ToString(@Creator) +		
		', ListDomainId: ' + dbo.Is_Odds_ToString(@ListDomainId)+
		', Ip:' + dbo.Is_Odds_ToString(@Ip)

EXEC [Is_Odds_WriteLog]
	@Creator=@Creator,	
	@PartnerID = @PartnerID,
	@TypeName= 'Partner',	
	@SPName=@SPName,
	@IP =@IP,
	@Detail=@LogData
	

GO
GRANT EXECUTE ON [dbo].[Is_Odds_PartnerUpdDomainSetting] TO [bodbAdminNet]
GO
GRANT EXECUTE ON [dbo].[Is_Odds_PartnerUpdDomainSetting] TO [vnteam]
GO
