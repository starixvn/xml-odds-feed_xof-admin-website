USE [bodb_oddsfeed]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Odds_User_Logout]
	@OnUserID INT
AS

/*  
 * Created: 20130124@Thai
 * Task : User logout
 * DB: [bodb_oddsfeed]
 *  
 * Revisions:     
 * Param's Explanation: 		
*/

BEGIN
	SET NOCOUNT ON

	DELETE FROM On_User WHERE OnUserID = @OnUserID
END

GO
GRANT EXECUTE ON [Odds_User_Logout] To [bodbAdminNet]