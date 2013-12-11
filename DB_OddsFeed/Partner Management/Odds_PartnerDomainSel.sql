IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_PartnerDomainSel')
	BEGIN
		DROP  PROCEDURE  [dbo].[Is_Odds_PartnerDomainSel]
	END

GO
CREATE PROCEDURE [dbo].[Is_Odds_PartnerDomainSel]
	@PartnerID INT = 0
AS
/*
Created: 20100322@Jacob
Task : Select list Domain of partner
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	- @PartnerID : partner id
*/
SET NOCOUNT ON
SELECT D.[DomainID]
      ,[DomainName]
      ,[CreatedDate]
      ,[IP]
      ,[ISP]
      ,[Creator]
      ,[LastUpdate]      
FROM [dbo].[Odds_Domain] D WITH(NOLOCK) INNER JOIN [dbo].[Odds_DomainSetting] S WITH(NOLOCK) ON D.[DomainID]=S.[DomainID]
WHERE [PartnerID] = @PartnerID

GO
GRANT EXECUTE ON [dbo].[Is_Odds_PartnerDomainSel] TO [bodbAdminNet]
GO
GRANT EXECUTE ON [dbo].[Is_Odds_PartnerDomainSel] TO [vnteam]
GO