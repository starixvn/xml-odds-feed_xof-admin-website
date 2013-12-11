 set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go





-- =============================================
-- Author:		Smith
-- Create date: 1/11/2011
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Is_Odds_SportTypeSel]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @sporttypes nvarchar(1000)
	
	set @sporttypes = (select SValue 
					from SystemSettings  (nolock)
					where [Type] = 'SportTypes')

	select s.SportType, s.SportName
    from bodb02.dbo.Sports s with (nolock) 
	where s.SportType in (select val from dbo.strSplit(@sporttypes,','))
    order by s.SportName
END


GO
GRANT EXECUTE ON [dbo].[Is_Odds_SportTypeSel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_SportTypeSel] TO [vnteam]
GO




