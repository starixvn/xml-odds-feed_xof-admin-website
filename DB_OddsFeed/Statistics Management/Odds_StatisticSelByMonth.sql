IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_StatisticSelByMonth')
	BEGIN
		DROP  PROCEDURE  [dbo].Is_Odds_StatisticSelByMonth
	END



GO

CREATE Procedure [dbo].[Is_Odds_StatisticSelByMonth]	
@Month SMALLDATETIME= NULL -- The first date of mont
AS

SET NOCOUNT ON;

DECLARE @Today SMALLDATETIME
SELECT @Today= GETDATE()

IF (DATEDIFF(month, @Today,@Month)=0 and DATEDIFF(year,@Today,@Month)=0) -- Current month
BEGIN -- SELECT data in the current month
	
	SELECT P.UserName AS PartnerName, P.PartnerID, S.TotalRequest	
	FROM(	
			SELECT PartnerID, COUNT(*) AS TotalRequest
				FROM Odds_Statistics WITH(NOLOCK)
				WHERE PartnerID IS NOT NULL
				  AND [Date] BETWEEN @Month AND @Today
				GROUP BY PartnerID
	)S INNER JOIN Odds_Partner P ON P.PartnerID=S.PartnerID
	
	
END
ELSE -- Data in the pass
BEGIN	
	SELECT P.UserName AS PartnerName, P.PartnerID, S.TotalRequest	
	FROM(		
		SELECT PartnerID, TotalRequest
		FROM Odds_StatisticsSum NOLOCK
		WHERE IsForMonth=1
			AND [Date]=@Month
	)S INNER JOIN Odds_Partner P ON P.PartnerID=S.PartnerID
	
END


GO

GO
GRANT EXECUTE ON [dbo].[Is_Odds_StatisticSelByMonth] TO [bodbAdminNet]
GO
GRANT EXECUTE ON [dbo].[Is_Odds_StatisticSelByMonth] TO [vnteam]
GO