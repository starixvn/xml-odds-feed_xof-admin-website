 
-- =============================================
-- Author:		Smith
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[Is_Odds_GetOddsData]
	-- Add the parameters for the stored procedure here
	---@versionDate datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
     declare @versionDate datetime,@versionDateL datetime
     select @VersionDate=(select top(1) VersionDate from OutRightOdds (nolock))
     select @VersionDateL=(select top(1) VersionDate from LiveOdds (nolock))


    -- Insert statements for procedure here
	select * from DeadballOdds
	where VersionDate=@versionDate
	order by matchID 

	select * from LiveOdds
	where VersionDate=@VersionDateL
	order by matchID 

	select * from OutRightOdds
	where VersionDate=@versionDate
	order by leagueID,Odds 
END
GO
GRANT EXECUTE ON [dbo].[Is_Odds_GetOddsData] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_GetOddsData] TO [vnteam]
GO
