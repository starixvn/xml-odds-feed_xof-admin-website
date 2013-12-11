USE  bodb_Maintenance

/* Clear data and reset tables*/
TRUNCATE TABLE Odds_System

TRUNCATE TABLE Odds_Domain

TRUNCATE TABLE Odds_OddsProfile

TRUNCATE TABLE Odds_Partner

TRUNCATE TABLE Odds_IPSetting

TRUNCATE TABLE Odds_DomainSetting

truncate table Odds_LogType

GO

set ansi_warnings off 

GO
/*Insert default data into 'Odds_LogType'*/

Insert into Odds_LogType(TypeName,Description)
select 'Partner',	'Write log for updating partner info'
union all
select 'Statistics','Write log for statistics info'
union all
select 'Profile','Write log for profile info'
union all
select 'Domain','Write log for Domain info'
union all
select 'News','Write log for News'



/*Insert default data into 'Odds_System'*/

insert into Odds_System (SystemName, Creator, CreatedDate, LastUpdate)
select 'IBC', 1, current_timestamp, current_timestamp
union all
select '12BET', 1, current_timestamp, current_timestamp


/*Insert default data into Odds_Domain*/

insert into Odds_Domain (DomainName, SystemID, Creator, CreatedDate, LastUpdate)
select 'http://portal.ibcbet.com', 1, 1, current_timestamp, current_timestamp -- IBC


union all
select 'http://portal-1.ibcbet.com', 1, 1, current_timestamp, current_timestamp -- IBC


union all
select 'http://portal-2.ibcbet.com', 1, 1, current_timestamp, current_timestamp -- IBC
union all
select 'http://portal-3.ibcbet.com', 1, 1, current_timestamp, current_timestamp -- IBC
union all
select 'http://localhost', 1, 1, current_timestamp, current_timestamp -- IBC for testing case
union all
select 'http://feeds.12bet.com', 2, 1, current_timestamp, current_timestamp -- 12BET
union all
select 'http://feeds.12ying.com', 2, 1, current_timestamp, current_timestamp -- 12BET



/*Insert default data into Odds_OddsProfile*/

insert into Odds_OddsProfile (ProfileName, Creator, IsDeleted, CreatedDate, LastUpdate, SPName, SPParams)

select 'OddsFeed_common_OddsSel', 1, 0, current_timestamp, current_timestamp, 'OddsFeed_common_OddsSel',''
union all
select 'OddsFeed_BetBrain_OddsSel', 1, 0, current_timestamp, current_timestamp, 'OddsFeed_BetBrain_OddsSel',''
union all
select 'OddsFeed_AffFull_OddsSel', 1, 0, current_timestamp, current_timestamp, 'OddsFeed_AffFull_OddsSel',''
union all
select 'OddsFeed_mrlazypunter_OddsSel', 1, 0, current_timestamp, current_timestamp, 'OddsFeed_mrlazypunter_OddsSel',''
union all
select 'OddsFeed_Score22_OddsSel', 1, 0, current_timestamp, current_timestamp, 'OddsFeed_Score22_OddsSel',''
union all
select 'OddsFeed_commonfull_OddsSel', 1, 0, current_timestamp, current_timestamp, 'OddsFeed_commonfull_OddsSel',''
union all
select 'OddsFeed_TipEx_OddsSel', 1, 0, current_timestamp, current_timestamp, 'OddsFeed_TipEx_OddsSel','timestamp'
union all
select 'OddsFeed_BetRadar_OddsSel', 1, 0, current_timestamp, current_timestamp, 'OddsFeed_BetRadar_OddsSel',''
union all
select 'OddsFeed_Share_OddsSel_12BET', 1, 0, current_timestamp, current_timestamp, 'OddsFeed_Share_OddsSel_12BET',''



/*Insert default data into Odds_Partner*/

