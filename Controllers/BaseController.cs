// Họ và tên: Vũ Lương Duy
// Mã sinh viên: 23203100014
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DoAnNhom06.Controllers;

public abstract class BaseController : Controller
{
    protected string? UserName => HttpContext.Session.GetString("UserName");
    protected string? UserRole => HttpContext.Session.GetString("UserRole");
    protected string? HoTen => HttpContext.Session.GetString("HoTen");

    /// <summary>Mã sinh viên của phiên hiện tại. Chỉ lấy từ tài khoản đăng nhập, không lấy từ URL.</summary>
    protected string? StudentId => HttpContext.Session.GetString("StudentId");

    protected bool SignedIn => !string.IsNullOrEmpty(UserName);
    protected bool LaAdmin => UserRole == Models.VaiTro.Admin;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        ViewBag.UserName = UserName;
        ViewBag.UserRole = UserRole;
        ViewBag.HoTen = HoTen;
        base.OnActionExecuting(context);
    }
}
