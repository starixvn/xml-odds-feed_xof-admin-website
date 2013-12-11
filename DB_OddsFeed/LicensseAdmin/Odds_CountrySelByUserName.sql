USE [bodb_oddsfeed]
GO
IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Odds_CountrySelByUserName')
	BEGIN
		DROP  PROCEDURE  [dbo].[Odds_CountrySelByUserName]
	END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Odds_CountrySelByUserName]    
 @UserName varchar(50)    
AS    
/*    
Created: 20130129@Thai    
Task : Select list of countries by username   
DB: bodb_oddsfeed, bodb02    
Revisions:    
 -     
    
Param's Explanation:    
 -     
*/    
SET NOCOUNT ON;    
    
DECLARE @CountryCodeList VARCHAR(MAX)    
    
select @CountryCodeList = CountryCodeList        
 from dbo.PartnerProfile a WITH(NOLOCK)    
 inner join dbo.Partner b WITH(NOLOCK) on a.PartnerID = b.PartnerID    
 where b.UserName = @UserName    
   
If @CountryCodeList <> 'all'  
 begin  
  Select CountryCode as CountryID, CountryName
  From dbo.Country with (nolock)
  Where IBCCode in (select val from dbo.strSplit(@CountryCodeList,','))
  order by CountryName   
 end  
Else   
 begin  
  Select CountryCode as CountryID, CountryName
  From dbo.Country with (nolock)  
  order by CountryName
 end  
   
   GO
GRANT EXECUTE ON [dbo].[Odds_CountrySelByUserName] TO [bodbAdminNet]
GO
GRANT EXECUTE ON [dbo].[Odds_CountrySelByUserName] TO [vnteam]
GO      