// Họ và tên: Vũ Lương Duy
// Mã sinh viên: 23203100014
using DoAnNhom06.Filters;
using DoAnNhom06.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnNhom06.Services;
namespace DoAnNhom06.Controllers;

[RequireRole(VaiTro.Admin)]
public class LoaiPhongController(AppDbContext db) : BaseController
{
    public async Task<IActionResult> Index()
    {
        var ds = await db.LoaiPhongs.AsNoTracking()
            .Include(x => x.PhongKTXs)
            .OrderBy(x => x.TenLoaiPhong)
            .ToListAsync();
        return View(ds);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View("Form", new LoaiPhong { TrangThai = TrangThaiLoaiPhong.DangSuDung, SoNguoiToiDa = 4 });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TenLoaiPhong,SoNguoiToiDa,DonGiaThang,MoTa,TrangThai")] LoaiPhong model)
    {
        model.TenLoaiPhong = model.TenLoaiPhong.Trim();
        await KiemTraTrungTenAsync(model, 0);
        if (!ModelState.IsValid) return View("Form", model);

        db.LoaiPhongs.Add(model);
        await db.SaveChangesAsync();
        TempData["ThongBao"] = $"Đã thêm loại phòng {model.TenLoaiPhong}.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var loai = await db.LoaiPhongs.AsNoTracking().FirstOrDefaultAsync(x => x.MaLoaiPhong == id);
        return loai is null ? NotFound() : View("Form", loai);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("TenLoaiPhong,SoNguoiToiDa,DonGiaThang,MoTa,TrangThai")] LoaiPhong model)
    {
        var loai = await db.LoaiPhongs.FirstOrDefaultAsync(x => x.MaLoaiPhong == id);
        if (loai is null) return NotFound();

        model.MaLoaiPhong = id;
        model.TenLoaiPhong = model.TenLoaiPhong.Trim();
        await KiemTraTrungTenAsync(model, id);

        if (model.SoNguoiToiDa < loai.SoNguoiToiDa && ModelState.IsValid)
        {
            var phongVuotCho = await PhongTruyVan.KemSoCho(db)
                .Where(x => x.MaLoaiPhong == id && x.SoNguoiDangO > model.SoNguoiToiDa)
                .Select(x => x.TenPhong)
                .ToListAsync();
            if (phongVuotCho.Count > 0)
                ModelState.AddModelError(nameof(model.SoNguoiToiDa),
                    $"Không thể giảm sức chứa xuống {model.SoNguoiToiDa}: phòng {string.Join(", ", phongVuotCho)} đang có nhiều người hơn.");
        }
        if (!ModelState.IsValid) return View("Form", model);

        loai.TenLoaiPhong = model.TenLoaiPhong;
        loai.SoNguoiToiDa = model.SoNguoiToiDa;
        loai.DonGiaThang = model.DonGiaThang;
        loai.MoTa = model.MoTa;
        loai.TrangThai = model.TrangThai;
        await db.SaveChangesAsync();
        TempData["ThongBao"] = $"Đã cập nhật loại phòng {loai.TenLoaiPhong}.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var loai = await db.LoaiPhongs.FirstOrDefaultAsync(x => x.MaLoaiPhong == id);
        if (loai is null) return NotFound();

        var coPhong = await db.PhongKTXs.AnyAsync(x => x.MaLoaiPhong == id);
        var coDangKy = await db.DangKyKTXs.AnyAsync(x => x.NguyenVongLoaiPhong == id);
        if (coPhong || coDangKy)
        {
            TempData["Loi"] = $"Không thể xóa {loai.TenLoaiPhong} vì đang có phòng hoặc đăng ký liên quan. Hãy chuyển sang Ngừng sử dụng.";
            return RedirectToAction(nameof(Index));
        }

        db.LoaiPhongs.Remove(loai);
        await db.SaveChangesAsync();
        TempData["ThongBao"] = $"Đã xóa loại phòng {loai.TenLoaiPhong}.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>Tên loại phòng bắt buộc và không trùng (kiểm tra bằng LINQ/CSDL).</summary>
    private async Task KiemTraTrungTenAsync(LoaiPhong model, int id)
    {
        if (await db.LoaiPhongs.AnyAsync(x => x.TenLoaiPhong == model.TenLoaiPhong && x.MaLoaiPhong != id))
            ModelState.AddModelError(nameof(model.TenLoaiPhong), "Tên loại phòng đã tồn tại.");
    }
}
