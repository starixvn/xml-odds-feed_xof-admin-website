 set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[Is_Odds_DomainBySystemSel]
	-- Add the parameters for the stored procedure here
	@systemId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select DomainID, DomainName
	from Domain inner join DomainSystem on Domain.DomainID = DomainSystem.DomainID
	where SystemID = @systemId
END

GO
GRANT EXECUTE ON [dbo].[Is_Odds_DomainBySystemSel] TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].[Is_Odds_DomainBySystemSel] TO [vnteam]
GO
