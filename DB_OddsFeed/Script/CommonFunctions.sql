USE [bodb_Maintenance]
GO
/****** Object:  UserDefinedFunction [dbo].[strSplit]    Script Date: 02/27/2010 16:31:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function  [dbo].[strSplit] ( @string nvarchar( 4000), @splitter char( 1) )
returns @res table ( id int primary key, rank int, val nvarchar( 4000) )
as
begin
     if substring ( @string, len ( @string), 1)<>@splitter
          set @string= @string+@splitter
     declare @start int, @word nvarchar(4000), @charindex int, @i int
     set @i=1
     set @start=1
     set @charindex= charindex( @splitter, @string, @start)
     while (@charindex <> 0)begin
          set @word= substring( @string, @start, @charindex - @start)
          set @start= @charindex +1
          set @charindex= charindex( @splitter, @string, @start)
          insert into @res  values ( @start, @i, LTrim(Rtrim(@word)))
          set @i=@i+1
     end
     return
end

GO
USE [bodb_Maintenance]
GO
/****** Object:  UserDefinedFunction [dbo].[Is_Odds_GetLogType]    Script Date: 02/27/2010 16:31:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[Is_Odds_GetLogType] (
	@LogName varchar(2048))
RETURNS int
AS
/*
	Task : Get Log Type
	Author : Johnny@20100202
	
*/
BEGIN	
	DECLARE @LogType int	
	SELECT @LogType=TypeID from Odds_LogType (nolock)
    where TypeName=@LogName
    
	RETURN @LogType
END
GO
USE [bodb_Maintenance]
GO
/****** Object:  UserDefinedFunction [dbo].[Is_Odds_IPConvertToNumber]    Script Date: 02/27/2010 16:31:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Function [dbo].[Is_Odds_IPConvertToNumber] (
	@IP varchar(22)
)
RETURNS bigint
AS
/*
    DB : DB052.bodb_Maintenance
	Task : Convert IP format to integer number
	Author : Johnny@20100127
	
*/
BEGIN	   
	SET @IP=ltrim(rtrim(@IP))
	If(@Ip LIKE '%_%.%_%.%_%.%_%')
	Begin
		Declare @IpA bigint, @IpB int, @IpC int, @IpD int, @IpI bigint
		
		Select @IpA = Left(@Ip, PATIndex('%.%', @IP) - 1)	
		Select @Ip = Right(@Ip, Len(@Ip) - Len(@IpA) - 1)
		
		Select @IpB = Left(@Ip, PATIndex('%.%', @IP) - 1)	
		Select @Ip = Right(@Ip, Len(@Ip) - Len(@IpB) - 1)
		
		Select @IpC = Left(@Ip, PATIndex('%.%', @IP) - 1)	
		Select @Ip = Right(@Ip, Len(@Ip) - Len(@IpC) - 1)
		
		Select @IpD = @Ip
		
		Return (@IpA * 256*256*256) + (@IpB * 256 * 256) + (@IpC * 256) + @IpD	
	End
	Return 0
END
GO
USE [bodb_Maintenance]
GO
/****** Object:  UserDefinedFunction [dbo].[Is_Odds_ToString]    Script Date: 02/27/2010 16:32:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[Is_Odds_ToString] (
 @Value sql_variant
)
RETURNS nvarchar(4000)
AS
/*
    DB : DB02.bodb02, DB03.on_user,DB052.Affiliate_12BET
	Task : Convert a value to string for all data types
	Author : Peter@20090909
	
*/
BEGIN	   
	return cast(isnull(@Value,'') as nvarchar(4000))
END
GO

USE [bodb_Maintenance]
GO

/****** Object:  UserDefinedFunction [dbo].[Is_Odds_NumberConvertToIP]    Script Date: 03/19/2010 16:53:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Function [dbo].[Is_Odds_NumberConvertToIP] (
	@Number bigint
)
RETURNS varchar(22)
AS
/*
    DB : DB052.bodb_Maintenance
	Task : Convert a number to ip string
	Author : Peter@20100305
	
*/
BEGIN	 
    declare @IpA int,  @IpB int,@IpC int,@IpD int,@Result varchar(22)
    
	
    SET @IpA=(@Number-@Number%16777216)/16777216
    SET @IpB=(@Number%16777216-(@Number%16777216)%65536)/65536
    SET @IpC = ((@Number%16777216) % 65536 - ((@Number % 16777216) % 65536 % 256)) / 256;
    SET @IpD = (@Number % 16777216) % 65536 % 256;
    SET @Result= cast(@IpA as varchar(3)) + '.' + cast(@IpB as varchar(3)) + '.' 
                 + cast(@IpC as varchar(3)) + '.' + cast(@IpD as varchar(3))
	
	Return @Result
END



GO
USE [bodb_Maintenance]
GO
/****** Object:  UserDefinedFunction [dbo].[f_DateTime_UnixTime]    Script Date: 03/20/2010 14:54:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER function [dbo].[f_DateTime_UnixTime]
	( @DAY datetime )
returns  int
as
/*
Function: F_DATETIME_TO_UNIX_TIME

	Finds UNIX time as the difference in seconds between
	1970-01-01 00:00:00 and input parameter @DAY after
	rounding @DAY to the neareast whoie second.

	Valid datetime range is 1901-12-13 20:45:51.500 through
	2038-01-19 03:14:07.497.  This range is limited to the smallest
	through the largest possible integer.

	Datetimes outside this range will return null.
*/
begin
declare @wkdt datetime

-- Return null if outside of valid UNIX Time range
if @DAY < '1901-12-13 20:45:51.500' or  @DAY > '2038-01-19 03:14:07.497'
	return null

-- Round off datetime to nearest whole second
select @wkdt = dateadd(ms,round(datepart(ms,@DAY),-3)-datepart(ms,@DAY),@DAY)

-- If date GE 1901-12-14
if @wkdt >= 712	return datediff(ss,25567,@wkdt) 

-- Handles time GE '1901-12-13 20:45:52.000 and LT 1901-12-14
return -2147472000-datediff(ss,@wkdt,712)

end
