CREATE TABLE t_order(
	f_orderId int IDENTITY PRIMARY KEY,
	f_memberId INT NOT NULL,
	f_orderNumber char(18)	NOT NULL,
	f_totalPrice decimal(10,2) NOT NULL,
	f_state	tinyint NOT NULL DEFAULT 1,
	f_recipientName nvarchar(50) NOT NULL,
	f_phone	char(10) NOT NULL,
	f_address nvarchar(326) NULL,
	f_shippingMethod tinyint NOT NULL,
	f_remark nvarchar(50) NOT NULL,
	f_createTime DATETIME DEFAULT GETDATE(),
	f_updateTime DATETIME DEFAULT GETDATE()
);

CREATE TABLE t_orderState(
	f_orderStateId int IDENTITY PRIMARY KEY,
	f_orderId INT NOT NULL,
	f_state	tinyint NOT NULL DEFAULT 1,
	f_remark nvarchar(50) NOT NULL,
	f_createTime DATETIME DEFAULT GETDATE(),
	f_updateTime DATETIME DEFAULT GETDATE()
);