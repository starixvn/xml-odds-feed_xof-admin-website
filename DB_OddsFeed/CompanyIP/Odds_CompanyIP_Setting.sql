USE [bodb_oddsfeed]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Odds_CompanyIP_Setting]
	@Creator INT = 0,	
	@IPFrom varchar(30),
	@IPTo varchar(30),
	@IPFromOld varchar(30)= '',
	@IPToOld varchar(30) = '',
	@Description nvarchar(100),
	@IP varchar(25) = '',
	@ErrCode int = 0 Output,
	@ErrMsg nvarchar(200) = '' Output
AS
/*
Created: 20130516@Thai
Task : Update Company IP
DB: [bodb_oddsfeed]
Revisions:
	- 
Param's Explanation:
	- @IPFrom 	
	- @IPTo 
*/
SET NOCOUNT ON;

If (@IPFrom is not null and @IPTo is not null and dbo.Is_Odds_IPConvertToNumber(@IPTo) >= dbo.Is_Odds_IPConvertToNumber(@IPFrom))
Begin
	If (@IPFromOld is not null and @IPFromOld <> '' and @IPToOld is not null and @IPToOld <>'')
	Begin -- Update the new IP
		Update dbo.CompanyIP
		Set IPFrom = dbo.Is_Odds_IPConvertToNumber(@IPFrom),
			IPTo = dbo.Is_Odds_IPConvertToNumber(@IPTo),
			[Description] = @Description
		Where IPFrom = dbo.Is_Odds_IPConvertToNumber(@IPFromOld) and
			  IPTo = dbo.Is_Odds_IPConvertToNumber(@IPToOld)
	End		
	Else
	Begin -- Insert a new IP
		INSERT INTO [dbo].[CompanyIP](
					[IPFrom], 
					[IPTo],
					[Creator],
					[CreatedDate],
					[UpdatedBy],
					[LastUpdate],
					[Description]
				)
				VALUES(
					dbo.Is_Odds_IPConvertToNumber(@IPFrom),
					dbo.Is_Odds_IPConvertToNumber(@IPTo),
					@Creator,
					GetDate(),
					@Creator,
					GetDate(),
					@Description
				)
	End
	IF @@ERROR <> 0
	BEGIN	
		--Write log here and store errmsg in a string
		SELECT @ErrMsg = @ErrMsg + 'Error! Update Company IP setting unsuccessful.' + ' IPFrom: ' + @IPFrom + ', IPTo: ' + @IPTo + ';', @ErrCode = 10052				
	END
End
Else
Begin
	--Write log here and store errmsg in a string				
	SELECT @ErrMsg = @ErrMsg + 'Error! Update Company IP setting unsuccessful. ' + ' IPFrom: ' + @IPFrom + ' is larger than  IPTo: ' + @IPTo + ';', @ErrCode = 10053
End
			

--Write log function
declare @SPName varchar(50)
select @SPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData nvarchar(4000)
SELECT @LogData = 'Update Company IP Setting:'+		
		', IPFrom: ' + @IPFrom +
		', IPTo: ' + @IPTo +
		', Description: ' + @Description +
		', Ip:' + @Ip

EXEC [Odds_WriteLog]
	@Creator=@Creator,		
	@TypeName= 'Company',	
	@SPName=@SPName,
	@IP =@IP,
	@Detail=@LogData
	
GO
GRANT EXECUTE ON [Odds_CompanyIP_Setting] To [bodbAdminNet]