IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_SystemSelByPage')
	BEGIN
		DROP  PROCEDURE  [dbo].[Is_Odds_SystemSelByPage]
	END

GO

CREATE PROCEDURE [dbo].[Is_Odds_SystemSelByPage]
(
	@SystemName VARCHAR(50)  = '',	
	@PageIndex INT = 1	,
	@PageSize INT = 20	,
	@PageCount INT =0 OUTPUT		
)
AS
/*
Created: 20100317@Jacob
Task : Search system by system name
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:		
	- @SystemName : If NULL or empty, search all	
	- @PageIndex : The page number
	- @PageSize : Page size
*/

SET NOCOUNT ON;

IF (@PageSize<=0) SET @PageSize = 20;

SELECT @SystemName=ISNULL(@SystemName,'');

WITH Systems AS
(
	SELECT TOP(@PageIndex*@PageSize) ROW_NUMBER() OVER (ORDER BY [SystemName] ASC) AS Row,
		   S.[SystemID]
		  ,S.[SystemName]		  
		  ,S.[CreatedDate]
		  ,S.[LastUpdate]		  
		  ,S.[Creator]
		   FROM [dbo].[System] S WITH(NOLOCK)
		   WHERE (LEN(@SystemName)=0 OR [SystemName] LIKE ('%'+@SystemName+'%'))			
)

SELECT  [SystemID]
		  ,[SystemName]		  
		  ,[CreatedDate]
		  ,[LastUpdate]		  
		  ,[Creator]
FROM Systems
WHERE  Row BETWEEN (@PageIndex-1)*@PageSize + 1 AND @PageIndex*@PageSize
	
DECLARE @RowCount INT

SELECT @RowCount=COUNT(S.[SystemID])
FROM [dbo].[System] S WITH(NOLOCK)
WHERE (LEN(@SystemName)=0 OR [SystemName] LIKE ('%'+@SystemName+'%'))			
			
IF (@RowCount % @PageSize = 0) SET @PageCount = (@RowCount / @PageSize);
ELSE SET @PageCount = (@RowCount / @PageSize) + 1;



GO
GRANT EXECUTE ON [dbo].[Is_Odds_SystemSelByPage] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_SystemSelByPage] TO [vnteam]
GO
