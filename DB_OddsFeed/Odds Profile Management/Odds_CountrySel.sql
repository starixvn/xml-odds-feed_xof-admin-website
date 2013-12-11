 set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Author:		Smith
-- Create date: 1/11/2011
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Is_Odds_CountrySel]
	-- Add the parameters for the stored procedure here
 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	select ResourceID as CountryCode, Resource_EN as CountryName
	from bodb02.dbo.refresource (nolock) where Refid='CountryCOde' 

END


GO
GRANT EXECUTE ON [dbo].[Is_Odds_CountrySel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_CountrySel] TO [vnteam]
GO


