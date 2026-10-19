//Trần Văn Cường
using DoAnNhom06.Filters;
using DoAnNhom06.Models;
using DoAnNhom06.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Controllers;

[RequireRole(VaiTro.Admin, VaiTro.SinhVien)]
public class PhongKTXController(AppDbContext db) : BaseController
{
    public async Task<IActionResult> Index(string? q, string? toa, int? tang, string? gioitinh, string? trangthai,
        int? loai, string? concho, string? sort, int page = 1)
    {
        var vm = new PhongDanhSachViewModel
        {
            Q = q,
            ToaNha = toa,
            Tang = tang,
            GioiTinhPhong = gioitinh,
            TrangThai = trangthai,
            MaLoaiPhong = loai,
            ConCho = concho,
            Sort = string.IsNullOrEmpty(sort) ? "ten_asc" : sort,
            Page = page,
            LaAdmin = LaAdmin
        };
        vm.LoaiPhongs = await db.LoaiPhongs.AsNoTracking().OrderBy(x => x.TenLoaiPhong).ToListAsync();
        vm.ToaNhas = await db.PhongKTXs.AsNoTracking().Select(p => p.ToaNha).Distinct().OrderBy(x => x).ToListAsync();
        vm = await PhongTruyVan.TruyVanAsync(db, vm);
        return View(vm);
    }

    public async Task<IActionResult> Details(int id)
    {
        var phong = await PhongTruyVan.KemSoCho(db).FirstOrDefaultAsync(x => x.MaPhong == id);
        if (phong is null) return NotFound();

        ViewBag.LaAdmin = LaAdmin;
        if (LaAdmin)
        {

            ViewBag.DangO = await db.PhanPhongs.AsNoTracking()
                .Include(x => x.DangKyKTX).ThenInclude(x => x.SinhVien)
                .Where(x => x.MaPhong == id && x.TrangThai == TrangThaiPhanPhong.DangO)
                .OrderBy(x => x.NgayVao)
                .ToListAsync();
            ViewBag.LichSu = await db.PhanPhongs.AsNoTracking()
                .Include(x => x.DangKyKTX).ThenInclude(x => x.SinhVien)
                .Where(x => x.MaPhong == id)
                .OrderByDescending(x => x.NgayVao)
                .ToListAsync();
        }
        return View(phong);
    }

    [HttpGet, RequireRole(VaiTro.Admin)]
    public async Task<IActionResult> Create()
    {
        await NapDuLieuFormAsync(laTaoMoi: true);
        return View("Form", new PhongKTX { TrangThai = TrangThaiPhong.DangSuDung, GioiTinhPhong = GioiTinh.Nam, Tang = 1 });
    }

    [HttpPost, ValidateAntiForgeryToken, RequireRole(VaiTro.Admin)]
    public async Task<IActionResult> Create([Bind("TenPhong,MaLoaiPhong,ToaNha,Tang,GioiTinhPhong,TrangThai,MoTa")] PhongKTX model)
    {
        model.TenPhong = model.TenPhong.Trim();
        model.ToaNha = model.ToaNha.Trim();
        await KiemTraAsync(model, id: 0, maLoaiCu: null);
        if (!ModelState.IsValid)
        {
            await NapDuLieuFormAsync(laTaoMoi: true);
            return View("Form", model);
        }

        db.PhongKTXs.Add(model);
        await db.SaveChangesAsync();
        TempData["ThongBao"] = $"Đã thêm phòng {model.TenPhong}.";
        return RedirectToAction(nameof(Details), new { id = model.MaPhong });
    }

    [HttpGet, RequireRole(VaiTro.Admin)]
    public async Task<IActionResult> Edit(int id)
    {
        var phong = await db.PhongKTXs.AsNoTracking().FirstOrDefaultAsync(x => x.MaPhong == id);
        if (phong is null) return NotFound();
        await NapDuLieuFormAsync(laTaoMoi: false);
        return View("Form", phong);
    }

