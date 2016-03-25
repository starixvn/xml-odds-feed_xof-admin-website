USE [bodb_oddsfeed]
GO

ALTER PROCEDURE [dbo].[Odds_GetProfileAllPartnersBySystemId]
	@SystemID SMALLINT,	
	@AffsOddsType varchar(max)= '' OUTPUT,
	--@AffsOddsRegion varchar(max) OUTPUT,
	@AffsLanguageList varchar(max) = '' OUTPUT,	
	@AffsSportIDList varchar(max)  = '' OUTPUT,
	@AffsCountryCodeList varchar(max) = '' OUTPUT,
	@AffsLeagueIDList varchar(max)  = '' OUTPUT,	
	@AffsOutrightIDList varchar(max)  = '' OUTPUT,
	@AffsBettypeIDList varchar(max)  = '' OUTPUT,	
	@ErrCode INT  = 0 OUTPUT,
	@ErrMsg NVARCHAR(200) = '' OUTPUT
AS
/*
 *	Created: 20130821@Thai
 *	Task : Get profile of all partners under the current licensee 
 *	DB: [bodb_oddsfeed]
 *  Description:
			If user removes some selections that some affiliates under this licensee have the number of selections greater than the current licensee's profile, we must display a warning message
			 " You are removing some selections that affiliates under this licensee are available. Are you sure to remove these selections for all affiliates' odds profile also?"
 *	Revisions:
 *	- 20160315@bobby.nguyen: #54093 - Update for showing only parent bet types and update parent - child bet types
 * 
 *  Modify: 
*/		
BEGIN
	SET NOCOUNT ON
	Declare @temp varchar(50)

	--Check Licensee name
	IF NOT EXISTS(SELECT 1 FROM [dbo].[System] WITH(NOLOCK) WHERE [SystemID] = @SystemID)
	BEGIN
		SELECT @ErrCode = 10501, @ErrMsg = 'Error! The licensee name was not existed.'
		RETURN
	END

	-- Get the profiles of the licensee's affiliates

	-- Odds Type
	select @AffsOddsType = @AffsOddsType + ',' + OddsType from dbo.PartnerProfile
								where PartnerID in (select PartnerID from dbo.Partner where IsDeleted = 0 and SystemID = @SystemID)
							
	-- Language
	select @AffsLanguageList = @AffsLanguageList + ',' + LanguageList from dbo.PartnerProfile
								where PartnerID in (select PartnerID from dbo.Partner where IsDeleted = 0 and SystemID = @SystemID)

	-- Sport
	select @AffsSportIDList = @AffsSportIDList + ',' + SportIDList from dbo.PartnerProfile
								where PartnerID in (select PartnerID from dbo.Partner where IsDeleted = 0 and SystemID = @SystemID)							

	-- Country
	select @AffsCountryCodeList = @AffsCountryCodeList + ',' + CountryCodeList from dbo.PartnerProfile
								where PartnerID in (select PartnerID from dbo.Partner where IsDeleted = 0 and SystemID = @SystemID)							

	select @AffsLeagueIDList = @AffsLeagueIDList + ',' + LeagueIDList from dbo.PartnerProfile
								where PartnerID in (select PartnerID from dbo.Partner where IsDeleted = 0 and SystemID = @SystemID)							

	-- Outright
	select @AffsOutrightIDList = @AffsOutrightIDList + ',' + OutrightIDList from dbo.PartnerProfile
								where PartnerID in (select PartnerID from dbo.Partner where IsDeleted = 0 and SystemID = @SystemID)							

	-- Bettype
	select @AffsBettypeIDList = @AffsBettypeIDList + ',' + [dbo].[f_Odds_GroupBettype_Format] (BetTypeIDList, 0) from dbo.PartnerProfile
								where PartnerID in (select PartnerID from dbo.Partner where IsDeleted = 0 and SystemID = @SystemID)							
					
END
GO
GRANT EXECUTE ON [dbo].Odds_GetProfileAllPartnersBySystemId TO [bodbAdminNet]
GO
GRANT EXECUTE ON [dbo].Odds_GetProfileAllPartnersBySystemId TO [vnteam]
GO
