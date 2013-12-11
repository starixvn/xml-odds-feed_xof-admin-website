ALTER PROCEDURE [dbo].[Is_Odds_PartnerCheckPermission]
	@Username varchar(50),
	@DomainName nvarchar(300),
	@IP varchar(15),
	@Result bit = 0 Output,
	@SPName varchar(100) = '' Output,
	@CacheTime int=0 output,
	@SystemName varchar(20) = '' Output,
    @ProfileID smallint=0 output,
	@TimeStamp int =0 output,	
	@ErrCode int = 0 Output,
	@ErrMsg nvarchar(200) = '' Output
AS
/*
Created: 20100127@Johnny
Task : Each time Partner request odd, we'll insert 1 row into statistics to keep track of partner request odds
DB: DB052.bodb_Maintenance
Revisions:
	- 20100320@Peter : Return converted timestamp value from Ken's request

Param's Explanation:
	- @Username : Username of partner
	- @DomainName : Domain name access
	- @PartnerID : return PartnerID		
	- @IP : IP request Odds feed, IP is alway allowed: '202.78.228.8'
	- @Result : OK or not
	- @SPName : Return SpName for partner call
*/
--Check Domain name
Declare @DomainId int, @IPNum bigint, @PartnerID int, @IPDefault varchar(15), @LastRequest datetime, @TimeRange int,
       @IsAllowed bit, @SystemID int,@DomainDefault varchar(200),@DomainDefault1 varchar(200),@IPDefault1 varchar(15),@IPDefault2 varchar(15)
       
 set @TimeStamp=dbo.f_DateTime_UnixTime(current_timestamp)       


declare @IPDefaultTable table(IPPattern varchar(30))
declare @DomainDefaultTable table(DomainPattern varchar(100))
Insert into @IPDefaultTable
select '202.78.228.%' 
union all 
select '202.78.230.%' 
union all 
select '61.218.85.%' 
union all 
select '218.211.34.%' 
union all 
select '211.75.184.%' 
union all 
Select '127.0.0.1' 


Insert into @DomainDefaultTable
select 'http://localhost%'
union all 
select 'http://dungtest%'
union all 
select 'http://wtest.sunplus.vn%'

select @IPNum = dbo.Is_Odds_IPConvertToNumber(@IP)

declare @count int
select @count=count(1) from Odds_Partner (nolock)
where Username = @Username

--If duplicate UserName we check System first based on Domain 
if(@count>=2)
begin
   Select @SystemID=SystemID from  Odds_Domain (nolock) where DomainName = @DomainName
end


Select @PartnerId = PartnerId,@SystemID = SystemID, @ProfileId = ProfileId, @LastRequest = LastRequest,@IsAllowed= IsAllowed
From Odds_Partner (nolock)
where Username = @Username and SystemID = isnull(@SystemID,SystemID) and IsAllowed=1

select @SystemName = SystemName from Odds_System (nolock) where SystemID = @SystemID

Select @DomainId = DomainId from Odds_Domain (nolock) where DomainName = @DomainName And SystemID = @SystemID

select @SPName = SPName,@CacheTime=TimeRange from Odds_OddsProfile(nolock) where ProfileId = @ProfileId

--Write log function
declare @StoreName varchar(50)
select @StoreName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData nvarchar(4000)
SELECT @LogData = 'Partner get Odds feed, PartnerId:' + dbo.Is_Odds_ToString(@PartnerId) +
		', DomainName ' + dbo.Is_Odds_ToString(@DomainName) + 
		', Ip:' + dbo.Is_Odds_ToString(@Ip)

EXEC [dbo].[Is_Odds_WriteLog]
	@Creator=@PartnerID,	
	@PartnerID = @PartnerID,
	@TypeName= 'Partner',	
	@SPName=@StoreName,
	@IP =@IP,
	@Detail=@LogData
	
--check partner id
if isnull(@PartnerId,0) = 0
 Begin
			select @ErrCode = 10018, @ErrMsg = 'Invalid username!'
			return
 End
--check allow
 if isnull(@IsAllowed,0) = 0
  Begin
	select @ErrCode = 10019, @ErrMsg = 'Your account is disabled now. Please contact with Administrator.'
			return
  End

-- Check profile
if (not exists(select (1) from Odds_OddsProfile(nolock) WHERE [ProfileID] = @ProfileID AND [IsDeleted]=0))
Begin
	select @ErrCode = 10020, @ErrMsg = 'Invalid profile. Please contact with Administrator.'
			return
  End

			
if( 
   not exists (select (1) from @IPDefaultTable where @IP like IPPattern)  
   and (not exists (select (1) from @DomainDefaultTable where @DomainName like DomainPattern))  
   )
 BEGIN
	
		--Check IP range		
      
       if Not Exists(Select 1 from Odds_IPSetting (nolock) where PartnerId = @PartnerId and @IPNum between IPFrom and IpTo )
	   Begin
		     select @ErrCode = 10021, @ErrMsg = 'You are not allowed to get odds feed from your location.'
		     return
	   End 
	   
	   --Check privilegde with domain name

		if isnull(@DomainId, 0) = 0 
		Begin
			select @ErrCode = 10022, @ErrMsg = 'Invalid Domain name'
			return
		End
		
		if Not Exists(Select 1 from Odds_DomainSetting (nolock) where PartnerId = @PartnerId and DomainId = @DomainId ) 
		Begin
			select @ErrCode = 10023, @ErrMsg = 'This Partner does not have permission to access this domain'
			return
		End

 END

Update Odds_Partner with (rowlock, Updlock)
set LastRequest = getdate()
where PartnerId = @PartnerId

Insert into Odds_Statistics(PartnerId, DomainId, ProfileId, Ip, Date)
Values (@PartnerId, @DomainId, @ProfileId, @Ip, Getdate())
--Get Profile of this partner
SELECT *
FROM [dbo].[Odds_OddsProfile] WITH(NOLOCK)
WHERE [ProfileID] = @ProfileID
AND [IsDeleted]=0

GO
GRANT EXECUTE ON [dbo].[Is_Odds_PartnerCheckPermission] TO [bodbAdminNet]	
GO
GRANT EXECUTE ON [dbo].[Is_Odds_PartnerCheckPermission] TO [vnteam]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_PartnerCheckPermission] TO [vnteam]