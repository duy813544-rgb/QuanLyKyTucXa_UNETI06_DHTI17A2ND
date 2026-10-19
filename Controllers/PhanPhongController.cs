// Họ và tên: Nguyễn Bá Hùng
// Mã sinh viên: 23274800011
// Nội dung thực hiện: Module 4- Quản lý phân phòng KTX:    
using System.Data;
using DoAnNhom06.Filters;
using DoAnNhom06.Models;
using DoAnNhom06.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Controllers;

[RequireRole(VaiTro.Admin)]
public class PhanPhongController(AppDbContext db) : BaseController
{
    public async Task<IActionResult> Index(string? q)
    {
        var dangO = db.PhanPhongs.AsNoTracking()
            .Include(x => x.DangKyKTX).ThenInclude(x => x.SinhVien)
            .Include(x => x.PhongKTX).ThenInclude(x => x.LoaiPhong)
            .Where(x => x.TrangThai == TrangThaiPhanPhong.DangO);
        var choDuyet = db.DangKyKTXs.AsNoTracking()
            .Include(x => x.SinhVien).Include(x => x.NguyenVong)
            .Where(x => x.TrangThai == TrangThaiDangKy.ChoDuyet);

        if (!string.IsNullOrWhiteSpace(q))
        {
            var tuKhoa = q.Trim();
            dangO = dangO.Where(x => x.DangKyKTX.MaSinhVien.Contains(tuKhoa)
                || x.DangKyKTX.SinhVien.HoTen.Contains(tuKhoa)
                || x.PhongKTX.TenPhong.Contains(tuKhoa));
            choDuyet = choDuyet.Where(x => x.MaSinhVien.Contains(tuKhoa) || x.SinhVien.HoTen.Contains(tuKhoa));
        }

        ViewBag.Q = q;
        return View(new PhanPhongIndexViewModel
        {
            Q = q,
            DangO = await dangO.OrderBy(x => x.PhongKTX.TenPhong).ToListAsync(),
            ChoDuyet = await choDuyet.OrderBy(x => x.NgayDangKy).ToListAsync()
        });
    }

    [HttpGet]
    public async Task<IActionResult> Assign(int maDangKy)
    {
        var dk = await LayDangKyChoDuyetAsync(maDangKy);
        if (dk is null)
        {
            TempData["Loi"] = "Đăng ký không còn ở trạng thái Chờ duyệt.";
            return RedirectToAction(nameof(Index));
        }
        return View(await TaoViewModelPhanPhongAsync(dk, null));
    }


    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Assign(int maDangKy, int? maPhong)
    {
        var dk = await LayDangKyChoDuyetAsync(maDangKy);
        if (dk is null)
        {
            TempData["Loi"] = "Đăng ký không còn ở trạng thái Chờ duyệt.";
            return RedirectToAction(nameof(Index));
        }
        if (maPhong is null)
        {
            ModelState.AddModelError(string.Empty, "Vui lòng chọn phòng.");
            return View(await TaoViewModelPhanPhongAsync(dk, null));
        }

        var loi = await KiemTraPhanPhongAsync(dk, maPhong.Value);
        if (loi is not null)
        {
            ModelState.AddModelError(string.Empty, loi);
            return View(await TaoViewModelPhanPhongAsync(dk, maPhong));
        }

        try
        {
            await using var giaoDich = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable);

            loi = await KiemTraPhanPhongAsync(dk, maPhong.Value);
            if (loi is not null)
            {
                ModelState.AddModelError(string.Empty, loi);
                return View(await TaoViewModelPhanPhongAsync(dk, maPhong));
            }

            db.PhanPhongs.Add(new PhanPhong
            {
                MaDangKy = dk.MaDangKy,
                MaPhong = maPhong.Value,
                NgayVao = DateOnly.FromDateTime(DateTime.Now),
                TrangThai = TrangThaiPhanPhong.DangO
            });
            dk.TrangThai = TrangThaiDangKy.DaPhanPhong;
            await db.SaveChangesAsync();
            await giaoDich.CommitAsync();
        }
        catch (DbUpdateException ex)
        {

            ModelState.AddModelError(string.Empty, "Không thể phân phòng: " + ex.GetBaseException().Message);
            return View(await TaoViewModelPhanPhongAsync(dk, maPhong));
        }

