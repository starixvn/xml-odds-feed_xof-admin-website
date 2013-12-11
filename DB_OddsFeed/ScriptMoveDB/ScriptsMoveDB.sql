/*
Des: Script to import datas from bodb_Maintenance --> bodb_oddsfeed
Author: Thai
CreateDate: 2013-04-09

*/
/*================================================================================*/
/*
Des: Script to import bodb_Maintenance.dbo.Odds_System --> bodb_oddsfeed.dbo.System
Author: Thai
CreateDate: 2013-04-09

*/

-- step1: remove data in bodb_oddsfeed.dbo.System
delete from bodb_oddsfeed.dbo.System

-- step2: copy data from  bodb_Maintenance.dbo.Odds_System --> bodb_oddsfeed.dbo.System
INSERT INTO [bodb_oddsfeed].[dbo].[System]
           ([SystemName]
           ,[Creator]
           ,[CreatedDate]
           ,[LastUpdate]
           ,[LicenseeID]
           ,[IsActive]
           ,OldSystemID)

SELECT [SystemName]
      ,[Creator]
      ,[CreatedDate]
      ,[LastUpdate]
      ,LicenseeID     
      ,1
      ,SystemID
  FROM [bodb_Maintenance].[dbo].[Odds_System] a
	
  INNER JOIN bodb_licensee.dbo.Licensees b ON a.SystemName = b.LicenseeName

/*================================================================================*/

/*
Des: Script to import bodb_Maintenance.dbo.Odds_OddsProfile --> bodb_oddsfeed.dbo.Partner
Author: Thai
CreateDate: 2013-04-09

*/

-- step1: remove data in bodb_oddsfeed.dbo.Partner
delete from bodb_oddsfeed.dbo.Partner

-- step2: copy data from  bodb_Maintenance.dbo.Odds_OddsProfile --> bodb_oddsfeed.dbo.Partner
INSERT INTO [bodb_oddsfeed].[dbo].[Partner]
           ([SystemID]
           ,[UserName]
           ,[Site]
           ,[IsAllowed]
           ,[Remark]
           ,[CreatedDate]
           ,[LastUpdate]
           ,[LastRequest]
           ,[Creator]
           ,[UpdatedBy]
           ,[ContactPerson]
           ,[ContactEmail]
           ,[IsDeleted]
           ,OldPartnerID)

SELECT
       [SystemID]      
      ,[UserName]
      ,[Site]
      ,[IsAllowed]
      ,[Remark]
      ,[CreatedDate]
      ,[LastUpdate]
      ,[LastRequest]
      ,[Creator]
      ,NULL as [UpdatedBy]
      ,[ContactPerson]
      ,[ContactEmail]
      ,[IsAllowed] -- note
      ,PartnerID
  FROM [bodb_Maintenance].[dbo].[Odds_Partner]

/*================================================================================*/
/*
Des: Script to import bodb_Maintenance.dbo.Odds_Domain --> bodb_oddsfeed.dbo.Domain
Author: Thai
CreateDate: 2013-04-09

*/

-- step1: remove data in bodb_oddsfeed.dbo.Domain
delete from bodb_oddsfeed.dbo.Domain

-- step2: copy data from bodb_Maintenance.dbo.Odds_Domain --> bodb_oddsfeed.dbo.Domain

INSERT INTO [bodb_oddsfeed].[dbo].[Domain]
           ([DomainName]
           ,[CreatedDate]
           ,[IP]
           ,[ISP]
           ,[Creator]
           ,[LastUpdate]
           ,[IsActive]
           ,[IsOddsDomain]
           ,OldDomainID)

SELECT [DomainName]      
      ,[CreatedDate]
      ,[IP]
      ,[ISP]
      ,[Creator]
      ,[LastUpdate]
	  ,1
	  ,NULL
	  ,DomainID
  FROM [bodb_Maintenance].[dbo].[Odds_Domain]

/*================================================================================*/
/*
Des: Script to import bodb_Maintenance.dbo.dbo.Odds_Domain --> bodb_oddsfeed.dbo.SystemDomain
Author: Thai
CreateDate: 2013-04-09

*/

-- step1: remove data in bodb_oddsfeed.dbo.SystemDomain
delete from bodb_oddsfeed.dbo.SystemDomain

-- step2: copy data from bodb_Maintenance.dbo.Odds_Domain --> bodb_oddsfeed.dbo.SystemDomain
INSERT INTO [bodb_oddsfeed].[dbo].[SystemDomain]
           ([SystemID]
           ,[DomainID])

SELECT	c.[SystemID]
		,b.[DomainID]      
  FROM [bodb_Maintenance].[dbo].[Odds_Domain] a
   INNER JOIN [bodb_oddsfeed].dbo.Domain b ON a.DomainID = b.OldDomainID
   INNER JOIN [bodb_oddsfeed].dbo.System c ON a.SystemID = c.OldSystemID
   
/*================================================================================*/
/*
Des: Script to import bodb_Maintenance.dbo.Odds_DomainSetting --> bodb_oddsfeed.dbo.PartnerDomain
Author: Thai
CreateDate: 2013-04-09

*/

-- step1: remove data in bodb_oddsfeed.dbo.PartnerDomain
delete from bodb_oddsfeed.dbo.PartnerDomain

-- step2: copy data from bodb_Maintenance.dbo.Odds_DomainSetting --> bodb_oddsfeed.dbo.PartnerDomain
INSERT INTO [bodb_oddsfeed].[dbo].[PartnerDomain]
           ([PartnerID]
           ,[DomainID])

SELECT c.[PartnerID]
      ,b.[DomainID]
  FROM [bodb_Maintenance].[dbo].[Odds_DomainSetting] a
  INNER JOIN [bodb_oddsfeed].dbo.Domain b ON a.DomainID = b.OldDomainID
  INNER JOIN [bodb_oddsfeed].dbo.Partner c ON a.[PartnerID] = c.OldPartnerID
  
/*================================================================================*/
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
    

/*================================================================================*/
/*
Des: Script to import bodb_Maintenance.dbo.Odds_IPSetting --> bodb_oddsfeed.dbo.PartnerIP
Author: Thai
CreateDate: 2013-04-09

*/

-- step1: remove data in bodb_oddsfeed.dbo.PartnerIP
delete from bodb_oddsfeed.dbo.PartnerIP

-- step2: copy data from bodb_Maintenance.dbo.Odds_IPSetting --> bodb_oddsfeed.dbo.PartnerIP
INSERT INTO [bodb_oddsfeed].[dbo].[PartnerIP]
           ([PartnerID]
           ,[IPFrom]
           ,[IPTo]
           ,[Country]
           ,[Region]
           ,[City]
           ,[ISP])

SELECT [PartnerID]
      ,[IPFrom]
      ,[IPTo]
      ,[Country]
      ,[Region]
      ,[City]
      ,[ISP]
  FROM [bodb_Maintenance].[dbo].[Odds_IPSetting]


/*================================================================================*/
					 
/*
Des: Script to import bodb_Maintenance.dbo.Odds_LogType --> bodb_oddsfeed.dbo.LogType
Author: Thai
CreateDate: 2013-04-09

*/

-- step1: remove data in bodb_oddsfeed.dbo.LogType
delete from bodb_oddsfeed.dbo.LogType

-- step2: copy data from bodb_Maintenance.dbo.Odds_LogType --> bodb_oddsfeed.dbo.LogType
INSERT INTO [bodb_oddsfeed].[dbo].[LogType]
           ([TypeName]
           ,[Description])
           
SELECT [TypeName]
      ,[Description]
  FROM [bodb_Maintenance].[dbo].[Odds_LogType]





