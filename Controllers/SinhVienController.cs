// Họ và tên: Vũ Lương Duy
// Mã sinh viên: 23203100014
using DoAnNhom06.Filters;
using DoAnNhom06.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Controllers;

[RequireRole(VaiTro.Admin)]
public class SinhVienController(AppDbContext db) : BaseController
{
    public async Task<IActionResult> Index(string? q, string? trangThai)
    {
        var truyVan = db.SinhViens.AsNoTracking().Include(x => x.TaiKhoan).AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            var tuKhoa = q.Trim();
            truyVan = truyVan.Where(x => x.MaSinhVien.Contains(tuKhoa) || x.HoTen.Contains(tuKhoa));
        }
        if (!string.IsNullOrWhiteSpace(trangThai)) truyVan = truyVan.Where(x => x.TrangThai == trangThai);

        ViewBag.Q = q;
        ViewBag.TrangThai = trangThai;
        return View(await truyVan.OrderBy(x => x.MaSinhVien).ToListAsync());
    }

    public async Task<IActionResult> Details(string id)
    {
        var sv = await db.SinhViens.AsNoTracking().Include(x => x.TaiKhoan)
            .FirstOrDefaultAsync(x => x.MaSinhVien == id);
        if (sv is null) return NotFound();

        var lichSuDangKy = await db.DangKyKTXs.AsNoTracking().Include(x => x.NguyenVong)
            .Where(x => x.MaSinhVien == id).OrderByDescending(x => x.MaDangKy).ToListAsync();
        var lichSuPhong = await db.PhanPhongs.AsNoTracking()
            .Include(x => x.PhongKTX).ThenInclude(x => x.LoaiPhong)
            .Include(x => x.DangKyKTX)
            .Where(x => x.DangKyKTX.MaSinhVien == id)
            .OrderByDescending(x => x.NgayVao).ThenByDescending(x => x.MaPhanPhong)
            .ToListAsync();

        return View(new SinhVienHomeViewModel
        {
            SinhVien = sv,
            TaiKhoan = sv.TaiKhoan,
            DangKyChoDuyet = lichSuDangKy.FirstOrDefault(x => x.TrangThai == TrangThaiDangKy.ChoDuyet),
            DangKyHienTai = lichSuDangKy.FirstOrDefault(x => x.TrangThai == TrangThaiDangKy.DaPhanPhong),
            PhongHienTai = lichSuPhong.FirstOrDefault(x => x.TrangThai == TrangThaiPhanPhong.DangO),
            LichSuDangKy = lichSuDangKy,
            LichSuPhong = lichSuPhong
        });
    }

    [HttpGet]
    public IActionResult Create() => View("Form", new SinhVienEditViewModel { LaTaoMoi = true, GioiTinh = GioiTinh.Nam });

    /// <summary>Tạo hồ sơ sinh viên và tài khoản đăng nhập trong một giao dịch (quan hệ 1-1).</summary>
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SinhVienEditViewModel model)
    {
        model.LaTaoMoi = true;
        model.MaSinhVien = model.MaSinhVien.Trim();
        model.HoTen = model.HoTen.Trim();
        model.Email = model.Email.Trim();
        model.TenDangNhap = string.IsNullOrWhiteSpace(model.TenDangNhap) ? model.MaSinhVien : model.TenDangNhap.Trim();

        if (await db.SinhViens.AnyAsync(x => x.MaSinhVien == model.MaSinhVien))
            ModelState.AddModelError(nameof(model.MaSinhVien), "Mã sinh viên đã tồn tại.");
        if (await db.TaiKhoans.AnyAsync(x => x.TenDangNhap == model.TenDangNhap))
            ModelState.AddModelError(nameof(model.TenDangNhap), "Tên đăng nhập đã tồn tại.");
        if (string.IsNullOrWhiteSpace(model.MatKhau) || model.MatKhau.Length < 6)
            ModelState.AddModelError(nameof(model.MatKhau), "Mật khẩu ban đầu cần ít nhất 6 ký tự.");
        KiemTraNgaySinh(model);
        if (!ModelState.IsValid) return View("Form", model);

        await using var giaoDich = await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        db.SinhViens.Add(new SinhVien
        {
            MaSinhVien = model.MaSinhVien,
            HoTen = model.HoTen,
            NgaySinh = model.NgaySinh!.Value,
            GioiTinh = model.GioiTinh,
            Lop = model.Lop.Trim(),
            Khoa = model.Khoa.Trim(),
            SoDienThoai = model.SoDienThoai.Trim(),
            Email = model.Email,
            DiaChi = model.DiaChi?.Trim(),
            TrangThai = TrangThaiChung.DangHoatDong,
            TaiKhoan = new TaiKhoan
            {
                TenDangNhap = model.TenDangNhap!,
                MatKhau = model.MatKhau!,
                HoTen = model.HoTen,
                Email = model.Email,
                VaiTro = VaiTro.SinhVien,
                TrangThai = TrangThaiChung.DangHoatDong
            }
        });
        await db.SaveChangesAsync();
        await giaoDich.CommitAsync();

        TempData["ThongBao"] = $"Đã thêm sinh viên {model.MaSinhVien}. Tài khoản đăng nhập: {model.TenDangNhap}.";
        return RedirectToAction(nameof(Details), new { id = model.MaSinhVien });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var sv = await db.SinhViens.AsNoTracking().FirstOrDefaultAsync(x => x.MaSinhVien == id);
        if (sv is null) return NotFound();
        return View("Form", new SinhVienEditViewModel
        {
            LaTaoMoi = false,
            MaSinhVien = sv.MaSinhVien,
            HoTen = sv.HoTen,
            NgaySinh = sv.NgaySinh,
            GioiTinh = sv.GioiTinh,
            Lop = sv.Lop,
            Khoa = sv.Khoa,
            SoDienThoai = sv.SoDienThoai,
            Email = sv.Email,
            DiaChi = sv.DiaChi,
            TrangThai = sv.TrangThai
        });
    }

    /// <summary>Mã sinh viên là khóa chính, không đổi khi sửa. Tài khoản đăng nhập quản lý ở TaiKhoanController.</summary>
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, SinhVienEditViewModel model)
    {
        var sv = await db.SinhViens.Include(x => x.TaiKhoan).FirstOrDefaultAsync(x => x.MaSinhVien == id);
        if (sv is null) return NotFound();

        model.LaTaoMoi = false;
        model.MaSinhVien = id;
        model.HoTen = model.HoTen.Trim();
        model.Email = model.Email.Trim();
        KiemTraNgaySinh(model);
        if (!ModelState.IsValid) return View("Form", model);

        sv.HoTen = model.HoTen;
        sv.NgaySinh = model.NgaySinh!.Value;
        sv.GioiTinh = model.GioiTinh;
        sv.Lop = model.Lop.Trim();
        sv.Khoa = model.Khoa.Trim();
        sv.SoDienThoai = model.SoDienThoai.Trim();
        sv.Email = model.Email;
        sv.DiaChi = model.DiaChi?.Trim();
        sv.TrangThai = model.TrangThai == TrangThaiChung.Khoa ? TrangThaiChung.Khoa : TrangThaiChung.DangHoatDong;
        sv.TaiKhoan.HoTen = sv.HoTen;
        sv.TaiKhoan.Email = sv.Email;
        await db.SaveChangesAsync();

        TempData["ThongBao"] = $"Đã cập nhật hồ sơ sinh viên {sv.MaSinhVien}.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var sv = await db.SinhViens.Include(x => x.TaiKhoan).FirstOrDefaultAsync(x => x.MaSinhVien == id);
        if (sv is null) return NotFound();

        // Đã có đăng ký hoặc phân phòng thì không xóa, để giữ lịch sử. Có thể khóa hồ sơ thay thế.
        if (await db.DangKyKTXs.AnyAsync(x => x.MaSinhVien == id))
        {
            TempData["Loi"] = $"Không thể xóa sinh viên {id} vì đã có lịch sử đăng ký KTX. Hãy đổi trạng thái sang Khóa.";
            return RedirectToAction(nameof(Details), new { id });
        }

        db.TaiKhoans.Remove(sv.TaiKhoan);
        db.SinhViens.Remove(sv);
        await db.SaveChangesAsync();
        TempData["ThongBao"] = $"Đã xóa sinh viên {id} và tài khoản liên quan.";
        return RedirectToAction(nameof(Index));
    }

    private void KiemTraNgaySinh(SinhVienEditViewModel model)
    {
        if (model.NgaySinh is { } ngay && ngay >= DateOnly.FromDateTime(DateTime.Now))
            ModelState.AddModelError(nameof(model.NgaySinh), "Ngày sinh phải trước ngày hôm nay.");
    }
}
