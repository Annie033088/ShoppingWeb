CREATE TABLE t_users(
	f_uid int IDENTITY PRIMARY KEY,
	f_account	varchar(20)	NOT NULL,
	f_hash 	varchar(100) NOT NULL,
	f_name	varchar(20)	NULL,
	f_status	bit NOT NULL DEFAULT 1,
	f_roleId	tinyint NOT NULL,
);