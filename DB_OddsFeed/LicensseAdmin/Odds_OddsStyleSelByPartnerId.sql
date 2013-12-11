USE [bodb_oddsfeed]
GO
/****** Object:  StoredProcedure [dbo].[Odds_SystemSettingsSelByPartnerId]    Script Date: 01/23/2013 15:11:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Odds_OddsStyleSelByPartnerId]
	@PartnerID INT
AS
/*
Created: 20130121@Thai
Task : Select odds style by partnerId
* DB  : bodb_oddsfeed  
Revisions:
	- 

Param's Explanation:
	
*/

SET NOCOUNT ON;

Select OddsRegion from dbo.PartnerProfile
Where PartnerID = @PartnerID

GO
GRANT EXECUTE ON [Odds_OddsStyleSelByPartnerId] To [bodbAdminNet]
