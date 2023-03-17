use master
go
CREATE DATABASE OnlinePharmacy
go
USE OnlinePharmacy

SET DATEFORMAT dmy; 

CREATE TABLE Menu (
    [id] INT IDENTITY(1,1) PRIMARY KEY,
	[parentId] INT default 0,
    [title] NVARCHAR(30),
	[description] Nvarchar(500),
    [url] NVARCHAR(MAX) DEFAULT '#',
	[meta] NVARCHAR(50),
	hide bit default 0,
	[order] int,
	[create_at] datetime,
	[modified_at] datetime,
	[deleted_at] datetime
)
--DBCC CHECKIDENT (Menu, RESEED, 0)


insert into Menu(parentId,title,meta) values 
	(0,N'Thực phẩm chức năng','thuc-pham-chuc-nang'),
	(0,N'Dược mỹ phẩm','duoc-my-pham'),
	(0,N'Thuốc','thuoc'),
	(1,N'Dinh dưỡng','dinh-duong'),
	(1,N'Vitamin, khoáng chất','vitamin-khoang-chat')


create table ProductCategory (
	[id] int IDENTITY(1,1) primary key,
	[parentId] int default 0,
	[name] nvarchar(100),
	[desc] text,
	[create_at] datetime,
	[modified_at] datetime,
	[deleted_at] datetime
)
--DBCC CHECKIDENT (ProductCategory, RESEED, 0)
--delete from ProductCategory
insert into ProductCategory(parentId,name) values 
	(0,N'Thực phẩm chức năng'),
	(0,N'Dược mỹ phẩm'),
	(0,N'Thuốc'),
	(1,N'Dinh dưỡng'),
	(1,N'Vitamin, khoáng chất')

create table Product (
	[id] int IDENTITY(1,1) primary key,
	[categoryId] int,
	[name] nvarchar(max),
	[desc] text,
	[image] varchar(max),
	[price] decimal,
	[unit] nvarchar(20),
	[create_at] datetime,
	[modified_at] datetime,
	[deleted_at] datetime,
	FOREIGN KEY ([categoryId]) REFERENCES ProductCategory ([id])
)
--DBCC CHECKIDENT (Product, RESEED, 0)

insert into Product(categoryId,name,image,price,unit) values
	(4,N'Sữa bột Nestlé Nutren Junior hỗ trợ hệ tiêu hóa giúp trẻ hấp thu dinh dưỡng (850g)','00500184-sua-nutren-junior-nestle-health-science-hop-850g-2256-62a8_large.jpg',525.000,N'hộp'),
	(4,N'Sữa Ensure Gold Vigor Abbott hương vani bổ sung dinh dưỡng, khoáng chất (237ml)','00014844-sua-ensure-gold-vigor-237ml-8990-62bd_large.jpg',53.958,N'chai'),
	(4,N'Nước Yến Sào cho trẻ em 15% Yến hương vani (4 hũ x 72g)','00500698-yen-sao-cho-tre-em-greenbabi-huong-vani-4-hu-x-72g-3370-63eb_large.jpg',131.200,N'hộp'),
	(4,N'Sữa bột Nepro 1 VitaDairy hỗ trợ bồi bổ sức khỏe cho người bệnh thận (400g)','00013459-sua-bot-abbott-glucerna-cho-benh-nhan-tieu-duong-2450-62af-large.jpg',782.000,N'hộp')




create table ProductInventory (
	[id] int primary key,
	[quantity] int,
	[create_at] datetime,
	[modified_at] datetime,
	[deleted_at] datetime,
	FOREIGN KEY ([id]) REFERENCES Product ([id])
)

insert into ProductInventory(id,quantity) values
	(0,2),
	(1,3),
	(2,5),
	(3,0)

create table BlogTag (
	[id] int IDENTITY(1,1) primary key, 
	[name] nvarchar(30),
	[frequency] int default 0
)



insert into BlogTag(name) values
	(N'làm đẹp'),
	(N'mặt nạ'),
	(N'chăm sóc tóc')

create table Blog (
	[id] int IDENTITY(1,1) primary key,
	[title] NVARCHAR(max),
	[author] nvarchar(100),
	[content] nvarchar(max),
	[summary] nvarchar(max),
	[tags] varchar(50),
	[create_at] datetime,
	[modified_at] datetime,
	[deleted_at] datetime,
)

insert into Blog(title,author,content,summary,tags,create_at) values
	(N'8 bí quyết làm đẹp trước khi ngủ giúp bạn trở nên xinh đẹp hơn',N'Hường',N'temp',N'Để sáng ra cơ thể và làn da căng tràn sức sống, bạn cần chăm chút từ tối hôm qua. Vậy bạn đã biết những bí quyết làm đẹp trước khi ngủ chưa? Đêm đến','0,1','27/02/2023'),
	(N'9 thói quen tốt cho da bạn nên làm mỗi ngày nếu muốn trẻ đẹp',N'Hường',N'temp',N'Những thói quen tốt cho da dưới đây được các chuyên gia khuyên áp dụng để có một làm da tươi tắn, khỏe mạnh, đẩy lùi lão hóa. 1. Thường xuyên làm sạch màn hình','0,1','01/03/2023'),
	(N'Ngỡ ngàng với các lợi ích của vitamin E đối với tóc và da của bạn',N'Phương Linh',N'temp',N'Những thói quen tốt cho da dưới đây được các chuyên gia khuyên áp dụng để có một làm da tươi tắn, khỏe mạnh, đẩy lùi lão hóa. 1. Thường xuyên làm sạch màn hình','0,2','01/03/2023')





