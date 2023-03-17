/* SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO"; */
USE master
go
CREATE DATABASE longchau
go
USE longchau;
go

CREATE TABLE roles(
    [roleId] int NOT NULL PRIMARY KEY,
    [roleName] nvarchar(255)
)  ;

INSERT INTO roles VALUES 
(-1,'NOT SET'),
(0,'admin');

CREATE TABLE expressBrand(
    [brandId] int NOT NULL IDENTITY(0,1)  PRIMARY KEY,
    [brandName] nvarchar(255) NOT NULL,
    [nationName] nvarchar(255) DEFAULT NULL
)  ;
INSERT INTO expressBrand VALUES
('',''),
('Grab','VietNam'),
('Gojek','VietNam'),
('Be','VietNam'),
('GHTK','VietNam');

CREATE TABLE users (
    [userId] int NOT NULL identity(1,1) PRIMARY KEY,
    [email] varchar(255) DEFAULT NULL UNIQUE,
    [name] nvarchar(255) NOT NULL,
    [password] varchar(255) NOT NULL,
    [phone] char(11) DEFAULT NULL UNIQUE,
    [birthday] date DEFAULT NULL,
    [address] nvarchar(255) DEFAULT NULL,
    [dateExpire] date DEFAULT NULL, 
    [roleId] int DEFAULT -1,
    FOREIGN KEY ([roleId]) REFERENCES roles ([roleId])
)  ;

INSERT INTO users VALUES ('admin@gmail.com','Admin','e10adc3949ba59abbe56e057f20f883e','0968278202','2002-07-04','VietNam','2222-12-31',0);

