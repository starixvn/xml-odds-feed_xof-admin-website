USE [bodb_oddsfeed]
GO
/****** Object:  StoredProcedure [dbo].[Odds_Dashboard_RecentActivities]    Script Date: 07/03/2013 18:03:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[Odds_Dashboard_RecentActivities]
	@UserID INT,
	@RoleID INT,
	@SystemID INT,
	@PageSize int = 5,
	@PageIndex int = 1,  
	@PageCount int = 0 OUTPUT,  
	@RowCount int = 0 OUTPUT
AS  
  
/**    
 * Created : 20130327@Long.Luu
 * Task  : Get all recent activities
 * DB  : bodb_oddsfeed  
 *    
 * Revisions:       
 * Param's Explanation:   
 */  

BEGIN
	SET NOCOUNT ON
	
	IF(@UserID = 0) -- get activities of system
	BEGIN
		-- calc row count
		select @RowCount = count(1)
		from UserLog u with (nolock)
			inner join Partner p with(nolock) on p.PartnerID = u.PartnerID
			inner join AdminUser a with(nolock) on u.creator = a.userid
		where u.partnerid<>0 and p.SystemID=@SystemID
		
		-- calc page count
		select @PageCount= @RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;
		
		WITH News AS
		(
			select top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY u.loggeddate desc) AS row
				, a.userid
				, a.username
				, u.detail
				, u.loggeddate
				
			from UserLog u with (nolock)
                inner join Partner p with(nolock) on p.PartnerID = u.PartnerID  
				inner join AdminUser a with(nolock) on u.creator = a.userid
			where  u.partnerid<>0 and p.SystemID=@SystemID
		)

		SELECT  [row]
				, userid
				, username
				, detail
				, loggeddate
		FROM News  
		WHERE  [row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize  
	END
	ELSE
	BEGIN -- get activities of userID
		-- calc row count
		select @RowCount = count(1)
		from UserLog u with (nolock)
			inner join AdminUser a with(nolock) on u.creator = a.userid
		where u.Creator = @UserID
		
		-- calc page count
		select @PageCount= @RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;
		
		WITH News AS
		(
			select top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY u.loggeddate desc) AS row
				, a.userid
				, a.username
				, u.detail
				, u.loggeddate
			from UserLog u with (nolock)
				inner join AdminUser a with(nolock) on u.creator = a.userid
			where u.Creator = @UserID
		)

		SELECT  [row]
				, userid
				--, username
				, detail
				, loggeddate
		FROM News  
		WHERE  [row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize  
	END
		
	--IF(@RoleID = 1) -- it admin
	--BEGIN
	---- calc row count
	--select @RowCount = count(1)
	--from UserLog u with (nolock)
	--	inner join AdminUser a with(nolock) on u.creator = a.userid
	--where u.partnerid in (select userid from adminusersystem where systemid = @SystemID)
	
	---- calc page count
	--select @PageCount= @RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;
	
	--WITH News AS
	--(
	--	select top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY u.loggeddate desc) AS row
	--		, a.userid
	--		, a.username
	--		, u.detail
	--		, u.loggeddate
	--	from UserLog u with (nolock)
	--		inner join AdminUser a with(nolock) on u.creator = a.userid
	--	where u.partnerid in (select userid from adminusersystem where systemid = @SystemID)
	--)

	--SELECT  [row]
	--		, userid
	--		, username
	--		, detail
	--		, loggeddate
	--FROM News  
	--WHERE  [row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize  
	--END
	--ELSE IF(@RoleID = 2) -- licensee
	--BEGIN
	---- calc row count
	--select @RowCount = count(1)
	--from UserLog u with (nolock)
	--	inner join AdminUser a with(nolock) on u.creator = a.userid
	--where a.roleid <> 1 and u.partnerid in (select userid from adminusersystem where systemid = @SystemID)
	
	---- calc page count
	--select @PageCount= @RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;
	
	--WITH News AS
	--(
	--	select top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY u.loggeddate desc) AS row
	--		, a.userid
	--		, a.username
	--		, u.detail
	--		, u.loggeddate
	--	from UserLog u with (nolock)
	--		inner join AdminUser a with(nolock) on u.creator = a.userid
	--	where a.roleid <> 1 and u.partnerid in 
	--		(select userid from adminusersystem where systemid = @SystemID)
	--)

	--SELECT  [row]
	--		, userid
	--		, username
	--		, detail
	--		, loggeddate
	--FROM News  
	--WHERE  [row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize  
	--END
	--ELSE -- sub-account
	--BEGIN
	---- calc row count
	--select @RowCount = count(1)
	--from UserLog u with (nolock)
	--	inner join AdminUser a with(nolock) on u.creator = a.userid
	--where a.roleid = 3 and u.partnerid in (select userid from adminusersystem where systemid = @SystemID)
	
	---- calc page count
	--select @PageCount= @RowCount/@PageSize + case when @RowCount%@PageSize>0 then 1 else 0 end;
	
	--WITH News AS
	--(
	--	select top (@PageSize*@PageIndex)  ROW_NUMBER() OVER (ORDER BY u.loggeddate desc) AS row
	--		, a.userid
	--		, a.username
	--		, u.detail
	--		, u.loggeddate
	--	from UserLog u with (nolock)
	--		inner join AdminUser a with(nolock) on u.creator = a.userid
	--	where a.roleid = 3 and u.partnerid in 
	--		(select userid from adminusersystem where systemid = @SystemID)
	--)

	--SELECT  [row]
	--		, userid
	--		, username
	--		, detail
	--		, loggeddate
	--FROM News  
	--WHERE  [row] BETWEEN @PageSize * (@PageIndex-1) + 1 and @PageIndex*@PageSize  
	--END
END


