# Quản lý ký túc xá – Đồ án nhóm 06 (UNETI, DHTI17A2ND)

Ứng dụng web quản lý ký túc xá: quản lý tài khoản và phân quyền, loại phòng, phòng, sinh viên, đăng ký KTX, phân/chuyển/trả phòng và thống kê.

Công nghệ: ASP.NET Core 10 MVC · Entity Framework Core 10 · SQL Server · Razor Views.

## 1. Yêu cầu môi trường

- Windows 10/11
- Visual Studio 2022 (17.12 trở lên)
- .NET 10 SDK (kiểm tra bằng lệnh `dotnet --list-sdks`)
- SQL Server (LocalDB, Express hoặc Developer) > = 2019 
- SQL Server Management Studio (SSMS) hoặc Azure Data Studio để chạy script SQL
- Git for Windows (để Visual Studio làm việc với GitHub)

## 2. Cấu trúc thư mục chính


DoAnNhom06/
├── Controllers/     Xử lý yêu cầu: Account, TaiKhoan, LoaiPhong, PhongKTX, SinhVien,
│                    DangKyKTX, PhanPhong, ThongKe, Home, Base
├── Models/          Thực thể dữ liệu và ViewModel (PhongKTX, SinhVien, DangKyKTX, TaiKhoan, LoaiPhong, PhanPhong...)
├── Data/            AppDbContext.cs – cấu hình EF Core và kết nối CSDL
├── Services/        PhongTruyVan.cs – truy vấn phòng và số chỗ còn trống
├── Filters/         RequireRoleAttribute.cs – kiểm tra quyền truy cập theo vai trò
├── Helpers/         Hàm tiện ích dùng chung (mã sinh viên, định dạng, thông báo)
├── Views/           Giao diện Razor theo từng chức năng, Shared/ là layout và thành phần chung
├── wwwroot/         File tĩnh: css/site.css, thư viện JS/CSS
├── Program.cs       Điểm khởi động ứng dụng, đăng ký dịch vụ và đường dẫn
├── appsettings.json Chuỗi kết nối CSDL (ConnectionStrings:KyTucXa)
└── Properties/      launchSettings.json – cổng chạy ứng dụng


Thư mục `Forms/` và `Controls/` chứa mã WinForms cũ không thuộc phiên bản web; các file này không được dùng khi chạy ứng dụng web.

## 3. Chạy dự án lần đầu

1. Clone repository:
   ```
   git clone https://github.com/duy813544-rgb/QuanLyKyTucXa_UNETI06_DHTI17A2ND.git
   ```
2. Mở file `DoAnNhom06.slnx` bằng Visual Studio.
3. Mở `appsettings.json`, kiểm tra chuỗi kết nối phù hợp với máy bạn:
   ```json
   "ConnectionStrings": {
     "KyTucXa": "Server=localhost;Database=QL_KY_TUC_XA;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```
   Nếu dùng SQL Server Express, đổi `Server=localhost` thành `Server=localhost\SQLEXPRESS`.
4. Tạo cơ sở dữ liệu: mở `QuanLyKyTucXa_UNETI06.ipynb` (hoặc file SQL tương ứng) bằng SSMS và chạy toàn bộ script. Script tạo database `QL_KY_TUC_XA`, các bảng và dữ liệu mẫu.
5. Nhấn **F5** (hoặc `dotnet run`) để chạy. Ứng dụng mở tại:
   - https://localhost:60998
   - http://localhost:60999

> **Lưu ý:** Repository hiện không có thư mục `Migrations/` và `Program.cs` không tự tạo bảng hay nạp dữ liệu mẫu. Vì vậy phải chạy script SQL ở bước 4 trước khi đăng nhập.

## 4. Tài khoản kiểm thử

| Tên đăng nhập | Mật khẩu | Vai trò | Ghi chú |
|---|---|---|---|
| admin | 123456 | Quản trị viên | Đang hoạt động |
| admin2 | 123456 | Quản trị viên | Đang hoạt động |
| sv001 | 456789 | Sinh viên | Mật khẩu khác các tài khoản còn lại |
| sv002 – sv027 | 123456 | Sinh viên | Đang hoạt động |
| sv028 – sv030 | 123456 | Sinh viên | Trạng thái **Khóa** (không đăng nhập được) |

Sinh viên đăng nhập bằng mã sinh viên trong cột tên đăng nhập. Tài khoản sinh viên chỉ thấy dữ liệu của chính mình; đăng nhập `sv001` rồi truy cập trực tiếp `/LoaiPhong`, `/ThongKe/Dashboard` hoặc `/SinhVien` sẽ bị chặn bằng trang "Không có quyền".

## 5. Dữ liệu mẫu

Script SQL nạp sẵn:

| Bảng | Số dòng |
|---|---|
| TaiKhoan | 32 (2 Admin + 30 sinh viên) |
| SinhVien | 30 |
| LoaiPhong | 4 |
| PhongKTX | 20 (18 đang sử dụng, 2 tạm ngừng) |
| DangKyKTX | 30 (10 chờ duyệt, 10 đã phân phòng, 10 đã hủy) |
| PhanPhong | 26 (23 đang ở, 2 đã trả phòng, 1 đã chuyển phòng) |

Cơ sở dữ liệu có thêm các ràng buộc: tên đăng nhập và tên loại phòng là duy nhất; mỗi đăng ký chỉ có một bản ghi phân phòng "đang ở" tại một thời điểm; view `vw_PhongSoCho` tính số chỗ còn trống của từng phòng.
