// Họ và tên: Nguyễn Bá Hùng
// Mã sinh viên: 23274800011
// Nội dung thực hiện: Dashboard và thống kê bằng LINQ (Module 4):  
using DoAnNhom06.Filters;
using DoAnNhom06.Models;
using DoAnNhom06.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Controllers;

public class HomeController(AppDbContext db) : BaseController
{
    [RequireRole(VaiTro.Admin, VaiTro.SinhVien)]
    public async Task<IActionResult> Index()
    {
        if (UserRole == VaiTro.SinhVien) return await TrangSinhVienAsync();
        return await DashboardAsync();
    }

    private async Task<IActionResult> DashboardAsync()
    {

        var tongCho = await db.PhongKTXs
            .Where(p => p.TrangThai == TrangThaiPhong.DangSuDung)
            .SumAsync(p => p.LoaiPhong.SoNguoiToiDa);
        var dangO = await db.PhanPhongs.CountAsync(pp => pp.TrangThai == TrangThaiPhanPhong.DangO);

        var vm = new DashboardViewModel
        {
            TongPhong = await db.PhongKTXs.CountAsync(),
            TongSinhVien = await db.SinhViens.CountAsync(),
            TongDangKy = await db.DangKyKTXs.CountAsync(),
            DangKyChoDuyet = await db.DangKyKTXs.CountAsync(x => x.TrangThai == TrangThaiDangKy.ChoDuyet),
            SinhVienDangO = dangO,
            TongCho = tongCho,
            ChoDangSuDung = dangO,
            ChoConTrong = tongCho - dangO,
            PhongDayDu = await PhongTruyVan.KemSoCho(db)
                .CountAsync(x => x.TrangThai == TrangThaiPhong.DangSuDung && x.SucChua - x.SoNguoiDangO <= 0)
        };
        return View("Index", vm);
    }

    private async Task<IActionResult> TrangSinhVienAsync()
    {

        if (StudentId is null) return RedirectToAction(nameof(AccountController.Login), "Account");

        var sinhVien = await db.SinhViens.AsNoTracking().Include(x => x.TaiKhoan)
            .FirstOrDefaultAsync(x => x.MaSinhVien == StudentId);
        if (sinhVien is null) return RedirectToAction(nameof(AccountController.Login), "Account");

        var lichSu = await db.DangKyKTXs.AsNoTracking().Include(x => x.NguyenVong)
            .Where(x => x.MaSinhVien == StudentId)
            .OrderByDescending(x => x.MaDangKy)
            .ToListAsync();

        var dangDuyet = lichSu.FirstOrDefault(x => x.TrangThai == TrangThaiDangKy.ChoDuyet);
        var dangO = lichSu.FirstOrDefault(x => x.TrangThai == TrangThaiDangKy.DaPhanPhong);
        PhanPhong? phongHienTai = null;
        if (dangO is not null)
        {
            phongHienTai = await db.PhanPhongs.AsNoTracking()
                .Include(x => x.PhongKTX).ThenInclude(x => x.LoaiPhong)
                .FirstOrDefaultAsync(x => x.MaDangKy == dangO.MaDangKy && x.TrangThai == TrangThaiPhanPhong.DangO);
        }

        return View("Student", new SinhVienHomeViewModel
        {
            SinhVien = sinhVien,
            TaiKhoan = sinhVien.TaiKhoan,
            DangKyChoDuyet = dangDuyet,
            DangKyHienTai = dangO,
            PhongHienTai = phongHienTai,
            LichSuDangKy = lichSu
        });
    }

    public IActionResult Error() => View();
}
