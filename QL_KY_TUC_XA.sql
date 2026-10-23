USE [master]
GO
IF DB_ID(N'QL_KY_TUC_XA') IS NOT NULL
BEGIN
    ALTER DATABASE [QL_KY_TUC_XA] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [QL_KY_TUC_XA];
END
GO
CREATE DATABASE [QL_KY_TUC_XA];
GO

USE [QL_KY_TUC_XA]
GO

/****** Object:  Table [dbo].[LoaiPhong]    Script Date: 23/10/2026 10:06:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoaiPhong](
	[MaLoaiPhong] [int] IDENTITY(1,1) NOT NULL,
	[TenLoaiPhong] [nvarchar](100) NOT NULL,
	[SoNguoiToiDa] [int] NOT NULL,
	[DonGiaThang] [decimal](18, 0) NOT NULL,
	[MoTa] [nvarchar](250) NULL,
	[TrangThai] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_LoaiPhong] PRIMARY KEY CLUSTERED 
(
	[MaLoaiPhong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[PhongKTX]    Script Date: 23/10/2026 10:06:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhongKTX](
	[MaPhong] [int] IDENTITY(1,1) NOT NULL,
	[TenPhong] [nvarchar](50) NOT NULL,
	[MaLoaiPhong] [int] NOT NULL,
	[ToaNha] [nvarchar](50) NOT NULL,
	[Tang] [int] NOT NULL,
	[GioiTinhPhong] [nvarchar](10) NOT NULL,
	[TrangThai] [nvarchar](20) NOT NULL,
	[MoTa] [nvarchar](250) NULL,
 CONSTRAINT [PK_PhongKTX] PRIMARY KEY CLUSTERED 
(
	[MaPhong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[PhanPhong]    Script Date: 23/10/2026 10:06:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhanPhong](
	[MaPhanPhong] [int] IDENTITY(1,1) NOT NULL,
	[MaDangKy] [int] NOT NULL,
	[MaPhong] [int] NOT NULL,
	[NgayVao] [date] NOT NULL,
	[NgayRa] [date] NULL,
	[TrangThai] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_PhanPhong] PRIMARY KEY CLUSTERED 
(
	[MaPhanPhong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  View [dbo].[vw_PhongSoCho]    Script Date: 23/10/2026 10:06:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* ----- 3. View số chỗ (tính từ dữ liệu thực tế, không lưu sẵn) ----- */
CREATE VIEW [dbo].[vw_PhongSoCho] AS
SELECT p.MaPhong, p.TenPhong, p.ToaNha, p.Tang, p.GioiTinhPhong, p.TrangThai,
       lp.MaLoaiPhong, lp.TenLoaiPhong, lp.SoNguoiToiDa,
       ISNULL(x.SoNguoiDangO, 0) AS SoNguoiDangO,
       lp.SoNguoiToiDa - ISNULL(x.SoNguoiDangO, 0) AS SoChoCon
FROM dbo.PhongKTX p
JOIN dbo.LoaiPhong lp ON lp.MaLoaiPhong = p.MaLoaiPhong
OUTER APPLY (SELECT COUNT(*) AS SoNguoiDangO FROM dbo.PhanPhong pp
             WHERE pp.MaPhong = p.MaPhong AND pp.TrangThai = N'DangO') x;
GO

