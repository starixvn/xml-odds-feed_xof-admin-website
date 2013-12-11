 select s.SystemName, p.username,p.Site,d.DomainName + '/feeds/' +   p.username + '/odds.xml' as url, 
   case when IpFrom<> IPTo then dbo.Is_Odds_NumberConvertToIP(i.ipfrom) + '->' + dbo.Is_Odds_NumberConvertToIP(i.ipto) 
        else dbo.Is_Odds_NumberConvertToIP(i.ipfrom) 
    end as IP
from Odds_Partner p (nolock)
    inner join Odds_System s (nolock) on s.systemid=p.systemid    
    inner join Odds_DomainSetting ds (nolock) on ds.partnerid=p.partnerid
    inner join Odds_Domain d (nolock) on d.domainid=ds.domainid
   inner join Odds_IPSetting i (nolock) on i.partnerid=p.partnerid
   
  
