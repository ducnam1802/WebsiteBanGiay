use master
if exists (select*from sysdatabases where name = 'QlyGiay')
drop database QlyGiay
go 
create database QlyGiay
go
use QlyGiay
go
CREATE TABLE KichCo
(
  MaKichCo INT NOT NULL IDENTITY (1,1),
  KichCo FLOAT NOT NULL,
  PRIMARY KEY (MaKichCo)
);

CREATE TABLE MauSac
(
  MaMauSac INT NOT NULL IDENTITY (1,1),
  TenMauSac nchar(10) NOT NULL,
  PRIMARY KEY (MaMauSac)
);

CREATE TABLE ThuongHieu
(
  MaThuongHieu INT NOT NULL IDENTITY (1,1),
  TenThuongHieu nvarchar(50)NOT NULL ,
  PRIMARY KEY (MaThuongHieu)
);

CREATE TABLE NguoiDung
(
  MaNguoiDung INT NOT NULL IDENTITY (1,1) ,
  TenNguoiDung nvarchar(50) NOT NULL,
  TenDangNhap nvarchar(50) NOT NULL,
  MatKhau nvarchar(50) NOT NULL,
  Email nvarchar(50) NOT NULL,
  SDT nchar(10) NOT NULL,
  DiaChi nvarchar(50) NOT NULL,
  PRIMARY KEY (MaNguoiDung)
);

Create table NhanVien
(
    MaAd INT NOT NULL IDENTITY,
    TaiKhoan nvarchar(100) NOT NULL ,
    MatKhau nvarchar(100) NOT NULL,
	TenAd nvarchar(50) NOT NULL,
    primary key (MaAD)
);

CREATE TABLE DonHang
(
  MaDonHang INT NOT NULL IDENTITY (1,1),
  DiaChi nvarchar(200) NOT NULL,
  SDT nchar(10) NOT NULL,
  TinhTrang nvarchar(50) NOT NULL,
  TongTien NUMERIC(18) NOT NULL,
  NgayXuatDon DATE NOT NULL,
  NgayDuKien DATE NOT NULL,
  TenNguoiNhan nvarchar(50) NOT NULL,
  MaNguoiDung INT NOT NULL,
  DaNhanHang BIT NOT NULL DEFAULT 0,
  PRIMARY KEY (MaDonHang),
  FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung)
);

CREATE TABLE SanPham
(
  MaSP INT NOT NULL IDENTITY (1,1),
  TenSP nvarchar(20) NOT NULL,
  GiaSP numeric(18,0) NOT NULL,
  SoLuong INT NOT NULL,
  MoTA nvarchar(150) NOT NULL,
  MaKichCo INT,
  MaThuongHieu INT,
  MaMauSac INT,
  PRIMARY KEY (MaSP),
  FOREIGN KEY (MaKichCo) REFERENCES KichCo(MaKichCo),
  FOREIGN KEY (MaThuongHieu) REFERENCES ThuongHieu(MaThuongHieu),
  FOREIGN KEY (MaMauSac) REFERENCES MauSac(MaMauSac)
);

CREATE TABLE SanPham_Anh
(
  MaAnh INT NOT NULL IDENTITY (1,1),
  Anh nvarchar(max) NOT NULL,
  MaSP INT NOT NULL,
  PRIMARY KEY (MaAnh),
  FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);

CREATE TABLE ChiTietDonHang
(
  MaChiTietDonHang INT NOT NULL IDENTITY (1,1),
  Gia NUMERIC(18) NOT NULL,
  SoLuong INT NOT NULL,
  MaDonHang INT NOT NULL,
  MaSP INT NOT NULL,
  PRIMARY KEY (MaChiTietDonHang),
  FOREIGN KEY (MaDonHang) REFERENCES DonHang(MaDonHang),
  FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP)
);


--insert dữ liệu Kích cỡ 
insert into KichCo values (39)
insert into KichCo values (40)
insert into KichCo values (41)
insert into KichCo values (42)
insert into KichCo values (43)
insert into KichCo values (44)
insert into KichCo values (45)
--insert dữ liệu Thương hiệu
insert into ThuongHieu values (N'Nike')
insert into ThuongHieu values (N'Adidas')
insert into ThuongHieu values (N'Convert')
--insert dữ liệu Màu sắc
insert into MauSac values (N'Đen')
insert into MauSac values (N'Trắng')
insert into MauSac values (N'Đỏ')
insert into MauSac values (N'Vàng')
insert into MauSac values (N'Xanh lam')
insert into MauSac values (N'Xanh dương')
--insert dữ liệu Sản phẩm
insert into SanPham values (N'Giày Nike AirForce 1', 5900000, 40, N'Giày sang xịn mịn', 1,1,3)
insert into SanPham values (N'Giày Adidas', 10000000, 40, N'Giày đẹp', 2,2,2)
insert into SanPham values (N'Giày Convert', 1500000, 40, N'Giày sang xịn mịn', 3,3,1)
insert into SanPham values (N'YEEZY BOOST 350 V2', 12000000, 40, N'Giày sang xịn mịn', 4,1,3)
insert into SanPham values (N'DUNK HIGH AMBUSH', 16000000, 40, N'Giày sang xịn mịn', 5,1,6)
insert into SanPham values (N'AIR ZOOM ALPHAFLY', 6800000, 40, N'Giày sang xịn mịn', 3,1,5)
insert into SanPham values (N'JORDAN 1 MID TUFT', 5800000, 40, N'Giày sang xịn mịn', 4,1,3)
insert into SanPham values (N'AIR JORDAN 1 CHICAGO', 60000000, 40, N'Giày sang xịn mịn', 5,1,3)
insert into SanPham values (N'NMD_R1 SPECTOO', 2900000, 40, N'Giày sang xịn mịn', 7,2,2)
insert into SanPham values (N'NMD_R1 SPECTOO', 2800000, 40, N'Giày sang xịn mịn', 6,3,2)

--insert hình ảnh
insert into SanPham_Anh values ('/Content/images/sp1.png',1)
insert into SanPham_Anh values ('/Content/images/sp2.png',2)
insert into SanPham_Anh values ('/Content/images/sp3.png',3)
insert into SanPham_Anh values ('/Content/images/sp4.png',4)
insert into SanPham_Anh values ('/Content/images/sp5.png',5)
insert into SanPham_Anh values ('/Content/images/sp6.png',6)
insert into SanPham_Anh values ('/Content/images/sp7.png',7)
insert into SanPham_Anh values ('/Content/images/sp8.png',8)
insert into SanPham_Anh values ('/Content/images/sp9.png',9)
insert into SanPham_Anh values ('/Content/images/sp10.png',10)

--insert admin
insert into NhanVien values (N'admin',N'admin123',N'Vũ Đức Nam')

--insert Khách hàng
insert into NguoiDung values (N'Vũ Đức Nam',N'ducnam',N'123456',N'vuducnam@gmail.com',N'0367590203',N'LandMark 81')
