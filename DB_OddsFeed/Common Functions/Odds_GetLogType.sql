ALTER function [dbo].[Odds_GetLogType] (
	@LogName varchar(2048))
RETURNS int
AS
/*
	Task : Get Log Type
	Author : Johnny@20100202
	
*/
BEGIN	
    
	DECLARE @LogType int	
	SELECT @LogType=TypeID from LogType (nolock)
    where TypeName=@LogName
    
	RETURN @LogType
END
