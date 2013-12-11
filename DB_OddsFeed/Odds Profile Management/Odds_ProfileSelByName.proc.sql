
ALTER PROCEDURE [dbo].[Is_Odds_ProfileSelByName]
	@userName nvarchar(256) = NULL
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

SET NOCOUNT ON;

declare @ProfileID int,@PartnerID int
select  @ProfileID = ProfileID,@PartnerID=PartnerID from Odds_Partner (nolock)
where UserName = @userName

print @ProfileID

declare @domain nvarchar(256)
select  top(1) @domain = DomainName
from Odds_Domain d (nolock)
inner join Odds_DomainSetting ds (nolock) on d.DomainID = ds.DomainID
where ds.PartnerID = @PartnerID
print @domain

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
	  ,DelayTimes
	  ,@domain as Domain
FROM [dbo].[Odds_OddsProfile] WITH(NOLOCK)
WHERE [ProfileID] = @profileid
AND [IsDeleted]=0




GO
GRANT EXECUTE ON [dbo].[Is_Odds_ProfileSelByName] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_ProfileSelByName] TO [vnteam]