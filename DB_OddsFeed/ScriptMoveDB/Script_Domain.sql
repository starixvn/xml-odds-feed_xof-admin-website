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