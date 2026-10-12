// Họ và tên: Vũ Lương Duy
// Mã sinh viên: 23203100014
using DoAnNhom06.Controllers;
using DoAnNhom06.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Filters;

/// <summary>
/// Chặn truy cập theo vai trò. Mỗi request đối chiếu lại với bản ghi TaiKhoan trong CSDL:
/// vai trò và trạng thái khóa lấy từ CSDL, không tin giá trị do trình duyệt gửi.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class RequireRoleAttribute(params string[] vaiTroDuocPhep) : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var session = context.HttpContext.Session;
        var tenDangNhap = session.GetString("UserName");
        if (string.IsNullOrEmpty(tenDangNhap))
        {
            context.Result = new RedirectToActionResult("Welcome", "Account", null);
            return;
        }

        var db = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        var taiKhoan = await db.TaiKhoans.AsNoTracking().Include(x => x.SinhVien)
            .FirstOrDefaultAsync(x => x.TenDangNhap == tenDangNhap);

        // Tài khoản bị khóa hoặc đã bị xóa: hủy phiên ngay, không dùng Session cũ.
        if (taiKhoan is null
            || taiKhoan.TrangThai != TrangThaiChung.DangHoatDong
            || (taiKhoan.SinhVien is not null && taiKhoan.SinhVien.TrangThai != TrangThaiChung.DangHoatDong))
        {
            session.Clear();
            context.Result = new RedirectToActionResult("Welcome", "Account", null);
            return;
        }

        session.SetString("UserRole", taiKhoan.VaiTro);
        session.SetString("HoTen", taiKhoan.HoTen);
        session.SetString("MaTaiKhoan", taiKhoan.MaTaiKhoan.ToString());
        if (taiKhoan.SinhVien is not null) session.SetString("StudentId", taiKhoan.SinhVien.MaSinhVien);
        else session.Remove("StudentId");

        if (vaiTroDuocPhep.Length > 0 && !vaiTroDuocPhep.Contains(taiKhoan.VaiTro))
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            if (context.Controller is Controller controller)
            {
                controller.ViewData["UserName"] = taiKhoan.TenDangNhap;
                controller.ViewData["UserRole"] = taiKhoan.VaiTro;
                controller.ViewData["HoTen"] = taiKhoan.HoTen;
                context.Result = controller.View("AccessDenied");
            }
            else
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
            return;
        }

        await next();
    }
}
