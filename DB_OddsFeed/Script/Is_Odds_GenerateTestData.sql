---*****************Beginning of Script add new Profile***********************---
Declare @ProfileID smallint,
	@ErrCode int,
	@ErrMsg nvarchar(200)

Select @ProfileID = 0,
	@ErrCode = 0,
	@ErrMsg = ''

EXEC Is_Odds_ProfileIns @ProfileID = @ProfileID Output,
	@ProfileName = '1 x 2',
	@Description = 'test thử về profile',
	@Creator = 8,
	@TimeRange = 60,
	@SPName = 'OddsFeed1x2',
	@IP = '127.0.0.5',
	@ErrCode = @ErrCode output,
	@ErrMsg = @ErrMsg output


Select 'Insert new profile' as FunctionName, @ErrCode as ErrCode, @ErrMsg as ErrMsg, @ProfileID as ProfileID
---*****************End of Script add new Profile***********************---

---*****************Beginning of Script add new Domain***********************---
Declare @DomainID smallint,
	@ErrCode int,
	@ErrMsg nvarchar(200)

Select @DomainID = 0,
	@ErrCode = 0,
	@ErrMsg = ''

EXEC Is_Odds_DomainIns @DomainID = @DomainID Output,
	@DomainName = 'portal.12bet.com',
	@Creator = 8,	
	@IP = '127.0.0.1',
	@ISP = 'FPT Telecom',
	@ErrCode = @ErrCode Output,
	@ErrMsg = @ErrMsg Output

Select 'Insert new Domain' as FunctionName, @ErrCode as ErrCode, @ErrMsg as ErrMsg, @DomainID as DomainID
---*****************End of Script add new Domain***********************---

---*****************Beginning of Script add new Partner***********************---
Declare @PartnerID smallint,
		@ErrCode int,
		@ErrMsg nvarchar(4000)
		
Select @PartnerID = 0,
		@ErrCode = 0,
		@ErrMsg = ''

Exec Is_Odds_PartnerIns	
		@PartnerID = @PartnerID Output,
		@SystemID = 2,	
		@ProfileID = 1,	
		@Username = 'testnewuser1211hfdgh2fdsf312',
		@Site = '',
		@IsAllowed = 1,
		@Remark = 'hello world',
		@Creator = 10,
		@ListDomainId = '1,2',--Each DomainID is separate by comma ','
		@ListIPFrom = '192.168.1.3,192.168.1.3',
		@ListIPTo = '192.168.1.0,192.168.1.5',
		@IP = '127.0.0.1'
		@ErrCode = @ErrCode Output,
		@ErrMsg = @ErrMsg Output
		
Select @PartnerId as PartnerId, @ErrCode as ErrCode, @ErrMsg as ErrMsg

Select * from odds_partner nolock
---*****************End of script add new Partner***********************---

---*****************Beginning of Script Update Partner Info***********************---
Declare @PartnerID smallint,
		@ErrCode int,
		@ErrMsg nvarchar(4000)
		
Select @PartnerID = 0,
		@ErrCode = 0,
		@ErrMsg = ''

Exec Is_Odds_PartnerUpdInfo	
		@PartnerID = 5,
		@SystemID = 3,	
		@ProfileID = 1,	
		@Site = 'test.com',
		@IsAllowed = 1,
		@Remark = 'In god we trust',
		@Creator = 5,
		@ErrCode = @ErrCode Output,
		@ErrMsg = @ErrMsg Output
		
Select @PartnerId as PartnerId, @ErrCode as ErrCode, @ErrMsg as ErrMsg

Select * from odds_partner nolock
---*****************End of script Update Partner Info***********************---

---*****************Beginning of Script Update Domain setting of Partner***********************---
Declare @PartnerID smallint,
		@ErrCode int,
		@ErrMsg nvarchar(4000)
		
Select @PartnerID = 0,
		@ErrCode = 0,
		@ErrMsg = ''

Exec Is_Odds_PartnerUpdDomainSetting	
		@PartnerID = 5,
		@ListDomainId = '1,2,3,4,5',	
		@IP = '198.276.309.123',
		@Creator = 5,
		@ErrCode = @ErrCode Output,
		@ErrMsg = @ErrMsg Output
		
Select @PartnerId as PartnerId, @ErrCode as ErrCode, @ErrMsg as ErrMsg

Select * from odds_DomainSetting nolock
---*****************End of script Update Domain setting of Partner***********************---