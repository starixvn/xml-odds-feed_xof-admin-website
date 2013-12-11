 IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_PartnerSPName')
	BEGIN
		DROP  PROCEDURE  [dbo].[Is_Odds_PartnerSPName]
	END

GO

CREATE Procedure [dbo].[Is_Odds_PartnerSPName]
	@ProfileId int,
	@Ip varchar(15),
	@SPName varchar(100) = '' Output
AS
/*
Created: 20100127@Johnny
Task : 
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	- @ProfileId
	- @SPName 
*/

--Get SPName of Partner
Select @SPName = SPName From Odds_OddsProfile (nolock) where ProfileId = @ProfileId 


GO
GRANT EXECUTE ON [dbo].[Is_Odds_PartnerSPName] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_PartnerSPName] TO [vnteam]
GO

