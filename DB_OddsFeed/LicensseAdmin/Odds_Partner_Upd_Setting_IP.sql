USE [bodb_oddsfeed]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Odds_Partner_Upd_Setting_IP]
	@Creator INT = 0,
	@PartnerID INT,	
	@IPFrom varchar(30),
	@IPTo varchar(30),
	@IPFromOld varchar(30),
	@IPToOld varchar(30),
	@IP varchar(25) = '',
	@ErrCode int = 0 Output,
	@ErrMsg nvarchar(200) = '' Output
AS
/*
Created: 20130516@Thai
Task : Update Partner IP
DB: [bodb_oddsfeed]
Revisions:
	- 
Param's Explanation:
	- @PartnerID : output partner is inserted
	- @IPFrom 	
	- @IPTo 
*/
SET NOCOUNT ON;

--Check PartnerID is existed or not
if Not Exists(select 1 from Partner with (nolock) where PartnerId = @PartnerId)
Begin
	Select @ErrCode = 10014, @ErrMsg = 'This affiliate does not exist.'
	Return
End

If (@IPFrom is not null and @IPTo is not null and dbo.Is_Odds_IPConvertToNumber(@IPTo) >= dbo.Is_Odds_IPConvertToNumber(@IPFrom))
Begin
	If (@IPFromOld is not null and @IPFromOld <> '' and @IPToOld is not null and @IPToOld <>'')
	Begin -- Update the new IP
		Update PartnerIP
		Set IPFrom = dbo.Is_Odds_IPConvertToNumber(@IPFrom),
			IPTo = dbo.Is_Odds_IPConvertToNumber(@IPTo)
		Where PartnerID = @PartnerID and
			  IPFrom = dbo.Is_Odds_IPConvertToNumber(@IPFromOld) and
			  IPTo = dbo.Is_Odds_IPConvertToNumber(@IPToOld)
	End		
	Else
	Begin -- Insert a new IP
		Insert into PartnerIP([PartnerID], [IPFrom], [IPTo])
		values(@PartnerID, dbo.Is_Odds_IPConvertToNumber(@IPFrom), dbo.Is_Odds_IPConvertToNumber(@IPTo))			
	End
	IF @@ERROR <> 0
	BEGIN	
		--Write log here and store errmsg in a string
		SELECT @ErrMsg = @ErrMsg + 'Error! Update IP setting unsuccessful. PartnerID: '+@PartnerID+', IPFrom: ' + @IPFrom + ', IPTo: ' + @IPTo + ';', @ErrCode = 10052				
	END
End
Else
Begin
	--Write log here and store errmsg in a string				
	SELECT @ErrMsg = @ErrMsg + 'Error! Update IP setting unsuccessful. PartnerID: '+@PartnerID+', IPFrom: ' + @IPFrom + ' is larger than  IPTo: ' + @IPTo + ';', @ErrCode = 10053
End
			

--Write log function
declare @SPName varchar(50)
select @SPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData nvarchar(4000)
SELECT @LogData = 'Update Domain, IP Setting, PartnerId:' + dbo.Is_Odds_ToString(@PartnerId) +		
		', IPFrom: ' + @IPFrom +
		', IPTo: ' + @IPTo +
		', Ip:' + @Ip

EXEC [Odds_WriteLog]
	@Creator=@Creator,	
	@PartnerID = @PartnerID,
	@TypeName= 'Partner',	
	@SPName=@SPName,
	@IP =@IP,
	@Detail=@LogData
	
GO
GRANT EXECUTE ON [Odds_Partner_Upd_Setting_IP] To [bodbAdminNet]