/****** Object:  Table [dbo].[DangKyKTX]    Script Date: 23/10/2026 10:06:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DangKyKTX](
	[MaDangKy] [int] IDENTITY(1,1) NOT NULL,
	[MaSinhVien] [nvarchar](20) NOT NULL,
	[NgayDangKy] [date] NOT NULL,
	[NgayBatDau] [date] NOT NULL,
	[NgayKetThucDuKien] [date] NOT NULL,
	[NguyenVongLoaiPhong] [int] NULL,
	[GhiChu] [nvarchar](300) NULL,
	[TrangThai] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_DangKyKTX] PRIMARY KEY CLUSTERED 
(
	[MaDangKy] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[SinhVien]    Script Date: 23/10/2026 10:06:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SinhVien](
	[MaSinhVien] [nvarchar](20) NOT NULL,
	[MaTaiKhoan] [int] NOT NULL,
	[HoTen] [nvarchar](100) NOT NULL,
	[NgaySinh] [date] NOT NULL,
	[GioiTinh] [nvarchar](10) NOT NULL,
	[Lop] [nvarchar](50) NOT NULL,
	[Khoa] [nvarchar](100) NOT NULL,
	[SoDienThoai] [varchar](15) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[DiaChi] [nvarchar](200) NULL,
	[TrangThai] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_SinhVien] PRIMARY KEY CLUSTERED 
(
	[MaSinhVien] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[TaiKhoan]    Script Date: 23/10/2026 10:06:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaiKhoan](
	[MaTaiKhoan] [int] IDENTITY(1,1) NOT NULL,
	[TenDangNhap] [nvarchar](50) NOT NULL,
	[MatKhau] [nvarchar](100) NOT NULL,
	[HoTen] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[VaiTro] [nvarchar](20) NOT NULL,
	[TrangThai] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_TaiKhoan] PRIMARY KEY CLUSTERED 
(
	[MaTaiKhoan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[DangKyKTX] ON 
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (1, N'SV001', CAST(N'2026-03-11' AS Date), CAST(N'2026-03-14' AS Date), CAST(N'2026-12-09' AS Date), 1, NULL, N'DaPhanPhong')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (2, N'SV002', CAST(N'2026-04-10' AS Date), CAST(N'2026-04-13' AS Date), CAST(N'2027-01-08' AS Date), 1, NULL, N'DaPhanPhong')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (3, N'SV003', CAST(N'2026-05-10' AS Date), CAST(N'2026-05-13' AS Date), CAST(N'2027-02-07' AS Date), NULL, NULL, N'DaPhanPhong')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (4, N'SV004', CAST(N'2026-06-09' AS Date), CAST(N'2026-06-12' AS Date), CAST(N'2027-03-09' AS Date), NULL, NULL, N'DaPhanPhong')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (5, N'SV005', CAST(N'2026-06-29' AS Date), CAST(N'2026-07-02' AS Date), CAST(N'2027-03-29' AS Date), 1, NULL, N'DaPhanPhong')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (6, N'SV006', CAST(N'2026-07-29' AS Date), CAST(N'2026-08-01' AS Date), CAST(N'2027-04-28' AS Date), NULL, NULL, N'DaPhanPhong')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (7, N'SV007', CAST(N'2026-08-18' AS Date), CAST(N'2026-08-21' AS Date), CAST(N'2027-05-18' AS Date), NULL, NULL, N'DaPhanPhong')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (8, N'SV008', CAST(N'2026-07-14' AS Date), CAST(N'2026-07-17' AS Date), CAST(N'2027-04-13' AS Date), 2, NULL, N'DaPhanPhong')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (9, N'SV009', CAST(N'2026-08-28' AS Date), CAST(N'2026-08-31' AS Date), CAST(N'2027-05-28' AS Date), NULL, NULL, N'DaPhanPhong')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (10, N'SV010', CAST(N'2026-05-30' AS Date), CAST(N'2026-06-02' AS Date), CAST(N'2027-02-27' AS Date), NULL, NULL, N'DaPhanPhong')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (11, N'SV011', CAST(N'2026-08-08' AS Date), CAST(N'2026-08-11' AS Date), CAST(N'2027-05-08' AS Date), NULL, NULL, N'ChoDuyet')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (12, N'SV012', CAST(N'2026-06-19' AS Date), CAST(N'2026-06-22' AS Date), CAST(N'2027-03-19' AS Date), 4, N'muon chuyen', N'ChoDuyet')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (13, N'SV013', CAST(N'2026-07-09' AS Date), CAST(N'2026-07-12' AS Date), CAST(N'2027-04-08' AS Date), 4, N'phong dep', N'ChoDuyet')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (14, N'SV014', CAST(N'2026-03-31' AS Date), CAST(N'2026-04-03' AS Date), CAST(N'2026-12-29' AS Date), 3, N'ban be tot', N'ChoDuyet')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (15, N'SV015', CAST(N'2026-09-28' AS Date), CAST(N'2026-10-05' AS Date), CAST(N'2027-07-02' AS Date), 1, N'ko', N'ChoDuyet')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (16, N'SV016', CAST(N'2026-03-21' AS Date), CAST(N'2026-03-24' AS Date), CAST(N'2026-12-19' AS Date), 1, N'phong xin', N'ChoDuyet')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (17, N'SV017', CAST(N'2026-04-20' AS Date), CAST(N'2026-04-23' AS Date), CAST(N'2027-01-18' AS Date), 2, N'sao cung dc', N'ChoDuyet')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (18, N'SV018', CAST(N'2026-05-20' AS Date), CAST(N'2026-05-23' AS Date), CAST(N'2027-02-17' AS Date), 1, N'ok', N'ChoDuyet')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (19, N'SV019', CAST(N'2026-06-14' AS Date), CAST(N'2026-06-17' AS Date), CAST(N'2027-03-14' AS Date), 2, N'ok', N'ChoDuyet')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (20, N'SV020', CAST(N'2026-07-04' AS Date), CAST(N'2026-07-07' AS Date), CAST(N'2027-04-03' AS Date), 1, N'ko y kien', N'ChoDuyet')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (21, N'SV021', CAST(N'2026-08-03' AS Date), CAST(N'2026-08-06' AS Date), CAST(N'2027-05-03' AS Date), NULL, NULL, N'DaHuy')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (22, N'SV022', CAST(N'2026-08-23' AS Date), CAST(N'2026-08-26' AS Date), CAST(N'2027-05-23' AS Date), NULL, NULL, N'DaHuy')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (23, N'SV023', CAST(N'2026-09-02' AS Date), CAST(N'2026-09-05' AS Date), CAST(N'2027-06-02' AS Date), 2, NULL, N'DaHuy')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (24, N'SV024', CAST(N'2026-09-28' AS Date), CAST(N'2026-10-03' AS Date), CAST(N'2027-06-30' AS Date), 4, NULL, N'DaHuy')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (25, N'SV025', CAST(N'2026-09-28' AS Date), CAST(N'2026-10-07' AS Date), CAST(N'2027-07-04' AS Date), NULL, NULL, N'DaHuy')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (26, N'SV026', CAST(N'2026-09-07' AS Date), CAST(N'2026-09-10' AS Date), CAST(N'2027-06-07' AS Date), NULL, NULL, N'DaHuy')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (27, N'SV027', CAST(N'2026-04-30' AS Date), CAST(N'2026-05-03' AS Date), CAST(N'2027-01-28' AS Date), NULL, NULL, N'DaHuy')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (28, N'SV028', CAST(N'2026-08-13' AS Date), CAST(N'2026-08-16' AS Date), CAST(N'2027-05-13' AS Date), NULL, NULL, N'DaHuy')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (29, N'SV029', CAST(N'2026-09-12' AS Date), CAST(N'2026-09-15' AS Date), CAST(N'2027-06-12' AS Date), NULL, NULL, N'DaHuy')
INSERT [dbo].[DangKyKTX] ([MaDangKy], [MaSinhVien], [NgayDangKy], [NgayBatDau], [NgayKetThucDuKien], [NguyenVongLoaiPhong], [GhiChu], [TrangThai]) VALUES (30, N'SV030', CAST(N'2026-09-17' AS Date), CAST(N'2026-09-20' AS Date), CAST(N'2027-06-17' AS Date), NULL, NULL, N'DaHuy')
SET IDENTITY_INSERT [dbo].[DangKyKTX] OFF
GO

SET IDENTITY_INSERT [dbo].[LoaiPhong] ON 
INSERT [dbo].[LoaiPhong] ([MaLoaiPhong], [TenLoaiPhong], [SoNguoiToiDa], [DonGiaThang], [MoTa], [TrangThai]) VALUES (1, N'Phòng 4 người', 6, CAST(604001 AS Decimal(18, 0)), N'Phòng tiêu chuẩn 4 giường tầng', N'DangSuDung')
INSERT [dbo].[LoaiPhong] ([MaLoaiPhong], [TenLoaiPhong], [SoNguoiToiDa], [DonGiaThang], [MoTa], [TrangThai]) VALUES (2, N'Phòng 6 người', 6, CAST(450000 AS Decimal(18, 0)), N'Phòng tiết kiệm 6 giường tầng', N'DangSuDung')
INSERT [dbo].[LoaiPhong] ([MaLoaiPhong], [TenLoaiPhong], [SoNguoiToiDa], [DonGiaThang], [MoTa], [TrangThai]) VALUES (3, N'Phòng 8 người', 8, CAST(350000 AS Decimal(18, 0)), N'Phòng tập thể 8 giường tầng', N'DangSuDung')
INSERT [dbo].[LoaiPhong] ([MaLoaiPhong], [TenLoaiPhong], [SoNguoiToiDa], [DonGiaThang], [MoTa], [TrangThai]) VALUES (4, N'Phòng dịch vụ 2 người', 2, CAST(1200000 AS Decimal(18, 0)), N'Phòng khép kín, có điều hòa', N'DangSuDung')
SET IDENTITY_INSERT [dbo].[LoaiPhong] OFF
GO

SET IDENTITY_INSERT [dbo].[PhanPhong] ON 
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (1, 1, 1, CAST(N'2026-03-16' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (2, 2, 1, CAST(N'2026-04-15' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (3, 3, 1, CAST(N'2026-05-15' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (4, 4, 1, CAST(N'2026-06-14' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (5, 5, 2, CAST(N'2026-07-04' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (6, 6, 2, CAST(N'2026-08-03' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (7, 7, 2, CAST(N'2026-08-23' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (8, 8, 3, CAST(N'2026-07-19' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (9, 9, 3, CAST(N'2026-09-02' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (10, 10, 4, CAST(N'2026-06-04' AS Date), CAST(N'2026-08-03' AS Date), N'DaChuyen')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (11, 10, 5, CAST(N'2026-08-03' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (12, 11, 5, CAST(N'2026-08-13' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (13, 12, 6, CAST(N'2026-06-24' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (14, 13, 6, CAST(N'2026-07-14' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (15, 14, 3, CAST(N'2026-04-05' AS Date), CAST(N'2026-07-04' AS Date), N'DaTraPhong')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (16, 16, 9, CAST(N'2026-03-26' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (17, 17, 9, CAST(N'2026-04-25' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (18, 18, 9, CAST(N'2026-05-25' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (19, 19, 9, CAST(N'2026-06-19' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (20, 20, 10, CAST(N'2026-07-09' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (21, 21, 10, CAST(N'2026-08-08' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (22, 22, 10, CAST(N'2026-08-28' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (23, 23, 11, CAST(N'2026-09-07' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (24, 27, 12, CAST(N'2026-05-05' AS Date), CAST(N'2026-07-14' AS Date), N'DaTraPhong')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (25, 28, 13, CAST(N'2026-08-18' AS Date), NULL, N'DangO')
INSERT [dbo].[PhanPhong] ([MaPhanPhong], [MaDangKy], [MaPhong], [NgayVao], [NgayRa], [TrangThai]) VALUES (26, 29, 14, CAST(N'2026-09-17' AS Date), NULL, N'DangO')
SET IDENTITY_INSERT [dbo].[PhanPhong] OFF
GO

SET IDENTITY_INSERT [dbo].[PhongKTX] ON 
INSERT [dbo].[PhongKTX] ([MaPhong], [TenPhong], [MaLoaiPhong], [ToaNha], [Tang], [GioiTinhPhong], [TrangThai], [MoTa]) VALUES (1, N'A101', 1, N'Tòa A', 1, N'Nam', N'DangSuDung', N'Phòng A101 - Tòa A, tầng 1')
INSERT [dbo].[PhongKTX] ([MaPhong], [TenPhong], [MaLoaiPhong], [ToaNha], [Tang], [GioiTinhPhong], [TrangThai], [MoTa]) VALUES (2, N'A102', 1, N'Tòa A', 1, N'Nam', N'DangSuDung', N'Phòng A102 - Tòa A, tầng 1')
INSERT [dbo].[PhongKTX] ([MaPhong], [TenPhong], [MaLoaiPhong], [ToaNha], [Tang], [GioiTinhPhong], [TrangThai], [MoTa]) VALUES (3, N'A103', 2, N'Tòa A', 1, N'Nam', N'DangSuDung', N'Phòng A103 - Tòa A, tầng 1')
INSERT [dbo].[PhongKTX] ([MaPhong], [TenPhong], [MaLoaiPhong], [ToaNha], [Tang], [GioiTinhPhong], [TrangThai], [MoTa]) VALUES (4, N'A201', 1, N'Tòa A', 2, N'Nam', N'DangSuDung', N'Phòng A201 - Tòa A, tầng 2')
INSERT [dbo].[PhongKTX] ([MaPhong], [TenPhong], [MaLoaiPhong], [ToaNha], [Tang], [GioiTinhPhong], [TrangThai], [MoTa]) VALUES (5, N'A202', 3, N'Tòa A', 2, N'Nam', N'DangSuDung', N'Phòng A202 - Tòa A, tầng 2')
INSERT [dbo].[PhongKTX] ([MaPhong], [TenPhong], [MaLoaiPhong], [ToaNha], [Tang], [GioiTinhPhong], [TrangThai], [MoTa]) VALUES (6, N'A203', 4, N'Tòa A', 2, N'Nam', N'DangSuDung', N'Phòng A203 - Tòa A, tầng 2')
INSERT [dbo].[PhongKTX] ([MaPhong], [TenPhong], [MaLoaiPhong], [ToaNha], [Tang], [GioiTinhPhong], [TrangThai], [MoTa]) VALUES (7, N'A301', 2, N'Tòa A', 3, N'Nam', N'DangSuDung', N'Phòng A301 - Tòa A, tầng 3')
INSERT [dbo].[PhongKTX] ([MaPhong], [TenPhong], [MaLoaiPhong], [ToaNha], [Tang], [GioiTinhPhong], [TrangThai], [MoTa]) VALUES (8, N'A302', 4, N'Tòa A', 3, N'Nam', N'TamNgung', N'Phòng A302 - Tòa A, tầng 3')
INSERT [dbo].[PhongKTX] ([MaPhong], [TenPhong], [MaLoaiPhong], [ToaNha], [Tang], [GioiTinhPhong], [TrangThai], [MoTa]) VALUES (9, N'B101', 1, N'Tòa B', 1, N'Nữ', N'DangSuDung', N'Phòng B101 - Tòa B, tầng 1')
INSERT [dbo].[PhongKTX] ([MaPhong], [TenPhong], [MaLoaiPhong], [ToaNha], [Tang], [GioiTinhPhong], [TrangThai], [MoTa]) VALUES (10, N'B102', 1, N'Tòa B', 1, N'Nữ', N'DangSuDung', N'Phòng B102 - Tòa B, tầng 1')
INSERT [dbo].[PhongKTX] ([MaPhong], [TenPhong], [MaLoaiPhong], [ToaNha], [Tang], [GioiTinhPhong], [TrangThai], [MoTa]) VALUES (11, N'B103', 2, N'Tòa B', 1, N'Nữ', N'DangSuDung', N'Phòng B103 - Tòa B, tầng 1')
INSERT [dbo].[PhongKTX] ([MaPhong], [TenPhong], [MaLoaiPhong], [ToaNha], [Tang], [GioiTinhPhong], [TrangThai], [MoTa]) VALUES (12, N'B201', 1, N'Tòa B', 2, N'Nữ', N'DangSuDung', N'Phòng B201 - Tòa B, tầng 2')
INSERT [dbo].[PhongKTX] ([MaPhong], [TenPhong], [MaLoaiPhong], [ToaNha], [Tang], [GioiTinhPhong], [TrangThai], [MoTa]) VALUES (13, N'B202', 3, N'Tòa B', 2, N'Nữ', N'DangSuDung', N'Phòng B202 - Tòa B, tầng 2')
INSERT [dbo].[PhongKTX] ([MaPhong], [TenPhong], [MaLoaiPhong], [ToaNha], [Tang], [GioiTinhPhong], [TrangThai], [MoTa]) VALUES (14, N'B203', 4, N'Tòa B', 2, N'Nữ', N'DangSuDung', N'Phòng B203 - Tòa B, tầng 2')
INSERT [dbo].[PhongKTX] ([MaPhong], [TenPhong], [MaLoaiPhong], [ToaNha], [Tang], [GioiTinhPhong], [TrangThai], [MoTa]) VALUES (15, N'B301', 2, N'Tòa B', 3, N'Nữ', N'DangSuDung', N'Phòng B301 - Tòa B, tầng 3')
INSERT [dbo].[PhongKTX] ([MaPhong], [TenPhong], [MaLoaiPhong], [ToaNha], [Tang], [GioiTinhPhong], [TrangThai], [MoTa]) VALUES (16, N'B302', 4, N'Tòa B', 3, N'Nữ', N'DangSuDung', N'Phòng B302 - Tòa B, tầng 3')
INSERT [dbo].[PhongKTX] ([MaPhong], [TenPhong], [MaLoaiPhong], [ToaNha], [Tang], [GioiTinhPhong], [TrangThai], [MoTa]) VALUES (17, N'C101', 2, N'Tòa C', 1, N'Nam', N'DangSuDung', N'Phòng C101 - Tòa C, tầng 1')
INSERT [dbo].[PhongKTX] ([MaPhong], [TenPhong], [MaLoaiPhong], [ToaNha], [Tang], [GioiTinhPhong], [TrangThai], [MoTa]) VALUES (18, N'C102', 3, N'Tòa C', 1, N'Nam', N'DangSuDung', N'Phòng C102 - Tòa C, tầng 1')
INSERT [dbo].[PhongKTX] ([MaPhong], [TenPhong], [MaLoaiPhong], [ToaNha], [Tang], [GioiTinhPhong], [TrangThai], [MoTa]) VALUES (19, N'C201', 2, N'Tòa C', 2, N'Nữ', N'DangSuDung', N'Phòng C201 - Tòa C, tầng 2')
INSERT [dbo].[PhongKTX] ([MaPhong], [TenPhong], [MaLoaiPhong], [ToaNha], [Tang], [GioiTinhPhong], [TrangThai], [MoTa]) VALUES (20, N'C202', 1, N'Tòa C', 2, N'Nữ', N'TamNgung', N'Phòng C202 - Tòa C, tầng 2')
SET IDENTITY_INSERT [dbo].[PhongKTX] OFF
GO

INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV001', 3, N'Nguyễn Văn An', CAST(N'2004-02-02' AS Date), N'Nam', N'TI17A2HN', N'Điện - Điện tử', N'0911234567', N'sv001@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV002', 4, N'Trần Minh Bình', CAST(N'2005-03-03' AS Date), N'Nam', N'TI17A3HN', N'Cơ khí', N'0912469134', N'sv002@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV003', 5, N'Lê Quốc Cường', CAST(N'2003-04-04' AS Date), N'Nam', N'TI17A4HN', N'Kinh tế', N'0913703701', N'sv003@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV004', 6, N'Phạm Đức Dũng', CAST(N'2004-05-05' AS Date), N'Nam', N'TI17A1HN', N'Ngoại ngữ', N'0914938268', N'sv004@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV005', 7, N'Hoàng Văn Em', CAST(N'2005-06-06' AS Date), N'Nam', N'TI17A2HN', N'Công nghệ thông tin', N'0916172835', N'sv005@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV006', 8, N'Vũ Hữu Phúc', CAST(N'2003-07-07' AS Date), N'Nam', N'TI17A3HN', N'Điện - Điện tử', N'0917407402', N'sv006@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV007', 9, N'Đặng Quang Giang', CAST(N'2004-08-08' AS Date), N'Nam', N'TI17A4HN', N'Cơ khí', N'0918641969', N'sv007@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV008', 10, N'Bùi Thanh Hải', CAST(N'2005-09-09' AS Date), N'Nam', N'TI17A1HN', N'Kinh tế', N'0919876536', N'sv008@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV009', 11, N'Đỗ Mạnh Hùng', CAST(N'2003-10-10' AS Date), N'Nam', N'TI17A2HN', N'Ngoại ngữ', N'0921111103', N'sv009@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV010', 12, N'Ngô Việt Khoa', CAST(N'2004-11-11' AS Date), N'Nam', N'TI17A3HN', N'Công nghệ thông tin', N'0922345670', N'sv010@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV011', 13, N'Dương Tuấn Kiệt', CAST(N'2005-12-12' AS Date), N'Nam', N'TI17A4HN', N'Điện - Điện tử', N'0923580237', N'sv011@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV012', 14, N'Lý Gia Long', CAST(N'2003-01-13' AS Date), N'Nam', N'TI17A1HN', N'Cơ khí', N'0924814804', N'sv012@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV013', 15, N'Phan Hoàng Nam', CAST(N'2004-02-14' AS Date), N'Nam', N'TI17A2HN', N'Kinh tế', N'0926049371', N'sv013@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV014', 16, N'Trịnh Công Phú', CAST(N'2005-03-15' AS Date), N'Nam', N'TI17A3HN', N'Ngoại ngữ', N'0927283938', N'sv014@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV015', 17, N'Mai Xuân Quý', CAST(N'2003-04-16' AS Date), N'Nam', N'TI17A4HN', N'Công nghệ thông tin', N'0928518505', N'sv015@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV016', 18, N'Nguyễn Thị Lan', CAST(N'2004-05-17' AS Date), N'Nữ', N'TI17A1HN', N'Điện - Điện tử', N'0929753072', N'sv016@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV017', 19, N'Trần Thu Hà', CAST(N'2005-06-18' AS Date), N'Nữ', N'TI17A2HN', N'Cơ khí', N'0930987639', N'sv017@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV018', 20, N'Lê Ngọc Mai', CAST(N'2003-07-19' AS Date), N'Nữ', N'TI17A3HN', N'Kinh tế', N'0932222206', N'sv018@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV019', 21, N'Phạm Thùy Linh', CAST(N'2004-08-20' AS Date), N'Nữ', N'TI17A4HN', N'Ngoại ngữ', N'0933456773', N'sv019@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV020', 22, N'Hoàng Kim Ngân', CAST(N'2005-09-21' AS Date), N'Nữ', N'TI17A1HN', N'Công nghệ thông tin', N'0934691340', N'sv020@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV021', 23, N'Vũ Thanh Nhàn', CAST(N'2003-10-22' AS Date), N'Nữ', N'TI17A2HN', N'Điện - Điện tử', N'0935925907', N'sv021@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV022', 24, N'Đặng Bích Phương', CAST(N'2004-11-23' AS Date), N'Nữ', N'TI17A3HN', N'Cơ khí', N'0937160474', N'sv022@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV023', 25, N'Bùi Diệu Quyên', CAST(N'2005-12-24' AS Date), N'Nữ', N'TI17A4HN', N'Kinh tế', N'0938395041', N'sv023@sv.edu.vn', N'Hà Nội', N'DangHoatDong')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV024', 26, N'Đỗ Hồng Sen', CAST(N'2003-01-25' AS Date), N'Nữ', N'TI17A1HN', N'Ngoại ngữ', N'0939629608', N'sv024@sv.edu.vn', N'Hà Nội', N'Khoa')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV025', 27, N'Ngô Thảo Trang', CAST(N'2004-02-26' AS Date), N'Nữ', N'TI17A2HN', N'Công nghệ thông tin', N'0940864175', N'sv025@sv.edu.vn', N'Hà Nội', N'Khoa')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV026', 28, N'Dương Yến Vy', CAST(N'2005-03-27' AS Date), N'Nữ', N'TI17A3HN', N'Điện - Điện tử', N'0942098742', N'sv026@sv.edu.vn', N'Hà Nội', N'Khoa')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV027', 29, N'Lý Mỹ Xuân', CAST(N'2003-04-01' AS Date), N'Nữ', N'TI17A4HN', N'Cơ khí', N'0943333309', N'sv027@sv.edu.vn', N'Hà Nội', N'Khoa')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV028', 30, N'Phan Thái Yên', CAST(N'2004-05-02' AS Date), N'Nữ', N'TI17A1HN', N'Kinh tế', N'0944567876', N'sv028@sv.edu.vn', N'Hà Nội', N'Khoa')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV029', 31, N'Trịnh Bảo Châu', CAST(N'2005-06-03' AS Date), N'Nữ', N'TI17A2HN', N'Ngoại ngữ', N'0945802443', N'sv029@sv.edu.vn', N'Hà Nội', N'Khoa')
INSERT [dbo].[SinhVien] ([MaSinhVien], [MaTaiKhoan], [HoTen], [NgaySinh], [GioiTinh], [Lop], [Khoa], [SoDienThoai], [Email], [DiaChi], [TrangThai]) VALUES (N'SV030', 32, N'Mai Phương Thảo', CAST(N'2003-07-04' AS Date), N'Nữ', N'TI17A3HN', N'Công nghệ thông tin', N'0947037010', N'sv030@sv.edu.vn', N'Hà Nội', N'Khoa')
GO

SET IDENTITY_INSERT [dbo].[TaiKhoan] ON 
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (1, N'admin', N'123456', N'Quản trị viên 1', N'admin@ktx.edu.vn', N'Admin', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (2, N'admin2', N'123456', N'Quản trị viên 2', N'admin2@ktx.edu.vn', N'Admin', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (3, N'sv001', N'456789', N'Nguyễn Văn An', N'sv001@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (4, N'sv002', N'123456', N'Trần Minh Bình', N'sv002@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (5, N'sv003', N'123456', N'Lê Quốc Cường', N'sv003@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (6, N'sv004', N'123456', N'Phạm Đức Dũng', N'sv004@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (7, N'sv005', N'123456', N'Hoàng Văn Em', N'sv005@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (8, N'sv006', N'123456', N'Vũ Hữu Phúc', N'sv006@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (9, N'sv007', N'123456', N'Đặng Quang Giang', N'sv007@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (10, N'sv008', N'123456', N'Bùi Thanh Hải', N'sv008@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (11, N'sv009', N'123456', N'Đỗ Mạnh Hùng', N'sv009@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (12, N'sv010', N'123456', N'Ngô Việt Khoa', N'sv010@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (13, N'sv011', N'123456', N'Dương Tuấn Kiệt', N'sv011@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (14, N'sv012', N'123456', N'Lý Gia Long', N'sv012@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (15, N'sv013', N'123456', N'Phan Hoàng Nam', N'sv013@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (16, N'sv014', N'123456', N'Trịnh Công Phú', N'sv014@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (17, N'sv015', N'123456', N'Mai Xuân Quý', N'sv015@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (18, N'sv016', N'123456', N'Nguyễn Thị Lan', N'sv016@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (19, N'sv017', N'123456', N'Trần Thu Hà', N'sv017@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (20, N'sv018', N'123456', N'Lê Ngọc Mai', N'sv018@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (21, N'sv019', N'123456', N'Phạm Thùy Linh', N'sv019@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (22, N'sv020', N'123456', N'Hoàng Kim Ngân', N'sv020@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (23, N'sv021', N'123456', N'Vũ Thanh Nhàn', N'sv021@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (24, N'sv022', N'123456', N'Đặng Bích Phương', N'sv022@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (25, N'sv023', N'123456', N'Bùi Diệu Quyên', N'sv023@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (26, N'sv024', N'123456', N'Đỗ Hồng Sen', N'sv024@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (27, N'sv025', N'123456', N'Ngô Thảo Trang', N'sv025@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (28, N'sv026', N'123456', N'Dương Yến Vy', N'sv026@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (29, N'sv027', N'123456', N'Lý Mỹ Xuân', N'sv027@sv.edu.vn', N'SinhVien', N'DangHoatDong')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (30, N'sv028', N'123456', N'Phan Thái Yên', N'sv028@sv.edu.vn', N'SinhVien', N'Khoa')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (31, N'sv029', N'123456', N'Trịnh Bảo Châu', N'sv029@sv.edu.vn', N'SinhVien', N'Khoa')
INSERT [dbo].[TaiKhoan] ([MaTaiKhoan], [TenDangNhap], [MatKhau], [HoTen], [Email], [VaiTro], [TrangThai]) VALUES (32, N'sv030', N'123456', N'Mai Phương Thảo', N'sv030@sv.edu.vn', N'SinhVien', N'Khoa')
SET IDENTITY_INSERT [dbo].[TaiKhoan] OFF
GO

SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_DangKyKTX_MaSinhVien]    Script Date: 23/10/2026 10:06:23 AM ******/
CREATE NONCLUSTERED INDEX [IX_DangKyKTX_MaSinhVien] ON [dbo].[DangKyKTX]
(
	[MaSinhVien] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_DangKyKTX_ChoDuyet]    Script Date: 23/10/2026 10:06:23 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_DangKyKTX_ChoDuyet] ON [dbo].[DangKyKTX]
