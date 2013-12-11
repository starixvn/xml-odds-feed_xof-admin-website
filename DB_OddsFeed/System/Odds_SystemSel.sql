IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Odds_SystemSel')
	BEGIN
		DROP  PROCEDURE  [dbo].[Odds_SystemSel]
	END

GO

CREATE PROCEDURE [dbo].[Odds_SystemSel]
	@SystemID SMALLINT = NULL
AS
/*
Created: 20100322@Jacob
Task : Select list system
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	- @SystemID : get all if @SystemID isnull
*/

SET NOCOUNT ON

SELECT [SystemID]
      ,[SystemName]      
      ,[CreatedDate]
      ,[LastUpdate]
      ,[Creator]
      
FROM [dbo].[System] WITH(NOLOCK)
WHERE [SystemID] = ISNULL(@SystemID, [SystemID])
	AND IsActive = 1

GO
GRANT EXECUTE ON [dbo].[Odds_SystemSel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Odds_SystemSel] TO [vnteam]
GO
