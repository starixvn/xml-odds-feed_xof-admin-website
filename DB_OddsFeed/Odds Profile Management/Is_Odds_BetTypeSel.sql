 set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go




-- =============================================
-- Author:		Smith
-- Create date: 1/11/2011
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[Is_Odds_BetTypeSel]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @bettypes nvarchar(1000)
	
	set @bettypes = (select SValue 
					from SystemSettings 
					where [Type] = 'BetTypes')

	select bt.typeid, bt.typenamee from
	bodb02.dbo.bettype bt 
	where bt.typeid in (select val from dbo.strSplit(@bettypes,','))
	order by bt.typenamee
END

GO
GRANT EXECUTE ON [dbo].[Is_Odds_BetTypeSel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_BetTypeSel] TO [vnteam]
GO
