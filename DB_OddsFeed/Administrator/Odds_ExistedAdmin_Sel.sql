set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


ALTER PROCEDURE [dbo].[Odds_ExistedAdmin_Sel]
	@Username VARCHAR(50),
	@ErrCode INT  = 0 OUTPUT,
	@ErrMsg NVARCHAR(200) = '' OUTPUT
AS  
  
/**    
 * Created : 20130312@Long
 * Task  : Get existed administrator
 * DB  : bodb_oddsfeed,bodb02, bodb_Licensee 
 *    
 * Revisions:       
 * Param's Explanation:   
 */  
SET NOCOUNT ON;

IF EXISTS(SELECT TOP 1 *
			FROM bodb02.dbo.customer c with(nolock)
			WHERE c.roleid=6 and c.username=@Username)
BEGIN
	SELECT c.username, c.firstname, c.lastname, c.email, c.userpwd as password, '1' as roleid
	FROM bodb02.dbo.customer c with(nolock)
	WHERE c.roleid=6 and c.username=@Username
	ORDER BY c.username
END
ELSE
BEGIN
	IF EXISTS(SELECT TOP 1 *
				FROM bodb_Licensee.dbo.Users u with(nolock)
				WHERE u.userrole='Licensee' and u.username=@Username)
	BEGIN
		SELECT u.username, u.firstname, u.lastname, u.email, u.userpwd as password, '2' as roleid
		FROM bodb_Licensee.dbo.Users u with(nolock)
		WHERE u.userrole='Licensee' and u.username=@Username
		ORDER BY u.username
	END
	ELSE
	BEGIN
		SELECT @ErrCode = 10043, @ErrMsg = 'Error! Cannot find this username. This account is not existed.'
		RETURN
	END
END
  

