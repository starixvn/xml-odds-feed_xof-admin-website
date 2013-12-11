/*
Des: Script to import bodb_Maintenance.dbo.Odds_OddsProfile --> bodb_oddsfeed.dbo.OddsSystemProfile
Author: Thai
CreateDate: 2013-04-09

*/

-- step1: remove data in bodb_oddsfeed.dbo.OddsSystemProfile
delete from bodb_oddsfeed.dbo.OddsSystemProfile

-- step2: copy data from bodb_Maintenance.dbo.Odds_OddsProfile --> bodb_oddsfeed.dbo.OddsSystemProfile

-- systemid =1
INSERT INTO [bodb_oddsfeed].[dbo].[OddsSystemProfile]
           ([SystemID]
           ,[CreatedDate]
           ,[Creator]
           ,[Editable]
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
           ,[DelayTimes]
           ,[NumberOfIPs]
           ,[OddsTypeFlag]
           ,[LanguageListFlag]
           ,[OddsRegionFlag]
           ,[SportIDListFlag]
           ,[BetTypeIDListFlag]
           ,[LeagueIDListFlag]
           ,[CountryCodeListFlag]
           ,[LeagueKeywordFlag]
           ,[LeagueKeyword_OFlag]
           ,[DelayTimesFlag])
           
SELECT 
	   top 1 a.[SystemID]
	  ,b.CreatedDate
	  ,b.[Creator]
	  ,1
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
      ,1000
      ,1
      ,1
      ,1
      ,1
      ,1
      ,1
      ,1
      ,1
      ,1
      ,1
  FROM  [bodb_Maintenance].[dbo].[Odds_Partner] a
    INNER JOIN [bodb_Maintenance].[dbo].[Odds_OddsProfile] b ON a.ProfileID = b.ProfileID
    where systemid =1
    
 -- systemid =2
INSERT INTO [bodb_oddsfeed].[dbo].[OddsSystemProfile]
           ([SystemID]
           ,[CreatedDate]
           ,[Creator]
           ,[Editable]
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
           ,[DelayTimes]
           ,[NumberOfIPs]
           ,[OddsTypeFlag]
           ,[LanguageListFlag]
           ,[OddsRegionFlag]
           ,[SportIDListFlag]
           ,[BetTypeIDListFlag]
           ,[LeagueIDListFlag]
           ,[CountryCodeListFlag]
           ,[LeagueKeywordFlag]
           ,[LeagueKeyword_OFlag]
           ,[DelayTimesFlag])
           
SELECT 
	   top 1 a.[SystemID]
	  ,b.CreatedDate
	  ,b.[Creator]
	  ,1
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
      ,1000
      ,1
      ,1
      ,1
      ,1
      ,1
      ,1
      ,1
      ,1
      ,1
      ,1
  FROM  [bodb_Maintenance].[dbo].[Odds_Partner] a
    INNER JOIN [bodb_Maintenance].[dbo].[Odds_OddsProfile] b ON a.ProfileID = b.ProfileID
    where systemid =2
    
-- systemid =5
INSERT INTO [bodb_oddsfeed].[dbo].[OddsSystemProfile]
           ([SystemID]
           ,[CreatedDate]
           ,[Creator]
           ,[Editable]
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
           ,[DelayTimes]
           ,[NumberOfIPs]
           ,[OddsTypeFlag]
           ,[LanguageListFlag]
           ,[OddsRegionFlag]
           ,[SportIDListFlag]
           ,[BetTypeIDListFlag]
           ,[LeagueIDListFlag]
           ,[CountryCodeListFlag]
           ,[LeagueKeywordFlag]
           ,[LeagueKeyword_OFlag]
           ,[DelayTimesFlag])
           
SELECT 
	   top 1 a.[SystemID]
	  ,b.CreatedDate
	  ,b.[Creator]
	  ,1
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
      ,1000
      ,1
      ,1
      ,1
      ,1
      ,1
      ,1
      ,1
      ,1
      ,1
      ,1
  FROM  [bodb_Maintenance].[dbo].[Odds_Partner] a
    INNER JOIN [bodb_Maintenance].[dbo].[Odds_OddsProfile] b ON a.ProfileID = b.ProfileID
    where systemid =5






