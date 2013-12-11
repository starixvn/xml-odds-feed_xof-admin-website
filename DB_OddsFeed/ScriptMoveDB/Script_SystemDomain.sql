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