    [HttpPost, ValidateAntiForgeryToken, RequireRole(VaiTro.Admin)]
    public async Task<IActionResult> Edit(int id, [Bind("TenPhong,MaLoaiPhong,ToaNha,Tang,GioiTinhPhong,TrangThai,MoTa")] PhongKTX model)
    {
        var phong = await db.PhongKTXs.FirstOrDefaultAsync(x => x.MaPhong == id);
        if (phong is null) return NotFound();

        model.MaPhong = id;
        model.TenPhong = model.TenPhong.Trim();
        model.ToaNha = model.ToaNha.Trim();
        var maLoaiCu = phong.MaLoaiPhong;
        await KiemTraAsync(model, id, maLoaiCu);
        if (!ModelState.IsValid)
        {
            await NapDuLieuFormAsync(laTaoMoi: false);
            return View("Form", model);
        }

        phong.TenPhong = model.TenPhong;
        phong.MaLoaiPhong = model.MaLoaiPhong;
        phong.ToaNha = model.ToaNha;
        phong.Tang = model.Tang;
        phong.GioiTinhPhong = model.GioiTinhPhong;
        phong.TrangThai = model.TrangThai;
        phong.MoTa = model.MoTa;
        await db.SaveChangesAsync();
        TempData["ThongBao"] = $"Đã cập nhật phòng {phong.TenPhong}.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost, ValidateAntiForgeryToken, RequireRole(VaiTro.Admin)]
    public async Task<IActionResult> ChangeStatus(int id, string trangThai)
    {
        var phong = await db.PhongKTXs.FirstOrDefaultAsync(x => x.MaPhong == id);
        if (phong is null) return NotFound();
        if (trangThai is not (TrangThaiPhong.DangSuDung or TrangThaiPhong.TamNgung))
        {
            TempData["Loi"] = "Trạng thái phòng không hợp lệ.";
            return RedirectToAction(nameof(Details), new { id });
        }
        phong.TrangThai = trangThai;
        await db.SaveChangesAsync();
        TempData["ThongBao"] = $"Phòng {phong.TenPhong}: {Nhan.NhanPhong(trangThai)}.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost, ValidateAntiForgeryToken, RequireRole(VaiTro.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        var phong = await db.PhongKTXs.FirstOrDefaultAsync(x => x.MaPhong == id);
        if (phong is null) return NotFound();

        // Phòng đã từng có sinh viên ở thì không xóa, để giữ lịch sử phân phòng.
        if (await db.PhanPhongs.AnyAsync(x => x.MaPhong == id))
        {
            TempData["Loi"] = $"Không thể xóa phòng {phong.TenPhong} vì đã phát sinh lịch sử phân phòng. Hãy chuyển sang Tạm ngừng.";
            return RedirectToAction(nameof(Details), new { id });
        }

        db.PhongKTXs.Remove(phong);
        await db.SaveChangesAsync();
        TempData["ThongBao"] = $"Đã xóa phòng {phong.TenPhong}.";
        return RedirectToAction(nameof(Index));
    }

    private async Task KiemTraAsync(PhongKTX model, int id, int? maLoaiCu)
    {
        if (await db.PhongKTXs.AnyAsync(x => x.TenPhong == model.TenPhong && x.MaPhong != id))
            ModelState.AddModelError(nameof(model.TenPhong), "Tên phòng đã tồn tại.");

        var loai = await db.LoaiPhongs.AsNoTracking().FirstOrDefaultAsync(x => x.MaLoaiPhong == model.MaLoaiPhong);
        if (loai is null)
        {
            ModelState.AddModelError(nameof(model.MaLoaiPhong), "Loại phòng không tồn tại.");
            return;
        }
        if (loai.TrangThai != TrangThaiLoaiPhong.DangSuDung && model.MaLoaiPhong != maLoaiCu)
            ModelState.AddModelError(nameof(model.MaLoaiPhong), "Loại phòng đang ngừng sử dụng, không thể gán cho phòng.");

        // Khi sửa phòng đang có người ở: kiểm tra sức chứa mới và giới tính quy định mới.
        if (id > 0)
        {
            var dangO = await db.PhanPhongs.AsNoTracking()
                .Where(x => x.MaPhong == id && x.TrangThai == TrangThaiPhanPhong.DangO)
                .Select(x => x.DangKyKTX.SinhVien.GioiTinh)
                .ToListAsync();
            if (dangO.Count > loai.SoNguoiToiDa)
                ModelState.AddModelError(nameof(model.MaLoaiPhong),
                    $"Phòng đang có {dangO.Count} sinh viên, vượt sức chứa {loai.SoNguoiToiDa} của loại phòng này.");
            if (dangO.Any(g => g != model.GioiTinhPhong))
                ModelState.AddModelError(nameof(model.GioiTinhPhong),
                    "Phòng đang có sinh viên khác giới tính, không thể đổi giới tính quy định của phòng.");
        }
    }

    private async Task NapDuLieuFormAsync(bool laTaoMoi)
    {
        ViewBag.LaTaoMoi = laTaoMoi;
        ViewBag.LoaiPhongs = await db.LoaiPhongs.AsNoTracking().OrderBy(x => x.TenLoaiPhong).ToListAsync();
    }
}
