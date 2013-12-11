IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_RemainPartnerDomainSel')
	BEGIN
		DROP  PROCEDURE  [dbo].[Is_Odds_RemainPartnerDomainSel]
	END

GO

GO
CREATE PROCEDURE [dbo].[Is_Odds_RemainPartnerDomainSel]
	@PartnerID INT = 0,
	@IgnoreDomain VARCHAR(4000)=''	

AS
/*
Created: 20100322@Jacob
Task : Select all domains which have not been assigned to partner
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	- @PartnerID : partner id
	- @IgnoreDomain: Ex: ,1,2,
Description
	If @ParnerID=0 AND LEN(@IgnoreDomain)>0, Get all domains which is not in @IgnoreDomain
	else Get all domains which have not been assigned to partner
*/
SET NOCOUNT ON;

DECLARE @RowCount INT

IF LEN(@IgnoreDomain)>0 AND @PartnerID=0
BEGIN
	SELECT [DomainID]
			  ,[DomainName]
			  ,[CreatedDate]
			  ,[IP]
			  ,[ISP]
			  ,[Creator]
			  ,[LastUpdate]      
		FROM [dbo].[Odds_Domain] D WITH(NOLOCK)
		WHERE CHARINDEX(','+LTRIM(STR([DomainID]))+',',@IgnoreDomain)=0
END
ELSE
BEGIN	
	SELECT [DomainID]
			  ,[DomainName]
			  ,[CreatedDate]
			  ,[IP]
			  ,[ISP]
			  ,[Creator]
			  ,[LastUpdate]      
		FROM [dbo].[Odds_Domain] D WITH(NOLOCK)
		WHERE NOT EXISTS (SELECT 1
					  FROM [dbo].[Odds_DomainSetting] S WITH(NOLOCK)
					  WHERE S.[PartnerID]=@PartnerID AND S.[DomainID]=D.[DomainID]
					  )
END
				

GO
GRANT EXECUTE ON [dbo].[Is_Odds_RemainPartnerDomainSel] TO [bodbAdminNet]
GO
GRANT EXECUTE ON [dbo].[Is_Odds_RemainPartnerDomainSel] TO [vnteam]
GO