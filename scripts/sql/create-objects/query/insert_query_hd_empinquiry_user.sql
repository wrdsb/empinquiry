SELECT TOP (1000) [employee_id]
,[userid]
,[admin]
FROM [HDHRP].[dbo].[hd_empinquiry_user]

/* Authorized person records */

insert into [hd_empinquiry_user] values('25342','DukeR',1) -- Robert Duke
insert into [hd_empinquiry_user] values('37574','RAJUV',1) -- Vanitha
insert into [hd_empinquiry_user] values('41405','MENONP',1)   -- Priya
insert into [hd_empinquiry_user] values('36333','TOMCHIM',1) -- Michelle
insert into [hd_empinquiry_user] values('48785','durairm',1) -- Meenakshi
insert into [hd_empinquiry_user] values('26077','SANDERJ', 1) -- Jeff Sanderson
insert into [hd_empinquiry_user] values('27675','marshad', 1) -- Dawn Marshall
/* Priya's Team Members Records */
insert into [hd_empinquiry_user] values('33313','BELISLE', 1) -- Eileen Belisle
insert into [hd_empinquiry_user] values('31299','PEGGMA', 1) -- Mary Ann Pegg
insert into [hd_empinquiry_user] values('31663','RAYMONC', 1) -- Christopher Raymond
insert into [hd_empinquiry_user] values('46367','PARVEZU', 1) -- Uzma Parveez
insert into [hd_empinquiry_user] values('48305','NAKSHAN', 1) -- Nakshatra Nakshatra
insert into [hd_empinquiry_user] values('28814','WolfeB', 1) --Luanne Wolfe

/* Gave access to employee as per Robert's request */
insert into [hd_empinquiry_user] values('44473','HIRMIZS', 1) --Hirmiz, Sarmd

/*T2512-01543 - Grant access to new employee inquiry system for specified users*/
insert into [hd_empinquiry_user] values('27674','KobetiR',1) -- Kobetic, Robert
insert into [hd_empinquiry_user] values('31818','MCGUIRC',1) -- McGuire, Chris
insert into [hd_empinquiry_user] values('31536','DETTWIT',1) -- Dettwiler, Troy
insert into [hd_empinquiry_user] values('46573','PATELPA',1) -- Patel, Parth
insert into [hd_empinquiry_user] values('46420','KIMPTOA',1) -- Kimpton, Alexander
insert into [hd_empinquiry_user] values('14426','SandhaE',1) -- Sandham, Elaine











  
  