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