CREATE TABLE category(
    [categoryId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [categoryName] nvarchar(255) NOT NULL
)  ;

INSERT INTO category VALUES
(N'Hệ tin mạch & tạo máu'),
(N'Hệ tiêu hóa và gan mật'), 
(N'Hệ thần kinh trung ương'), 
(N'Hệ hô hấp'),
(N'Thuốc kháng sinh'), 
(N'Dị ứng và hệ miễn dịch'),
(N'Thực phẩm chức năng'); 

CREATE TABLE medicine(
    [mdcId] varchar(10) NOT NULL PRIMARY KEY,
    [name] nvarchar(255),
    [type] nvarchar(255),
    [categoryId] int NOT NULL,
    [dateExpire] date NOT NULL,
    [labelerName] nvarchar(255),
    [description] nvarchar(max) DEFAULT NULL,
    [price] int DEFAULT 0,
    [quantity] int DEFAULT 0,
    [img] varbinary(MAX) NOT NULL,
    FOREIGN KEY ([categoryId]) REFERENCES category ([categoryId])
)  ;

CREATE TABLE import(
    [importId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [requestDate] datetime DEFAULT GETDATE(),
    [mdcId] varchar(10)  NOT NULL,
    [quantity] int DEFAULT 0 NOT NULL,
    [dateExpire] date NOT NULL DEFAULT '2022-01-01',
    [status] int DEFAULT 0 NOT NULL,
    FOREIGN KEY ([mdcId]) REFERENCES medicine ([mdcId]) ON DELETE CASCADE
)  ;

CREATE TABLE storage(
    [storageId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [importId] int NOT NULL,
    [quantity] int DEFAULT 10 NOT NULL,
    [dateExpire] date DEFAULT '2025-12-31',
    [status] int DEFAULT 0 NOT NULL,
    FOREIGN KEY ([importId]) REFERENCES import ([importId]) ON DELETE CASCADE
)  ;

CREATE TABLE transactions(
    [transId] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[userId] INT NOT NULL,
    [transDate] datetime NOT NULL,
    [totalPrice] int DEFAULT 0,
	[brandId] int DEFAULT 0,
	[expressState] int DEFAULT 0,  
    [state] int default 0 NOT NULL,
    FOREIGN KEY ([brandId]) REFERENCES expressBrand ([brandId]),
    FOREIGN KEY ([userId]) REFERENCES users ([userId])
)  ;

CREATE TABLE transactionDetail(
    [transId] INT NOT NULL,
    [mdcID] varchar(10)  NOT NULL,
    [quantity] int default 0 NOT NULL,
	[totalPrice] int default 0 NOT NULL,
    FOREIGN KEY ([mdcId]) REFERENCES medicine ([mdcId]) ON DELETE CASCADE,
    FOREIGN KEY ([transId]) REFERENCES transactions ([transId]) ON DELETE CASCADE
)  ;

CREATE TABLE OTP(
	[otpId] int NOT NULL IDENTITY(1,1)  PRIMARY KEY,
    [email] varchar(255) NOT NULL,
	[otpCode] varchar(25) NOT NULL,
	[date] DATETIME DEFAULT GETDATE(),
    FOREIGN KEY ([email]) REFERENCES users ([email])
)

IF EXISTS (SELECT * FROM SYS.OBJECTS WHERE NAME = 'updateToalPriceTransDetail')
	DROP TRIGGER updateToalPriceTransDetail
GO

CREATE TRIGGER updateToalPriceTransDetail
ON transactionDetail
AFTER INSERT
AS
BEGIN
    UPDATE transactionDetail
    SET transactionDetail.totalPrice = transactionDetail.quantity * (SELECT price FROM medicine WHERE mdcId = transactionDetail.mdcId)
    FROM inserted JOIN transactionDetail ON inserted.transId = transactionDetail.transId
END
GO

INSERT INTO medicine VALUES
('MDC00001','Telmisartan',N'Hộp 3 vỉ x 10 viên', 1, '2022-12-31', N'AN THIÊN',N'Thuốc Telmisartan được chỉ định trong điều trị tăng huyết áp ở người lớn. Phòng ngừa bệnh tim mạch: Giảm nguy cơ mắc bệnh tim mạch ở người lớn: Bệnh nhân có bệnh lý tim mạch có nguy cơ huyết khối động mạch như tiền sử bệnh mạch vành, đột quỵ, bệnh động mạch ngoại biên. Bệnh nhân đái tháo đường type 2 có tổn thương cơ quan đích.', 100000,0,(SELECT * FROM OPENROWSET(BULK N'C:\img\MDC00001.png', SINGLE_BLOB) as T1)),
('MDC00002','Clopidogrel',N'Hộp 3 vỉ x 10 viên', 1, '2022-10-31', N'USP',N'Thuốc Clopidogrel 75 Mv được chỉ định dùng trong các trường hợp sau:Làm giảm hay dự phòng các biến cố huyết khối do xơ vữa động mạch (nhồi máu cơ tim, đột quỵ, tai biến mạch máu não) ở bệnh nhân có tiền sử xơ vữa động mạch biểu hiện bởi nhồi máu cơ tim (trong thời gian vài ngày đến dưới 35 ngày), đột quỵ, thiếu máu cục bộ (từ 7 ngày đến dưới 6 tháng) hay bệnh động mạch ngoại biên đã được xác định.Dùng kết hợp với Aspirin ở bệnh nhân bị hội chứng đau thắt ngực không ổn định hay nhồi máu cơ tim không có sóng Q.', 100000,0,(SELECT * FROM OPENROWSET(BULK N'C:\img\MDC00002.png', SINGLE_BLOB) as T1)),
('MDC00003','Rotacor',N'Hộp 3 vỉ x 10 viên', 1, '2022-12-31', N'LEK',N'Atorvastatin được chỉ định hỗ trợ cho chế độ ăn kiêng trong điều trị cho các bệnh nhân bị tăng cholesterol toàn phần (C-toàn phần), cholesterol lipoprotein tỉ trọng thấp (LDL-C), apolipoprotein B (apo B) và triglycerid (TG) và giúp làm tăng cholesterol lipoprotein tỉ trọng cao (HDL-C) ở các bệnh nhân tăng cholesterol máu tiên phát (tăng cholesterol máu có tính gia đình dị hợp tử và không có tính gia đình), tăng lipid máu phối hợp (hỗn hợp) (nhóm lla và llb theo phân loại của Fredrickson), tăng triglycerid máu (nhóm IV, theo phân loại của Fredrickson) và ở các bệnh nhân có rối loạn betalipoprotein máu (nhóm III theo phân loại Fredrickson) mà không có đáp ứng đầy đủ với chế độ ăn.Atorvastatin cũng được chỉ định để làm giảm C- toàn phần và LDL-C ở các bệnh nhân có tăng cholesterol máu', 100000,0,(SELECT * FROM OPENROWSET(BULK N'C:\img\MDC00003.png', SINGLE_BLOB) as T1)), 
('MDC00004',N'Bổ Gan Trường Phúc', N'Hộp 3 vỉ x 10 viên', 2, '2022-12-31', N' CÔNG TY TNHH DƯỢC THẢO HOÀNG THÀNH',N' Bổ Gan Trường Phúc chỉ định điều trị trong các trường hợp sau:Hỗ trợ bổ gan, giải độc, kiện tỳ, tăng cường khí huyết.Giải độc gan, chống dị ứng, mề đay, lở ngửa, rôm sảy, mụn nhọt.Hỗ trợ điều trị trong bệnh viêm gan với các triệu chứng mệt mỏi, vàng da, chán ăn khó tiêu, táo bón, đau tức vùng gan, suy giảm chức năng gan do dùng nhiều bia rượu, thuốc hóa dược.', 100000,0,(SELECT * FROM OPENROWSET(BULK N'C:\img\MDC00004.png', SINGLE_BLOB) as T1)),
('MDC00005',N'Bài Thạch Trường Phúc', N'Hộp 3 vỉ x 10 viên', 2, '2022-12-31', N'CÔNG TY TNHH DƯỢC THẢO HOÀNG THÀNH',N'Bài Thạch Trường Phúc chỉ định điều trị trong các trường hợp sau:Phòng và hỗ trợ điều trị sỏi thận, sỏi đường tiết niệu, sỏi bàng quang, sỏi mật, tiểu buốt, tiểu rắt, tiểu ra máu.', 100000,0,(SELECT * FROM OPENROWSET(BULK N'C:\img\MDC00005.png', SINGLE_BLOB) as T1)),
('MDC00006',N'Omeprazol', N'Hộp 3 vỉ x 10 viên', 2, '2022-12-31', N'TVP',N'Omeprazol là một benzimidazol đã gắn các nhóm thế, có cấu trúc và tác dụng tương tự như pantoprazol, lansoprazol, esomeprazol.Omeprazol ức chế sự bài tiết acid của dạ dày do ức chế hệ enzym hydro/kali adenosin triphosphatase (H+/K+ ATPase) còn gọi là bơm proton ở tế bào thành của dạ dày. Omeprazol không có tác dụng lên thụ thể (receptor) acetylcholin hay thụ thể histamin. Uống hàng ngày một liều duy nhất 20 mg omeprazol tạo được sự ức chế tiết acid dạ dày mạnh và hiệu quả. Tác dụng tối đa đạt được sau 4 ngày điều trị. Ở bệnh nhân loét dạ dày, có thể duy trì việc giảm 80% acid dịch vị trong 24 giờ.Omeprazol có thể kìm hãm được vi khuẩn Helicobacter pylori ở người bệnh loét tá tràng và/hoặc viêm thực quản trào ngược bị nhiễm vi khuẩn này. Phối hợp omeprazol với một số thuốc kháng khuẩn (ví dụ: Clarithromycin, amoxicilin) có thể tiệt trừ H.pylori kèm theo liền ổ loét và thuyên giảm bệnh lâu dài.', 100000,0,(SELECT * FROM OPENROWSET(BULK N'C:\img\MDC00006.png', SINGLE_BLOB) as T1)),
('MDC00007',N'Celecoxib', N'Hộp 3 vỉ x 10 viên', 3, '2022-09-30', N'USP',N'Thuốc Celecoxib 200-Hv được chỉ định dùng trong các trường hợp sau:Điều trị triệu chứng của thoái hóa khớp (OA) và viêm khớp dạng thấp (RA).Giảm nhẹ các dấu hiệu và triệu chứng của viêm cột sống dính khớp.Kiểm soát đau cấp tính. Điều trị thống kinh nguyên phát.', 100000,0,(SELECT * FROM OPENROWSET(BULK N'C:\img\MDC00007.png', SINGLE_BLOB) as T1)),
('MDC00008',N'Bivinadol', N'Hộp 4 vỉ x 4 viên', 3, '2022-12-31', N'BRV',N'Viên sủi Bivinadol chỉ định điều trị trong các trường hợp sau:Điều trị các chứng đau cấp tính và mạn tính như: Đau đầu, đau răng, đau bụng kinh, đau thần kinh, đau khớp và đau cơ.Hạ sốt ở bệnh nhân bị cảm hay những bệnh có liên quan tới sốt.', 100000,0,(SELECT * FROM OPENROWSET(BULK N'C:\img\MDC00008.png', SINGLE_BLOB) as T1)),
('MDC00009',N'Actadol', N'Hộp 10 vỉ x 10 viên', 3, '2022-07-31', N'MEDIPHARCO',N'Paracetamol được dùng rộng rãi trong điều trị các chứng đau và sốt từ nhẹ đến vừa. Đau Paracetamol được dùng giảm đau tạm thời trong điều trị chứng đau nhẹ và vừa: Đau đầu, đau răng, đau bụng kinh, đau cơ... Thuốc có hiệu quả nhất là làm giảm đau cường độ thấp có nguồn gốc không phải nội tạng.Paracetamol không có tác dụng trị thấp khớp.Paracetamol là thuốc thay thế salicylat (được ưa thích ở người bệnh chống chỉ định hoặc không dung nạp salicylat) để giảm đau nhẹ hoặc hạ sốt.Sốt Paracetamol thường được dùng để giảm thân nhiệt ở người bệnh sốt, khi sốt có thể có hại hoặc khi hạ sốt, người bệnh sẽ dễ chịu hơn. Tuy vậy, liệu pháp hạ sốt nói chung không đặc hiệu, không ảnh hưởng đến tiến trình của bệnh cơ bản, và có thể che lấp tình trạng bệnh của người bệnh.', 100000,0,(SELECT * FROM OPENROWSET(BULK N'C:\img\MDC00009.png', SINGLE_BLOB) as T1)),
('MDC00010',N'Midorhum OPV', N'Hộp 10 vỉ x 10 viên', 4, '2022-12-31', N'CÔNG TY CỔ PHẦN DƯỢC PHẨM OPV',N'Thuốc Midorhum được chỉ định dùng trong các trường hợp sau:Ðiều trị các triệu chứng trong cảm lạnh và cảm cúm như đau nhức nhẹ, nhức đầu, sốt, ho, sổ mũi, hắt hơi, mẩn ngứa, chảy nước mắt.', 100000,0,(SELECT * FROM OPENROWSET(BULK N'C:\img\MDC00010.png', SINGLE_BLOB) as T1)),
('MDC00011',N'Neo-Corclion F TV.pharm', N'Hộp 2 vỉ x 10 viên', 4, '2022-12-31', N'CÔNG TY CỔ PHẦN DƯỢC PHẨM TV.PHARM',N'Thuốc Neo - Corclion F được chỉ định dùng trong các trường hợp sau:Điều trị giảm ho trong các trường hợp ho gió, ho khan.', 100000,0,(SELECT * FROM OPENROWSET(BULK N'C:\img\MDC00011.png', SINGLE_BLOB) as T1)),
('MDC00012',N'Cufo Lemon', N'Hộp 2 vỉ x 12 viên', 4, '2022-05-31', N'UNIQUE',N'Viên Ngậm Cufo Lemon có tác dụng hỗ trợ điều trị triệu chứng trong nhiễm khuẩn miệng và họng.', 100000,0,(SELECT * FROM OPENROWSET(BULK N'C:\img\MDC00012.png', SINGLE_BLOB) as T1)),
('MDC00013',N'Opelomin', N'Hộp 2 vỉ x 2 viên', 5, '2022-12-31', N'OPV',N'Thuốc Opelomin 6 được chỉ định dùng trong các trường hợp sau:Ðiều trị giun chỉ (Onchocerca volvulus).Ðiều trị giun lươn (Strongyloides stercoralis).', 100000,0,(SELECT * FROM OPENROWSET(BULK N'C:\img\MDC00013.png', SINGLE_BLOB) as T1)),
('MDC00014',N'Klavunamox', N'Hộp 3 vỉ x 15 viên', 5, '2022-12-31', N'ATABAY',N'Thuốc Klavunamox 625 mg được chỉ định dùng điều trị nhiễm khuẩn gây nên bởi các chủng nhạy cảm trong các trường hợp cụ thể sau đây:Nhiễm khuẩn bộ máy hô hấp:Nhiễm khuẩn đường hô hấp trên: Viêm xoang, viêm amidan, viêm tai giữa, những nhiễm khuẩn khác ở vùng tai-mũi-họng.Nhiễm khuẩn đường hô hấp dưới: Viêm phế quản cấp và mạn tính, viêm phổi, viêm mủ màng phổi, abces phổi.Nhiễm khuẩn da và mô mềm: Đinh, nhọt, abces, viêm mô tế bào, nhiễm khuẩn vết thương, nhiễm khuẩn trong bụng.Nhiễm khuẩn đường tiết niệu - sinh dục: Viêm bàng quang, viêm thận - bể thận, viêm niệu đạo, nhiễm khuẩn vùng khung chậu, giang mai, lậu.Các nhiễm khuẩn khác: Viêm xương tủy.', 100000,0,(SELECT * FROM OPENROWSET(BULK N'C:\img\MDC00014.png', SINGLE_BLOB) as T1)),
('MDC00015',N'Auclanityl Tipharco', N'Hộp 2 vỉ x 7 viên', 5, '2022-12-31', N'TIPHARCO',N'Điều trị các nhiễm khuẩn do các vi khuẩn nhạy cảm trong các trường hợp:Nhiễm khuẩn nặng đường hô hấp trên: Viêm amidan, viêm xoang, viêm tai giữa đã được điều trị bằng các kháng sinh thông thường nhưng không giảm.Nhiễm khuẩn đường hô hấp dưới bởi các chủng H.influenzae và Branhamella catarrbalis sản sinh beta – lactamase: Viêm phế quản cấp và đợt cấp của viêm phế quản mạn, viêm phổi mắc phải ở cộng đồng.Nhiễm khuẩn nặng đường tiết niệu bởi các chủng E. coli, Klebsiella và Enterobacter sản sinh: Viêm bàng quang, viêm niệu đạo, viêm bể thận.Nhiễm khuẩn da và mô mềm: Mụn nhọt, áp xe, nhiễm khuẩn vết thương.Nhiễm khuẩn xương và khớp: Viêm tủy xương.', 100000,0,(SELECT * FROM OPENROWSET(BULK N'C:\img\MDC00015.png', SINGLE_BLOB) as T1)),
('MDC00016',N'Histalong', N'Hộp 2 vỉ x 10 viên', 6, '2022-04-30', N'DR.REDDY',N'Thuốc Histalong L được chỉ định dùng trong trường hợp sau:Ðiều trị triệu chứng viêm mũi dị ứng (kể cả viêm mũi dị ứng dai dẳng) và mày đay ở người lớn và trẻ em từ 6 tuổi trở lên.', 100000,0,(SELECT * FROM OPENROWSET(BULK N'C:\img\MDC00016.png', SINGLE_BLOB) as T1)),
('MDC00017',N'Fefasdin', N'Hộp 10 vỉ x 10 viên', 6, '2022-12-31', N'KHAPHARCO PHARM. CO.',N'Thuốc FEFASDIN 60 được chỉ định dùng trong các trường hợp sau:Điều trị triệu chứng trong viêm mũi dị ứng theo mùa, mày đay mạn tính vô căn ở người lớn và trẻ em trên 6 tuổi.', 100000,0,(SELECT * FROM OPENROWSET(BULK N'C:\img\MDC00017.png', SINGLE_BLOB) as T1)),
('MDC00018',N'Telfast HD', N'Hộp 1 vỉ x 10 viên', 6, '2022-12-31', N'SANOFI',N'Thuốc Telfast 180mg được chỉ định dùng trong các trường hợp sau:Ðiều trị viêm mũi dị ứng: Telfast HD 180 mg được chỉ định để điều trị viêm mũi dị ứng theo mùa ở người lớn và trẻ em từ 12 tuổi trở lên.Ðiều trị mày đay vô căn mạn tính: Telfast HD 180 mg được chỉ định để điều trị các biểu hiện ngoài da không biến chứng của mày đay vô căn mạn tính ở người lớn và trẻ em từ 12 tuổi trở lên. Thuốc làm giảm ngứa và số lượng dát mày đay một cách đáng kể.', 100000,0,(SELECT * FROM OPENROWSET(BULK N'C:\img\MDC00018.png', SINGLE_BLOB) as T1)),
('MDC00019',N'S Prenatal', N'Hộp 3 vỉ x 10 viên', 7, '2022-12-31', N'Gmp',N'An tâm cho mẹ, khỏe mạnh cho bé cùng S-Prenatal - viên uống bổ sung vitamin và khoáng chất thiết yếu dành cho phụ nữ trước và trong giai đoạn mang thai, mẹ đang cho con bú, giúp cải thiện các triệu chứng khó chịu trong giai đoạn mang thai, bồi bổ sức khỏe cho cả mẹ và bé.', 100000,0,(SELECT * FROM OPENROWSET(BULK N'C:\img\MDC00019.png', SINGLE_BLOB) as T1)),
('MDC00020',N'An Não Vương', N'Hộp 3 vỉ x 10 viên', 7, '2021-12-31', N' USP',N' Thực phẩm chức năng an não vương bổ huyết, hoạt huyết tăng cường tuần hoàn não, hỗ trợ điều trị thiếu máu não', 80000,20,(SELECT * FROM OPENROWSET(BULK N'C:\img\MDC00020.png', SINGLE_BLOB) as T1)),
('MDC00021',N'Forest Gel', N'Hộp 20 gói', 7, '2021-12-31', N' NAPHARCO',N'Thực phẩm chức năng bảo vệ dạ dày Forest Gel giúp giảm các tác nhân gây tổn thương dạ dày, tá tràng như thừa acid dịch vị. Hỗ trợ làm giảm các triệu chứng của đau dạ dày, tá tràng: đau thượng vị, ợ hơi, ợ chua. Giúp nhanh liền vết loét, ngăn ngừa ung thư dạ dày, đại tràng', 100000,0,(SELECT * FROM OPENROWSET(BULK N'C:\img\MDC00021.png', SINGLE_BLOB) as T1));
GO