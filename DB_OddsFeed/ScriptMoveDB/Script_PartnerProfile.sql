/*
Des: Script to import bodb_Maintenance.dbo.Odds_OddsProfile --> bodb_oddsfeed.dbo.PartnerProfile
Author: Thai
CreateDate: 2013-04-09

*/

-- step1: remove data in bodb_oddsfeed.dbo.PartnerProfile
delete from bodb_oddsfeed.dbo.PartnerProfile

-- step2: copy data from bodb_Maintenance.dbo.Odds_OddsProfile --> bodb_oddsfeed.dbo.PartnerProfile
INSERT INTO [bodb_oddsfeed].[dbo].[PartnerProfile]
           ([PartnerID]
           ,[UpdatedBy]
           ,[LastUpdate]
           ,[OddsType]
           ,[LanguageList]
           ,[OddsRegion]
           ,[SportIDList]
           ,[BetTypeIDList]
           ,[LeagueIDList]
           ,[CountryCodeList]
           ,[LeagueKeyword]
           ,[LeagueKeyword_O]
           ,[DelayTimes])

SELECT a.[PartnerID]	  
	  ,b.[Creator]
	  ,b.[LastUpdate]
	  ,b.[OddsType]
	  ,b.[LanguageList]
      ,b.[OddsRegion]
      ,b.[SportIDList]
      ,b.[BetTypeIDList]
      ,b.[LeagueIDList]
      ,b.[CountryCodeList]
      ,b.[LeagueKeyword]
      ,b.[LeagueKeyword_O]
      ,b.[DelayTimes]            
  FROM  [bodb_Maintenance].[dbo].[Odds_Partner] a
  
  INNER JOIN [bodb_Maintenance].[dbo].[Odds_OddsProfile] b ON a.ProfileID = b.ProfileID
    





