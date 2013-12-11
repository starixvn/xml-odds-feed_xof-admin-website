USE [bodb_oddsfeed]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Odds_OutRightsSel')
	BEGIN
		DROP  PROCEDURE  [dbo].[Odds_OutRightsSel]
	END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Odds_OutRightsSel]
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
	SET NOCOUNT ON;

	SET @SportIDList = ISNULL(@SportIDList, '')
	SET @CountryCodeList = ISNULL(@CountryCodeList, '')

	select distinct t.leagueid, t.leaguename
	from 
	(
		select distinct l.leagueid, l.leaguename
		from bodb02.dbo.outright o WITH(NOLOCK)
			inner join bodb02.dbo.league l with (nolock)on l.leagueid=o.leagueid
		where o.oddsstatus != 'completed'
					and  l.isTest=0 and l.IsActive=1
					and l.sporttype in (select val from dbo.strSplit(@SportIDList,',') )
					and l.CountryCode in  (select val from dbo.strSplit(@CountryCodeList,','))
	) t
	order by t.leaguename
    
END

GO
GRANT EXECUTE ON [dbo].[Odds_OutRightsSel] TO [bodbAdminNet]
GO
GRANT EXECUTE ON [dbo].[Odds_OutRightsSel] TO [vnteam]
GO