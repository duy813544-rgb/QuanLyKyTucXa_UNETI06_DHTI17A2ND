// Họ và tên: Vũ Lương Duy
// Mã sinh viên: 23203100014
using DoAnNhom06.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DoAnNhom06.Controllers;

public abstract class BaseController : Controller
{
    protected string? UserName => HttpContext.Session.GetString("UserName");
    protected string? UserRole => HttpContext.Session.GetString("UserRole");
    protected string? HoTen => HttpContext.Session.GetString("HoTen");
    protected string? StudentId => HttpContext.Session.GetString("StudentId");

    protected bool SignedIn => !string.IsNullOrEmpty(UserName);
    protected bool LaAdmin => UserRole == VaiTro.Admin;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        ViewBag.UserName = UserName;
        ViewBag.UserRole = UserRole;
        ViewBag.HoTen = HoTen;

        // Sinh viên đang có đăng ký còn hiệu lực (chờ duyệt hoặc đã phân phòng)
        if (!LaAdmin && !string.IsNullOrEmpty(StudentId))
        {
            var db = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
            ViewBag.CoDangKyHieuLuc = db.DangKyKTXs.AsNoTracking().Any(x =>
                x.MaSinhVien == StudentId &&
                (x.TrangThai == TrangThaiDangKy.ChoDuyet || x.TrangThai == TrangThaiDangKy.DaPhanPhong));
        }
        else
        {
            ViewBag.CoDangKyHieuLuc = false;
        }

        base.OnActionExecuting(context);
    }
}