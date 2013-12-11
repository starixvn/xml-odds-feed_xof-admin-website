IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Odds_DomainSel')
	BEGIN
		DROP  PROCEDURE  [dbo].[Odds_DomainSel]
	END

GO


CREATE PROCEDURE [dbo].[Odds_DomainSel]
	@DomainID SMALLINT = NULL
AS
/*
Created: 20130221@Phillip
Task : Select list Domain
DB: DB052.bodb_oddsfeed
Revisions:
	- 

Param's Explanation:
	- @DomainID : get all if @DomainId isnull
*/

SET NOCOUNT ON;

SELECT [DomainID]
      ,[DomainName]
      ,[CreatedDate]
      ,[IP]
      ,[ISP]
      ,[Creator]
      ,[LastUpdate]
FROM [dbo].[Domain] WITH(NOLOCK) inner join [dbo].[DomainSystem] WITH(NOLOCK) on [dbo].[Domain].[DomainID] = [dbo].[DomainSystem].[DomainID]
where [DomainId] = ISNULL(@DomainId, [DomainId])

GO
GRANT EXECUTE ON [dbo].[Odds_DomainSel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Odds_DomainSel] TO [vnteam]
GO
