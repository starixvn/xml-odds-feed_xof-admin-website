 IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_StatisticsSel')
	BEGIN
		DROP  PROCEDURE  [dbo].[Is_Odds_StatisticsSel]
	END

GO

CREATE Procedure [Is_Odds_StatisticsSel]
	@PartnerId INT = null,
	@MonthDate smalldatetime--First date of the month
AS
/*
Created: 20100127@Johnny
Task : Statistics for partner
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	- @MonthDate
*/
Set Nocount On;


Declare @ToDate smalldatetime, @CurrDate smalldatetime, @MaxStaDate smalldatetime

Declare @Result as table(PartnerID smallint, DomainId smallint, ProfileID smallint, TotalRequest bigint)		

Select @CurrDate = convert(varchar(10), getdate(), 120)

If @MonthDate > @CurrDate
	return

Select @MaxStaDate = Max([Date]) From Odds_StatisticsSum nolock where isForMonth = 1

Select @MaxStaDate = isnull(@MaxStaDate, dateadd(d, -1, @MonthDate))

If(@MonthDate > @MaxStaDate)--Get data from Statistics table
Begin
	--Get data for current month
	Select @ToDate = case when datediff(m, @MonthDate, @CurrDate) = 0 then dateadd(d, -1, @CurrDate) else dateadd(d, -1, dateadd(m, 1, @monthDate)) end
	
	insert into @Result(PartnerId, DomainId, ProfileId, TotalRequest)
	Select PartnerId, DomainId, ProfileId, Sum(TotalRequest) as TotalRequest
	From
		(select PartnerID, DomainId, ProfileID, count(*) as TotalRequest--currentdate if monthdate has currentdate
		From Odds_Statistics with(nolock)
		Where datediff(m, @MonthDate, @CurrDate) = 0
			and isnull(@PartnerId, PartnerID) = PartnerID
			and date >= @CurrDate
		Group by PartnerID, DomainId, ProfileID
		
		Union All
		
		Select PartnerID, DomainId, ProfileID, TotalRequest--firstdate to currentdate
		From Odds_StatisticsSum nolock
		where isnull(@PartnerId, PartnerID) = PartnerID
			and IsForMonth = 0--daily statistics
			and [date] between @MonthDate and @ToDate 
		) tmp
	Group by PartnerId, DomainId, ProfileId
End
Else
Begin
	insert into @Result(PartnerId, DomainId, ProfileId, TotalRequest)
	Select PartnerId, DomainId, ProfileId, TotalRequest
	From Odds_StatisticsSum nolock
	Where IsForMonth = 1
		and [Date] = @MonthDate
End

Select p.Username, d.DomainName, pr.SPName, r.TotalRequest
From @Result r 
	inner join Odds_Partner p with(nolock) on r.PartnerID = p.PartnerId
	inner join Odds_Domain d with(nolock) on r.DomainId = d.DomainId
	inner join Odds_OddsProfile pr with(nolock) on r.ProfileId = pr.ProfileId

GO
GRANT EXECUTE ON [dbo].[Is_Odds_StatisticsSel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_StatisticsSel] TO [vnteam]
GO