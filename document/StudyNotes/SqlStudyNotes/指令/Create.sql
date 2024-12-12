CREATE TABLE t_members(
	f_memberId int IDENTITY PRIMARY KEY,
	f_memberAccount	varchar(20)	NOT NULL,
	f_hash 	varchar(101) NOT NULL,
	f_email	varchar(255) NOT NULL,
	f_phone	char(10) NULL,
	f_memberName nvarchar(50) NOT NULL,
	f_nickName nvarchar(25) NULL,
	f_status bit NOT NULL DEFAULT 1,
	f_points int NOT NULL DEFAULT 0,
	f_level tinyint NOT NULL DEFAULT 1
);