// Họ và tên: Vũ Lương Duy
// Mã sinh viên: 23203100014
using DoAnNhom06.Filters;
using DoAnNhom06.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Controllers;

[RequireRole(VaiTro.Admin)]
public class TaiKhoanController(AppDbContext db) : BaseController
{
    public async Task<IActionResult> Index(string? q, string? vaiTro)
    {
        var truyVan = db.TaiKhoans.AsNoTracking().Include(x => x.SinhVien).AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            var tuKhoa = q.Trim();
            truyVan = truyVan.Where(x => x.TenDangNhap.Contains(tuKhoa) || x.HoTen.Contains(tuKhoa));
        }
        if (!string.IsNullOrWhiteSpace(vaiTro)) truyVan = truyVan.Where(x => x.VaiTro == vaiTro);

        ViewBag.Q = q;
        ViewBag.VaiTro = vaiTro;
        var ds = await truyVan.OrderBy(x => x.VaiTro).ThenBy(x => x.TenDangNhap).ToListAsync();
        return View(ds);
    }

    [HttpGet]
    public IActionResult Create() => View("Form", new TaiKhoanEditViewModel { TrangThai = TrangThaiChung.DangHoatDong });

    /// <summary>Tạo tài khoản quản trị viên. Tài khoản sinh viên được tạo cùng hồ sơ sinh viên.</summary>
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TaiKhoanEditViewModel model)
    {
        model.TenDangNhap = model.TenDangNhap.Trim();
        model.HoTen = model.HoTen.Trim();
        model.Email = model.Email.Trim();
        if (string.IsNullOrWhiteSpace(model.MatKhau))
            ModelState.AddModelError(nameof(model.MatKhau), "Tài khoản mới cần mật khẩu.");
        else if (model.MatKhau.Length < 6)
            ModelState.AddModelError(nameof(model.MatKhau), "Mật khẩu cần ít nhất 6 ký tự.");
        await KiemTraTrungTenAsync(model, 0);
        if (!ModelState.IsValid) return View("Form", model);

        db.TaiKhoans.Add(new TaiKhoan
        {
            TenDangNhap = model.TenDangNhap,
            MatKhau = model.MatKhau!,
            HoTen = model.HoTen,
            Email = model.Email,
            VaiTro = VaiTro.Admin,
            TrangThai = model.TrangThai == TrangThaiChung.Khoa ? TrangThaiChung.Khoa : TrangThaiChung.DangHoatDong
        });
        await db.SaveChangesAsync();
        TempData["ThongBao"] = $"Đã tạo tài khoản quản trị {model.TenDangNhap}.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var tk = await db.TaiKhoans.AsNoTracking().FirstOrDefaultAsync(x => x.MaTaiKhoan == id);
        if (tk is null) return NotFound();
        return View("Form", new TaiKhoanEditViewModel
        {
            MaTaiKhoan = tk.MaTaiKhoan,
            TenDangNhap = tk.TenDangNhap,
            HoTen = tk.HoTen,
            Email = tk.Email,
            TrangThai = tk.TrangThai
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TaiKhoanEditViewModel model)
    {
        var tk = await db.TaiKhoans.Include(x => x.SinhVien).FirstOrDefaultAsync(x => x.MaTaiKhoan == id);
        if (tk is null) return NotFound();

        model.MaTaiKhoan = id;
        model.TenDangNhap = model.TenDangNhap.Trim();
        model.HoTen = model.HoTen.Trim();
        model.Email = model.Email.Trim();
        if (!string.IsNullOrWhiteSpace(model.MatKhau) && model.MatKhau.Length < 6)
            ModelState.AddModelError(nameof(model.MatKhau), "Mật khẩu cần ít nhất 6 ký tự.");
        if (model.TrangThai == TrangThaiChung.Khoa && tk.TenDangNhap == UserName)
            ModelState.AddModelError(nameof(model.TrangThai), "Không thể khóa tài khoản đang đăng nhập.");
        await KiemTraTrungTenAsync(model, id);
        if (!ModelState.IsValid) return View("Form", model);

        tk.TenDangNhap = model.TenDangNhap;
        tk.HoTen = model.HoTen;
        tk.Email = model.Email;
        tk.TrangThai = model.TrangThai == TrangThaiChung.Khoa ? TrangThaiChung.Khoa : TrangThaiChung.DangHoatDong;
        if (!string.IsNullOrWhiteSpace(model.MatKhau)) tk.MatKhau = model.MatKhau;

        // Đồng bộ họ tên và email với hồ sơ sinh viên (nếu tài khoản thuộc sinh viên).
        if (tk.SinhVien is not null)
        {
            tk.SinhVien.HoTen = tk.HoTen;
            tk.SinhVien.Email = tk.Email;
        }
        await db.SaveChangesAsync();
        TempData["ThongBao"] = $"Đã cập nhật tài khoản {tk.TenDangNhap}.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var tk = await db.TaiKhoans.FirstOrDefaultAsync(x => x.MaTaiKhoan == id);
        if (tk is null) return NotFound();

        if (tk.TenDangNhap == UserName)
        {
            TempData["Loi"] = "Không thể xóa tài khoản đang đăng nhập.";
            return RedirectToAction(nameof(Index));
        }
        // Tài khoản sinh viên gắn với lịch sử đăng ký và phân phòng: chỉ khóa, không xóa.
        if (tk.VaiTro == VaiTro.SinhVien)
        {
            TempData["Loi"] = "Không xóa tài khoản sinh viên để giữ lịch sử ở KTX. Hãy khóa tài khoản thay thế.";
            return RedirectToAction(nameof(Index));
        }

        db.TaiKhoans.Remove(tk);
        await db.SaveChangesAsync();
        TempData["ThongBao"] = $"Đã xóa tài khoản {tk.TenDangNhap}.";
        return RedirectToAction(nameof(Index));
    }

    private async Task KiemTraTrungTenAsync(TaiKhoanEditViewModel model, int id)
    {
        if (await db.TaiKhoans.AnyAsync(x => x.TenDangNhap == model.TenDangNhap && x.MaTaiKhoan != id))
            ModelState.AddModelError(nameof(model.TenDangNhap), "Tên đăng nhập đã tồn tại.");
    }
}
