ALTER PROCEDURE [dbo].[Odds_SportSelByUserName]  
 @UserName varchar(50)  
AS  
/*  
Created: 20130129@Thai  
Task : Select list of sports  
DB: bodb_oddsfeed, bodb02  
Revisions:  
 -   
  
Param's Explanation:  
 -   
*/  
SET NOCOUNT ON;  
  
DECLARE @SportIDList VARCHAR(MAX)  
  
select @SportIDList = SportIDList    
 from dbo.PartnerProfile a WITH(NOLOCK)  
 inner join dbo.Partner b WITH(NOLOCK) on a.PartnerID = b.PartnerID  
 where b.UserName = @UserName  

if @SportIDList <> 'all'
begin
	select s.SportType as SportID, s.SportName
	from bodb02.dbo.Sports s with (nolock) 
	where s.SportType in (select val from dbo.strSplit(@SportIDList,','))
	order by s.SportName
end
else
begin
	select s.SportType as SportID, s.SportName
	from bodb02.dbo.Sports s with (nolock) 
	where s.SportType in (select val from dbo.strSplit(
														(select SValue  
														  from SystemSetting with (nolock)  
														  where Type = 'SportTypes'
														 ),','
													   )
						)
	order by s.SportName
end  
 
Go
Grant execute on [Odds_SportSelByUserName] to [bodbAdminnet]