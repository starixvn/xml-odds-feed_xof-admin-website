
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

SELECT a.[PartnerID]
      ,b.[DomainID]
  FROM [bodb_Maintenance].[dbo].[Odds_DomainSetting] a
  INNER JOIN [bodb_oddsfeed].dbo.Domain b ON a.DomainID = b.OldDomainID