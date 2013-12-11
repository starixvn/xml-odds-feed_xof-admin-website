USE [bodb_oddsfeed]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].Odds_Delete_Setting_IP_Partner
	@PartnerId INT,	
	@Creator INT = 0,
	@IP varchar(30),
	@IPFromOld varchar(30),
	@IPToOld varchar(30),
	@ErrCode int = 0 Output,
	@ErrMsg nvarchar(200) = '' Output
AS
/*
Created: 20130624@Thai.Tran
Task : Delete Partner IP
DB: [bodb_oddsfeed]
Revisions:
	- 
Param's Explanation:
*/
SET NOCOUNT ON;

--Check PartnerID is existed or not
if Not Exists(select 1 from dbo.Partner with (nolock) where PartnerID = @PartnerId)
Begin
	Select @ErrCode = 10014, @ErrMsg = 'This affiliate does not exist.'
	Return
End

If (@IPFromOld is not null and 
	@IPFromOld <> '' and 
	@IPToOld is not null and 
	@IPToOld <>'')
Begin -- Update the new IP
	Delete dbo.PartnerIP
	Where PartnerID = @PartnerId and
			IPFrom = dbo.Is_Odds_IPConvertToNumber(@IPFromOld) and
			IPTo = dbo.Is_Odds_IPConvertToNumber(@IPToOld)
End		

IF @@ERROR <> 0
BEGIN	
	--Write log here and store errmsg in a string
	SELECT @ErrMsg = @ErrMsg + 'Error! Delete IP setting unsuccessful. PartnerId: '+ @PartnerId +', IPFrom: ' + @IPFromOld + ', IPTo: ' + @IPToOld + ';', @ErrCode = 10052				
END

--Write log function
declare @SPName varchar(50)
select @SPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData nvarchar(4000)
SELECT @LogData = 'Delete Domain, IP Setting, PartnerId:' + dbo.Is_Odds_ToString(@PartnerId) +		
		', IPFrom: ' + @IPFromOld +
		', IPTo: ' + @IPToOld +
		', Ip:' + @Ip

EXEC [Odds_WriteLog]
	@Creator=@Creator,	
	@PartnerID = @PartnerId,
	@TypeName= 'Partner',	
	@SPName=@SPName,
	@IP =@IP,
	@Detail=@LogData	
GO

GRANT EXECUTE ON Odds_Delete_Setting_IP_Partner To [bodbAdminNet]