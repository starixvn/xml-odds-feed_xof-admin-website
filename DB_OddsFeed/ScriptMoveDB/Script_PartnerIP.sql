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




