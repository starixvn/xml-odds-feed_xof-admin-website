IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Odds_DomainSelByPage')
	BEGIN
		DROP  PROCEDURE  [dbo].[Odds_DomainSelByPage]
	END

GO

CREATE PROCEDURE [dbo]. [Odds_DomainSelByPage]
	@PageSize int =30,
	@PageIndex int =1,
	@PageCount int=0 OUTPUT,
	@RowCount int =0 OUTPUT,
	@DomainName nvarchar(max) = ''
AS
/*
Created: 20100318 @Natalia
Task : OddsFeed Domain Management
DB: DB052.bodb_Maintenance

Select all Domain
*/
SET NOCOUNT ON

SELECT @RowCount =count(1) 
FROM Domain d with (nolock)
INNER JOIN [dbo].[AdminUser] a WITH(NOLOCK) ON d.[Creator] = a.[UserID]
WHERE d.[DomainName] like '%' + ISNULL(@DomainName, '') + '%'

SELECT @PageCount=@RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;

WITH News AS
(
	SELECT top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY d.[DomainName], d.DomainID ASC) AS Row
		  ,d.[DomainID]
		  ,d.[DomainName]
		  ,d.[CreatedDate]
		  ,d.[IP]
		  ,d.[ISP]
		  ,a.[Username] as [Creator]
		  ,d.[LastUpdate]
	FROM Domain d  WITH(NOLOCK) INNER JOIN [dbo].[AdminUser] a WITH(NOLOCK) ON d.[Creator] = a.[UserID]
	WHERE d.[DomainName] like '%' + ISNULL(@DomainName, '') + '%'
)
SELECT	   [Row]
		  ,[DomainID]
		  ,[DomainName]
		  ,[CreatedDate]
		  ,[IP]
		  ,[ISP]
		  ,[Creator]
		  ,[LastUpdate]
FROM News
WHERE [Row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize
ORDER BY [Row]

GO
GRANT EXECUTE ON [dbo].[Odds_DomainSelByPage] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Odds_DomainSelByPage] TO [vnteam]
GO