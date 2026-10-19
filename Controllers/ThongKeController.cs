// Họ và tên: Nguyễn Bá Hùng                     
// Mã sinh viên: 23274800011                 
// Nội dung thực hiện: Dashboard và thống kê bằng LINQ (Module 4):   
// số phòng/sinh viên theo loại, tòa; tỷ lệ sử dụng; vào KTX theo tháng; xuất Excel.
using ClosedXML.Excel;
using DoAnNhom06.Filters;
using DoAnNhom06.Models;
using DoAnNhom06.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Controllers;   // <<< TỰ SỬA nếu nhóm đổi namespace theo quy định đặt tên project

[RequireRole(VaiTro.Admin)]
public class ThongKeController(AppDbContext db) : BaseController
{
    public async Task<IActionResult> Index(int? nam)
        => View(await TaoThongKeAsync(nam));

    /// <summary>Xuất toàn bộ số liệu thống kê hiện tại ra file Excel (mỗi nhóm số liệu một sheet).</summary>
    public async Task<IActionResult> XuatExcel(int? nam)
    {
        var vm = await TaoThongKeAsync(nam);
        using var wb = new XLWorkbook();

        void VietSheet(string ten, string[] tieuDe, IEnumerable<object[]> dong)
        {
            var ws = wb.Worksheets.Add(ten);
            for (var c = 0; c < tieuDe.Length; c++) ws.Cell(1, c + 1).Value = tieuDe[c];
            var hang = ws.Range(1, 1, 1, tieuDe.Length);
            hang.Style.Font.Bold = true;
            hang.Style.Fill.BackgroundColor = XLColor.LightGray;
            var r = 2;
            foreach (var d in dong)
            {
                for (var c = 0; c < d.Length; c++)
                {
                    var o = d[c];
                    if (o is int i) ws.Cell(r, c + 1).Value = i;
                    else if (o is double db1) ws.Cell(r, c + 1).Value = db1;
                    else ws.Cell(r, c + 1).Value = o?.ToString() ?? "";
                }
                r++;
            }
            ws.Columns().AdjustToContents();
        }

        VietSheet("Tổng quan", ["Chỉ số", "Giá trị"],
        [
            ["Tổng số chỗ", vm.TongCho],
            ["Chỗ đang sử dụng", vm.ChoDangSuDung],
            ["Chỗ còn lại", vm.ChoConLai],
            ["Tỷ lệ sử dụng (%)", vm.TyLeSuDung],
        ]);
        VietSheet("Theo loại phòng", ["Loại phòng", "Số phòng", "Sức chứa", "Đang ở", "Chỗ còn", "Tỷ lệ (%)"],
            vm.SoPhongTheoLoai.Select(x => new object[] { x.TenLoai, x.SoPhong, x.SucChua, x.DangO, x.ConLai, x.TyLe }));
        VietSheet("Theo tòa", ["Tòa", "Số phòng", "Sức chứa", "Đang ở", "Chỗ còn", "Tỷ lệ (%)"],
            vm.TyLeTheoToa.Select(x => new object[] { x.ToaNha, x.SoPhong, x.SucChua, x.DangO, x.ConLai, x.TyLe }));
        VietSheet("Từng phòng", ["Phòng", "Tòa", "Sức chứa", "Đang ở", "Chỗ còn", "Tỷ lệ (%)"],
            vm.PhongTheoSoNguoi.Select(x => new object[] { x.TenPhong, x.ToaNha, x.SucChua, x.DangO, x.ConLai, x.TyLe }));
        VietSheet("SV theo tòa", ["Tòa", "Số sinh viên"],
            vm.SinhVienTheoToa.Select(x => new object[] { x.ToaNha, x.SoSinhVien }));
        VietSheet("SV theo loại phòng", ["Loại phòng", "Số sinh viên"],
            vm.SinhVienTheoLoai.Select(x => new object[] { x.TenLoai, x.SoSinhVien }));
        VietSheet("Đăng ký theo trạng thái", ["Trạng thái", "Số đăng ký"],
            vm.DangKyTheoTrangThai.Select(x => new object[] { Nhan.NhanDangKy(x.TrangThai), x.SoLuong }));
        VietSheet($"Vào KTX {vm.Nam}", ["Tháng", "Số lượt vào", "Số sinh viên"],
            vm.VaoKtxTheoThang.Select(x => new object[] { $"{x.Thang:00}/{x.Nam}", x.SoLuot, x.SoSinhVien }));

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return File(ms.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"ThongKeKTX_{DateTime.Now:yyyyMMdd_HHmm}.xlsx");   // <<< TỰ SỬA nếu muốn đổi tên file xuất
    }

