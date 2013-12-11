USE [bodb_oddsfeed]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Odds_LicenseeByIDSel')
	BEGIN
		DROP  PROCEDURE  [dbo].[Odds_LicenseeByIDSel]
	END

GO

CREATE PROCEDURE [dbo].[Odds_LicenseeByIDSel]
	@SystemID SMALLINT
AS
/*
Created: 20130307@Phillip
Task : Select list licensee
DB: DBLIC.bodb_Licensee
Revisions:
	- 

Param's Explanation:
	- 
*/

SET NOCOUNT ON

SELECT *
FROM [dbo].[System] WITH(NOLOCK)
WHERE [SystemID] = @SystemID
GO
GRANT EXECUTE ON [dbo].[Odds_LicenseeByIDSel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Odds_LicenseeByIDSel] TO [vnteam]
GO
