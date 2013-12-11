IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_ProfileSelByPage')
	BEGIN
		DROP  PROCEDURE  [dbo].[Is_Odds_ProfileSelByPage]
	END

GO
CREATE PROCEDURE [dbo].[Is_Odds_ProfileSelByPage]
	@ProfileName VARCHAR(50)='',
	@PageIndex INT=1, 
	@PageSize INT=20,
	@PageCount INT=0 OUTPUT
AS
/*
Created: 20100322@Jacob
Task : Search profile by name
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:
	
*/

SET NOCOUNT ON;

IF (@PageSize<=0) SET @PageSize = 20;

SELECT @ProfileName=ISNULL(@ProfileName,'');

WITH Profiles AS
(
	SELECT TOP(@PageIndex*@PageSize) ROW_NUMBER() OVER (ORDER BY [ProfileName] ASC) AS Row,
		  P.[ProfileID]
		  ,P.[ProfileName]
		  ,P.[Description]
		  ,P.[CreatedDate]
		  ,P.[Creator]
		  ,P.[IsDeleted]
		  ,P.[LastUpdate]
		  ,P.[TimeRange]
		  ,P.[SPName]
		  ,P.[SPParams]
		  ,[OddsType]
		  ,[LanguageList]
		  ,[OddsRegion]
		   FROM [dbo].[Odds_OddsProfile] P WITH(NOLOCK)
		   WHERE [IsDeleted]=0 
			AND (LEN(@ProfileName)=0 OR [ProfileName] LIKE ('%'+@ProfileName+'%'))			
)

SELECT   [ProfileID]
		  ,[ProfileName]
		  ,[Description]
		  ,[CreatedDate]
		  ,[Creator]
		  ,[IsDeleted]
		  ,[LastUpdate]
		  ,[TimeRange]
		  ,[SPName]
		  ,[SPParams]
		  ,[OddsType]
         ,[LanguageList]
        ,[OddsRegion]
FROM Profiles
WHERE  Row BETWEEN (@PageIndex-1)*@PageSize + 1 AND @PageIndex*@PageSize
	
DECLARE @RowCount INT

SELECT @RowCount=COUNT(P.[ProfileID])
FROM [dbo].[Odds_OddsProfile] P WITH(NOLOCK)
WHERE [IsDeleted]=0 AND (LEN(@ProfileName)=0 OR [ProfileName] LIKE ('%'+@ProfileName+'%'))			
			
IF (@RowCount % @PageSize = 0) SET @PageCount = (@RowCount / @PageSize);
ELSE SET @PageCount = (@RowCount / @PageSize) + 1;



GO
GRANT EXECUTE ON [dbo].[Is_Odds_ProfileSelByPage] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_ProfileSelByPage] TO [vnteam]
GO