    private async Task<ThongKeViewModel> TaoThongKeAsync(int? namChon)
    {
        // Số liệu phòng: sức chứa và số người đang ở của từng phòng, tính trên CSDL.
        var phong = await PhongTruyVan.KemSoCho(db).OrderBy(x => x.TenPhong).ToListAsync();
        var phongHoatDong = phong.Where(x => x.TrangThai == TrangThaiPhong.DangSuDung).ToList();

        // Số phòng theo loại + tỷ lệ sử dụng theo loại.
        var theoLoai = phongHoatDong
            .GroupBy(x => x.TenLoaiPhong)
            .Select(g => new SoPhongTheoLoai(g.Key, g.Count(), g.Sum(x => x.SucChua), g.Sum(x => x.SoNguoiDangO)))
            .OrderBy(x => x.TenLoai)
            .ToList();

        // Tỷ lệ sử dụng theo tòa.
        var tyLeToa = phongHoatDong
            .GroupBy(x => x.ToaNha)
            .Select(g => new TyLeTheoToa(g.Key, g.Count(), g.Sum(x => x.SucChua), g.Sum(x => x.SoNguoiDangO)))
            .OrderBy(x => x.ToaNha)
            .ToList();

        // Số người đang ở và chỗ còn của từng phòng (nhiều người nhất lên đầu).
        var theoSoNguoi = phong
            .Select(x => new PhongTheoSoNguoi(x.TenPhong, x.ToaNha, x.SucChua, x.SoNguoiDangO, x.SoChoCon))
            .OrderByDescending(x => x.DangO).ThenBy(x => x.TenPhong)
            .ToList();

        // Phòng đông nhất: lấy tất cả phòng bằng số người lớn nhất (có thể hòa).
        var soNguoiMax = theoSoNguoi.Count == 0 ? 0 : theoSoNguoi.Max(x => x.DangO);
        var phongDongNhat = soNguoiMax > 0
            ? theoSoNguoi.Where(x => x.DangO == soNguoiMax).ToList()
            : [];

        // Số sinh viên đang ở theo tòa và theo loại phòng: GroupBy trên truy vấn EF Core.
        var theoToa = await db.PhanPhongs.AsNoTracking()
            .Where(x => x.TrangThai == TrangThaiPhanPhong.DangO)
            .GroupBy(x => x.PhongKTX.ToaNha)
            .Select(g => new SinhVienTheoToa(g.Key, g.Count()))
            .OrderBy(x => x.ToaNha)
            .ToListAsync();

        var theoLoaiSv = await db.PhanPhongs.AsNoTracking()
            .Where(x => x.TrangThai == TrangThaiPhanPhong.DangO)
            .GroupBy(x => x.PhongKTX.LoaiPhong.TenLoaiPhong)
            .Select(g => new SinhVienTheoLoai(g.Key, g.Count()))
            .OrderBy(x => x.TenLoai)
            .ToListAsync();

        var theoTrangThaiDk = await db.DangKyKTXs.AsNoTracking()
            .GroupBy(x => x.TrangThai)
            .Select(g => new DangKyTheoTrangThai(g.Key, g.Count()))
            .OrderBy(x => x.TrangThai)
            .ToListAsync();

        // Vào KTX theo tháng: lấy (ngày vào, mã sinh viên) rồi gom trong bộ nhớ,
        // không phụ thuộc cách EF Core dịch DateOnly sang SQL.
        var luotVao = await db.PhanPhongs.AsNoTracking()
            .Select(x => new { x.NgayVao, x.DangKyKTX.MaSinhVien })
            .ToListAsync();

        var cacNam = luotVao.Select(x => x.NgayVao.Year).Distinct().OrderByDescending(y => y).ToList();
        // <<< TỰ SỬA nếu muốn mặc định là năm hiện tại thay vì năm mới nhất có dữ liệu:
        //     var nam = namChon ?? DateTime.Today.Year;
        var nam = namChon ?? (cacNam.Count > 0 ? cacNam[0] : DateTime.Today.Year);

        var theoThang = Enumerable.Range(1, 12)
            .Select(m =>
            {
                var trongThang = luotVao.Where(x => x.NgayVao.Year == nam && x.NgayVao.Month == m).ToList();
                return new VaoKtxTheoThang(nam, m, trongThang.Count, trongThang.Select(x => x.MaSinhVien).Distinct().Count());
            })
            .ToList();

        // Tổng chỗ = tổng sức chứa phòng đang hoạt động; chỗ đang dùng = số PhanPhong Đang ở.
        var tongCho = phongHoatDong.Sum(x => x.SucChua);
        var choDangSuDung = phong.Sum(x => x.SoNguoiDangO);

        return new ThongKeViewModel
        {
            SoPhongTheoLoai = theoLoai,
            TyLeTheoToa = tyLeToa,
            PhongTheoSoNguoi = theoSoNguoi,
            SinhVienTheoToa = theoToa,
            SinhVienTheoLoai = theoLoaiSv,
            DangKyTheoTrangThai = theoTrangThaiDk,
            VaoKtxTheoThang = theoThang,
            Nam = nam,
            CacNam = cacNam,
            PhongDongNhat = phongDongNhat,
            TongCho = tongCho,
            ChoDangSuDung = choDangSuDung,
            ChoConLai = tongCho - choDangSuDung
        };
    }
}