USE [bodb_oddsfeed]
GO
/****** Object:  UserDefinedFunction [dbo].[f_Odds_GroupBettype_Format]    Script Date: 3/15/2016 2:11:00 PM ******/
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
    DB : DB02.bodb02, DB03.on_user,DB052.Affiliate_12BET
	Task : Convert a value to string for all data types
	Author : Peter@20090909
	
*/
BEGIN	   
	DECLARE @bettypeGroup		VARCHAR(MAX)
		,@SupportedBettypes		VARCHAR(MAX)
		,@bettypeConversion		VARCHAR(MAX)

	
	IF(@BettypeIDList IS NOT NULL)
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
		SELECT @bettypeGroup = COALESCE(@bettypeGroup + ',', '') + CAST(g.Bettype AS VARCHAR(10))
		FROM bodb02.dbo.f_GroupBettype() g
			INNER JOIN CTE_Bettype b 
			ON g.GroupTypeId = b.Bettype
			LEFT JOIN CTE_Bettype cbt 
			ON g.Bettype = cbt.Bettype

		WHERE cbt.Bettype IS NULL
	END
	
	--@bettypeConversion
	SELECT @bettypeConversion = COALESCE(@bettypeConversion + ',', '') + CAST(bt.Bettype AS VARCHAR(10))
	FROM (
		SELECT t.Bettype
			, CASE WHEN g.Bettype IS NULL OR t.Bettype = g.GroupTypeId THEN 1 ELSE 0 END AS IsParent
		FROM (
			SELECT CAST (val AS INT) AS Bettype
			FROM dbo.strSplit (@BettypeIDList, ',')
			WHERE LEN(val)  > 0

			UNION ALL
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
				WHEN @IsShowChild = 0 AND IsParent = 0 THEN 0
				ELSE 1
			END
	GROUP BY bt.Bettype


	RETURN @bettypeConversion
END
GO