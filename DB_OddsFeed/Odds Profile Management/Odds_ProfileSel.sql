IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_ProfileSel')
	BEGIN
		DROP  PROCEDURE  [dbo].[Is_Odds_ProfileSel]
	END

GO
CREATE PROCEDURE [dbo].[Is_Odds_ProfileSel]
	@ProfileID SMALLINT = NULL
AS
/*
Created: 20100122@Johnny
Task : Select list Domain
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	- @DomainID : get all if @DomainId isnull
*/
SET NOCOUNT ON;

SELECT [ProfileID]
      ,[ProfileName]
      ,[Description]
      ,[CreatedDate]
      ,[Creator]
      ,[IsDeleted]
      ,[LastUpdate]
      ,[TimeRange]
      ,[SPName]
      ,[SPParams]
      ,[OddsType]
      ,[LanguageList]
      ,[OddsRegion]
      ,[SportIDList]
      ,[BettypeIDList]
      ,[LeagueIDList]
      ,[CountryCodeList]
      ,[LeagueKeyword]
	  ,[LeagueKeyword_O]
FROM [dbo].[Odds_OddsProfile] WITH(NOLOCK)
WHERE [ProfileID] = ISNULL(@ProfileID, [ProfileID])
AND [IsDeleted]=0


GO
GRANT EXECUTE ON [dbo].[Is_Odds_ProfileSel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_ProfileSel] TO [vnteam]
GO