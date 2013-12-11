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

