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