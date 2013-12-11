
ALTER PROCEDURE [dbo].Odds_Licensee_GetActivate
	@SystemID int	
AS
/*
Created: 20130704 @Thai.Tran
Task : Check if licensee is active/inactivate
DB: DB052.bodb_oddsfeed

*/
SET NOCOUNT ON
Select IsActive From dbo.System Where SystemID = @SystemID

GO
GRANT EXECUTE ON [dbo].Odds_Licensee_GetActivate TO [bodbAdminNet]
GO
GRANT VIEW DEFINITION ON [dbo].Odds_Licensee_GetActivate TO [vnteam]
GO