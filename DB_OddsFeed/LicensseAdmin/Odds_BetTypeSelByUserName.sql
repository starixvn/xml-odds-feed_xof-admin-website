ALTER PROCEDURE [dbo].[Odds_BetTypeSelByUserName]    
 @UserName varchar(50)    
AS    
/*    
Created: 20130129@Thai    
Task : Select list of bettypes    
DB: bodb_oddsfeed, bodb02    
Revisions:    
 -     
    
Param's Explanation:    
 -     
*/    
SET NOCOUNT ON;    
    
DECLARE @BetTypeIDList VARCHAR(MAX)    
    
select @BetTypeIDList = BetTypeIDList        
 from dbo.PartnerProfile a WITH(NOLOCK)    
 inner join dbo.Partner b WITH(NOLOCK) on a.PartnerID = b.PartnerID    
 where b.UserName = @UserName    
   
if @BetTypeIDList <> 'all'  
 begin  
  select bt.typeid as BetTypeID, bt.typenamee as BetTypeName
  from  bodb02.dbo.bettype bt with (nolock)  
  where bt.typeid in (select val from dbo.strSplit(@BetTypeIDList,','))  
  order by bt.typenamee  
 end  
 else   
 begin  
  select bt.typeid as BetTypeID, bt.typenamee as BetTypeName
  from  bodb02.dbo.bettype bt with (nolock)  
  where bt.typeid in (select val from dbo.strSplit(  
              (select SValue  
               from SystemSetting with (nolock)  
               where Type = 'BetTypes'  
               ) ,','  
             )  
      )  
  order by bt.typenamee  
 end  
   