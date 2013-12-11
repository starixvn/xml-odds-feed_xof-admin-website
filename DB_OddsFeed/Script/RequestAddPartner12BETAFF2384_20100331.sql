--Insert new Partner 
insert into Odds_Partner (UserName, SystemID, ProfileID, Site, IsAllowed, Remark, CreatedDate, LastUpdate, LastRequest, Creator,ContactPerson,ContactEmail)
select '12BETAFF2384', 
(select top(1) SystemID from Odds_System (nolock) where SystemName='12BET'),
(select top(1) ProfileID from Odds_OddsProfile (nolock) where ProfileName='OddsFeed_Share_OddsSel_12BET'),
'chinarfl.com',
1,
'Requested by Maggie 31st March 2010',
current_timestamp,
current_timestamp,
null,
1,
'harry wang',
'rugbywhx9@hotmail.com'


--Insert IP Setting 
insert into Odds_IPSetting (PartnerID, IPFrom, IPTo, Country)

select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12BETAFF2384'),
dbo.Is_odds_IPConvertToNumber('113.11.196.159'),
dbo.Is_odds_IPConvertToNumber('113.11.196.159'),
'China'

--Insert Domain Setting
insert into Odds_DomainSetting (partnerID, DomainID)
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12BETAFF2384'),
(select top(1) DomainID from Odds_Domain (nolock) where DomainName='http://feeds.12bet.com')