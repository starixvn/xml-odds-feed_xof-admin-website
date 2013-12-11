IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_IPSettingSel')
	BEGIN
		DROP  Procedure  [dbo].[Odds_IPSettingSel]
	END

GO



CREATE PROCEDURE [dbo].[Odds_IPSettingSel]
	@PartnerID SMALLINT = 0,	
	@IPFrom	BIGINT = NULL
AS
/*
Created: 20100322@Jacob
Task : Select IP Setting of a partner
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:	
	--@IPFrom: If null, select all	
*/

--Check Domain name
SET NOCOUNT ON;

SELECT [PartnerID],
	[IPFrom] ,
	[IPTo],
	[Country],
	[Region],
	[City],
	[ISP]
FROM [dbo].[PartnerIP] WITH(NOLOCK)
WHERE [PartnerId] = @PartnerID AND [IPFrom]=ISNULL(@IPFrom,[IPFrom])  

GO
GRANT EXECUTE ON [dbo].[Odds_IPSettingSel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Odds_IPSettingSel] TO [vnteam]
GO