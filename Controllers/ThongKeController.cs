// Họ và tên: Vũ Lương Duy
// Mã sinh viên: 23203100014
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

        // Gom theo tháng trên bộ nhớ sau khi lấy ngày vào: đơn giản và không phụ thuộc cách dịch DateOnly sang SQL.
        var ngayVao = await db.PhanPhongs.AsNoTracking().Select(x => x.NgayVao).ToListAsync();
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
