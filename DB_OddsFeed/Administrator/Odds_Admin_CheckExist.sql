set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

ALTER PROCEDURE [dbo].[Odds_Admin_CheckExist]	
	@SystemID INT = 0,
	@RoleID INT = 0,
	@Username VARCHAR(50) = '',
	@PageSize int = 10,  
	@PageIndex int = 1,  
	@PageCount int = 0 OUTPUT,  
	@RowCount int = 0 OUTPUT,
	@ErrCode INT  = 0 OUTPUT,
	@ErrMsg NVARCHAR(200) = '' OUTPUT
AS
/*
 *	Created: 20130307@Louis.Nguyen
 *	Task : select Administrator according to filter options Odds Feed - Licensee system
 *	DB: [bodb_oddsfeed]
 *  Description:
 *	Revisions:		
	
	Param's Explanation:
	- @SystemID : system id
	- @Username : Username of admin
*/
BEGIN 

SET NOCOUNT ON

IF(@RoleID = 0) -- all roles
BEGIN
	IF(@SystemID = 0) -- all systems
	BEGIN
		IF(@Username = '') -- all users
		BEGIN
			-- all system, all username, all roles
			-- calc row count
			select @RowCount = count(1)
			from AdminUser a with (nolock)
				left join adminuser c with(nolock) on a.creator = c.userid
			
			-- calc page count
			select @PageCount= @RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;
			
			WITH News AS
			(
				select top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY a.userid ASC) AS row
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
			WHERE  [row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize  
			ORDER BY userid
		END
		ELSE -- specific user
		BEGIN
			-- all system, specific Username ( LIKE), all roles
			-- calc row count
			select @RowCount = count(1)
			from AdminUser a with (nolock)
				left join adminuser c with(nolock) on a.creator = c.userid
			where a.username = @Username
			
			-- calc page count
			select @PageCount= @RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;
			
			WITH News AS
			(
				select top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY a.userid ASC) AS row 
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
				where a.username = @Username
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
			WHERE  [row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize  
			ORDER BY userid
		END
	END
	ELSE -- specific system
	BEGIN
		IF(@Username = '') -- all users
		BEGIN
			-- specific system, all username, all roles
			-- calc row count
			select @RowCount = count(1)
			from AdminUser a with (nolock)
				left join adminuser c with(nolock) on a.creator = c.userid
				left join adminusersystem s with(nolock) on s.userid = a.userid
			where s.systemid = @SystemID
			
			-- calc page count
			select @PageCount= @RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;

			WITH News AS
			(
				select top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY a.userid ASC) AS row 
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
					left join adminusersystem s with(nolock) on s.userid = a.userid
					left join userrole u with (nolock) on a.roleid = u.roleid
				where s.systemid = @SystemID
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
			WHERE  [row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize  
			ORDER BY userid
		END
		ELSE -- specific user
		BEGIN
			-- specific system,  specific Username ( LIKE), all roles
			-- calc row count
			select @RowCount = count(1)
			from AdminUser a with (nolock)
				left join adminuser c with(nolock) on a.creator = c.userid
				left join adminusersystem s with(nolock) on s.userid = a.userid
			where s.systemid = @SystemID and a.username = @Username
			
			-- calc page count
			select @PageCount= @RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;

			WITH News AS
			(
				select top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY a.userid ASC) AS row 
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
					left join adminusersystem s with(nolock) on s.userid = a.userid
					left join userrole u with (nolock) on a.roleid = u.roleid
				where s.systemid = @SystemID and a.username = @Username
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
			WHERE  [row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize  
			ORDER BY userid
		END
	END
END
ELSE -- specific role
BEGIN
	IF(@SystemID = 0) -- all systems
	BEGIN
		IF(@Username = '') -- all users
		BEGIN
			-- all system, all username, specific role
			-- calc row count
			select @RowCount = count(1)
			from AdminUser a with (nolock)
				left join adminuser c with(nolock) on a.creator = c.userid
			where a.roleid = @RoleID
			
			-- calc page count
			select @PageCount= @RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;
			
			WITH News AS
			(
				select top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY a.userid ASC) AS row
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
				where a.roleid = @RoleID
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
			WHERE  [row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize  
			ORDER BY userid
		END
		ELSE -- specific user
		BEGIN
			-- all system, specific Username ( LIKE), specific role
			-- calc row count
			select @RowCount = count(1)
			from AdminUser a with (nolock)
				left join adminuser c with(nolock) on a.creator = c.userid
			where a.roleid = @RoleID AND a.username = @Username
			
			-- calc page count
			select @PageCount= @RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;
			
			WITH News AS
			(
				select top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY a.userid ASC) AS row 
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
				where a.roleid = @RoleID AND a.username = @Username
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
			WHERE  [row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize  
			ORDER BY userid
		END
	END
	ELSE -- specific system
	BEGIN
		IF(@Username = '') -- all users
		BEGIN
			-- specific system, all username, specific role
			-- calc row count
			select @RowCount = count(1)
			from AdminUser a with (nolock)
				left join adminuser c with(nolock) on a.creator = c.userid
				left join adminusersystem s with(nolock) on s.userid = a.userid
			where a.roleid = @RoleID AND s.systemid = @SystemID
			
			-- calc page count
			select @PageCount= @RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;

			WITH News AS
			(
				select top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY a.userid ASC) AS row 
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
					left join adminusersystem s with(nolock) on s.userid = a.userid
					left join userrole u with (nolock) on a.roleid = u.roleid
				where a.roleid = @RoleID AND s.systemid = @SystemID
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
			WHERE  [row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize  
			ORDER BY userid
		END
		ELSE
		BEGIN
			-- specific system,  specific Username ( LIKE), specific role
			-- calc row count
			select @RowCount = count(1)
			from AdminUser a with (nolock)
				left join adminuser c with(nolock) on a.creator = c.userid
				left join adminusersystem s with(nolock) on s.userid = a.userid
			where a.roleid = @RoleID AND s.systemid = @SystemID and a.username = @Username
			
			-- calc page count
			select @PageCount= @RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;

			WITH News AS
			(
				select top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY a.userid ASC) AS row 
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
					left join adminusersystem s with(nolock) on s.userid = a.userid
					left join userrole u with (nolock) on a.roleid = u.roleid
				where a.roleid = @RoleID AND s.systemid = @SystemID and a.username = @Username
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
			WHERE  [row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize  
			ORDER BY userid
		END
	END
END

END

GO
GRANT EXECUTE ON [dbo].[Odds_Admin_CheckExist] TO [bodbAdminNet]
GO


