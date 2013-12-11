USE [bodb_oddsfeed]
GO
/****** Object:  StoredProcedure [dbo].[Odds_PartnerCheckPermission]    Script Date: 03/01/2013 13:48:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Odds_PartnerCheckPermission]
	@Username varchar(50),
	@DomainName nvarchar(300),
	@IP varchar(30),	
	@SystemName varchar(20) = '' Output,    	
	@ErrCode int = 0 Output,
	@ErrMsg nvarchar(200) = '' Output
AS
/*
Created: 20130228@Thai
Task : Each time Partner request odd, we'll increate 1 to 'TotalRequest' field of the table PartnerStatistics to keep track of partner request odds
DB: bodb_oddsfeed
Revisions:
	- 20100320@Peter : Return converted timestamp value from Ken's request	

Param's Explanation:
	- @Username : Username of partner
	- @DomainName : Domain name access	
	- @IP : IP request Odds feed, IP is alway allowed: '202.78.228.8'	
Modify:
	- 20130704@Thai: not allow aff get xml oddsfeed link if its licensee was deactivated
*/
--Check Domain name
Declare @DomainId int, @SystemID int, @PartnerId int, @IPNum bigint, @LastRequest datetime, @IsAllowed bit       
select @IPNum = dbo.Is_Odds_IPConvertToNumber(@IP)

--Get partner's info
Select @PartnerId = PartnerId,@SystemID = SystemID, @LastRequest = LastRequest,@IsAllowed= IsAllowed
From [Partner] with (nolock)
where Username = @Username and SystemID = isnull(@SystemID,SystemID) -- and IsAllowed=1

-- Check if partner's licensee is deactivated
IF Not Exists(Select 1 from dbo.System with (nolock) where SystemID = @SystemID and IsActive = 1) 
BEGIN
	select @ErrCode = 10022, @ErrMsg = 'Sorry! You are not allowed to get this xml odds feed data at the moment. Please contact Administrator.'
	--return
END
ELSE
BEGIN

	select @SystemName = SystemName from [System] with (nolock)
	where SystemID = @SystemID

	select @DomainId = DomainId
	from Domain  with (nolock)
	where DomainName = @DomainName 
		  and DomainId in (select DomainId from dbo.PartnerDomain with (nolock) where PartnerId = @PartnerId)
		  and DomainId in (select DomainId from dbo.SystemDomain  with (nolock) where SystemId = @SystemID)	  
		
		
	--check partner id
	If isnull(@PartnerId,0) = 0
	Begin
			select @ErrCode = 10018, @ErrMsg = 'Invalid username or Your account is disabled now. Please contact with Administrator!'
			--return
	End
	Else	 
	--check allow
	If isnull(@IsAllowed,0) = 0
	Begin			
			select @ErrCode = 10019, @ErrMsg = 'Your account is disabled now. Please contact with Administrator!'
			--return		
	End
	Else
	--check IP belongs to Company, System and Partner Ips
	If (not exists (select (1) from dbo.CompanyIP with (nolock) where @IPNum between IPFrom and IpTo))
	BEGIN
		----check IP belongs to default Ips of IT Admin 			
		--if(not exists (select (1) from dbo.SystemSetting with (nolock) where [Type]='CompanyIP' and @IP like SValue))
		--BEGIN
		
			--check IP belongs to setting Ips for Licensee		
			if( not exists (select (1) from dbo.SystemIP with (nolock) where SystemID = @SystemID and @IPNum between IPFrom and IpTo ))
			Begin
				
				--Check IP range for Partner
			   if Not Exists(Select 1 from dbo.PartnerIP with (nolock) where PartnerId = @PartnerId and @IPNum between IPFrom and IpTo )
			   Begin
					 select @ErrCode = 10021, @ErrMsg = 'You are not allowed to get odds feed from your location.'
					 --return
			   End 		  		

			End
			
		--END
	 END
	 
	--Check privilegde with domain name
	--if isnull(@DomainId_tmp, 0) = 0 
	--Begin
	--	select @ErrCode = 10022, @ErrMsg = 'Invalid Domain name'
	--	--return								
	--End
	Else
	-- Check if partner's domain is allowed
	If Not Exists(Select 1 from dbo.PartnerDomain with (nolock) where PartnerId = @PartnerId and DomainId = @DomainId ) 
	Begin
		select @ErrCode = 10023, @ErrMsg = 'Sorry! You do not have permission to access this domain. Please contact administrator.'
		--return
	End
END

If @PartnerId <> 0 
Begin
	-- Get valid domainId to insert into table PartnerStatistics
	if isnull(@DomainId, 0) = 0 
	begin
		select @DomainId = DomainId 
		from Domain  with (nolock)
		where DomainName = @DomainName	
	end

	--update parnter request
	Update dbo.[Partner] with (rowlock, Updlock)
	set LastRequest = getdate()
	where PartnerId = @PartnerId

	--update Statistic
	if(not exists (	select 1 from dbo.PartnerStatistics with (nolock)
					where PartnerId = @PartnerId and DomainId = @DomainId and IP = @Ip and convert(varchar(10), [Date], 120) = convert(varchar(10), getdate(), 120)
				   ))
	begin -- insert new record
		Insert into PartnerStatistics(PartnerId, DomainId, Ip, [Date], LastCallTime, TotalRequest)
		Values (@PartnerId, @DomainId, @Ip, convert(varchar(10), getdate(), 120), Getdate(), 1)
	end
	else -- update totalrequest
	begin
		Update PartnerStatistics with(updlock)
		set TotalRequest = TotalRequest + 1
			,LastCallTime = getdate()
		where PartnerId = @PartnerId and DomainId = @DomainId and IP = @Ip and convert(varchar(10), [Date], 120) = convert(varchar(10), getdate(), 120)
	end
End

--Get Profile of this partner
SELECT *
FROM dbo.PartnerProfile WITH(NOLOCK)
WHERE PartnerID = @PartnerId

GO
GRANT EXECUTE ON [Odds_PartnerCheckPermission] To [bodbAdminnet]