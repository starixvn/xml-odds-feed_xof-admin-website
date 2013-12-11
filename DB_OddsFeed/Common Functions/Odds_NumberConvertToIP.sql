
USE [bodb_Maintenance]
GO
/****** Object:  UserDefinedFunction [dbo].[Is_Odds_NumberConvertToIP]    Script Date: 03/05/2010 11:30:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Function [dbo].[Is_Odds_NumberConvertToIP] (
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