(
	[MaSinhVien] ASC
)
WHERE ([TrangThai]=N'ChoDuyet')
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_DangKyKTX_DaPhanPhong]    Script Date: 23/10/2026 10:06:23 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_DangKyKTX_DaPhanPhong] ON [dbo].[DangKyKTX]
(
	[MaSinhVien] ASC
)
WHERE ([TrangThai]=N'DaPhanPhong')
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_LoaiPhong_Ten]    Script Date: 23/10/2026 10:06:23 AM ******/
ALTER TABLE [dbo].[LoaiPhong] ADD  CONSTRAINT [UQ_LoaiPhong_Ten] UNIQUE NONCLUSTERED 
(
	[TenLoaiPhong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_PhanPhong_MaPhong_TrangThai]    Script Date: 23/10/2026 10:06:23 AM ******/
CREATE NONCLUSTERED INDEX [IX_PhanPhong_MaPhong_TrangThai] ON [dbo].[PhanPhong]
(
	[MaPhong] ASC,
	[TrangThai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/****** Object:  Index [UX_PhanPhong_DangO_MaDangKy]    Script Date: 23/10/2026 10:06:23 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_PhanPhong_DangO_MaDangKy] ON [dbo].[PhanPhong]
(
	[MaDangKy] ASC
)
WHERE ([TrangThai]=N'DangO')
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_PhongKTX_Ten]    Script Date: 23/10/2026 10:06:23 AM ******/
ALTER TABLE [dbo].[PhongKTX] ADD  CONSTRAINT [UQ_PhongKTX_Ten] UNIQUE NONCLUSTERED 
(
	[TenPhong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/****** Object:  Index [UQ_SinhVien_MaTaiKhoan]    Script Date: 23/10/2026 10:06:23 AM ******/
ALTER TABLE [dbo].[SinhVien] ADD  CONSTRAINT [UQ_SinhVien_MaTaiKhoan] UNIQUE NONCLUSTERED 
(
	[MaTaiKhoan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_TaiKhoan_TenDangNhap]    Script Date: 23/10/2026 10:06:23 AM ******/
ALTER TABLE [dbo].[TaiKhoan] ADD  CONSTRAINT [UQ_TaiKhoan_TenDangNhap] UNIQUE NONCLUSTERED 
(
	[TenDangNhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[DangKyKTX] ADD  CONSTRAINT [DF_DangKyKTX_NgayDangKy]  DEFAULT (CONVERT([date],getdate())) FOR [NgayDangKy]
GO

ALTER TABLE [dbo].[DangKyKTX] ADD  CONSTRAINT [DF_DangKyKTX_TrangThai]  DEFAULT (N'ChoDuyet') FOR [TrangThai]
GO

ALTER TABLE [dbo].[LoaiPhong] ADD  CONSTRAINT [DF_LoaiPhong_TrangThai]  DEFAULT (N'DangSuDung') FOR [TrangThai]
GO

ALTER TABLE [dbo].[PhanPhong] ADD  CONSTRAINT [DF_PhanPhong_TrangThai]  DEFAULT (N'DangO') FOR [TrangThai]
GO

ALTER TABLE [dbo].[PhongKTX] ADD  CONSTRAINT [DF_PhongKTX_TrangThai]  DEFAULT (N'DangSuDung') FOR [TrangThai]
GO

ALTER TABLE [dbo].[SinhVien] ADD  CONSTRAINT [DF_SinhVien_TrangThai]  DEFAULT (N'DangHoatDong') FOR [TrangThai]
GO

ALTER TABLE [dbo].[TaiKhoan] ADD  CONSTRAINT [DF_TaiKhoan_TrangThai]  DEFAULT (N'DangHoatDong') FOR [TrangThai]
GO

ALTER TABLE [dbo].[DangKyKTX]  WITH CHECK ADD  CONSTRAINT [FK_DangKyKTX_LoaiPhong] FOREIGN KEY([NguyenVongLoaiPhong])
REFERENCES [dbo].[LoaiPhong] ([MaLoaiPhong])
GO
ALTER TABLE [dbo].[DangKyKTX] CHECK CONSTRAINT [FK_DangKyKTX_LoaiPhong]
GO

ALTER TABLE [dbo].[DangKyKTX]  WITH CHECK ADD  CONSTRAINT [FK_DangKyKTX_SinhVien] FOREIGN KEY([MaSinhVien])
REFERENCES [dbo].[SinhVien] ([MaSinhVien])
GO
ALTER TABLE [dbo].[DangKyKTX] CHECK CONSTRAINT [FK_DangKyKTX_SinhVien]
GO

ALTER TABLE [dbo].[PhanPhong]  WITH CHECK ADD  CONSTRAINT [FK_PhanPhong_DangKyKTX] FOREIGN KEY([MaDangKy])
REFERENCES [dbo].[DangKyKTX] ([MaDangKy])
GO
ALTER TABLE [dbo].[PhanPhong] CHECK CONSTRAINT [FK_PhanPhong_DangKyKTX]
GO

ALTER TABLE [dbo].[PhanPhong]  WITH CHECK ADD  CONSTRAINT [FK_PhanPhong_PhongKTX] FOREIGN KEY([MaPhong])
REFERENCES [dbo].[PhongKTX] ([MaPhong])
GO
ALTER TABLE [dbo].[PhanPhong] CHECK CONSTRAINT [FK_PhanPhong_PhongKTX]
GO

ALTER TABLE [dbo].[PhongKTX]  WITH CHECK ADD  CONSTRAINT [FK_PhongKTX_LoaiPhong] FOREIGN KEY([MaLoaiPhong])
REFERENCES [dbo].[LoaiPhong] ([MaLoaiPhong])
GO
ALTER TABLE [dbo].[PhongKTX] CHECK CONSTRAINT [FK_PhongKTX_LoaiPhong]
GO

ALTER TABLE [dbo].[SinhVien]  WITH CHECK ADD  CONSTRAINT [FK_SinhVien_TaiKhoan] FOREIGN KEY([MaTaiKhoan])
REFERENCES [dbo].[TaiKhoan] ([MaTaiKhoan])
GO
ALTER TABLE [dbo].[SinhVien] CHECK CONSTRAINT [FK_SinhVien_TaiKhoan]
GO

ALTER TABLE [dbo].[DangKyKTX]  WITH CHECK ADD  CONSTRAINT [CK_DangKyKTX_Ngay] CHECK  (([NgayKetThucDuKien]>[NgayBatDau]))
GO
ALTER TABLE [dbo].[DangKyKTX] CHECK CONSTRAINT [CK_DangKyKTX_Ngay]
GO

ALTER TABLE [dbo].[DangKyKTX]  WITH CHECK ADD  CONSTRAINT [CK_DangKyKTX_TrangThai] CHECK  (([TrangThai]=N'DaHuy' OR [TrangThai]=N'DaTraPhong' OR [TrangThai]=N'DaPhanPhong' OR [TrangThai]=N'ChoDuyet'))
GO
ALTER TABLE [dbo].[DangKyKTX] CHECK CONSTRAINT [CK_DangKyKTX_TrangThai]
GO

ALTER TABLE [dbo].[LoaiPhong]  WITH CHECK ADD  CONSTRAINT [CK_LoaiPhong_Gia] CHECK  (([DonGiaThang]>(0)))
GO
ALTER TABLE [dbo].[LoaiPhong] CHECK CONSTRAINT [CK_LoaiPhong_Gia]
GO

ALTER TABLE [dbo].[LoaiPhong]  WITH CHECK ADD  CONSTRAINT [CK_LoaiPhong_SucChua] CHECK  (([SoNguoiToiDa]>(0)))
GO
ALTER TABLE [dbo].[LoaiPhong] CHECK CONSTRAINT [CK_LoaiPhong_SucChua]
GO

ALTER TABLE [dbo].[LoaiPhong]  WITH CHECK ADD  CONSTRAINT [CK_LoaiPhong_TrangThai] CHECK  (([TrangThai]=N'NgungSuDung' OR [TrangThai]=N'DangSuDung'))
GO
ALTER TABLE [dbo].[LoaiPhong] CHECK CONSTRAINT [CK_LoaiPhong_TrangThai]
GO

ALTER TABLE [dbo].[PhanPhong]  WITH CHECK ADD  CONSTRAINT [CK_PhanPhong_DangO] CHECK  (([TrangThai]=N'DangO' AND [NgayRa] IS NULL OR [TrangThai]<>N'DangO'))
GO
ALTER TABLE [dbo].[PhanPhong] CHECK CONSTRAINT [CK_PhanPhong_DangO]
GO

ALTER TABLE [dbo].[PhanPhong]  WITH CHECK ADD  CONSTRAINT [CK_PhanPhong_NgayRa] CHECK  (([NgayRa] IS NULL OR [NgayRa]>=[NgayVao]))
GO
ALTER TABLE [dbo].[PhanPhong] CHECK CONSTRAINT [CK_PhanPhong_NgayRa]
GO

ALTER TABLE [dbo].[PhanPhong]  WITH CHECK ADD  CONSTRAINT [CK_PhanPhong_TrangThai] CHECK  (([TrangThai]=N'DaTraPhong' OR [TrangThai]=N'DaChuyen' OR [TrangThai]=N'DangO'))
GO
ALTER TABLE [dbo].[PhanPhong] CHECK CONSTRAINT [CK_PhanPhong_TrangThai]
GO

ALTER TABLE [dbo].[PhongKTX]  WITH CHECK ADD  CONSTRAINT [CK_PhongKTX_GioiTinh] CHECK  (([GioiTinhPhong]=N'Nữ' OR [GioiTinhPhong]=N'Nam'))
GO
ALTER TABLE [dbo].[PhongKTX] CHECK CONSTRAINT [CK_PhongKTX_GioiTinh]
GO

ALTER TABLE [dbo].[PhongKTX]  WITH CHECK ADD  CONSTRAINT [CK_PhongKTX_Tang] CHECK  (([Tang]>(0)))
GO
ALTER TABLE [dbo].[PhongKTX] CHECK CONSTRAINT [CK_PhongKTX_Tang]
GO

ALTER TABLE [dbo].[PhongKTX]  WITH CHECK ADD  CONSTRAINT [CK_PhongKTX_TrangThai] CHECK  (([TrangThai]=N'TamNgung' OR [TrangThai]=N'DangSuDung'))
GO
ALTER TABLE [dbo].[PhongKTX] CHECK CONSTRAINT [CK_PhongKTX_TrangThai]
GO

ALTER TABLE [dbo].[SinhVien]  WITH CHECK ADD  CONSTRAINT [CK_SinhVien_Email] CHECK  (([Email] like N'%_@_%._%'))
GO
ALTER TABLE [dbo].[SinhVien] CHECK CONSTRAINT [CK_SinhVien_Email]
GO

ALTER TABLE [dbo].[SinhVien]  WITH CHECK ADD  CONSTRAINT [CK_SinhVien_GioiTinh] CHECK  (([GioiTinh]=N'Nữ' OR [GioiTinh]=N'Nam'))
GO
ALTER TABLE [dbo].[SinhVien] CHECK CONSTRAINT [CK_SinhVien_GioiTinh]
GO

ALTER TABLE [dbo].[SinhVien]  WITH CHECK ADD  CONSTRAINT [CK_SinhVien_SDT] CHECK  ((NOT [SoDienThoai] like '%[^0-9]%' AND (len([SoDienThoai])>=(9) AND len([SoDienThoai])<=(11))))
GO
ALTER TABLE [dbo].[SinhVien] CHECK CONSTRAINT [CK_SinhVien_SDT]
GO

ALTER TABLE [dbo].[SinhVien]  WITH CHECK ADD  CONSTRAINT [CK_SinhVien_TrangThai] CHECK  (([TrangThai]=N'Khoa' OR [TrangThai]=N'DangHoatDong'))
GO
ALTER TABLE [dbo].[SinhVien] CHECK CONSTRAINT [CK_SinhVien_TrangThai]
GO

ALTER TABLE [dbo].[TaiKhoan]  WITH CHECK ADD  CONSTRAINT [CK_TaiKhoan_Email] CHECK  (([Email] like N'%_@_%._%'))
GO
ALTER TABLE [dbo].[TaiKhoan] CHECK CONSTRAINT [CK_TaiKhoan_Email]
GO

ALTER TABLE [dbo].[TaiKhoan]  WITH CHECK ADD  CONSTRAINT [CK_TaiKhoan_TrangThai] CHECK  (([TrangThai]=N'Khoa' OR [TrangThai]=N'DangHoatDong'))
GO
ALTER TABLE [dbo].[TaiKhoan] CHECK CONSTRAINT [CK_TaiKhoan_TrangThai]
GO

ALTER TABLE [dbo].[TaiKhoan]  WITH CHECK ADD  CONSTRAINT [CK_TaiKhoan_VaiTro] CHECK  (([VaiTro]=N'SinhVien' OR [VaiTro]=N'Admin'))
GO
ALTER TABLE [dbo].[TaiKhoan] CHECK CONSTRAINT [CK_TaiKhoan_VaiTro]
GO

USE [master]
GO
