Alter Procedure [dbo].[Task_Is_Odds_StatisticsSumMonthly]
	@Month int, 
	@Year int
AS
/*
Created: 20100203@Johnny
Task : Backup all group settings monthly on last day of selected month
DB: DB052.bodb_Maintenance
Original:

Revisions:		         
Param's Explanation:    

• @Month: The month to backup
• @Year :  The year to backup
*/
SET NOCOUNT ON

Declare @FirstDay smalldatetime, @LastDay smalldatetime


Select @FirstDay = cast(cast(@Year as varchar(5)) + '-' + cast(@Month as varchar(5)) + '-' +'1' as smalldatetime)
--Get last day of inputted month
Select @LastDay = DATEADD(s,-60,DATEADD(mm, DATEDIFF(m,0,@FirstDay)+1,0))
	

if datediff(d,Current_TimeStamp,@LastDay) > 0
BEGIN
	SELECT 'The month is not completed yet. So you cannot backup data!' Result
	SELECT @FirstDay as FirstDay, @LastDay as LastDay
	RETURN
END

--Check StatisticsSum table has data or not, this is occured if re-run this task
if(exists(select top(1) 1 from Odds_StatisticsSum (nolock) where [Date]=@FirstDay and IsForMonth = 1))
BEGIN
	delete Odds_StatisticsSum where [date]=@FirstDay and IsForMonth = 1
END

--Create data in table sum
Insert Into Odds_StatisticsSum(PartnerID, ProfileID, DomainID, IsForMonth, [Date], TotalRequest)
select PartnerID, ProfileID, DomainID, 1, @FirstDay, Sum(TotalRequest) as TotalRequest
from Odds_StatisticsSum (nolock)
where [date] between @FirstDay and @LastDay
	and IsForMonth = 0
Group by PartnerID, ProfileID, DomainID


--Copy data in old month to odds_Statistics_BK table
If(Exists(Select 1 from Odds_Statistics nolock where [Date] between @FirstDay and @LastDay))
Begin
	Insert Into Odds_Statistics_BK(PartnerID, ProfileID, DomainID, IP, [Date])
	select PartnerID, ProfileID, DomainID, IP, [Date]
	from Odds_Statistics (nolock)
	where [date] between @FirstDay and @LastDay

	Delete Odds_Statistics where [date] between @FirstDay and @LastDay
End


GO
GRANT VIEW DEFINITION ON [dbo].[Task_Is_Odds_StatisticsSumMonthly] TO [vnteam]