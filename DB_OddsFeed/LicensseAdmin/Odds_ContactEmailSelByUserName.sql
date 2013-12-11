USE [bodb_oddsfeed]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Odds_ContactEmailSelByUserName')
	BEGIN
		DROP  PROCEDURE  [dbo].[Odds_ContactEmailSelByUserName]
	END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Odds_ContactEmailSelByUserName]	
	 @UserName varchar(50)
AS
/*
 *	Created: 20131016@Thai
 *	Task : Get contact email by partner name
 *	DB: [bodb_oddsfeed]
 *  Description:
 *	Revisions:
*/
BEGIN	    
    SET NOCOUNT ON;  
	Select ContactEmail From dbo.System
	Where SystemID = (Select SystemID From dbo.Partner Where UserName = @UserName)
END

GO
GRANT EXECUTE ON [dbo].[Odds_ContactEmailSelByUserName] TO [bodbAdminNet]
GO
GRANT EXECUTE ON [dbo].[Odds_ContactEmailSelByUserName] TO [vnteam]
GO