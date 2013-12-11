USE [bodb_oddsfeed]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'FN' AND name = 'RetrieveItemsNotInString')
	BEGIN
		DROP  FUNCTION  [dbo].[RetrieveItemsNotInString]
	END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
	Retrieve items in @string1 but not in @string2
*/
CREATE function  [dbo].[RetrieveItemsNotInString] (@string1 nvarchar( MAX), @string2 nvarchar( MAX))
returns varchar(MAX)
as
begin
	if(@string1 = '' or @string1 is null) return ''
	if(@string2 = '' or @string2 is null) return @string1
	
	DECLARE @RESULT_STRING nvarchar(max);
	SET @RESULT_STRING = '';
	
	SELECT @RESULT_STRING =  @RESULT_STRING + Val + ','  From dbo.strSplit(@string1, ',')
	WHERE Val NOT IN (
					SELECT Val From dbo.strSplit(@string2, ',')	
				 )        
								    		
    RETURN @RESULT_STRING
end

GO
GRANT EXECUTE ON [dbo].[RetrieveItemsNotInString] TO [bodbAdminNet]
GO
GRANT EXECUTE ON [dbo].[RetrieveItemsNotInString] TO [vnteam]
GO