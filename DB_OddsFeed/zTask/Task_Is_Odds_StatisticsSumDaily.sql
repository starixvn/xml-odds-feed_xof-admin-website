Alter Procedure [dbo].[Task_Is_Odds_StatisticsSumDaily]
	@Date datetime
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

select @Date = convert(varchar(10), @Date, 120)

if datediff(d,Current_TimeStamp,@Date) > 0
BEGIN
	SELECT 'The day is not completed yet. So you cannot backup data!' Result
	SELECT @Date as StatisticDay
	RETURN
END

--Check group History data is inserted
if(exists(select top(1) 1 from Odds_StatisticsSum (nolock) where [Date]=@Date))
BEGIN
	delete Odds_StatisticsSum where [date]=@Date
END
 
Insert Into Odds_StatisticsSum(PartnerID, ProfileID, DomainID, IsForMonth, [Date], TotalRequest)
select PartnerID, ProfileID, DomainID, 0, @Date, Count(*) as TotalRequest
from Odds_Statistics (nolock)
where datediff(d, @Date, [Date]) = 0
Group by PartnerID, ProfileID, DomainID


GO
GRANT VIEW DEFINITION ON [dbo].[Task_Is_Odds_StatisticsSumDaily] TO [vnteam]