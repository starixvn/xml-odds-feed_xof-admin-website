IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_PartnerIns')
	BEGIN
		DROP  PROCEDURE  [dbo].[Is_Odds_PartnerIns]
	END

GO

CREATE PROCEDURE [dbo].[Is_Odds_PartnerIns]
	@PartnerID INT = 0 Output,
	@SystemID SMALLINT,	
	@ProfileID SMALLINT,	
	@Username VARCHAR(50),
	@Site NVARCHAR(200),
	@IsAllowed BIT = 1,
	@Remark NVARCHAR(400),
	@Creator INT,
	@ContactPerson NVARCHAR(50),
	@ContactEmail NVARCHAR(50),
	@ListDomainId VARCHAR(1024),--Each DomainID is separate by comma ','
	@ListIPFrom VARCHAR(1024),
	@ListIPTo VARCHAR(1024),
	@Ip VARCHAR(25) = '',
	@ErrCode INT  = 0 OUTPUT,
	@ErrMsg NVARCHAR(200) = '' OUTPUT
AS
/*
Created: 20100122@Johnny
Task : Add new partner
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	- @PartnerID : OUTPUT partner is inserted
	- @SystemID : ID of System(IBCBet, 12bet, ...)
	- @ProfileID : Template contain List Bettypes are allowed to access
	- @Username : Username of partner
	- @Site : website of partner
	- @IsAllowed : allow or not allow partner get odds feed
	- @Remark	
	- @Creator: The admin who is inserting new partner
	- @ListDomainId: list id of domain (1,2,3,4)
	- @ListIPFrom : list IP FROM in number (12334343,434324321,312312321)
	- @ListIPTo: list IP to in number (12334343,434324321,312312321)
*/
SET NOCOUNT ON;
--Check Domain name
IF EXISTS(SELECT 1 FROM [dbo].[Odds_Partner] WITH(NOLOCK) WHERE [UserName] = @Username)
BEGIN
	SELECT @ErrCode = 10043, @ErrMsg = 'Error! Cannot insert this affiliate. The username was existed.'
	RETURN
END

--Check SystemID and ProfileId
IF NOT EXISTS(SELECT 1 FROM [dbo].[Odds_System] WITH(NOLOCK) WHERE [SystemId] = @SystemId)
BEGIN
	SELECT @ErrCode = 10044, @ErrMsg = 'Error! Cannot insert this partner. The system was not existed in Odds_System table.'
	RETURN
END

If Not EXISTS(SELECT 1 FROM [dbo].[Odds_OddsProfile] WITH(NOLOCK) WHERE [ProfileID] = @ProfileID)
BEGIN
	SELECT @ErrCode = 10045, @ErrMsg = 'Error! Cannot insert this partner. The profile was not existed in Odds_OddsProfile table.'
	RETURN
END


BEGIN TRANSACTION

INSERT INTO [dbo].[Odds_Partner](
	[SystemID]
	,[ProfileID]
	,[UserName]
	,[Site]
	,[IsAllowed]
	,[Remark]
	,[CreatedDate]
	,[LastUpdate]
	,[Creator]
	,[ContactPerson]
	,[ContactEmail]
)
VALUES(
	@SystemID
	,@ProfileID
	,@UserName
	,@Site
	,@IsAllowed
	,@Remark
	,GETDATE()
	,GETDATE()
	,@Creator
	,@ContactPerson
	,@ContactEmail
)

IF @@ERROR <> 0
BEGIN	
	SELECT @ErrMsg = N'Error! Insert partner unsuccessfully.', @ErrCode = 10050
	ROLLBACK TRANSACTION
	RETURN
END

SELECT @PartnerID = SCOPE_IDENTITY()


--DomainSetting
INSERT INTO [dbo].[Odds_DomainSetting]([PartnerId], [DomainId])
SELECT @PartnerId, Val From dbo.strSplit(@ListDomainId, ',')

