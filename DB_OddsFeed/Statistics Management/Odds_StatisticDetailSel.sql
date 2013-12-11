IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_StatisticDetailSel')
	BEGIN
		DROP  PROCEDURE  [dbo].Is_Odds_StatisticDetailSel
	END



GO

CREATE Procedure [dbo].[Is_Odds_StatisticDetailSel]
	@PartnerID INT,
	@Month SMALLDATETIME= NULL -- The first date of mont
AS

SET NOCOUNT ON;

DECLARE @Today SMALLDATETIME, @LasDateOfMonth SMALLDATETIME;
SELECT @Today= GETDATE()
SELECT @LasDateOfMonth = DATEADD(month, DATEDIFF(month, 0, @Month)+1,0);

IF (DATEDIFF(month, @Today,@Month)=0 and DATEDIFF(year,@Today,@Month)=0) -- Current month
BEGIN -- SELECT data in the current month
	
	    SELECT DATEADD(D, 0, DATEDIFF(D, 0, [Date])) [DateOfMonth], COUNT(*) AS TotalRequest
			FROM Odds_Statistics WITH(NOLOCK)
			WHERE PartnerID = @PartnerID
			  AND [Date] BETWEEN @Month AND @Today
			GROUP BY DATEADD(D, 0, DATEDIFF(D, 0, [Date]))	

	
END
ELSE -- Data in the pass
BEGIN			
	    SELECT [Date] AS [DateOfMonth], TotalRequest
			FROM Odds_StatisticsSum NOLOCK
			WHERE IsForMonth=0
			AND [Date]=@Month
			AND PartnerID = @PartnerID	
END


GO

GO
GRANT EXECUTE ON [dbo].Is_Odds_StatisticDetailSel TO [bodbAdminNet]
GO
GRANT EXECUTE ON [dbo].[Is_Odds_StatisticDetailSel] TO [vnteam]
GO