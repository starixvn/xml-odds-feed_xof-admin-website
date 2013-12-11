USE [bodb_oddsfeed]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Odds_LicenseeSelByPage')
	BEGIN
		DROP  PROCEDURE  [dbo].[Odds_LicenseeSelByPage]
	END

GO


CREATE PROCEDURE [dbo].[Odds_LicenseeSelByPage]
	@SystemName nvarchar(50) = null,
	@PageSize int =30,
	@PageIndex int =1,
	@PageCount int=0 OUTPUT,
	@RowCount int =0 OUTPUT
AS
/*
	* Created: 20130308 @Phillip
	* Task : OddsFeed Licensee Management
	* DB: DB052.bodb_oddsfeed
	* Description:Select all Domain
	* Modify:
		- Thai@20131015: Get 'ContactEmail' field
*/
SET NOCOUNT ON

SELECT @RowCount =count(1) 
FROM [dbo].[System] with (nolock)
WHERE [SystemName] LIKE '%' + ISNULL(@SystemName,'') + '%'

SELECT @PageCount=@RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;

WITH News AS
(
	SELECT top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY [SystemName] ASC) AS Row
	  ,[SystemID]
      ,[SystemName] 
	  ,s.[IsActive]     
      ,[CreatedDate]
      ,[LastUpdate]
      ,a.[Username] as [Creator]
      ,s.[ContactEmail]
	FROM [dbo].[System] s WITH(NOLOCK) LEFT JOIN [dbo].[AdminUser] a ON s.[Creator] = a.[UserID]
	WHERE [SystemName] LIKE '%' + ISNULL(@SystemName,'') + '%'
)

SELECT
     [Row]
    ,[SystemID]
    ,[SystemName]
	,[IsActive]
    ,[CreatedDate]
    ,[LastUpdate]
    ,[Creator]
    ,[ContactEmail]
FROM News
WHERE  [Row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize
ORDER BY [SystemName]

GO
GRANT EXECUTE ON [dbo].[Odds_LicenseeSelByPage] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Odds_LicenseeSelByPage] TO [vnteam]
GO