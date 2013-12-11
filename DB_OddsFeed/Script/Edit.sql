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
CREATE TABLE dbo.Tmp_Odds_Domain
	(
	DomainID smallint NOT NULL IDENTITY (1, 1),
	DomainName nvarchar(300) NULL,
	SystemID smallint NULL,
	CreatedDate datetime NULL,
	IP varchar(23) NULL,
	ISP varchar(256) NULL,
	Creator smallint NULL,
	LastUpdate datetime NULL
	)  ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_Odds_Domain ON
GO
IF EXISTS(SELECT * FROM dbo.Odds_Domain)
	 EXEC('INSERT INTO dbo.Tmp_Odds_Domain (DomainID, DomainName, CreatedDate, IP, ISP, Creator, LastUpdate)
		SELECT DomainID, DomainName, CreatedDate, IP, ISP, Creator, LastUpdate FROM dbo.Odds_Domain WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Odds_Domain OFF
GO
DROP TABLE dbo.Odds_Domain
GO
EXECUTE sp_rename N'dbo.Tmp_Odds_Domain', N'Odds_Domain', 'OBJECT' 
GO
ALTER TABLE dbo.Odds_Domain ADD CONSTRAINT
	PK_Domain PRIMARY KEY CLUSTERED 
	(
	DomainID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