insert into Odds_Partner (UserName, SystemID, ProfileID, Site, IsAllowed, Remark, CreatedDate, LastUpdate, LastRequest, Creator)
select 'BetRadar', 
(select top(1) SystemID from Odds_System (nolock) where SystemName='IBC'),
(select top(1) ProfileID from Odds_OddsProfile (nolock) where ProfileName='OddsFeed_BetRadar_OddsSel'),
'BetRadar.com',
0,
null,
current_timestamp,
current_timestamp,
null,
1
union all
select 'Score22', 
(select top(1) SystemID from Odds_System (nolock) where SystemName='IBC'),
(select top(1) ProfileID from Odds_OddsProfile (nolock) where ProfileName='OddsFeed_Score22_OddsSel'),
'www.Bet007.com',
1,
null,
current_timestamp,
current_timestamp,
null,
1
union all
select 'TipEx', 
(select top(1) SystemID from Odds_System (nolock) where SystemName='IBC'),
(select top(1) ProfileID from Odds_OddsProfile (nolock) where ProfileName='OddsFeed_TipEx_OddsSel'),
'www.tip-ex.com/',
1,
null,
current_timestamp,
current_timestamp,
null,
1
union all
select 'BetGenius', 
(select top(1) SystemID from Odds_System (nolock) where SystemName='IBC'),
(select top(1) ProfileID from Odds_OddsProfile (nolock) where ProfileName='OddsFeed_common_OddsSel'),
null,
1,
null,
current_timestamp,
current_timestamp,
null,
1
union all
select 'Asianodds', 
(select top(1) SystemID from Odds_System (nolock) where SystemName='IBC'),
(select top(1) ProfileID from Odds_OddsProfile (nolock) where ProfileName='OddsFeed_common_OddsSel'),
'www.Asianodds.org',
1,
null,
current_timestamp,
current_timestamp,
null,
1
union all
select 'betbrain', 
(select top(1) SystemID from Odds_System (nolock) where SystemName='IBC'),
(select top(1) ProfileID from Odds_OddsProfile (nolock) where ProfileName='OddsFeed_BetBrain_OddsSel'),
'www.Betbrain.com',
1,
null,
current_timestamp,
current_timestamp,
null,
1
union all
select 'betbrain', 
(select top(1) SystemID from Odds_System (nolock) where SystemName='12BET'),
(select top(1) ProfileID from Odds_OddsProfile (nolock) where ProfileName='OddsFeed_BetBrain_OddsSel'),
null,
1,
null,
current_timestamp,
current_timestamp,
null,
1
union all
select 'Flybet', 
(select top(1) SystemID from Odds_System (nolock) where SystemName='12BET'),
(select top(1) ProfileID from Odds_OddsProfile (nolock) where ProfileName='OddsFeed_Share_OddsSel_12BET'),
'Flybet.com.cn',
1,
null,
current_timestamp,
current_timestamp,
null,
1
union all
select '12betaff062', 
(select top(1) SystemID from Odds_System (nolock) where SystemName='12BET'),
(select top(1) ProfileID from Odds_OddsProfile (nolock) where ProfileName='OddsFeed_AffFull_OddsSel'),
'www.sportspunter.com',
1,
'Found 12bet text links under Asian handicap, also display 12bet outright odds in the world cup page',
current_timestamp,
current_timestamp,
null,
1
union all
select '12betaff286', 
(select top(1) SystemID from Odds_System (nolock) where SystemName='12BET'),
(select top(1) ProfileID from Odds_OddsProfile (nolock) where ProfileName='OddsFeed_Share_OddsSel_12BET'),
'www.oddsportal.com',
1,
'Display of the odds is subjected to the match, 12 bet also in the review of bookmakers pages',
current_timestamp,
current_timestamp,
null,
1
union all
select '12BETAFF807', 
(select top(1) SystemID from Odds_System (nolock) where SystemName='12BET'),
(select top(1) ProfileID from Odds_OddsProfile (nolock) where ProfileName='OddsFeed_Share_OddsSel_12BET'),
'www.in4bet.com',
1,
'12bet banner is displayed on the top of the home page ( in a rotate mode) ',
current_timestamp,
current_timestamp,
null,
1
union all
select '12betaff022', 
(select top(1) SystemID from Odds_System (nolock) where SystemName='12BET'),
(select top(1) ProfileID from Odds_OddsProfile (nolock) where ProfileName='OddsFeed_Share_OddsSel_12BET'),
'www.gooooal.com',
0,
'famous website in China. Based on good performing affiliate partner',
current_timestamp,
current_timestamp,
null,
1
union all
select '12BETAFF1497', 
(select top(1) SystemID from Odds_System (nolock) where SystemName='12BET'),
(select top(1) ProfileID from Odds_OddsProfile (nolock) where ProfileName='OddsFeed_Share_OddsSel_12BET'),
'www.theoddschecker.co.uk',
1,
'New Approved. Follow up.',
current_timestamp,
current_timestamp,
null,
1
union all
select '12BETAFF142', 
(select top(1) SystemID from Odds_System (nolock) where SystemName='12BET'),
(select top(1) ProfileID from Odds_OddsProfile (nolock) where ProfileName='OddsFeed_Share_OddsSel_12BET'),
'www.bet520.com',
1,
'website change version, will upload later.',
current_timestamp,
current_timestamp,
null,
1
union all
select '12BETAFF1455', 
(select top(1) SystemID from Odds_System (nolock) where SystemName='12BET'),
(select top(1) ProfileID from Odds_OddsProfile (nolock) where ProfileName='OddsFeed_Share_OddsSel_12BET'),
'gamblers-paradise.net',
1,
'New Approved, 12bet is on top of the odds page ( in a rotate mode)',
current_timestamp,
current_timestamp,
null,
1
union all
select '12BETAFF1773', 
(select top(1) SystemID from Odds_System (nolock) where SystemName='12BET'),
(select top(1) ProfileID from Odds_OddsProfile (nolock) where ProfileName='OddsFeed_Share_OddsSel_12BET'),
'oddsfinder.com',
1,
'12bet banner is displayed in the home page',
current_timestamp,
current_timestamp,
null,
1
union all
select '12BETAFF177', 
(select top(1) SystemID from Odds_System (nolock) where SystemName='12BET'),
(select top(1) ProfileID from Odds_OddsProfile (nolock) where ProfileName='OddsFeed_Share_OddsSel_12BET'),
'www.1soccer.com',
1,
'famous website in China, good performance affiliate partner',
current_timestamp,
current_timestamp,
null,
1
union all
select '12BETAFF009', 
(select top(1) SystemID from Odds_System (nolock) where SystemName='12BET'),
(select top(1) ProfileID from Odds_OddsProfile (nolock) where ProfileName='OddsFeed_Share_OddsSel_12BET'),
'www.mso.com.cn',
1,
'Famous website in China, Based on best performance affiliate partner.',
current_timestamp,
current_timestamp,
null,
1


