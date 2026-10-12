// Họ và tên: Nguyễn Bá Hùng
// Mã sinh viên: 23274800011
using DoAnNhom06.Filters;
using DoAnNhom06.Models;
using DoAnNhom06.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Controllers;

[RequireRole(VaiTro.Admin)]
public class ThongKeController(AppDbContext db) : BaseController
{
    public async Task<IActionResult> Index()
    {
        // Số liệu phòng: sức chứa và số người đang ở của từng phòng, tính trên CSDL.
        var phong = await PhongTruyVan.KemSoCho(db).OrderBy(x => x.TenPhong).ToListAsync();

        var theoLoai = phong
            .Where(x => x.TrangThai == TrangThaiPhong.DangSuDung)
            .GroupBy(x => x.TenLoaiPhong)
            .Select(g => new SoPhongTheoLoai(g.Key, g.Count(), g.Sum(x => x.SucChua), g.Sum(x => x.SoNguoiDangO)))
            .OrderBy(x => x.TenLoai)
            .ToList();

        var theoSoNguoi = phong
            .Select(x => new PhongTheoSoNguoi(x.TenPhong, x.ToaNha, x.SucChua, x.SoNguoiDangO, x.SoChoCon))
            .OrderByDescending(x => x.DangO).ThenBy(x => x.TenPhong)
            .ToList();

        // Số sinh viên đang ở theo tòa và theo loại phòng: GroupBy trên truy vấn EF Core.
        var theoToa = await db.PhanPhongs.AsNoTracking()
            .Where(x => x.TrangThai == TrangThaiPhanPhong.DangO)
            .GroupBy(x => x.PhongKTX.ToaNha)
            .OrderBy(g => g.Key)
            .Select(g => new SinhVienTheoToa(g.Key, g.Count()))
            .ToListAsync();

        var theoLoaiSv = await db.PhanPhongs.AsNoTracking()
            .Where(x => x.TrangThai == TrangThaiPhanPhong.DangO)
            .GroupBy(x => x.PhongKTX.LoaiPhong.TenLoaiPhong)
            .OrderBy(g => g.Key)
            .Select(g => new SinhVienTheoLoai(g.Key, g.Count()))
            .ToListAsync();

        // Luôn hiện đủ 4 trạng thái; trạng thái chưa có đăng ký nào thì số lượng là 0.
        var demDangKy = await db.DangKyKTXs.AsNoTracking()
            .GroupBy(x => x.TrangThai)
            .Select(g => new { TrangThai = g.Key, SoLuong = g.Count() })
            .ToDictionaryAsync(x => x.TrangThai, x => x.SoLuong);
        string[] cacTrangThaiDk =
        [
            TrangThaiDangKy.ChoDuyet,
            TrangThaiDangKy.DaPhanPhong,
            TrangThaiDangKy.DaTraPhong,
            TrangThaiDangKy.DaHuy
        ];
        var theoTrangThaiDk = cacTrangThaiDk
            .Select(t => new DangKyTheoTrangThai(t, demDangKy.GetValueOrDefault(t)))
            .ToList();

        // Gom theo tháng trên bộ nhớ: không phụ thuộc cách dịch DateOnly sang SQL.
        // Mỗi đăng ký chỉ tính một lần, vào ngày vào đầu tiên; bản ghi tạo do chuyển phòng không tính là vào KTX thêm.
        var cacLuotPhan = await db.PhanPhongs.AsNoTracking()
            .Select(x => new { x.MaDangKy, x.NgayVao })
            .ToListAsync();
        var ngayVao = cacLuotPhan
            .GroupBy(x => x.MaDangKy)
            .Select(g => g.Min(x => x.NgayVao))
            .ToList();
        var theoThang = ngayVao
            .GroupBy(d => new { d.Year, d.Month })
            .Select(g => new VaoKtxTheoThang(g.Key.Year, g.Key.Month, g.Count()))
            .OrderBy(x => x.Nam).ThenBy(x => x.Thang)
            .ToList();

        var tongCho = phong.Where(x => x.TrangThai == TrangThaiPhong.DangSuDung).Sum(x => x.SucChua);
        var choDangSuDung = phong.Sum(x => x.SoNguoiDangO);

        var vm = new ThongKeViewModel
        {
            SoPhongTheoLoai = theoLoai,
            PhongTheoSoNguoi = theoSoNguoi,
            SinhVienTheoToa = theoToa,
            SinhVienTheoLoai = theoLoaiSv,
            DangKyTheoTrangThai = theoTrangThaiDk,
            VaoKtxTheoThang = theoThang,
            PhongDongNhat = theoSoNguoi.FirstOrDefault(x => x.DangO > 0),
            TongCho = tongCho,
            ChoDangSuDung = choDangSuDung,
            ChoConLai = tongCho - choDangSuDung
        };
        return View(vm);
    }
}