set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER PROCEDURE [dbo].[Odds_AdminList_Top10_Sel]	
	@SystemID INT = 0,
	@Username VARCHAR(50) = ''
AS
/*
 *	Created: 20133005@Louis
 *	Task : Get top 10 existed affiliate
 *	DB: [bodb_oddsfeed]
 *  Description:
 *	Revisions:		
	
	Param's Explanation:
	- @SystemID : system id
	- @Username : Username of admin
*/
BEGIN 

SET NOCOUNT ON

DECLARE @MaxResultRow int;
SET @MaxResultRow = 10;

IF(@Username = '') -- all users
BEGIN
	WITH News AS
	(
		select top 10  ROW_NUMBER() OVER (ORDER BY  a.username ASC, a.userid ASC) AS row
			, a.userid
			, a.username
			, a.roleid
			, u.rolename
			, a.firstname
			, a.lastname
			, a.email
			, c.username as creator
			, a.createdate
			, a.isactive
		from AdminUser a with (nolock)
			left join adminuser c with(nolock) on a.creator = c.userid
			left join userrole u with (nolock) on a.roleid = u.roleid
	)

	SELECT  [row]
			, userid
			, username
			, roleid
			, rolename
			, firstname
			, lastname
			, email
			, creator
			, createdate
			, isactive
	FROM News  	
	ORDER BY [row]
	END
	ELSE -- specific user
		BEGIN
			WITH News AS
			(
				select top 10  ROW_NUMBER() OVER (ORDER BY a.username ASC, a.userid ASC) AS row 
					,a.userid
					, a.username
					, a.roleid
					, u.rolename
					, a.firstname
					, a.lastname
					, a.email
					, c.username as creator
					, a.createdate
					, a.isactive
				from AdminUser a with (nolock)
					left join adminuser c with(nolock) on a.creator = c.userid
					left join userrole u with (nolock) on a.roleid = u.roleid
				where a.username like '%'+@Username+'%'
			)

			SELECT  [row]
					, userid
					, username
					, roleid
					, rolename
					, firstname
					, lastname
					, email
					, creator
					, createdate
					, isactive
			FROM News  
			ORDER BY [row]
		END
END		

GO
GRANT EXECUTE ON [Odds_AdminList_Top10_Sel] To [bodbAdminNet]