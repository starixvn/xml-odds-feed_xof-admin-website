ALTER PROCEDURE [dbo].[Odds_BetTypeSelByUserName] 
(
	@UserName VARCHAR(50)
)
AS
/*    
Created: 20130129@Thai    
Task : Select list of bettypes    
DB: bodb_oddsfeed, bodb02    
Revisions:    
	- 20150909@bobby.nguyen: Re-format code and change setting key Type[Bettypes] to SName[SupportedBettypes]
    
Param's Explanation:    
 -     
*/
BEGIN
	SET NOCOUNT ON;

	DECLARE @BetTypeIDList VARCHAR(MAX)

	SELECT @BetTypeIDList = BetTypeIDList
	FROM dbo.PartnerProfile a WITH (NOLOCK)
	INNER JOIN dbo.Partner b WITH (NOLOCK) ON a.PartnerID = b.PartnerID
	WHERE b.UserName = @UserName

	IF @BetTypeIDList <> 'all'
	BEGIN
		SELECT bt.typeid AS BetTypeID
			, bt.typenamee AS BetTypeName
		FROM bodb02.dbo.bettype bt WITH (NOLOCK)
		WHERE bt.typeid IN (
				SELECT val
				FROM dbo.strSplit(@BetTypeIDList, ',')
				)
		ORDER BY bt.typenamee
	END
	ELSE
	BEGIN
		SELECT bt.typeid AS BetTypeID
			, bt.typenamee AS BetTypeName
		FROM bodb02.dbo.bettype bt WITH (NOLOCK)
		WHERE bt.typeid IN (
				SELECT val
				FROM dbo.strSplit((
							SELECT SValue
							FROM SystemSetting WITH (NOLOCK)
							WHERE SName = 'SupportedBettypes'
							), ',')
				)
		ORDER BY bt.typenamee
	END
END
GO

GRANT EXECUTE
	ON Odds_BetTypeSelByUserName
	TO [bodbAdminNet]
GO


