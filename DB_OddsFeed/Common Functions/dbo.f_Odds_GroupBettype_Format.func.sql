USE [bodb_oddsfeed]
GO
/****** Object:  UserDefinedFunction [dbo].[f_Odds_GroupBettype_Format]    Script Date: 8/2/2016 11:15:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION [dbo].[f_Odds_GroupBettype_Format] 
(
	@BettypeIDList	VARCHAR(MAX)
	, @IsShowChild	BIT = 1
)
RETURNS VARCHAR(MAX)
AS
/*
    Creator:20160408@bobby
	Task : #54093 support new bettype PH AIO
	DB:DBODDSFEED.bodb_Oddsfeed
	Revisions:
		-20160725@bobby: #61422 - Implement 19 new PH-AIO parent-child bet types
	Param's Explaination:
	
*/
BEGIN	   
	DECLARE @bettypeGroup		VARCHAR(MAX)
		,@SupportedBettypes		VARCHAR(MAX)
		,@bettypeConversion		VARCHAR(MAX)
	
	SET @bettypeConversion		= @BettypeIDList
	
	IF(@BettypeIDList IS NOT NULL 
		AND @BettypeIDList <> 'all')
	BEGIN
		
		
		SELECT @SupportedBettypes = SValue
		FROM dbo.SystemSetting WITH (NOLOCK)
		WHERE [Type]	= 'BetTypes'
			AND SName	= 'SupportedBettypes'

		;WITH CTE_Bettype AS 
		(
			SELECT CAST (val AS INT) AS Bettype
			FROM dbo.strSplit (@BettypeIDList, ',')
			where len(val)  > 0
		)
		SELECT @bettypeGroup = COALESCE(@bettypeGroup + ',', '') + CAST(g.GroupTypeId AS VARCHAR(10))
		FROM bodb02.dbo.f_GroupBettype() g
			INNER JOIN CTE_Bettype cbt 
			ON g.Bettype = cbt.Bettype
		
		GROUP BY g.GroupTypeId
		
		-- final bettypes
		SET @bettypeConversion = NULL
		SELECT @bettypeConversion = COALESCE(@bettypeConversion + ',', '') + CAST(bt.Bettype AS VARCHAR(10))
		FROM (
			SELECT t.Bettype
				, CASE WHEN g.Bettype IS NULL OR t.Bettype = g.GroupTypeId THEN 1 ELSE 0 END AS IsParent
			FROM (
				-- inputed bet types
				SELECT CAST (val AS INT) AS Bettype
				FROM dbo.strSplit (@BettypeIDList, ',')
				WHERE LEN(val)  > 0

				-- child bet types
				UNION 
				SELECT g.Bettype
				FROM dbo.strSplit (@bettypeGroup, ',') AS bg
					INNER JOIN bodb02.dbo.f_GroupBettype() g 
					ON bg.val = g.GroupTypeId
				WHERE LEN(val)  > 0 and g.GroupTypeId <> g.Bettype

				-- parent bet types
				UNION 
				SELECT CAST (val AS INT) AS Bettype
				FROM dbo.strSplit (@bettypeGroup, ',')
				WHERE LEN(val)  > 0
			) AS t
				LEFT JOIN bodb02.dbo.f_GroupBettype() g 
				ON t.Bettype = g.Bettype
				INNER JOIN dbo.strSplit (@SupportedBettypes, ',') st 
				ON t.Bettype = st.val
			WHERE t.Bettype > 0
		) AS bt
		WHERE 1 = CASE 
					WHEN @IsShowChild = 1 THEN 1
					WHEN @IsShowChild = 0 AND IsParent = 1 THEN 1
					ELSE 0
				END
		GROUP BY bt.Bettype
	END

	RETURN @bettypeConversion
END
