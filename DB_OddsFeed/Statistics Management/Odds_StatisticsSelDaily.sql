Alter Procedure [Is_Odds_StatisticsSelDaily]
	@PartnerId INT,
	@MonthDate smalldatetime--First date of the month
AS
/*
Created: 20100127@Johnny
Task : Statistics of 
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	- @MonthDate
*/
Set Nocount On;

Declare @ToDate smalldatetime, @CurrDate smalldatetime, @MaxStaDate smalldatetime

Declare @Result as table(DateRequest smalldatetime, DomainId smallint, ProfileID smallint, TotalRequest bigint)		

Select @CurrDate = convert(varchar(10), getdate(), 120)

If @MonthDate > @CurrDate
	return
	
Select @MaxStaDate = Max([Date]) From Odds_StatisticsSum nolock where isForMonth = 1

Select @MaxStaDate = isnull(@MaxStaDate, dateadd(d, -1, @MonthDate))

If(@MonthDate > @MaxStaDate)--Get data from Statistics table
Begin
	--Get data for current month
	Select @ToDate = case when datediff(m, @MonthDate, @CurrDate) = 0 then dateadd(d, -1, @CurrDate) else dateadd(d, -1, dateadd(m, 1, @monthDate)) end
	
	insert into @Result(DateRequest, DomainId, ProfileId, TotalRequest)
	Select DateRequest, DomainId, ProfileId, Sum(TotalRequest) as TotalRequest
	From
		(select @CurrDate as DateRequest, DomainId, ProfileID, count(*) as TotalRequest--currentdate if monthdate has currentdate
		From Odds_Statistics with(nolock)
		Where datediff(m, @MonthDate, @CurrDate) = 0
			and PartnerID = @PartnerId
			and date >= @CurrDate
		Group by DomainId, ProfileID
		
		Union All
		
		Select [Date] as DateRequest, DomainId, ProfileID, TotalRequest--firstdate to currentdate
		From Odds_StatisticsSum nolock
		where PartnerID = @PartnerId
			and IsForMonth = 0--daily statistics
			and [date] between @MonthDate and @ToDate 
		) tmp
	Group by DateRequest, DomainId, ProfileId
End
Else
Begin
	insert into @Result(DateRequest, DomainId, ProfileId, TotalRequest)
	Select [Date] as DateRequest, DomainId, ProfileId, TotalRequest
	From Odds_StatisticsSum nolock
	Where IsForMonth = 0
		and [Date] = @MonthDate
End

Select DateRequest, d.DomainName, pr.SPName, r.TotalRequest
From @Result r 
	inner join Odds_Domain d with(nolock) on r.DomainId = d.DomainId
	inner join Odds_OddsProfile pr with(nolock) on r.ProfileId = pr.ProfileId

GO
GRANT EXECUTE ON [dbo].[Is_Odds_StatisticsSelDaily] TO [bodbAdminNet]
GO
GRANT EXECUTE ON [dbo].[Is_Odds_StatisticsSelDaily] TO [vnteam]
GO