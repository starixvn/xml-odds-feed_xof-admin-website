IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_PartnerSel')
	BEGIN
		DROP  PROCEDURE  [dbo].[Is_Odds_PartnerSel]
	END

GO

CREATE PROCEDURE [dbo].[Is_Odds_PartnerSel]
	@PartnerID INT = 0	
AS
/*
Created: 20100122@Johnny
Task : Update Domain name
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	- @PartnerID : ID of Partner	
*/

--Check Domain name
SET NOCOUNT ON;

SELECT [PartnerID]
      ,[SystemID]
      ,[ProfileID]
      ,[UserName]
      ,[Site]
      ,[IsAllowed]
      ,[Remark]
      ,[CreatedDate]
      ,[LastUpdate]
      ,[LastRequest]
      ,[Creator]
      ,[ContactPerson]
      ,[ContactEmail]
FROM [dbo].[Odds_Partner] WITH(NOLOCK)
WHERE [PartnerId] = @PartnerID

GO
GRANT EXECUTE ON [dbo].[Is_Odds_PartnerSel] TO [bodbAdminNet]
GO
GRANT EXECUTE ON [dbo].[Is_Odds_PartnerSel] TO [vnteam]
GO