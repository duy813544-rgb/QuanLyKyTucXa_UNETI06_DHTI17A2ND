using DoAnNhom06.Models;
using Microsoft.EntityFrameworkCore;
namespace DoAnNhom06.Services;

public static class PhongTruyVan
{
    /// <summary>
    /// Mỗi phòng kèm sức chứa (SoNguoiToiDa của loại phòng) và số PhanPhong đang ở.
    /// Toàn bộ được dịch thành SQL; không tải dữ liệu rồi tính trên trình duyệt.
    /// </summary>
    public static IQueryable<PhongRow> KemSoCho(AppDbContext db) =>
        db.PhongKTXs.Select(p => new PhongRow
        {
            MaPhong = p.MaPhong,
            TenPhong = p.TenPhong,
            ToaNha = p.ToaNha,
            Tang = p.Tang,
            GioiTinhPhong = p.GioiTinhPhong,
            TrangThai = p.TrangThai,
            MoTa = p.MoTa,
            MaLoaiPhong = p.MaLoaiPhong,
            TenLoaiPhong = p.LoaiPhong.TenLoaiPhong,
            DonGiaThang = p.LoaiPhong.DonGiaThang,
            SucChua = p.LoaiPhong.SoNguoiToiDa,
            SoNguoiDangO = p.PhanPhongs.Count(pp => pp.TrangThai == TrangThaiPhanPhong.DangO)
        });

    /// <summary>
    /// Danh sách phòng: Tìm kiếm -> Lọc -> Sắp xếp -> Phân trang (Skip/Take trên truy vấn EF Core).
    /// Sinh viên chỉ xem phòng đang sử dụng.
    /// </summary>
    public static async Task<PhongDanhSachViewModel> TruyVanAsync(AppDbContext db, PhongDanhSachViewModel vm)
    {
        IQueryable<PhongRow> q = KemSoCho(db);

        if (!vm.LaAdmin) q = q.Where(x => x.TrangThai == TrangThaiPhong.DangSuDung);

        // Tìm kiếm theo tên phòng
        // Tìm kiếm theo tên phòng hoặc tên tòa nhà
        if (!string.IsNullOrWhiteSpace(vm.Q))
        {
            var tuKhoa = vm.Q.Trim();

            q = q.Where(x =>
                x.TenPhong.Contains(tuKhoa) ||
                x.ToaNha.Contains(tuKhoa));
        }

        // Lọc theo tòa nhà, tầng, loại phòng, giới tính, trạng thái, còn chỗ/hết chỗ
        if (!string.IsNullOrWhiteSpace(vm.ToaNha)) q = q.Where(x => x.ToaNha == vm.ToaNha);
        if (vm.Tang is not null) { var tang = vm.Tang.Value; q = q.Where(x => x.Tang == tang); }
        if (vm.MaLoaiPhong is not null) { var loai = vm.MaLoaiPhong.Value; q = q.Where(x => x.MaLoaiPhong == loai); }
        if (!string.IsNullOrWhiteSpace(vm.GioiTinhPhong)) q = q.Where(x => x.GioiTinhPhong == vm.GioiTinhPhong);
        if (!string.IsNullOrWhiteSpace(vm.TrangThai)) q = q.Where(x => x.TrangThai == vm.TrangThai);
        if (vm.ConCho == "con") q = q.Where(x => x.SucChua - x.SoNguoiDangO > 0);
        else if (vm.ConCho == "het") q = q.Where(x => x.SucChua - x.SoNguoiDangO <= 0);

        var tongSo = await q.CountAsync();
        vm.TotalCount = tongSo;
        vm.Page = Math.Clamp(vm.Page, 1, vm.TotalPages);

        IOrderedQueryable<PhongRow> sapXep = vm.Sort switch
        {
            "ten_desc" => q.OrderByDescending(x => x.TenPhong),
            "tang_asc" => q.OrderBy(x => x.Tang).ThenBy(x => x.TenPhong),
            "tang_desc" => q.OrderByDescending(x => x.Tang).ThenBy(x => x.TenPhong),
            "cho_asc" => q.OrderBy(x => x.SucChua - x.SoNguoiDangO).ThenBy(x => x.TenPhong),
            "cho_desc" => q.OrderByDescending(x => x.SucChua - x.SoNguoiDangO).ThenBy(x => x.TenPhong),
            _ => q.OrderBy(x => x.TenPhong)
        };

        vm.Items = await sapXep
            .Skip((vm.Page - 1) * vm.PageSize)
            .Take(vm.PageSize)
            .ToListAsync();
        return vm;
    }

    /// <summary>
    /// Kiểm tra phòng có nhận thêm sinh viên không. Trả về thông báo lỗi, hoặc null nếu hợp lệ.
    /// </summary>
    public static async Task<string?> KiemTraNhanThemAsync(AppDbContext db, int maPhong, string gioiTinhSinhVien)
    {
        var phong = await KemSoCho(db).FirstOrDefaultAsync(x => x.MaPhong == maPhong);
        if (phong is null) return "Phòng không tồn tại.";
        if (phong.TrangThai != TrangThaiPhong.DangSuDung) return $"Phòng {phong.TenPhong} đang tạm ngừng, không thể phân thêm sinh viên.";
        if (phong.GioiTinhPhong != gioiTinhSinhVien)
            return $"Giới tính không phù hợp: phòng {phong.TenPhong} dành cho sinh viên giới tính {phong.GioiTinhPhong}.";
        if (phong.SoChoCon <= 0) return $"Phòng {phong.TenPhong} đã đủ người ({phong.SoNguoiDangO}/{phong.SucChua}), không thể phân thêm.";
        return null;
    }

    /// <summary>Phòng đang hoạt động, đúng giới tính và còn chỗ, dùng cho chọn phòng khi phân/chuyển.</summary>
    public static Task<List<PhongRow>> PhongPhuHopAsync(AppDbContext db, string gioiTinhSinhVien, int? boQuaPhong = null)
    {
        var q = KemSoCho(db).Where(x =>
            x.TrangThai == TrangThaiPhong.DangSuDung
            && x.GioiTinhPhong == gioiTinhSinhVien
            && x.SucChua - x.SoNguoiDangO > 0);
        if (boQuaPhong is not null) { var bo = boQuaPhong.Value; q = q.Where(x => x.MaPhong != bo); }
        return q.OrderBy(x => x.ToaNha).ThenBy(x => x.TenPhong).ToListAsync();
    }
}
