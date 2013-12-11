IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Odds_DomainList_Top10_Sel')
	BEGIN
		DROP  PROCEDURE  [dbo].[Odds_DomainList_Top10_Sel]
	END
GO

Create PROCEDURE [dbo].[Odds_DomainList_Top10_Sel]
	@DomainName nvarchar(max) = ''
AS
/*
Created: 20130621@Louis.Nruyen
Task : Select top 10 of list Domain with specific name
DB: DB052.bodb_oddsfeed
Revisions:

Param's Explanation:
	- @DomainID : get all if @DomainId isnull
*/

SET NOCOUNT ON;

SELECT Top 10 [DomainID]
      ,[DomainName]
      ,[CreatedDate]
      ,[IP]
      ,[ISP]
      ,[Creator]
      ,[LastUpdate]
FROM [dbo].[Domain] WITH(NOLOCK) 	
where [DomainName] like '%' + ISNULL(@DomainName, '') + '%'
ORDER BY [DomainName]
GO
GRANT EXECUTE ON [dbo].[Odds_DomainList_Top10_Sel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Odds_DomainList_Top10_Sel] TO [vnteam]
GO
