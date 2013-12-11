IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_StatisticsIns')
	BEGIN
		DROP  PROCEDURE  [dbo].[Is_Odds_StatisticsIns]
	END

GO
CREATE Procedure [dbo].[Is_Odds_StatisticsIns]
	@PartnerID INT Output,
	@DomainID smallint,
	@ProfileID smallint,
	@IP varchar(15),
	@ErrCode int = 0,
	@ErrMsg nvarchar(200) = ''
AS
/*
Created: 20100127@Johnny
Task : Each time Partner request odd, we'll insert 1 row into statistics to keep track of partner request odds
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	- @PartnerID
	- @DomainID 
	- @ProfileID
	- @IP 
*/
SET NOCOUNT ON;
--Check Domain name
Insert into Odds_Statistics(
	[PartnerID]
	,[DomainID]
	,[ProfileID]
	,[IP]
	,[Date]
)
Values(
	@PartnerID
	,@DomainID
	,@ProfileID
	,@IP
	,getdate()
)

IF @@ERROR <> 0
BEGIN	
	Select @ErrMsg = N'Error! Insert OddsProfile unsuccessfully!', @ErrCode = 10016
	return
END

--Write log

GO
GRANT EXECUTE ON [dbo].[Is_Odds_StatisticsIns] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_StatisticsIns] TO [vnteam]
GO