USE [bodb_oddsfeed]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Odds_CompanyIP_DeleteSetting]
	@Creator INT = 0,	
	@IP varchar(30)= '',
	@IPFromOld varchar(30)= '',
	@IPToOld varchar(30) = '',
	@ErrCode int = 0 Output,
	@ErrMsg nvarchar(200) = '' Output
AS
/*
Created: 20130625@Louis.Nguyen
Task : Delete Company IP
DB: [bodb_oddsfeed]
Revisions:
	- 
Param's Explanation:
*/
SET NOCOUNT ON;

If (@IPFromOld is not null and 
	@IPFromOld <> '' and 
	@IPToOld is not null and 
	@IPToOld <>'')
Begin -- Update the new IP
	Delete FROM [dbo].[CompanyIP]
	Where [IPFrom]= dbo.Is_Odds_IPConvertToNumber(@IPFromOld) and
		  [IPTo] = dbo.Is_Odds_IPConvertToNumber(@IPToOld)
End		

IF @@ERROR <> 0
BEGIN	
	--Write log here and store errmsg in a string
	SELECT @ErrMsg = @ErrMsg + 'Error! Delete Company IP setting unsuccessful.' + ' IPFrom: ' + @IPFromOld + ', IPTo: ' + @IPToOld + ';', @ErrCode = 10052				
END

--Write log function
declare @SPName varchar(50)
select @SPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData nvarchar(4000)
SELECT @LogData = 'Delete Company IP Setting:'+		
		', IPFrom: ' + @IPFromOld +
		', IPTo: ' + @IPToOld +
		', Ip:' + @Ip

EXEC [Odds_WriteLog]
	@Creator=@Creator,		
	@TypeName= 'Company',	
	@SPName=@SPName,
	@IP =@IP,
	@Detail=@LogData
	
GO
GRANT EXECUTE ON [Odds_CompanyIP_DeleteSetting] To [bodbAdminNet]