USE [bodb_Maintenance]
GO
/****** Object:  UserDefinedFunction [dbo].[Is_Odds_IPConvertToNumber]    Script Date: 02/27/2010 09:10:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Function [dbo].[Is_Odds_IPConvertToNumber] (
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