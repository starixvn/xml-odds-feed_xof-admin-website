USE [bodb_oddsfeed]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Odds_User_ReCheckIn]   
 @OnUserId   INT   
 ,@IsKickedOut  BIT = 0 OUTPUT      
AS  
  
/*    
 * Created: 20130320@Thai
 * Task : User recheckin  
 * DB: bodb_oddsfeed
 *    
 * Revisions:       
 * Param's Explanation:   
*/  
  
BEGIN  
 SET NOCOUNT ON   
  
 IF EXISTS ( SELECT 1   
    FROM On_User WITH (NOLOCK)   
    WHERE OnUserID = @OnUserId AND IsKickedOut = 1 )      
  BEGIN      
   SET @IsKickedOut = 1        
  END      
 ELSE      
  BEGIN      
   SET @IsKickedOut = 0      
  
   UPDATE On_User WITH (ROWLOCK, UPDLOCK)  
   SET ReloadTime = GETDATE()       
   WHERE OnUserID = @OnUserId      
         
   IF @@ROWCOUNT = 0      
    SELECT @IsKickedOut = 1      
  END  
END   
  
GO
GRANT EXECUTE ON [Odds_User_ReCheckIn] To [bodbAdminNet]