        TempData["ThongBao"] = $"Đã phân phòng cho sinh viên {dk.MaSinhVien}.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Move(int id)
    {
        var pp = await db.PhanPhongs.AsNoTracking()
            .Include(x => x.DangKyKTX).ThenInclude(x => x.SinhVien)
            .Include(x => x.PhongKTX).ThenInclude(x => x.LoaiPhong)
            .FirstOrDefaultAsync(x => x.MaPhanPhong == id);
        if (pp is null) return NotFound();
        if (pp.TrangThai != TrangThaiPhanPhong.DangO)
        {
            TempData["Loi"] = "Sinh viên không còn đang ở phòng này nên không thể chuyển phòng.";
            return RedirectToAction(nameof(Index));
        }
        return View(new ChuyenPhongViewModel
        {
            HienTai = pp,
            PhongPhuHop = await PhongTruyVan.PhongPhuHopAsync(db, pp.DangKyKTX.SinhVien.GioiTinh, pp.MaPhong)
        });
    }

    /// <summary>
    /// Chuyển phòng: bản ghi cũ chuyển sang Đã chuyển và có NgayRa; tạo bản ghi mới Đang ở.
    /// Không sửa MaPhong của bản ghi cũ để giữ lịch sử.
    /// </summary>
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Move(int id, int? maPhongMoi)
    {
        var cu = await db.PhanPhongs
            .Include(x => x.DangKyKTX).ThenInclude(x => x.SinhVien)
            .FirstOrDefaultAsync(x => x.MaPhanPhong == id);
        if (cu is null) return NotFound();
        if (cu.TrangThai != TrangThaiPhanPhong.DangO)
        {
            TempData["Loi"] = "Sinh viên không còn đang ở phòng này nên không thể chuyển phòng.";
            return RedirectToAction(nameof(Index));
        }
        if (maPhongMoi is null)
        {
            ModelState.AddModelError(string.Empty, "Vui lòng chọn phòng mới.");
            return await TraLaiChuyenPhongAsync(id, null);
        }
        if (maPhongMoi == cu.MaPhong)
        {
            ModelState.AddModelError(string.Empty, "Không thể chuyển vào chính phòng đang ở.");
            return await TraLaiChuyenPhongAsync(id, maPhongMoi);
        }

        var homNay = DateOnly.FromDateTime(DateTime.Now);
        if (homNay < cu.NgayVao)
        {
            ModelState.AddModelError(string.Empty, "Ngày chuyển không được trước ngày vào phòng.");
            return await TraLaiChuyenPhongAsync(id, maPhongMoi);
        }

        var loi = await PhongTruyVan.KiemTraNhanThemAsync(db, maPhongMoi.Value, cu.DangKyKTX.SinhVien.GioiTinh);
        if (loi is not null)
        {
            ModelState.AddModelError(string.Empty, loi);
            return await TraLaiChuyenPhongAsync(id, maPhongMoi);
        }

        try
        {
            await using var giaoDich = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            loi = await PhongTruyVan.KiemTraNhanThemAsync(db, maPhongMoi.Value, cu.DangKyKTX.SinhVien.GioiTinh);
            if (loi is not null)
            {
                ModelState.AddModelError(string.Empty, loi);
                return await TraLaiChuyenPhongAsync(id, maPhongMoi);
            }

            // Bước 1: đóng bản ghi cũ trước, để giải phóng chỉ mục "một bản ghi đang ở cho mỗi đăng ký".
            cu.TrangThai = TrangThaiPhanPhong.DaChuyen;
            cu.NgayRa = homNay;
            await db.SaveChangesAsync();

            // Bước 2: mở bản ghi mới ở phòng mới.
            db.PhanPhongs.Add(new PhanPhong
            {
                MaDangKy = cu.MaDangKy,
                MaPhong = maPhongMoi.Value,
                NgayVao = homNay,
                TrangThai = TrangThaiPhanPhong.DangO
            });
            await db.SaveChangesAsync();
            await giaoDich.CommitAsync();
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError(string.Empty, "Không thể chuyển phòng: " + ex.GetBaseException().Message);
            return await TraLaiChuyenPhongAsync(id, maPhongMoi);
        }

        TempData["ThongBao"] = $"Đã chuyển sinh viên {cu.DangKyKTX.MaSinhVien} sang phòng mới.";
        return RedirectToAction(nameof(Index));
    }

