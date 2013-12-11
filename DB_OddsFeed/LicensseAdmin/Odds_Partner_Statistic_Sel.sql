USE [bodb_oddsfeed]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
ALTER PROCEDURE [dbo].[Odds_Partner_Statistic_Sel]        
 @SystemId int,
 @PartnerName nvarchar(Max) = '',  
 @DomainName nvarchar(Max) = '',  
 @DateStart smalldatetime,  
 @DateEnd smalldatetime,
 @PageSize int =30,  
 @PageIndex int =1,  
 @PageCount int=0 OUTPUT,  
 @RowCount int =0 OUTPUT   
AS    
    
/**      
 * Created : 20130220@Thai  
 * Task  : Get partner' statistic  
 * DB  : bodb_oddsfeed    
 *      
 * Revisions:         
 * Param's Explanation:     
 * Modify:
		- 20130319@Thai: Get more data from table "PartnerStatistics_Archive"
 */    
BEGIN    
 SET NOCOUNT ON    
    DECLARE @SystemName nvarchar(50)
    Select @SystemName = SystemName From dbo.System Where SystemID = @SystemId
    
	 -- paging
	SELECT @RowCount =count(1) FROM
	(
	 SELECT s.PartnerID, p.UserName, d.DomainName, s.IP, s.Date, s.LastCallTime, s.TotalRequest
	 FROM [dbo].[PartnerStatistics] s with (nolock)  
	 INNER JOIN [dbo].[Partner] p with (nolock) on s.PartnerID = p.PartnerID  
	 INNER JOIN dbo.Domain d with (nolock) on s.DomainID = d.DomainID  
	 WHERE 	
		p.SystemId = @SystemId and
		p.[UserName] like '%' + ISNULL(@PartnerName, p.[UserName]) + '%' and  
		d.[DomainName] like '%' + ISNULL(@DomainName,d.[DomainName]) + '%' and  
		s.[Date] between @DateStart and @DateEnd 
	
	UNION
	
	 SELECT s.PartnerID, p.UserName, d.DomainName, s.IP, s.Date, s.LastCallTime, s.TotalRequest
	 FROM dbo.PartnerStatistics_Archive s with (nolock)  
	 INNER JOIN [dbo].[Partner] p with (nolock) on s.PartnerID = p.PartnerID  
	 INNER JOIN dbo.Domain d with (nolock) on s.DomainID = d.DomainID  
	 WHERE 	
		p.SystemId = @SystemId and
		p.[UserName] like '%' + ISNULL(@PartnerName, p.[UserName]) + '%' and  
		d.[DomainName] like '%' + ISNULL(@DomainName,d.[DomainName]) + '%' and  
		s.[Date] between @DateStart and @DateEnd 
	) as RES

	SELECT @PageCount=@RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;  
		 
	-- build and get data
	With News AS  
	( 
		SELECT top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY RES.[UserName] ASC, RES.[DomainName] ASC) AS Row
			 ,RES.[PartnerID] as [PartnerID]
			 ,RES.[UserName] as [Partner]  
			 ,RES.[DomainName] as [Domain]  
			 ,RES.[IP]  
			 ,convert(varchar(15),RES.[Date],106) as [Date]
			 ,( convert(varchar(15), RES.[LastCallTime], 106) + '  ' + convert(varchar(15), RES.[LastCallTime], 108)) as [LastCallTime]
			 ,RES.[TotalRequest]  
		FROM (
				SELECT s.PartnerID, p.UserName, d.DomainName, s.IP, s.Date, s.LastCallTime, s.TotalRequest
				FROM [dbo].[PartnerStatistics] s with (nolock)  
				 INNER JOIN [dbo].[Partner] p with (nolock) on s.PartnerID = p.PartnerID  
				 INNER JOIN dbo.Domain d with (nolock) on s.DomainID = d.DomainID  
				 WHERE 	
					p.SystemId = @SystemId and
					p.[UserName] like '%' + ISNULL(@PartnerName, p.[UserName]) + '%' and  
					d.[DomainName] like '%' + ISNULL(@DomainName,d.[DomainName]) + '%' and    
					s.[Date] between @DateStart and @DateEnd   					
					
				UNION
				
				SELECT s.PartnerID, p.UserName, d.DomainName, s.IP, s.Date, s.LastCallTime, s.TotalRequest
				FROM dbo.PartnerStatistics_Archive s with (nolock)  
				 INNER JOIN [dbo].[Partner] p with (nolock) on s.PartnerID = p.PartnerID  
				 INNER JOIN dbo.Domain d with (nolock) on s.DomainID = d.DomainID  
				 WHERE 	
					p.SystemId = @SystemId and
					p.[UserName] like '%' + ISNULL(@PartnerName, p.[UserName]) + '%' and  
					d.[DomainName] like '%' + ISNULL(@DomainName,d.[DomainName]) + '%' and  
					s.[Date] between @DateStart and @DateEnd   
			) AS RES	
	)		

	SELECT  [Row]
			,[Partner]  
			,[Domain]  
			,[IP]  
			,[Des] = case 
						when exists (select 1 from dbo.CompanyIP where dbo.Is_Odds_IPConvertToNumber([IP]) between IPFrom and IPTo) then 'Company IP'
						when exists (select 1 from dbo.SystemIP where SystemID = @SystemId and dbo.Is_Odds_IPConvertToNumber([IP]) between IPFrom and IPTo) then @SystemName + ''' IP'
						when exists (select 1 from dbo.PartnerIP where PartnerID = News.[PartnerID] and dbo.Is_Odds_IPConvertToNumber([IP]) between IPFrom and IPTo) then [Partner] + ''' IP'
						else 'Unknown'
					 end
			,[Date]  
			,[LastCallTime]  
			,[TotalRequest]	
	FROM  News
	WHERE  [Row] BETWEEN (@PageIndex-1)*@PageSize + 1 and @PageIndex*@PageSize  
	ORDER BY [Row]
		  
END    
GO

GRANT EXECUTE ON [Odds_Partner_Statistic_Sel] To [bodbAdminnet]