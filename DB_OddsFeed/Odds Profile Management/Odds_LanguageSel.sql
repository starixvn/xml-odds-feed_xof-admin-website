 set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


-- =============================================
-- Author:		Smith
-- Create date: 11/01/2011
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Is_Odds_LanguageSel] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Code,Name_EN
	FROM Languages (nolock)
	ORDER BY Name_EN
END

GO
GRANT EXECUTE ON [dbo].[Is_Odds_LanguageSel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_LanguageSel] TO [vnteam]
GO

