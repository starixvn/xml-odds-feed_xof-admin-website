USE [bodb_oddsfeed]
GO
/****** Object:  StoredProcedure [dbo].[Odds_Dashboard_CurrentOnlUsers]    Script Date: 07/04/2013 13:00:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




ALTER PROCEDURE [dbo].[Odds_Dashboard_CurrentOnlUsers]
	@UserID INT,
	@RoleID INT,
	@SystemID INT,
	@PageSize int = 10,
	@PageIndex int = 1,  
	@PageCount int = 0 OUTPUT,  
	@RowCount int = 0 OUTPUT
AS  
  
/**    
 * Created : 20130327@Long.Luu
 * Task  : Get all currently online users
 * DB  : bodb_oddsfeed  
 *    
 * Revisions:       
 * Param's Explanation:   
 */  

BEGIN
	SET NOCOUNT ON
	IF(@RoleID = 1) -- it admin
	BEGIN
		-- calc row count
		select @RowCount = count(1)
		from On_User o with (nolock)
			inner join adminuser c with(nolock) on o.userid = c.userid
			inner join userrole u with (nolock) on c.roleid = u.roleid
         where o.IsKickedOut=0
		
		-- calc page count
		select @PageCount= @RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;
		
		WITH News AS
		(
			select top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY c.userid ASC) AS row
				, c.userid
				, c.username
				, c.lastlogin
				, o.reloadtime
				, o.IP
				, u.rolename
				from On_User o with (nolock)
					inner join adminuser c with(nolock) on o.userid = c.userid
					inner join userrole u with (nolock) on c.roleid = u.roleid
				 where o.IsKickedOut=0
		)

		SELECT  [row]
				, userid
				, username
				, lastlogin
				, reloadtime
				, IP
				, rolename
		FROM News  
		WHERE  [row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize  
		ORDER BY userid
	END
	ELSE IF(@RoleID = 2) -- licensee
	BEGIN
		-- calc row count
		select @RowCount = count(1)
		from On_User o with (nolock)
			inner join adminuser c with(nolock) on o.userid = c.userid
			inner join userrole u with (nolock) on c.roleid = u.roleid
		where c.roleid <> 1 and o.userid in (select userid from adminusersystem where systemid = @SystemID)
               and o.IsKickedOut=0
		
		-- calc page count
		select @PageCount= @RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;
		
		WITH News AS
		(
			select top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY c.userid ASC) AS row
				, c.userid
				, c.username
				, c.lastlogin
				, o.reloadtime
				, o.IP
				, u.rolename
			from On_User o with (nolock)
				inner join adminuser c with(nolock) on o.userid = c.userid
				inner join userrole u with (nolock) on c.roleid = u.roleid
			where c.roleid <> 1 and o.userid in (select userid from adminusersystem where systemid = @SystemID)
               and o.IsKickedOut=0
		)

		SELECT  [row]
				, userid
				, username
				, lastlogin
				, reloadtime
				, IP
				, rolename
		FROM News  
		WHERE  [row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize  
		ORDER BY userid
	END
	ELSE -- sub-account
	BEGIN
		-- calc row count
		select @RowCount = count(1)
		from On_User o with (nolock)
			inner join adminuser c with(nolock) on o.userid = c.userid
			inner join userrole u with (nolock) on c.roleid = u.roleid
		where c.roleid = 3 and o.userid in (select userid from adminusersystem where systemid = @SystemID)
              and o.IsKickedOut=0
		
		-- calc page count
		select @PageCount= @RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;
		
		WITH News AS
		(
			select top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY c.userid ASC) AS row
				, c.userid
				, c.username
				, c.lastlogin
				, o.reloadtime
				, o.IP
				, u.rolename
			from On_User o with (nolock)
				inner join adminuser c with(nolock) on o.userid = c.userid
				inner join userrole u with (nolock) on c.roleid = u.roleid
			where c.roleid = 3 and o.userid in (select userid from adminusersystem where systemid = @SystemID)
                     and o.IsKickedOut=0 
		)

		SELECT  [row]
				, userid
				, username
				, lastlogin
				, reloadtime
				, IP
				, rolename
		FROM News  
		WHERE  [row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize  
		ORDER BY userid
	END
END



