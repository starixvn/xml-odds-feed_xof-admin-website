set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Author:		Smith
-- Create date: 1/11/2011
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[Is_Odds_LeagueSel]
	-- Add the parameters for the stored procedure here
    @SportIDList varchar(MAX),	
	@CountryCodeList varchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	select distinct t.leagueid, t.leaguename,t.CountryCode,t.IsOutRight 
	from 
	(
		
		select distinct l.leagueid, l.leaguename,l.CountryCode, 0 as  IsOutRight
		from bodb02.dbo.match m with (nolock) 
			inner join bodb02.dbo.league l with (nolock)on l.leagueid=m.leagueid
		where m.eventstatus != 'completed' 
					and  l.isTest=0 and l.IsActive=1
					and m.sporttype in (select val from dbo.strSplit(@SportIDList,',')) 
                     and l.CountryCode in  (select val from dbo.strSplit(@CountryCodeList,','))  
				
        union all
		select distinct l.leagueid, l.leaguename,l.CountryCode, 1 as IsOutRight
		from bodb02.dbo.outright o WITH(NOLOCK)
			inner join bodb02.dbo.league l with (nolock)on l.leagueid=o.leagueid
		where o.oddsstatus != 'completed'
					and  l.isTest=0 and l.IsActive=1
					and l.sporttype in (select val from dbo.strSplit(@SportIDList,',') )
					and l.CountryCode in  (select val from dbo.strSplit(@CountryCodeList,','))  
		
		
	) t
	order by t.IsOutRight,t.leaguename
    
END


GO
GRANT EXECUTE ON [dbo].[Is_Odds_LeagueSel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_LeagueSel] TO [vnteam]
GO


