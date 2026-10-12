// Họ và tên: Vũ Lương Duy
// Mã sinh viên: 23203100014
using DoAnNhom06.Filters;
using DoAnNhom06.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Controllers;

public class AccountController(AppDbContext db) : BaseController
{
    /// <summary>Trang chủ giới thiệu: người chưa đăng nhập chọn Đăng nhập hoặc Đăng ký.</summary>
    [HttpGet]
    public IActionResult Welcome() => SignedIn ? RedirectToAction("Index", "Home") : View();

    [HttpGet]   

    public IActionResult Login(string? returnUrl = null)
    {
        if (SignedIn) return RedirectToAction("Index", "Home");
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
    {
        ViewBag.Username = username?.Trim();
        ViewBag.ReturnUrl = returnUrl;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
        {
            ModelState.AddModelError(string.Empty, "Vui lòng nhập tên đăng nhập và mật khẩu.");
            return View();
        }

        var tenDangNhap = username.Trim();
        var taiKhoan = await db.TaiKhoans.AsNoTracking().Include(x => x.SinhVien)
            .FirstOrDefaultAsync(x => x.TenDangNhap == tenDangNhap);

        // Thông báo chung khi sai tài khoản hoặc sai mật khẩu, để không cho dò tên đăng nhập.
        if (taiKhoan is null || taiKhoan.MatKhau != password)
        {
            ModelState.AddModelError(string.Empty, "Sai tên đăng nhập hoặc mật khẩu.");
            return View();
        }
        if (taiKhoan.TrangThai != TrangThaiChung.DangHoatDong)
        {
            ModelState.AddModelError(string.Empty, "Tài khoản đã bị khóa. Vui lòng liên hệ quản trị viên.");
            return View();
        }
        if (taiKhoan.VaiTro == VaiTro.SinhVien)
        {
            if (taiKhoan.SinhVien is null)
            {
                ModelState.AddModelError(string.Empty, "Tài khoản sinh viên chưa được liên kết với hồ sơ sinh viên.");
                return View();
            }
            if (taiKhoan.SinhVien.TrangThai != TrangThaiChung.DangHoatDong)
            {
                ModelState.AddModelError(string.Empty, "Hồ sơ sinh viên đã bị khóa.");
                return View();
            }
        }

        HttpContext.Session.Clear();
        HttpContext.Session.SetString("UserName", taiKhoan.TenDangNhap);
        HttpContext.Session.SetString("UserRole", taiKhoan.VaiTro);
        HttpContext.Session.SetString("HoTen", taiKhoan.HoTen);
        HttpContext.Session.SetString("MaTaiKhoan", taiKhoan.MaTaiKhoan.ToString());
        if (taiKhoan.SinhVien is not null) HttpContext.Session.SetString("StudentId", taiKhoan.SinhVien.MaSinhVien);

        return Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl!) : RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register() => SignedIn ? RedirectToAction("Index", "Home") : View(new DangKyTaiKhoanViewModel());

    /// <summary>Sinh viên tự tạo tài khoản: tạo TaiKhoan và SinhVien trong một giao dịch.</summary>
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(DangKyTaiKhoanViewModel model)
    {
        if (SignedIn) return RedirectToAction("Index", "Home");
        model.TenDangNhap = model.TenDangNhap.Trim();
        model.MaSinhVien = model.MaSinhVien.Trim();
        model.Email = model.Email.Trim();
        model.HoTen = model.HoTen.Trim();

        if (await db.TaiKhoans.AnyAsync(x => x.TenDangNhap == model.TenDangNhap))
            ModelState.AddModelError(nameof(model.TenDangNhap), "Tên đăng nhập đã được sử dụng.");
        if (await db.SinhViens.AnyAsync(x => x.MaSinhVien == model.MaSinhVien))
            ModelState.AddModelError(nameof(model.MaSinhVien), "Mã sinh viên đã có hồ sơ. Hãy liên hệ quản trị viên để được hỗ trợ.");
        if (!ModelState.IsValid) return View(model);

        await using var giaoDich = await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        var taiKhoan = new TaiKhoan
        {
            TenDangNhap = model.TenDangNhap,
            MatKhau = model.MatKhau,
            HoTen = model.HoTen,
            Email = model.Email,
            VaiTro = VaiTro.SinhVien,
            TrangThai = TrangThaiChung.DangHoatDong
        };
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
            TrangThai = TrangThaiChung.DangHoatDong,
            TaiKhoan = taiKhoan
        });
        await db.SaveChangesAsync();
        await giaoDich.CommitAsync();

        TempData["ThongBao"] = "Tạo tài khoản thành công. Hãy đăng nhập để gửi đăng ký ở ký túc xá.";
        return RedirectToAction(nameof(Login));
    }

    /// <summary>Thông tin cá nhân đầy đủ, trả về dạng khung nhỏ để hiện trong hộp thoại.</summary>
    [HttpGet]
    [RequireRole(VaiTro.Admin, VaiTro.SinhVien)]
    public async Task<IActionResult> Profile()
    {
        var taiKhoan = await db.TaiKhoans.AsNoTracking().Include(x => x.SinhVien)
            .FirstOrDefaultAsync(x => x.TenDangNhap == UserName);
        return taiKhoan is null ? NotFound() : PartialView("_Profile", taiKhoan);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Welcome));
    }

    [HttpGet]
    [RequireRole(VaiTro.Admin, VaiTro.SinhVien)]
    public IActionResult ChangePassword() => View();

    [HttpPost, ValidateAntiForgeryToken]
    [RequireRole(VaiTro.Admin, VaiTro.SinhVien)]
    public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
    {
        var taiKhoan = await db.TaiKhoans.FirstOrDefaultAsync(x => x.TenDangNhap == UserName);
        if (taiKhoan is null || taiKhoan.MatKhau != currentPassword)
            ModelState.AddModelError(string.Empty, "Mật khẩu hiện tại không đúng.");
        if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
            ModelState.AddModelError(nameof(newPassword), "Mật khẩu mới cần ít nhất 6 ký tự.");
        if (newPassword != confirmPassword)
            ModelState.AddModelError(nameof(confirmPassword), "Mật khẩu xác nhận không khớp.");
        if (!ModelState.IsValid) return View();

        taiKhoan!.MatKhau = newPassword;
        await db.SaveChangesAsync();
        TempData["ThongBao"] = "Đổi mật khẩu thành công.";
        return RedirectToAction("Index", "Home");
    }
   

}
