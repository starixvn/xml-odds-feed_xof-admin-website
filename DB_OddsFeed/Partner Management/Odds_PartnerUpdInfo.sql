IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_PartnerUpdInfo')
	BEGIN
		DROP  PROCEDURE  [dbo].[Is_Odds_PartnerUpdInfo]
	END

GO
CREATE PROCEDURE [dbo].[Is_Odds_PartnerUpdInfo]
	@PartnerID INT,
	@SystemID SMALLINT = NULL,	
	@ProfileID SMALLINT = NULL,	
	@Site NVARCHAR(200) = NULL,
	@IsAllowed BIT = NULL,
	@Remark NVARCHAR(100) = NULL,	
	@Creator INT = NULL,
	@ContactPerson NVARCHAR(50),
	@ContactEmail NVARCHAR(50),
	@ListDomainId VARCHAR(1024),--Each DomainID is separate by comma ','
	@IP VARCHAR(25) = '',
	@ErrCode INT = 0 OUTPUT,
	@ErrMsg NVARCHAR(200) = '' OUTPUT
AS
/*
Created: 20100122@Johnny
Task : Update Domain name
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	- @PartnerID : OUTPUT partner is inserted
	- @SystemID : ID of System(IBCBet, 12bet, ...)
	- @ProfileID : Template contain List Bettypes are allowed to access
	- @Username : Username of partner
	- @Site : website of partner
	- @IsAllow : allow or not allow partner get odds feed
	- @Remark
	- @CreatedDate
	- @Creator
*/
SET NOCOUNT ON;
--Check Domain name
IF NOT EXISTS(SELECT 1 From [dbo].[Odds_Partner] WITH(NOLOCK) WHERE [PartnerID] = @PartnerID)
BEGIN
	SELECT @ErrCode = 10046, @ErrMsg = 'Error! Cannot update this partner. It was not existed.'
	RETURN
END

--Check SystemID and ProfileId
IF @SystemId IS NOT NULL AND NOT EXISTS(SELECT 1 FROM [dbo].[Odds_System] WITH(NOLOCK) WHERE [SystemId] = @SystemId)
BEGIN
	SELECT @ErrCode = 10047, @ErrMsg = 'Error! Cannot update this partner. The system was not existed in Odds_System table.'
	RETURN
END

IF @ProfileID IS NOT NULL AND NOT EXISTS(SELECT 1 FROM [dbo].[Odds_OddsProfile] WITH(NOLOCK) WHERE [ProfileID] = @ProfileID)
BEGIN
	SELECT @ErrCode = 10048, @ErrMsg = 'Error! Cannot update this partner. The profile was not existed in Odds_OddsProfile table.'
	RETURN
END


BEGIN TRANSACTION
--DomainSetting
DELETE FROM [dbo].[Odds_DomainSetting] WHERE [PartnerID]=@PartnerId;

INSERT INTO [dbo].[Odds_DomainSetting]([PartnerID], [DomainId])
SELECT @PartnerId, Val From dbo.strSplit(@ListDomainId, ',')

if @@Error <> 0
BEGIN			
	SELECT @ErrMsg = 'Error! Update partner unsuccessfully. Error when inserting domain list.', @ErrCode = 10051			
	ROLLBACK TRANSACTION
	RETURN
END

UPDATE [dbo].[Odds_Partner] WITH(Rowlock, UpdLock)
SET [SystemID] = ISNULL(@SystemID, [SystemID])
	,[ProfileID] = ISNULL(@ProfileID, [ProfileID])
	,[Site] = ISNULL(@Site, [Site])
	,[IsAllowed] = ISNULL(@IsAllowed, [IsAllowed])
	,[Remark] = ISNULL(@Remark, Remark)
	,[LastUpdate] = GETDATE()
	,[ContactPerson]= ISNULL(@ContactPerson, [ContactPerson])
	,[ContactEmail]= ISNULL(@ContactEmail, [ContactEmail])
WHERE [PartnerId] = @PartnerId

IF @@ERROR <> 0
BEGIN	
	SELECT @ErrMsg = N'Error! Update partner unsuccessfully.', @ErrCode = 10049
	ROLLBACK TRANSACTION
	RETURN
END

COMMIT TRANSACTION


DECLARE @SPName VARCHAR(50)
SELECT @SPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Update Partner info, PartnerId:' + dbo.Is_Odds_ToString(@PartnerId) +
		', SystemID: ' + dbo.Is_Odds_ToString(@SystemId) +
		', ProfileID: ' + dbo.Is_Odds_ToString(@ProfileID) +
		', Site : ' + dbo.Is_Odds_ToString(@Site) +
		', IsAllow: ' + dbo.Is_Odds_ToString(@IsAllowed) +
		', Remark: ' + dbo.Is_Odds_ToString(@Remark) +
		', ContactPerson: ' + dbo.Is_Odds_ToString(@ContactPerson) +
		', ContactEmail ' + dbo.Is_Odds_ToString(@ContactEmail) +
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
GRANT EXECUTE ON [dbo].[Is_Odds_PartnerUpdInfo] TO [bodbAdminNet]
GO
GRANT EXECUTE ON [dbo].[Is_Odds_PartnerUpdInfo] TO [vnteam]
GO
