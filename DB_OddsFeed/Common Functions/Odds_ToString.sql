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
