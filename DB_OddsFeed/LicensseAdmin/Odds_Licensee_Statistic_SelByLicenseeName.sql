USE [bodb_oddsfeed]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
ALTER PROCEDURE [dbo].[Odds_Licensee_Statistic_Sel] 
	@LicenseeId int = null,   	
	@Systemname nvarchar(max) = '',   
	@PeriodOption varchar(10),
	@DateStart smalldatetime,  
	@DateEnd smalldatetime,
	@PageSize int =30,  
	@PageIndex int =1,  
	@PageCount int=0 OUTPUT,  
	@RowCount int =0 OUTPUT  
AS    
    
/**      
 * Created : 20130306@Thai  
 * Task  : Get licensee's statistic  
 * DB  : bodb_oddsfeed    
 *      
 * Revisions:         
 * Param's Explanation:     
 */    
BEGIN    
	SET NOCOUNT ON    
	declare @year varchar(4)
	set @year = convert(varchar(4), @DateStart, 120)
	
	IF ISNULL(@Systemname, '') = '' -- Get all requests group by licensee
	BEGIN
		-- build data
		create table #temp (SystemId int, SystemName nvarchar(50), [Date] varchar(20), TotalRequest int, [Order] int);
		
		If @PeriodOption = 'Date' or @PeriodOption = 'Month'
		Begin
			insert into #temp(SystemId, SystemName, [Date], TotalRequest)			
					select s.SystemId, s.SystemName, convert(varchar(20), ps.Date,106), sum(TotalRequest) as TotalRequest
					from dbo.System s with (nolock)
						inner join dbo.Partner p with (nolock) on s.SystemId = p.SystemId
						inner join dbo.PartnerStatistics ps with (nolock) on p.PartnerId = ps.PartnerId
					where ps.[Date] between @DateStart and @DateEnd 
					group by s.SystemId, s.SystemName, ps.Date
					order by s.SystemName, ps.Date
		End
		Else
		If @PeriodOption = 'Week'
		Begin
			insert into #temp(SystemId, SystemName, [Date], TotalRequest)			
					select s.SystemId, s.SystemName, convert(int,datepart(wk,date)) as [Week], sum(TotalRequest) as TotalRequest
					from dbo.System s with (nolock)
						inner join dbo.Partner p with (nolock) on s.SystemId = p.SystemId
						inner join dbo.PartnerStatistics ps with (nolock) on p.PartnerId = ps.PartnerId
					where ps.[Date] between @DateStart and @DateEnd 					
					GROUP BY s.SystemId, s.SystemName, convert(int ,datepart(wk,date))
					order by s.SystemName, [Week]
					
		End
		Else		
		If @PeriodOption = 'Year'
		Begin
			insert into #temp(SystemId, SystemName, [Date], TotalRequest, [Order])			
					select s.SystemId, s.SystemName, convert(varchar(3), ps.Date,107) as [Month], sum(TotalRequest) as TotalRequest, month(ps.Date)
					from dbo.System s with (nolock)
						inner join dbo.Partner p with (nolock) on s.SystemId = p.SystemId
						inner join dbo.PartnerStatistics ps with (nolock) on p.PartnerId = ps.PartnerId
					where ps.[Date] between @DateStart and @DateEnd 
					group by s.SystemId, s.SystemName, convert(varchar(3), ps.Date,107), month(ps.Date)
					order by s.SystemName, month(ps.Date)
		End
		
		-- paging
		SELECT @RowCount =count(1) FROM #temp
		SELECT @PageCount=@RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;  
		
		-- get data
		With News AS  
		( 
			SELECT top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY SystemId ASC) AS Row
				,SystemId, SystemName, Date, TotalRequest
			FROM #temp
		)		
		
		SELECT  [Row]
				, SystemId
				, SystemName
				, [Date] = case @PeriodOption when 'Year' then ([Date]  + ' ' + @year) else [Date] end
				, @year as [Year]
				, TotalRequest				
		FROM  News  
		WHERE  [Row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize  
		ORDER BY [Row]  
		
		drop table #temp

	END
	ELSE
	BEGIN
		-- build data
		create table #temp (SystemId int, SystemName nvarchar(50), [Date] varchar(20), TotalRequest int, [Order] int);
		
		If @PeriodOption = 'Date' or @PeriodOption = 'Month'
		Begin
			insert into #temp(SystemId, SystemName, [Date], TotalRequest)			
					select s.SystemId, s.SystemName, convert(varchar(20), ps.Date,106), sum(TotalRequest) as TotalRequest
					from dbo.System s with (nolock)
						inner join dbo.Partner p with (nolock) on s.SystemId = p.SystemId
						inner join dbo.PartnerStatistics ps with (nolock) on p.PartnerId = ps.PartnerId
					where ps.[Date] between @DateStart and @DateEnd and
						s.SystemName like '%' + ISNULL(@Systemname, '') + '%'
					group by s.SystemId, s.SystemName, ps.Date
					order by s.SystemName, ps.Date
		End
		Else
		If @PeriodOption = 'Week'
		Begin
			insert into #temp(SystemId, SystemName, [Date], TotalRequest)			
					select s.SystemId, s.SystemName, convert(int,datepart(wk,date)) as [Week], sum(TotalRequest) as TotalRequest
					from dbo.System s with (nolock)
						inner join dbo.Partner p with (nolock) on s.SystemId = p.SystemId
						inner join dbo.PartnerStatistics ps with (nolock) on p.PartnerId = ps.PartnerId
					where ps.[Date] between @DateStart and @DateEnd and
						s.SystemName like '%' + ISNULL(@Systemname, '') + '%'					
					GROUP BY s.SystemId, s.SystemName, convert(int ,datepart(wk,date))
					order by s.SystemName, [Week]
					
		End
		Else		
		If @PeriodOption = 'Year'
		Begin
			insert into #temp(SystemId, SystemName, [Date], TotalRequest, [Order])			
					select s.SystemId, s.SystemName, convert(varchar(3), ps.Date,107) as [Month], sum(TotalRequest) as TotalRequest, month(ps.Date)
					from dbo.System s with (nolock)
						inner join dbo.Partner p with (nolock) on s.SystemId = p.SystemId
						inner join dbo.PartnerStatistics ps with (nolock) on p.PartnerId = ps.PartnerId
					where ps.[Date] between @DateStart and @DateEnd and
						s.SystemName like '%' + ISNULL(@Systemname, '') + '%'
					group by s.SystemId, s.SystemName, convert(varchar(3), ps.Date,107), month(ps.Date)
					order by s.SystemName, month(ps.Date)
		End
		
		-- paging
		SELECT @RowCount =count(1) FROM #temp
		SELECT @PageCount=@RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;  
		
		-- get data
		With News AS  
		( 
			SELECT top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY SystemId ASC) AS Row
				,SystemId, SystemName, Date, TotalRequest
			FROM #temp
		)		
		
		SELECT  [Row]
				, SystemId
				, SystemName
				, [Date] = case @PeriodOption when 'Year' then ([Date]  + ' ' + @year) else [Date] end
				, @year as [Year]
				, TotalRequest				
		FROM  News  
		WHERE  [Row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize  
		ORDER BY [Row]  
		
		drop table #temp

	END
END    
GO

GRANT EXECUTE ON [Odds_Licensee_Statistic_Sel] To [bodbAdminnet]