if @@Error <> 0
BEGIN			
	SELECT @ErrMsg = 'Error! Insert partner unsuccessfully. Error when inserting domain list.', @ErrCode = 10051			
	ROLLBACK TRANSACTION
	RETURN
END
-----------------------------------

--IPSetting
DECLARE @Rank INT, @IPFrom VARCHAR(20), @IPTo VARCHAR(20)

SELECT @ErrMsg='';

SELECT @Rank = MIN(Rank) FROM dbo.strSplit(@ListIPFrom, ',')

WHILE @Rank IS NOT NULL
BEGIN
	SELECT @IPFrom = Val FROM dbo.StrSplit(@ListIPFrom, ',') WHERE Rank = @Rank
	SELECT @IPTo = Val FROM dbo.StrSplit(@ListIPTo, ',') WHERE Rank = @Rank

	IF LEN(@IPFrom) > 0 AND LEN(@IPFrom) > 0 
	BEGIN
		If (CONVERT(BIGINT,@IPTo) >= CONVERT(BIGINT,@IPFrom))
		BEGIN
			INSERT INTO [dbo].[Odds_IPSetting]([PartnerID], [IPFrom], [IPTo])
				VALUES(@PartnerID, CONVERT(BIGINT,@IPFrom), CONVERT(BIGINT,@IPTo))
			
			IF @@ERROR <> 0
			BEGIN	
				--Write log here and store errmsg in a string
				SELECT @ErrMsg = @ErrMsg + 'Error! Insert IP setting unsuccessful. PartnerID: '+@PartnerID+', IPFrom: ' + @IPFrom + ', IPTo: ' + @IPTo + ';', @ErrCode = 10052				
			END
		END
		ELSE
		BEGIN
			--Write log here and store errmsg in a string
			SELECT @ErrMsg = @ErrMsg + 'Error! Insert IP setting unsuccessful. PartnerID: '+@PartnerID+', IPFrom: ' + @IPFrom + ' is larger than  IPTo: ' + @IPTo + ';', @ErrCode = 10053
		END
	END

	DECLARE @OldRank INT
	
	SELECT @OldRank = @Rank
	SELECT @Rank = NULL-- Reset @Rank
	
	SELECT @Rank = MIN(Rank) From dbo.strSplit(@ListIPFrom, ',') WHERE Rank > @OldRank
END

COMMIT TRANSACTION

--Write the log 
DECLARE @SPName VARCHAR(50)
SELECT @SPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData NVARCHAR(4000)
SELECT @LogData = 'Add News Partner, PartnerId:' + dbo.Is_Odds_ToString(@PartnerId) +
		', SystemID: ' + dbo.Is_Odds_ToString(@SystemId) +
		', ProfileID: ' + dbo.Is_Odds_ToString(@ProfileID) +
		', Username: ' + dbo.Is_Odds_ToString(@Username) +
		', Site : ' + dbo.Is_Odds_ToString(@Site) +
		', IsAllow: ' + dbo.Is_Odds_ToString(@IsAllowed) +
		', Remark: ' + dbo.Is_Odds_ToString(@Remark) +
		', ContactPerson: ' + dbo.Is_Odds_ToString(@ContactPerson) +
		', ContactEmail ' + dbo.Is_Odds_ToString(@ContactEmail) +		
		', Creator: ' + dbo.Is_Odds_ToString(@Creator) +
		', ListDomainId: ' + dbo.Is_Odds_ToString(@ListDomainId) +
		', ListIPFrom: ' + dbo.Is_Odds_ToString(@ListIPFrom) +
		', ListIPTo: ' + dbo.Is_Odds_ToString(@ListIPTo) +
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
GRANT EXECUTE ON [dbo].[Is_Odds_PartnerIns] TO [bodbAdminNet]
GO
GRANT EXECUTE ON [dbo].[Is_Odds_PartnerIns] TO [vnteam]
GO