    /// <summary>Trả phòng: bản ghi chuyển Đã trả phòng, ghi NgayRa; đăng ký chuyển Đã trả phòng.</summary>
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckOut(int id)
    {
        var pp = await db.PhanPhongs.Include(x => x.DangKyKTX).FirstOrDefaultAsync(x => x.MaPhanPhong == id);
        if (pp is null) return NotFound();
        if (pp.TrangThai != TrangThaiPhanPhong.DangO)
        {
            TempData["Loi"] = "Sinh viên đã trả phòng trước đó, không thể trả phòng lần thứ hai.";
            return RedirectToAction(nameof(Index));
        }

        var homNay = DateOnly.FromDateTime(DateTime.Now);
        if (homNay < pp.NgayVao)
        {
            TempData["Loi"] = "Ngày trả phòng không được trước ngày vào phòng.";
            return RedirectToAction(nameof(Index));
        }

        await using var giaoDich = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        pp.TrangThai = TrangThaiPhanPhong.DaTraPhong;
        pp.NgayRa = homNay;
        pp.DangKyKTX.TrangThai = TrangThaiDangKy.DaTraPhong;
        await db.SaveChangesAsync();
        await giaoDich.CommitAsync();

        TempData["ThongBao"] = $"Đã trả phòng cho sinh viên {pp.DangKyKTX.MaSinhVien}.";
        return RedirectToAction(nameof(Index));
    }

    private Task<DangKyKTX?> LayDangKyChoDuyetAsync(int maDangKy) =>
        db.DangKyKTXs.Include(x => x.SinhVien).Include(x => x.NguyenVong)
            .FirstOrDefaultAsync(x => x.MaDangKy == maDangKy && x.TrangThai == TrangThaiDangKy.ChoDuyet);

    /// <summary>Điều kiện phân một đăng ký vào một phòng. Trả về lỗi đầu tiên, hoặc null nếu hợp lệ.</summary>
    private async Task<string?> KiemTraPhanPhongAsync(DangKyKTX dk, int maPhong)
    {
        if (dk.SinhVien.TrangThai != TrangThaiChung.DangHoatDong)
            return "Sinh viên đang bị khóa, không thể phân phòng.";
        if (await db.PhanPhongs.AnyAsync(x => x.TrangThai == TrangThaiPhanPhong.DangO && x.DangKyKTX.MaSinhVien == dk.MaSinhVien))
            return "Sinh viên đang ở một phòng khác. Hãy chuyển phòng hoặc trả phòng trước.";
        return await PhongTruyVan.KiemTraNhanThemAsync(db, maPhong, dk.SinhVien.GioiTinh);
    }

    private async Task<PhanPhongChonViewModel> TaoViewModelPhanPhongAsync(DangKyKTX dk, int? maPhong) =>
        new()
        {
            DangKy = dk,
            MaPhong = maPhong,
            PhongPhuHop = await PhongTruyVan.PhongPhuHopAsync(db, dk.SinhVien.GioiTinh)
        };

    private async Task<IActionResult> TraLaiChuyenPhongAsync(int id, int? maPhongMoi)
    {
        var hienTai = await db.PhanPhongs.AsNoTracking()
            .Include(x => x.DangKyKTX).ThenInclude(x => x.SinhVien)
            .Include(x => x.PhongKTX).ThenInclude(x => x.LoaiPhong)
            .FirstAsync(x => x.MaPhanPhong == id);
        return View("Move", new ChuyenPhongViewModel
        {
            HienTai = hienTai,
            MaPhongMoi = maPhongMoi,
            PhongPhuHop = await PhongTruyVan.PhongPhuHopAsync(db, hienTai.DangKyKTX.SinhVien.GioiTinh, hienTai.MaPhong)
        });
    }
}
