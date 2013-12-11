IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_PartnerUpdIPSetting')
	BEGIN
		DROP  PROCEDURE  [dbo].[Is_Odds_PartnerUpdIPSetting]
	END

GO
CREATE Procedure [dbo].[Is_Odds_PartnerUpdIPSetting]
	@Creator INT = 0,
	@PartnerID INT,
	@ListIPFrom varchar(300),
	@ListIPTo varchar(300),
	@IP varchar(25) = '',
	@ErrCode int = 0 Output,
	@ErrMsg nvarchar(200) = '' Output
AS
/*
Created: 20100122@Johnny
Task : Update IP Setting
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	- @PartnerID : output partner is inserted
	- @IPFrom 	
	- @IPTo 
*/
SET NOCOUNT ON;

--Check PartnerID and IPFrom is existed or not
if Not Exists(select 1 from Odds_Partner(nolock) where PartnerId = @PartnerId)
Begin
	Select @ErrCode = 10014,
		@ErrMsg = 'This partner does not exist.'
	Return
End

--Delete all Old Setting

Begin Transaction

Begin Try

	Delete Odds_IPSetting where PartnerId = @PartnerId

	--IPSetting
	Declare @Rank int, @IPFrom varchar(20), @IPTo varchar(20)

	Select @Rank = min(Rank) from dbo.strSplit(@ListIPFrom, ',')

	While @Rank is not null
	Begin
		Select @IPFrom = Val from dbo.StrSplit(@ListIPFrom, ',') where Rank = @Rank
		Select @IPTo = Val from dbo.StrSplit(@ListIPTo, ',') where Rank = @Rank
		
		If (@IPFrom is not null and @IPTo is not null and dbo.Is_Odds_IPConvertToNumber(@IPTo) >= dbo.Is_Odds_IPConvertToNumber(@IPFrom))
		Begin
			Insert into Odds_IPSetting([PartnerID], [IPFrom], [IPTo])
			values(@PartnerID, dbo.Is_Odds_IPConvertToNumber(@IPFrom), dbo.Is_Odds_IPConvertToNumber(@IPTo))			
		End
		Else
		Begin
			--Write log here and store errmsg in a string
			Select @ErrMsg = @ErrMsg + 'Error! IPFrom: ' + @IPFrom + ', IPTo: ' + @IPTo + ';', @ErrCode = 10013		
		End
		
		Declare @OldRank int
		
		Select @OldRank = @Rank
		Select @Rank = null-- Reset @Rank
		
		Select @Rank = min(Rank) From dbo.strSplit(@ListIPFrom, ',') where Rank > @OldRank
	End
	
	Commit Transaction
End Try
Begin Catch
	Rollback Transaction
	Select @ErrMsg = N'Update List IP unsuccessful!', @ErrCode = 10033	
End Catch

--Write log function
declare @SPName varchar(50)
select @SPName = OBJECT_NAME(@@ProcID) 

DECLARE @LogData nvarchar(4000)
SELECT @LogData = 'Update IP Setting, PartnerId:' + dbo.Is_Odds_ToString(@PartnerId) +
		', ListIPFrom: ' + @ListIPFrom +
		', ListIPTo: ' + @ListIPTo +
		', Ip:' + @Ip

EXEC [Is_Odds_WriteLog]
	@Creator=@Creator,	
	@PartnerID = @PartnerID,
	@TypeName= 'Partner',	
	@SPName=@SPName,
	@IP =@IP,
	@Detail=@LogData


GO
GRANT EXECUTE ON [dbo].[Is_Odds_PartnerUpdIPSetting] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_PartnerUpdIPSetting] TO [vnteam]
GO