/*Insert default data into Odds_IPSetting*/
insert into Odds_IPSetting (PartnerID, IPFrom, IPTo, Country)

select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='BetRadar'),
dbo.Is_odds_IPConvertToNumber('195.139.104.225'),
dbo.Is_odds_IPConvertToNumber('195.139.104.225'),
null
union all
select(select top(1) PartnerID from Odds_Partner (nolock) where UserName='BetRadar'),
dbo.Is_odds_IPConvertToNumber('194.19.15.162'),
dbo.Is_odds_IPConvertToNumber('194.19.15.162'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='BetRadar'),
dbo.Is_odds_IPConvertToNumber('89.202.213.194'),
dbo.Is_odds_IPConvertToNumber('89.202.213.194'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Score22'),
dbo.Is_odds_IPConvertToNumber('59.34.17.67'),
dbo.Is_odds_IPConvertToNumber('59.34.17.67'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Score22'),
dbo.Is_odds_IPConvertToNumber('59.34.17.68'),
dbo.Is_odds_IPConvertToNumber('59.34.17.68'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Score22'),
dbo.Is_odds_IPConvertToNumber('59.34.17.66'),
dbo.Is_odds_IPConvertToNumber('59.34.17.66'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Score22'),
dbo.Is_odds_IPConvertToNumber('61.143.225.19'),
dbo.Is_odds_IPConvertToNumber('61.143.225.19'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Score22'),
dbo.Is_odds_IPConvertToNumber('210.51.15.31'),
dbo.Is_odds_IPConvertToNumber('210.51.15.31'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Score22'),
dbo.Is_odds_IPConvertToNumber('210.184.135.246'),
dbo.Is_odds_IPConvertToNumber('210.184.135.246'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Score22'),
dbo.Is_odds_IPConvertToNumber('210.184.135.247'),
dbo.Is_odds_IPConvertToNumber('210.184.135.247'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Score22'),
dbo.Is_odds_IPConvertToNumber('210.184.135.244'),
dbo.Is_odds_IPConvertToNumber('210.184.135.244'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Score22'),
dbo.Is_odds_IPConvertToNumber('61.143.225.21'),
dbo.Is_odds_IPConvertToNumber('61.143.225.21'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Score22'),
dbo.Is_odds_IPConvertToNumber('61.143.225.24'),
dbo.Is_odds_IPConvertToNumber('61.143.225.24'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Score22'),
dbo.Is_odds_IPConvertToNumber('75.126.26.162'),
dbo.Is_odds_IPConvertToNumber('75.126.26.162'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='TipEx'),
dbo.Is_odds_IPConvertToNumber('193.65.178.10'),
dbo.Is_odds_IPConvertToNumber('193.65.178.10'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='TipEx'),
dbo.Is_odds_IPConvertToNumber('193.65.178.11'),
dbo.Is_odds_IPConvertToNumber('193.65.178.11'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='TipEx'),
dbo.Is_odds_IPConvertToNumber('193.65.178.9'),
dbo.Is_odds_IPConvertToNumber('193.65.178.9'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='TipEx'),
dbo.Is_odds_IPConvertToNumber('59.188.19.211'),
dbo.Is_odds_IPConvertToNumber('59.188.19.211'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='TipEx'),
dbo.Is_odds_IPConvertToNumber('210.184.135.247'),
dbo.Is_odds_IPConvertToNumber('210.184.135.247'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='TipEx'),
dbo.Is_odds_IPConvertToNumber('59.188.19.212'),
dbo.Is_odds_IPConvertToNumber('59.188.19.212'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='TipEx'),
dbo.Is_odds_IPConvertToNumber('193.65.178.12'),
dbo.Is_odds_IPConvertToNumber('193.65.178.12'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='TipEx'),
dbo.Is_odds_IPConvertToNumber('219.90.112.83'),
dbo.Is_odds_IPConvertToNumber('219.90.112.83'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='TipEx'),
dbo.Is_odds_IPConvertToNumber('219.90.112.84'),
dbo.Is_odds_IPConvertToNumber('219.90.112.84'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='TipEx'),
dbo.Is_odds_IPConvertToNumber('219.90.112.85'),
dbo.Is_odds_IPConvertToNumber('219.90.112.85'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='BetGenius'),
dbo.Is_odds_IPConvertToNumber('213.152.246.45'),
dbo.Is_odds_IPConvertToNumber('213.152.246.45'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='BetGenius'),
dbo.Is_odds_IPConvertToNumber('213.152.246.46'),
dbo.Is_odds_IPConvertToNumber('213.152.246.46'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='BetGenius'),
dbo.Is_odds_IPConvertToNumber('213.152.246.47'),
dbo.Is_odds_IPConvertToNumber('213.152.246.47'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='BetGenius'),
dbo.Is_odds_IPConvertToNumber('213.152.246.36'),
dbo.Is_odds_IPConvertToNumber('213.152.246.36'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='BetGenius'),
dbo.Is_odds_IPConvertToNumber('213.152.246.37'),
dbo.Is_odds_IPConvertToNumber('213.152.246.37'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='BetGenius'),
dbo.Is_odds_IPConvertToNumber('213.152.246.18'),
dbo.Is_odds_IPConvertToNumber('213.152.246.18'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='BetGenius'),
dbo.Is_odds_IPConvertToNumber('213.152.246.27'),
dbo.Is_odds_IPConvertToNumber('213.152.246.27'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='BetGenius'),
dbo.Is_odds_IPConvertToNumber('213.152.246.32'),
dbo.Is_odds_IPConvertToNumber('213.152.246.32'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='BetGenius'),
dbo.Is_odds_IPConvertToNumber('78.86.122.19'),
dbo.Is_odds_IPConvertToNumber('78.86.122.19'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='BetGenius'),
dbo.Is_odds_IPConvertToNumber('77.48.50.214'),
dbo.Is_odds_IPConvertToNumber('77.48.50.214'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='BetGenius'),
dbo.Is_odds_IPConvertToNumber('85.92.214.142'),
dbo.Is_odds_IPConvertToNumber('85.92.214.142'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='BetGenius'),
dbo.Is_odds_IPConvertToNumber('85.92.214.178'),
dbo.Is_odds_IPConvertToNumber('85.92.214.178'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='BetGenius'),
dbo.Is_odds_IPConvertToNumber('85.92.214.215'),
dbo.Is_odds_IPConvertToNumber('85.92.214.215'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='BetGenius'),
dbo.Is_odds_IPConvertToNumber('85.92.214.230'),
dbo.Is_odds_IPConvertToNumber('85.92.214.230'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='BetGenius'),
dbo.Is_odds_IPConvertToNumber('87.194.123.85'),
dbo.Is_odds_IPConvertToNumber('87.194.123.85'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='BetGenius'),
dbo.Is_odds_IPConvertToNumber('195.222.12.29'),
dbo.Is_odds_IPConvertToNumber('195.222.12.29'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Asianodds'),
dbo.Is_odds_IPConvertToNumber('85.17.152.1'),
dbo.Is_odds_IPConvertToNumber('85.17.152.15'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Asianodds'),
dbo.Is_odds_IPConvertToNumber('77.235.50.255'),
dbo.Is_odds_IPConvertToNumber('77.235.50.255'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Asianodds'),
dbo.Is_odds_IPConvertToNumber('77.235.37.3'),
dbo.Is_odds_IPConvertToNumber('77.235.37.3'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='betbrain' and 
SystemID = (select top(1) SystemID from Odds_System (nolock) where SystemName = 'IBC')),
dbo.Is_odds_IPConvertToNumber('81.181.93.37'),
dbo.Is_odds_IPConvertToNumber('81.181.93.37'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='betbrain' and 
SystemID = (select top(1) SystemID from Odds_System (nolock) where SystemName = 'IBC')),
dbo.Is_odds_IPConvertToNumber('83.136.93.128'),
dbo.Is_odds_IPConvertToNumber('83.136.93.255'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='betbrain' and 
SystemID = (select top(1) SystemID from Odds_System (nolock) where SystemName = 'IBC')),
dbo.Is_odds_IPConvertToNumber('83.151.156.212'),
dbo.Is_odds_IPConvertToNumber('83.151.156.212'),
null   
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='betbrain' and 
SystemID = (select top(1) SystemID from Odds_System (nolock) where SystemName = '12BET')),
dbo.Is_odds_IPConvertToNumber('83.136.93.25'),
dbo.Is_odds_IPConvertToNumber('83.136.93.25'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='betbrain' and 
SystemID = (select top(1) SystemID from Odds_System (nolock) where SystemName = '12BET')),
dbo.Is_odds_IPConvertToNumber('83.136.93.128'),
dbo.Is_odds_IPConvertToNumber('83.136.93.128'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='betbrain' and 
SystemID = (select top(1) SystemID from Odds_System (nolock) where SystemName = '12BET')),
dbo.Is_odds_IPConvertToNumber('85.151.156.212'),
dbo.Is_odds_IPConvertToNumber('85.151.156.212'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='betbrain' and 
SystemID = (select top(1) SystemID from Odds_System (nolock) where SystemName = '12BET')),
dbo.Is_odds_IPConvertToNumber('81.181.93.37'),
dbo.Is_odds_IPConvertToNumber('81.181.93.37'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Flybet'),
dbo.Is_odds_IPConvertToNumber('58.67.138.166'),
dbo.Is_odds_IPConvertToNumber('58.67.138.166'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Flybet'),
dbo.Is_odds_IPConvertToNumber('58.67.138.170'),
dbo.Is_odds_IPConvertToNumber('58.67.138.170'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Flybet'),
dbo.Is_odds_IPConvertToNumber('219.136.252.70'),
dbo.Is_odds_IPConvertToNumber('219.136.252.70'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Flybet'),
dbo.Is_odds_IPConvertToNumber('116.22.44.8'),
dbo.Is_odds_IPConvertToNumber('116.22.44.8'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Flybet'),
dbo.Is_odds_IPConvertToNumber('61.140.125.120'),
dbo.Is_odds_IPConvertToNumber('61.140.125.120'),
null 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12betaff286'),
dbo.Is_odds_IPConvertToNumber('82.142.118.133'),
dbo.Is_odds_IPConvertToNumber('82.142.118.133'),
'Czech' 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12betaff286'),
dbo.Is_odds_IPConvertToNumber('195.250.147.186'),
dbo.Is_odds_IPConvertToNumber('195.250.147.186'),
'Czech' 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12betaff286'),
dbo.Is_odds_IPConvertToNumber('195.250.144.194'),
dbo.Is_odds_IPConvertToNumber('195.250.144.194'),
'Czech' 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12BETAFF807'),
dbo.Is_odds_IPConvertToNumber('195.190.13.91'),
dbo.Is_odds_IPConvertToNumber('195.190.13.91'),
'Russian Federation/Ukraine' 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12BETAFF1497'),
dbo.Is_odds_IPConvertToNumber('87.106.41.94'),
dbo.Is_odds_IPConvertToNumber('87.106.41.94'),
'United Kindom of Great Britain/Germany'  
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12BETAFF142'),
dbo.Is_odds_IPConvertToNumber('218.213.194.138'),
dbo.Is_odds_IPConvertToNumber('218.213.194.138'),
'China/HK'
union all 
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12BETAFF1455'),
dbo.Is_odds_IPConvertToNumber('90.185.222.141'),
dbo.Is_odds_IPConvertToNumber('90.185.222.141'),
'Denmark/Denmark' 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12BETAFF177'),
dbo.Is_odds_IPConvertToNumber('59.38.111.137'),
dbo.Is_odds_IPConvertToNumber('59.38.111.137'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12BETAFF062'),
dbo.Is_odds_IPConvertToNumber('207.228.252.35'),
dbo.Is_odds_IPConvertToNumber('207.228.252.36'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12BETAFF022'),
dbo.Is_odds_IPConvertToNumber('222.66.110.114'),
dbo.Is_odds_IPConvertToNumber('222.66.110.114'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12BETAFF1773'),
dbo.Is_odds_IPConvertToNumber('83.170.115.226'),
dbo.Is_odds_IPConvertToNumber('83.170.115.226'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12BETAFF009'),
dbo.Is_odds_IPConvertToNumber('211.155.25.24'),
dbo.Is_odds_IPConvertToNumber('211.155.25.24'),
null
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12BETAFF009'),
dbo.Is_odds_IPConvertToNumber('211.155.25.22'),
dbo.Is_odds_IPConvertToNumber('211.155.25.22'),
null


/*Insert default data into Odds_DomainSetting*/
insert into Odds_DomainSetting (partnerID, DomainID)
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='BetRadar'),
(select top(1) DomainID from Odds_Domain (nolock) where DomainName='http://portal.ibcbet.com') 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Score22'),
(select top(1) DomainID from Odds_Domain (nolock) where DomainName='http://portal.ibcbet.com') 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='TipEx'),
(select top(1) DomainID from Odds_Domain (nolock) where DomainName='http://portal.ibcbet.com') 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='BetGenius'),
(select top(1) DomainID from Odds_Domain (nolock) where DomainName='http://portal.ibcbet.com') 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Asianodds'),
(select top(1) DomainID from Odds_Domain (nolock) where DomainName='http://portal.ibcbet.com') 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Betbrain' and 
SystemID = (select top(1) SystemID from Odds_System (nolock) where SystemName = 'IBC')),
(select top(1) DomainID from Odds_Domain (nolock) where DomainName='http://portal.ibcbet.com') 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='betbrain' and 
SystemID = (select top(1) SystemID from Odds_System (nolock) where SystemName = '12BET')),
(select top(1) DomainID from Odds_Domain (nolock) where DomainName='http://feeds.12bet.com')  
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='Flybet'),
(select top(1) DomainID from Odds_Domain (nolock) where DomainName='http://feeds.12bet.com') 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12betaff062'),
(select top(1) DomainID from Odds_Domain (nolock) where DomainName='http://feeds.12bet.com') 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12betaff286'),
(select top(1) DomainID from Odds_Domain (nolock) where DomainName='http://feeds.12bet.com') 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12BETAFF807'),
(select top(1) DomainID from Odds_Domain (nolock) where DomainName='http://feeds.12bet.com') 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12betaff022'),
(select top(1) DomainID from Odds_Domain (nolock) where DomainName='http://feeds.12bet.com') 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12BETAFF1497'),
(select top(1) DomainID from Odds_Domain (nolock) where DomainName='http://feeds.12bet.com') 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12BETAFF142'),
(select top(1) DomainID from Odds_Domain (nolock) where DomainName='http://feeds.12bet.com') 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12BETAFF1455'),
(select top(1) DomainID from Odds_Domain (nolock) where DomainName='http://feeds.12bet.com') 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12BETAFF1773'),
(select top(1) DomainID from Odds_Domain (nolock) where DomainName='http://feeds.12bet.com') 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12BETAFF177'),
(select top(1) DomainID from Odds_Domain (nolock) where DomainName='http://feeds.12bet.com') 
union all
select (select top(1) PartnerID from Odds_Partner (nolock) where UserName='12BETAFF009'),
(select top(1) DomainID from Odds_Domain (nolock) where DomainName='http://feeds.12bet.com')


Insert into Odds_DomainSetting(PartnerID,DomainID)
select 1,2
union all 
select 1,3
union all 
select 1,4
union all 
select 2,2
union all 
select 2,3
union all 
select 2,4
union all 
select 3,2
union all 
select 3,3
union all 
select 3,4
union all 
select 4,2
union all 
select 4,3
union all 
select 4,4
union all 
select 5,2
union all 
select 5,3
union all 
select 5,4
union all 
select 6,2
union all 
select 6,3
union all 
select 6,4
union all 
select 7,7
union all 
select 8,7
union all 
select 9,7
union all 
select 10,7
union all 
select 11,7
union all 
select 12,7
union all 
select 13,7
union all 
select 14,7
union all 
select 15,7
union all 
select 16,7
union all 
select 17,7
union all 
select 18,7
union all 
select 19,7




