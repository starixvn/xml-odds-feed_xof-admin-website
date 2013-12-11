USE [bodb_oddsfeed]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Odds_LicenseeSel')
	BEGIN
		DROP  PROCEDURE  [dbo].[Odds_LicenseeSel]
	END

GO

CREATE PROCEDURE [dbo].[Odds_LicenseeSel]
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


SELECT [LicenseeID]
      ,[LicenseeName]
FROM bodb_Licensee.dbo.licensees WITH(NOLOCK)
WHERE [LicenseeID] not in (SELECT [LicenseeID] 
							FROM [dbo].[System] with (nolock) 
							WHERE [LicenseeID] is not null)


GO
GRANT EXECUTE ON [dbo].[Odds_LicenseeSel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Odds_LicenseeSel] TO [vnteam]
GO
