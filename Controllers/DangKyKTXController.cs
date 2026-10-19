// Họ và tên: Nguyễn Bá Hùng
// Mã sinh viên: 23274800011
// Nội dung thực hiện: Module 4- Quản lý đăng ký ở KTX:    
using System.Data;
using DoAnNhom06.Filters;
using DoAnNhom06.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Controllers;

[RequireRole(VaiTro.Admin, VaiTro.SinhVien)]
public class DangKyKTXController(AppDbContext db) : BaseController
{

    public async Task<IActionResult> Index(string? trangThai, string? q)
    {
        if (!LaAdmin)
        {
            var cuaToi = await db.DangKyKTXs.AsNoTracking().Include(x => x.NguyenVong)
                .Where(x => x.MaSinhVien == StudentId)
                .OrderByDescending(x => x.MaDangKy)
                .ToListAsync();
            return View("MyIndex", cuaToi);
        }

        var truyVan = db.DangKyKTXs.AsNoTracking().Include(x => x.SinhVien).Include(x => x.NguyenVong).AsQueryable();
        if (!string.IsNullOrWhiteSpace(trangThai)) truyVan = truyVan.Where(x => x.TrangThai == trangThai);
        if (!string.IsNullOrWhiteSpace(q))
        {
            var tuKhoa = q.Trim();
            truyVan = truyVan.Where(x => x.MaSinhVien.Contains(tuKhoa) || x.SinhVien.HoTen.Contains(tuKhoa));
        }
        ViewBag.TrangThai = trangThai;
        ViewBag.Q = q;
        return View(await truyVan.OrderByDescending(x => x.MaDangKy).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var dk = await db.DangKyKTXs.AsNoTracking()
            .Include(x => x.SinhVien).Include(x => x.NguyenVong)
            .Include(x => x.PhanPhongs).ThenInclude(x => x.PhongKTX).ThenInclude(x => x.LoaiPhong)
            .FirstOrDefaultAsync(x => x.MaDangKy == id);
        if (dk is null) return NotFound();


        if (!LaAdmin && dk.MaSinhVien != StudentId) return NotFound();
        return View(dk);
    }

    [HttpGet, RequireRole(VaiTro.SinhVien)]
    public async Task<IActionResult> Create()
    {
        if ((bool)(ViewBag.CoDangKyHieuLuc ?? false))
        {
            TempData["Loi"] = "Bạn đã có đăng ký đang hiệu lực, không thể đăng ký thêm.";
            return RedirectToAction(nameof(Index));
        }

        var homNay = DateOnly.FromDateTime(DateTime.Now);
        await NapDanhSachLoaiAsync();
        return View(new DangKyKTXCreateViewModel { NgayBatDau = homNay, NgayKetThucDuKien = homNay.AddMonths(6) });
    }

    [HttpPost, ValidateAntiForgeryToken, RequireRole(VaiTro.SinhVien)]
    public async Task<IActionResult> Create(DangKyKTXCreateViewModel model)
    {
        var maSv = StudentId;
        if (maSv is null) return RedirectToAction("Login", "Account");

        var sinhVien = await db.SinhViens.AsNoTracking().FirstOrDefaultAsync(x => x.MaSinhVien == maSv);
        if (sinhVien is null || sinhVien.TrangThai != TrangThaiChung.DangHoatDong)
            ModelState.AddModelError(string.Empty, "Hồ sơ sinh viên không tồn tại hoặc đã bị khóa.");

        var homNay = DateOnly.FromDateTime(DateTime.Now);
        if (model.NgayBatDau is { } batDau)
        {
            if (batDau < homNay) ModelState.AddModelError(nameof(model.NgayBatDau), "Ngày bắt đầu không được trước hôm nay.");
            if (model.NgayKetThucDuKien is { } ketThuc && ketThuc <= batDau)
                ModelState.AddModelError(nameof(model.NgayKetThucDuKien), "Ngày kết thúc dự kiến phải sau ngày bắt đầu.");
        }
        if (model.NguyenVongLoaiPhong is { } nv
            && !await db.LoaiPhongs.AnyAsync(x => x.MaLoaiPhong == nv && x.TrangThai == TrangThaiLoaiPhong.DangSuDung))
            ModelState.AddModelError(nameof(model.NguyenVongLoaiPhong), "Loại phòng nguyện vọng không hợp lệ.");

        if (await db.DangKyKTXs.AnyAsync(x => x.MaSinhVien == maSv && x.TrangThai == TrangThaiDangKy.ChoDuyet))
            ModelState.AddModelError(string.Empty, "Bạn đang có một đơn chờ duyệt. Hãy chờ duyệt hoặc hủy đơn đó trước.");
        if (await db.DangKyKTXs.AnyAsync(x => x.MaSinhVien == maSv && x.TrangThai == TrangThaiDangKy.DaPhanPhong)
            || await db.PhanPhongs.AnyAsync(x => x.TrangThai == TrangThaiPhanPhong.DangO && x.DangKyKTX.MaSinhVien == maSv))
            ModelState.AddModelError(string.Empty, "Bạn đang ở KTX theo đăng ký đã được phân phòng, không thể gửi thêm đơn.");

        if (!ModelState.IsValid)
        {
            await NapDanhSachLoaiAsync();
            return View(model);
        }

        try
        {
            db.DangKyKTXs.Add(new DangKyKTX
            {
                MaSinhVien = maSv,
                NgayDangKy = homNay, 
                NgayBatDau = model.NgayBatDau!.Value,
                NgayKetThucDuKien = model.NgayKetThucDuKien!.Value,
                NguyenVongLoaiPhong = model.NguyenVongLoaiPhong,
                GhiChu = model.GhiChu?.Trim(),
                TrangThai = TrangThaiDangKy.ChoDuyet
            });
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {

            ModelState.AddModelError(string.Empty, "Không thể gửi đơn: " + ex.GetBaseException().Message);
            await NapDanhSachLoaiAsync();
            return View(model);
        }

        TempData["ThongBao"] = "Đã gửi đơn đăng ký ở KTX. Đơn đang chờ quản trị viên duyệt.";
        return RedirectToAction(nameof(Index));
    }


    [HttpPost, ValidateAntiForgeryToken, RequireRole(VaiTro.SinhVien)]
    public async Task<IActionResult> Cancel(int id)
    {
        var dk = await db.DangKyKTXs.FirstOrDefaultAsync(x => x.MaDangKy == id && x.MaSinhVien == StudentId);
        if (dk is null) return NotFound();
        if (dk.TrangThai != TrangThaiDangKy.ChoDuyet)
        {
            TempData["Loi"] = "Chỉ có thể hủy đơn đang chờ duyệt.";
            return RedirectToAction(nameof(Index));
        }
        dk.TrangThai = TrangThaiDangKy.DaHuy;
        await db.SaveChangesAsync();
        TempData["ThongBao"] = "Đã hủy đơn đăng ký.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken, RequireRole(VaiTro.Admin)]
    public async Task<IActionResult> Reject(int id)
    {
        var dk = await db.DangKyKTXs.FirstOrDefaultAsync(x => x.MaDangKy == id);
        if (dk is null) return NotFound();
        if (dk.TrangThai != TrangThaiDangKy.ChoDuyet)
        {
            TempData["Loi"] = "Chỉ có thể từ chối đơn đang chờ duyệt.";
            return RedirectToAction(nameof(Details), new { id });
        }
        dk.TrangThai = TrangThaiDangKy.DaHuy;
        await db.SaveChangesAsync();
        TempData["ThongBao"] = $"Đã từ chối đơn đăng ký #{id}.";
        return RedirectToAction(nameof(Details), new { id });
    }

    private async Task NapDanhSachLoaiAsync() =>
        ViewBag.LoaiPhongs = await db.LoaiPhongs.AsNoTracking()
            .Where(x => x.TrangThai == TrangThaiLoaiPhong.DangSuDung)
            .OrderBy(x => x.TenLoaiPhong).ToListAsync();

}
