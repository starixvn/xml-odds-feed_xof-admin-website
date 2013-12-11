USE [bodb_oddsfeed]
GO
/****** Object:  StoredProcedure [dbo].[Odds_SystemSettingsSelByPartnerId]    Script Date: 01/23/2013 15:11:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].Odds_OddsStyleSelBySystemId
	@SystemID INT
AS
/*
Created: 20130121@Thai
Task : Select odds style by systemId
* DB  : bodb_oddsfeed  
Revisions:
	- 

Param's Explanation:
	
*/

SET NOCOUNT ON;

Select OddsRegion from dbo.OddsSystemProfile
Where SystemID = @SystemID

GO
GRANT EXECUTE ON Odds_OddsStyleSelBySystemId To [bodbAdminNet]
