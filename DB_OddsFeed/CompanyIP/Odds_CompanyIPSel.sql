USE [bodb_oddsfeed]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Odds_CompanyIPSel')
	BEGIN
		DROP  PROCEDURE  [dbo].[Odds_CompanyIPSel]
	END

GO

CREATE PROCEDURE [dbo].[Odds_CompanyIPSel]
AS
/*
Created: 20130326@Phillip
Task : Select list Company IPs
DB: DBODDSFEED
Revisions:
	- 

Param's Explanation:
	- 
*/

SET NOCOUNT ON

SELECT dbo.Is_Odds_NumberConvertToIP([IPFrom]) as [IPFrom]
      ,dbo.Is_Odds_NumberConvertToIP([IPTo]) as [IPTo]
	  ,[Description]
FROM [dbo].[CompanyIP] WITH(NOLOCK)
ORDER BY [LastUpdate]

GO
GRANT EXECUTE ON [dbo].[Odds_CompanyIPSel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Odds_CompanyIPSel] TO [vnteam]
GO
