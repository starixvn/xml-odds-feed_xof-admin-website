 /*
   Deploy script on 19/03/2010
     - Change the data type of the column Date on table Odds_Statistics from smalldatetime -> datetime
     - Change the data type of the column Date on table Odds_Statistics_BK from smalldatetime -> datetime
     - Remove live odds for all profile
     - Add new profile : 
        OddsFeed_Share_OddsSel_12BET.sql  
     - Redeploy these sps from source control:
		OddsFeed_AffFull_OddsSel.sql
        OddsFeed_BetBrain_OddsSel.sql
        OddsFeed_BetRadar_OddsSel.sql  
        OddsFeed_common_OddsSel.sql  
        OddsFeed_commonfull_OddsSel.sql 
        OddsFeed_mrlazypunter_OddsSel.sql
        OddsFeed_Score22_OddsSel.sql
        OddsFeed_TipEx_OddsSel.sql
        
     - Run this script:
          Is_Odds_InsertDefaultData.sql   
 */
 
 /*
   Friday, March 19, 20101:39:12 PM
   User: sa
   Server: 10.20.20.211
   Database: bodb_Maintenance
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Odds_Statistics_BK
	(
	PartnerID smallint NULL,
	DomainID smallint NULL,
	ProfileID smallint NULL,
	IP varchar(23) NULL,
	Date datetime NULL
	)  ON [PRIMARY]
GO
IF EXISTS(SELECT * FROM dbo.Odds_Statistics_BK)
	 EXEC('INSERT INTO dbo.Tmp_Odds_Statistics_BK (PartnerID, DomainID, ProfileID, IP, Date)
		SELECT PartnerID, DomainID, ProfileID, IP, CONVERT(datetime, Date) FROM dbo.Odds_Statistics_BK WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.Odds_Statistics_BK
GO
EXECUTE sp_rename N'dbo.Tmp_Odds_Statistics_BK', N'Odds_Statistics_BK', 'OBJECT' 
GO
COMMIT

/*
   Friday, March 19, 20101:37:05 PM
   User: sa
   Server: 10.20.20.211
   Database: bodb_Maintenance
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Odds_Statistics
	(
	PartnerID smallint NULL,
	DomainID smallint NULL,
	ProfileID smallint NULL,
	IP varchar(23) NULL,
	Date datetime NULL
	)  ON [PRIMARY]
GO
IF EXISTS(SELECT * FROM dbo.Odds_Statistics)
	 EXEC('INSERT INTO dbo.Tmp_Odds_Statistics (PartnerID, DomainID, ProfileID, IP, Date)
		SELECT PartnerID, DomainID, ProfileID, IP, CONVERT(datetime, Date) FROM dbo.Odds_Statistics WITH (HOLDLOCK TABLOCKX)')
GO
DROP TABLE dbo.Odds_Statistics
GO
EXECUTE sp_rename N'dbo.Tmp_Odds_Statistics', N'Odds_Statistics', 'OBJECT' 
GO
COMMIT
