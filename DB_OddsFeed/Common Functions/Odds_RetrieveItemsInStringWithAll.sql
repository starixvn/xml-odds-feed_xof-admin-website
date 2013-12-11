USE [bodb_oddsfeed]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'FN' AND name = 'RetrieveItemsInStringWithAll')
	BEGIN
		DROP  FUNCTION  [dbo].[RetrieveItemsInStringWithAll]
	END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
	Retrieve items in @string1 and in @string2, check with 'all'
*/
CREATE function  [dbo].[RetrieveItemsInStringWithAll] (@string1 nvarchar( MAX), @string2 nvarchar( MAX))
returns varchar(MAX)
as
begin
	IF @string2 = 'all'
		RETURN @string1		
	
	DECLARE @RESULT_STRING nvarchar(max);
	SET @RESULT_STRING = '';
	
	SELECT @RESULT_STRING =  @RESULT_STRING + Val + ','  From dbo.strSplit(@string1, ',')
	WHERE Val IN (
					SELECT Val From dbo.strSplit(@string2, ',')	
				 )
    
    RETURN @RESULT_STRING
end

GO
GRANT EXECUTE ON [dbo].[RetrieveItemsInStringWithAll] TO [bodbAdminNet]
GO
GRANT EXECUTE ON [dbo].[RetrieveItemsInStringWithAll] TO [vnteam]
GO