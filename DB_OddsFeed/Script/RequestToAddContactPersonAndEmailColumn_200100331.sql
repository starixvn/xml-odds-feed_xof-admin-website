  --Add two new columns contactEmail, contact 
   

Alter table Odds_Partner
drop column ContactPerson,ContactEmail
Alter table Odds_Partner
add ContactPerson nvarchar(50), ContactEmail nvarchar(50)

go
update Odds_partner with (rowlock,updlock)
set ContactPerson='',
    ContactEmail=''
where username= 'Asianodds'

update Odds_partner with (rowlock,updlock)
set ContactPerson='',
    ContactEmail=''
where username= 'betbrain' and SystemId=1


update Odds_partner with (rowlock,updlock)
set ContactPerson='Matt Stephenson',
    ContactEmail='matt.stephenson@betgenius.com'
where username= 'BetGenius'

update Odds_partner with (rowlock,updlock)
set ContactPerson='J.hoysater / Petersen',
    ContactEmail='j.hoysater@betradar.com; m.petersen@betradar.com'
where username= 'BetRadar'

update Odds_partner with (rowlock,updlock)
set ContactPerson='',
    ContactEmail=''
where username= 'Score22'

update Odds_partner with (rowlock,updlock)
set ContactPerson='Einar / Kari',
    ContactEmail='mpl_m@yahoo.com;oc@tip-ex.com'
where username= 'TipEx'

update Odds_partner with (rowlock,updlock)
set ContactPerson='',
    ContactEmail=''
where username= 'betbrain' and SystemId=2

update Odds_partner with (rowlock,updlock)
set ContactPerson='',
    ContactEmail=''
where username= 'Flybet'

update Odds_partner with (rowlock,updlock)
set ContactPerson=N'雷 项',
    ContactEmail='lubsbryanlee@gmail.com'
where username= '12BETAFF009'

update Odds_partner with (rowlock,updlock)
set ContactPerson='zhu wenjie',
    ContactEmail='steve.zhu@gooooal.com'
where username= '12betaff022'

update Odds_partner with (rowlock,updlock)
set ContactPerson='Daniel Kirk',
    ContactEmail='editor@sportspunter.com'
where username= '12betaff062'

update Odds_partner with (rowlock,updlock)
set ContactPerson=N'志伟 张',
    ContactEmail='zzw520z_cn@163.com'
where username= '12BETAFF142'

update Odds_partner with (rowlock,updlock)
set ContactPerson='Jens Hansen',
    ContactEmail='admin@gamblers-paradise.net'
where username= '12BETAFF1455'

update Odds_partner with (rowlock,updlock)
set ContactPerson='Paul King',
    ContactEmail='paul@theoddschecker.co.uk'
where username= '12BETAFF1497'

update Odds_partner with (rowlock,updlock)
set ContactPerson=N'斌 张',
    ContactEmail='133228893@qq.com'
where username= '12BETAFF177'

update Odds_partner with (rowlock,updlock)
set ContactPerson='Pantelitsa Efstratiou',
    ContactEmail='affiliates@oddsfinder.com'
where username= '12BETAFF1773'

update Odds_partner with (rowlock,updlock)
set ContactPerson='Patrik Dzurenda',
    ContactEmail='affiliate-op@livesport.cz'
where username= '12betaff286'

update Odds_partner with (rowlock,updlock)
set ContactPerson='Denis Avdeev',
    ContactEmail='service@in4bet.com'
where username= '12BETAFF807'

select * from Odds_Partner