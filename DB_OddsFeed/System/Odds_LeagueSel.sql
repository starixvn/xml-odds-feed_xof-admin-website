USE [bodb_oddsfeed]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Odds_LeagueSel')
	BEGIN
		DROP  PROCEDURE  [dbo].[Odds_LeagueSel]
	END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Odds_LeagueSel]
	-- Add the parameters for the stored procedure here
    @SportIDList varchar(MAX) = null,	
	@CountryCodeList varchar(MAX) = null
AS
/*
 *	Created: 20130306@Phillip
 *	Task : Add System (Licensee) into Odds Feed - Licensee system
 *	DB: [bodb_oddsfeed]
 *  Description:
 *	Revisions:
*/
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET @SportIDList = ISNULL(@SportIDList, '')
	SET @CountryCodeList = ISNULL(@CountryCodeList, '')

	SET NOCOUNT ON;
	select distinct t.leagueid, t.leaguename
	from 
	(
		select distinct l.leagueid, l.leaguename
		from bodb02.dbo.match m with (nolock) 
			inner join bodb02.dbo.league l with (nolock)on l.leagueid=m.leagueid
		where m.eventstatus != 'completed' 
					and  l.isTest=0 and l.IsActive=1
					and m.sporttype in (select val from dbo.strSplit(@SportIDList,',')) 
                     and l.CountryCode in  (select val from dbo.strSplit(@CountryCodeList,','))  
	) t
	order by t.leaguename
    
END

GO
GRANT EXECUTE ON [dbo].[Odds_LeagueSel] TO [bodbAdminNet]
GO
GRANT EXECUTE ON [dbo].[Odds_LeagueSel] TO [vnteam]
GO