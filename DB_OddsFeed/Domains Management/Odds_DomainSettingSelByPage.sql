IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Odds_DomainSettingSelByPage')
	BEGIN
		DROP  PROCEDURE  [dbo].[Odds_DomainSettingSelByPage]
	END

GO
-- =============================================
-- Author:		Phillip	
-- Create date: 20130221
-- Description:	Get Settings by Domain ID
-- =============================================
CREATE PROCEDURE [dbo].[Odds_DomainSettingSelByPage]	
	@DomainID SMALLINT =0,
	@PageSize INT =20,
	@PageIndex INT =1,
	@PageCount INT=0 OUTPUT,
	@RowCount INT =0 OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

	SELECT @RowCount = COUNT(1) 
	FROM (
		SELECT [DomainID]
		FROM PartnerDomain WITH(nolock)
		WHERE [DomainID]=@DomainID 
		UNION ALL
		SELECT [DomainID]
		FROM SystemDomain WITH(nolock)
		WHERE [DomainID]=@DomainID
	)  as SubQueryAlias

	IF(ISNULL(@PageSize,0)=0) SET @PageSize=20;
	IF(ISNULL(@PageIndex,0)=0) SET @PageIndex=1;
	
	SELECT @PageCount=@RowCount/@PageSize + CASE WHEN @RowCount%@PageSize>0 THEN 1 ELSE 0 END;

	WITH DomainSettingList AS
		(SELECT D.DomainID,
				D.DomainName,
				P.Username,
				P.Site,
				P.IsAllowed,
				P.Remark,
				P.SystemID,
				S.SystemName
		FROM Domain D INNER JOIN
				PartnerDomain PD with(nolock) on PD.DomainID=D.DomainID inner join 
				[dbo].[Partner] P with(nolock) on P.PartnerID=PD.PartnerID inner join
				[dbo].[System] S with(nolock) on P.SystemID = S.SystemID
		WHERE D.DomainID=@DomainID
		UNION ALL
		SELECT  D.DomainID,
				D.DomainName,
				S.SystemName,
				'' as Site,
				S.IsActive as IsAllowed,
				'' as Remark,
				S.SystemID,
				S.SystemName
		FROM Domain D INNER JOIN
				SystemDomain SD with(nolock) on SD.DomainID=D.DomainID inner join 
				[dbo].[System] S with(nolock) on SD.SystemID = S.SystemID
		WHERE D.DomainID=@DomainID
		)
	SELECT	 TOP((@PageSize*@PageIndex)) ROW_NUMBER() OVER (ORDER BY SystemID, DomainID ASC) AS Row,
			 SystemID,
			 SystemName,
			 DomainID,
			 DomainName,
			 Username,
			 [Site],
			 IsAllowed,
			 Remark
	FROM DomainSettingList 
END

	
GO
GRANT EXECUTE ON [dbo].[Odds_DomainSettingSelByPage] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Odds_DomainSettingSelByPage] TO [vnteam]
GO