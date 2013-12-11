 IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Is_Odds_PartnerSelAll')
	BEGIN
		DROP  PROCEDURE  [dbo].[Is_Odds_PartnerSelAll]
	END

GO

CREATE PROCEDURE [dbo].[Is_Odds_PartnerSelAll]
(
	@UserName VARCHAR(50)  = '',
	@ProfileID INT = NULL,
	@DomainID  INT = NULL,
	@SystemID  INT = NULL,
	@PageIndex INT = 1	,
	@PageSize INT = 20	,
	@PageCount INT =0 OUTPUT		
)
AS
/*
Created: 20100317@Jacob
Task : Search partner by user name, profile name and domain name
DB: DB052.bodb_Maintenance
Revisions:
	- 

Param's Explanation:		
	- @Username : Username of partner
	- @ProfileID IF NULL, search all
	- @DomainID  IF NULL, search all
	- @PageIndex : The page number
	- @PageSize : Page size
*/

SET NOCOUNT ON;

IF (@PageSize<=0) SET @PageSize = 20;

SELECT @ProfileID=ISNULL(@ProfileID,0), @DomainID=ISNULL(@DomainID,0);

WITH Partners AS
(
	SELECT TOP(@PageIndex*@PageSize) ROW_NUMBER() OVER (ORDER BY [UserName] ASC) AS Row,
		   P.[PartnerID]
		  ,P.[SystemID]
		  ,P.[ProfileID]
		  ,P.[UserName]
		  ,P.[Site]
		  ,P.[IsAllowed]
		  ,P.[Remark]
		  ,P.[CreatedDate]
		  ,P.[LastUpdate]
		  ,P.[LastRequest]
		  ,P.[Creator]
		  ,P.[ContactPerson]
		  ,P.[ContactEmail]
		  ,F.[ProfileName]
		  ,S.[SystemName]
		   FROM ([dbo].[Odds_Partner] P WITH(NOLOCK) LEFT JOIN [dbo].[Odds_OddsProfile] F WITH(NOLOCK) ON P.[ProfileID]=F.[ProfileID])
				LEFT JOIN [dbo].[Odds_System] S WITH(NOLOCK) ON P.[SystemID]=S.[SystemID]
		   WHERE (LEN(@UserName)=0 OR [UserName] LIKE ('%'+@UserName+'%'))
		   -- Search in Profile
		   AND (ISNULL(@ProfileID,0) = 0 OR P.[ProfileID] = @ProfileID)
		   --- Search in System
		   AND (ISNULL(@SystemID,0) = 0 OR P.[SystemID] = @SystemID)						   
		   -- Search in Domain		   
		   AND (ISNULL(@DomainID,0) = 0 OR
					EXISTS(SELECT 1 
					FROM [dbo].[Odds_DomainSetting] S WITH(NOLOCK)
					WHERE S.[PartnerID]=P.[PartnerID] AND S.[DomainID] = @DomainID
					)
			)
			
			
)

SELECT  [PartnerID]
	  ,[SystemID]
	  ,[ProfileID]
	  ,[UserName]
	  ,[Site]
	  ,[IsAllowed]
	  ,[Remark]
	  ,[CreatedDate]
	  ,[LastUpdate]
	  ,[LastRequest]
	  ,[Creator]
	  ,[ProfileName]
	  ,[SystemName]
      ,[ContactPerson]
      ,[ContactEmail]
FROM Partners
WHERE  Row BETWEEN (@PageIndex-1)*@PageSize + 1 AND @PageIndex*@PageSize
	
DECLARE @RowCount INT

SELECT @RowCount=COUNT(P.[PartnerID])
FROM [dbo].[Odds_Partner] P WITH(NOLOCK)
WHERE (LEN(@UserName)=0 OR [UserName] LIKE ('%'+@UserName+'%'))
 -- Search in Profile
		   AND (ISNULL(@ProfileID,0) = 0 OR P.[ProfileID] = @ProfileID)
		   --- Search in System
		   AND (ISNULL(@SystemID,0) = 0 OR P.[SystemID] = @SystemID)						   
		   -- Search in Domain		   
		   AND (ISNULL(@DomainID,0) = 0 OR
					EXISTS(SELECT 1 
					FROM [dbo].[Odds_DomainSetting] S WITH(NOLOCK)
					WHERE S.[PartnerID]=P.[PartnerID] AND S.[DomainID] = @DomainID
					)
			)
						
IF (@RowCount % @PageSize = 0) SET @PageCount = (@RowCount / @PageSize);
ELSE SET @PageCount = (@RowCount / @PageSize) + 1;

GO
GRANT EXECUTE ON [dbo].[Is_Odds_PartnerSelAll] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_PartnerSelAll] TO [vnteam]
GO




