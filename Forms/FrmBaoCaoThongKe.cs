using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;
using DoAnNhom06.Helpers;
using DoAnNhom06.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// UC11 - Báo cáo thống kê (gộp UC17 Thống kê sinh viên, UC18 Thống kê phòng,
    /// UC19 Thống kê doanh thu, UC20 Xuất báo cáo). Khác với 4 thẻ KPI ngắn ở
    /// Trang chủ Quản trị viên (chỉ vài số liệu tổng quan để xem lướt), màn hình
    /// này báo cáo ĐẦY ĐỦ dữ liệu của toàn bộ hệ thống theo từng nhóm (sinh viên,
    /// phòng, hợp đồng/hóa đơn, doanh thu, nhân viên). Nút "Xuất Excel" chỉ xuất
    /// đúng dữ liệu của TAB đang được chọn (VD: đang xem tab Sinh viên thì xuất
    /// danh sách sinh viên, không kèm các tab khác).
    /// </summary>
    public partial class FrmBaoCaoThongKe : Form
    {
        public FrmBaoCaoThongKe()
        {
            InitializeComponent();
        }

        private void FrmBaoCaoThongKe_Load(object sender, EventArgs e)
        {
            TaiToanBoBaoCao();
        }

        private void btnLamMoi_Click(object sender, EventArgs e) => TaiToanBoBaoCao();

        private void TaiToanBoBaoCao()
        {
            using var db = new AppDbContext();

            NapTongQuanHeThong(db);
            NapThongKeSinhVien(db);
            NapThongKePhong(db);
            NapHopDongHoaDon(db);
            NapDoanhThu(db);
            NapNhanVien(db);
        }

        // ---------------------------------------------------------------
        // Tab 0: TỔNG QUAN HỆ THỐNG - toàn bộ số liệu chính, mỗi dòng 1 chỉ số.
        // ---------------------------------------------------------------
        private void NapTongQuanHeThong(AppDbContext db)
        {
            int tongSinhVien = db.SinhViens.Count();
            int tongNhanVien = db.NhanViens.Count();
            int tongTaiKhoan = db.TaiKhoans.Count();
            int tongKhuNha = db.KhuNhas.Count();
            int tongLoaiPhong = db.LoaiPhongs.Count();
            int tongPhong = db.Phongs.Count();
            int tongSucChua = db.Phongs.Sum(p => (int?)p.SoLuongToiDa) ?? 0;
            int tongDangO = db.Phongs.Sum(p => (int?)p.SoLuongHienTai) ?? 0;
            int phongConTrong = db.Phongs.Count(p => (p.SoLuongHienTai ?? 0) < (p.SoLuongToiDa ?? 0));
            double tyLeLapDay = tongSucChua > 0 ? (double)tongDangO / tongSucChua * 100.0 : 0;

            int dangKyChoDuyet = db.DangKyOs.Count(d => d.TrangThai == TrangThaiDangKy.ChoDuyet);
            int dangKyDaDuyet = db.DangKyOs.Count(d => d.TrangThai == TrangThaiDangKy.DaDuyet);
            int dangKyTuChoi = db.DangKyOs.Count(d => d.TrangThai == TrangThaiDangKy.TuChoi);

            int hopDongConHieuLuc = db.HopDongs.Count(h => h.TrangThai == "Còn hiệu lực");
            int tongHopDong = db.HopDongs.Count();

            int tongHoaDon = db.HoaDons.Count();
            int hoaDonDaThanhToan = db.HoaDons.Count(h => h.TrangThai == TrangThaiHoaDon.DaThanhToan);
            int hoaDonChoXacNhan = db.HoaDons.Count(h => h.TrangThai == TrangThaiHoaDon.ChoXacNhan);

            decimal tongDaThu = db.HoaDons.Where(h => h.TrangThai == TrangThaiHoaDon.DaThanhToan).Sum(h => (decimal?)h.TongTien) ?? 0;
            decimal tongPhaiThu = db.HoaDons.Sum(h => (decimal?)h.TongTien) ?? 0;
            decimal tongConLai = tongPhaiThu - tongDaThu;

            int soBaoCaoDaXuat = db.BaoCaos.Count();

            var danhSach = new (string ChiSo, string GiaTri)[]
            {
                ("Tổng số sinh viên", tongSinhVien.ToString("N0")),
                ("Tổng số nhân viên", tongNhanVien.ToString("N0")),
                ("Tổng số tài khoản đăng nhập", tongTaiKhoan.ToString("N0")),
                ("Tổng số khu nhà", tongKhuNha.ToString("N0")),
                ("Tổng số loại phòng", tongLoaiPhong.ToString("N0")),
                ("Tổng số phòng", tongPhong.ToString("N0")),
                ("Tổng sức chứa (tất cả phòng)", tongSucChua.ToString("N0")),
                ("Tổng số sinh viên đang ở", tongDangO.ToString("N0")),
                ("Số phòng còn trống", phongConTrong.ToString("N0")),
                ("Tỷ lệ lấp đầy", tyLeLapDay.ToString("N1") + "%"),
                ("Đăng ký ở - Chờ duyệt", dangKyChoDuyet.ToString("N0")),
                ("Đăng ký ở - Đã duyệt", dangKyDaDuyet.ToString("N0")),
                ("Đăng ký ở - Từ chối", dangKyTuChoi.ToString("N0")),
                ("Tổng số hợp đồng", tongHopDong.ToString("N0")),
                ("Hợp đồng còn hiệu lực", hopDongConHieuLuc.ToString("N0")),
                ("Tổng số hóa đơn", tongHoaDon.ToString("N0")),
                ("Hóa đơn đã thanh toán", hoaDonDaThanhToan.ToString("N0")),
                ("Hóa đơn chờ xác nhận", hoaDonChoXacNhan.ToString("N0")),
                ("Tổng doanh thu đã thu", tongDaThu.ToString("N0") + " đ"),
                ("Tổng số tiền còn phải thu", tongConLai.ToString("N0") + " đ"),
                ("Số báo cáo đã xuất trước đây", soBaoCaoDaXuat.ToString("N0")),
            };

            dgvTongQuan.DataSource = danhSach.Select(x => new { ChiSo = x.ChiSo, GiaTri = x.GiaTri }).ToList();

            lblTongQuan.Text = $"Tổng hợp toàn bộ số liệu của hệ thống, tính đến {DateTime.Now:dd/MM/yyyy HH:mm}. " +
                "Đây là báo cáo đầy đủ (khác với 4 thẻ số liệu ngắn ở Trang chủ, ở đây liệt kê tất cả các chỉ số chính).";

            // Cập nhật 4 thẻ KPI ở đầu trang (tái dùng các số liệu vừa tính ở trên, không
            // truy vấn lại CSDL) + mốc thời gian cập nhật gần nhất trên thanh tiêu đề.
            cardTongSinhVien.TieuDe = "Tổng sinh viên";
            cardTongSinhVien.GiaTri = tongSinhVien.ToString("N0");
            cardTongSinhVien.MoTa = $"{tongDangO:N0} đang ở KTX";

            cardTongPhongBc.TieuDe = "Tổng số phòng";
            cardTongPhongBc.GiaTri = tongPhong.ToString("N0");
            cardTongPhongBc.MoTa = $"Lấp đầy {tyLeLapDay:N0}% ({phongConTrong:N0} phòng trống)";

            cardDoanhThuBc.TieuDe = "Doanh thu đã thu";
            cardDoanhThuBc.GiaTri = tongDaThu >= 1000000
                ? (tongDaThu / 1000000m).ToString("N1") + " triệu"
                : tongDaThu.ToString("N0") + " đ";
            cardDoanhThuBc.MoTa = $"Còn phải thu {tongConLai:N0} đ";

            cardChoXacNhanBc.TieuDe = "Hóa đơn chờ xác nhận";
            cardChoXacNhanBc.GiaTri = hoaDonChoXacNhan.ToString("N0");
            cardChoXacNhanBc.MoTa = $"Trong tổng số {tongHoaDon:N0} hóa đơn";

            cardTongSinhVien.Invalidate();
            cardTongPhongBc.Invalidate();
            cardDoanhThuBc.Invalidate();
            cardChoXacNhanBc.Invalidate();

            lblCapNhat.Text = $"Cập nhật lúc: {DateTime.Now:HH:mm:ss dd/MM/yyyy}";
        }

        /// <summary>Tự vẽ tiêu đề từng tab của tabBaoCao: tab đang chọn có chữ đậm, đổi màu
        /// theo đúng màu nhấn riêng của tab đó, và có 1 gạch chân màu nhấn ở dưới; các tab
        /// còn lại hiển thị mờ hơn - thay cho kiểu tab phẳng mặc định của Windows.</summary>
        private void tabBaoCao_DrawItem(object? sender, DrawItemEventArgs e)
        {
            var trang = tabBaoCao.TabPages[e.Index];
            bool dangChon = e.Index == tabBaoCao.SelectedIndex;
            Color mauNhan = MauTab(e.Index);

            using var brushNen = new SolidBrush(dangChon ? Color.White : Color.FromArgb(238, 240, 244));
            e.Graphics.FillRectangle(brushNen, e.Bounds);

            using var font = new Font("Segoe UI", 9.5F, dangChon ? FontStyle.Bold : FontStyle.Regular);
            using var brushChu = new SolidBrush(dangChon ? mauNhan : Color.FromArgb(95, 99, 104));
            using var fmt = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            e.Graphics.DrawString(trang.Text, font, brushChu, e.Bounds, fmt);

            if (dangChon)
            {
                using var penGach = new Pen(mauNhan, 3F);
                e.Graphics.DrawLine(penGach, e.Bounds.Left + 12, e.Bounds.Bottom - 2,
                    e.Bounds.Right - 12, e.Bounds.Bottom - 2);
            }
        }

        // ---------------------------------------------------------------
        // Tab 1: SINH VIÊN (UC17) - toàn bộ danh sách sinh viên + trạng thái ở.
        // ---------------------------------------------------------------
        private void NapThongKeSinhVien(AppDbContext db)
        {
            var dangOTheoMaSv = db.DangKyOs
                .Where(d => d.TrangThai == TrangThaiDangKy.DaDuyet)
                .ToList()
                .GroupBy(d => d.MaSv)
                .ToDictionary(g => g.Key!, g => g.First().MaPhong ?? "");

            var danhSachSv = db.SinhViens.OrderBy(sv => sv.MaSv).ToList();

            dgvSinhVien.DataSource = danhSachSv.Select(sv => new
            {
                MaSv = sv.MaSv,
                HoTen = sv.HoTen,
                GioiTinh = sv.GioiTinh ?? "",
                Lop = sv.Lop ?? "",
                SoDienThoai = sv.SoDienThoai ?? "",
                QueQuan = sv.QueQuan ?? "",
                TrangThaiO = dangOTheoMaSv.ContainsKey(sv.MaSv) ? "Đang ở" : "Chưa ở",
                PhongHienTai = dangOTheoMaSv.TryGetValue(sv.MaSv, out var phong) ? phong : ""
            }).ToList();

            int tongSo = danhSachSv.Count;
            int soNam = danhSachSv.Count(sv => sv.GioiTinh == "Nam");
            int soNu = danhSachSv.Count(sv => sv.GioiTinh == "Nữ");
            int soDangO = dangOTheoMaSv.Count;

            lblSinhVien.Text = $"Tổng số sinh viên: {tongSo:N0}  |  Nam: {soNam:N0}  -  Nữ: {soNu:N0}  |  " +
                $"Đang ở: {soDangO:N0}  -  Chưa ở: {tongSo - soDangO:N0}";
        }

        // ---------------------------------------------------------------
        // Tab 2: PHÒNG (UC18) - toàn bộ danh sách phòng, khu nhà, loại phòng, sức chứa.
        // ---------------------------------------------------------------
        private void NapThongKePhong(AppDbContext db)
        {
            var danhSachPhong = db.Phongs
                .Include(p => p.MaKhuNavigation)
                .Include(p => p.MaLoaiNavigation)
                .OrderBy(p => p.MaPhong)
                .ToList();

            dgvPhong.DataSource = danhSachPhong.Select(p =>
            {
                int toiDa = p.SoLuongToiDa ?? 0;
                int hienTai = p.SoLuongHienTai ?? 0;
                return new
                {
                    MaPhong = p.MaPhong,
                    KhuNha = p.MaKhuNavigation?.TenKhu ?? "",
                    LoaiPhong = p.MaLoaiNavigation?.TenLoai ?? "",
                    DonGia = p.MaLoaiNavigation?.DonGia?.ToString("N0") + " đ",
                    SucChua = toiDa,
                    DangO = hienTai,
                    ConTrong = Math.Max(0, toiDa - hienTai),
                    TyLeLapDay = toiDa > 0 ? ((double)hienTai / toiDa * 100).ToString("N0") + "%" : "0%"
                };
            }).ToList();

            int tongPhong = danhSachPhong.Count;
            int tongSucChua = danhSachPhong.Sum(p => p.SoLuongToiDa ?? 0);
            int tongDangO = danhSachPhong.Sum(p => p.SoLuongHienTai ?? 0);
            int soPhongDayHoanToan = danhSachPhong.Count(p => (p.SoLuongHienTai ?? 0) >= (p.SoLuongToiDa ?? 0) && (p.SoLuongToiDa ?? 0) > 0);

            lblPhong.Text = $"Tổng số phòng: {tongPhong:N0}  |  Tổng sức chứa: {tongSucChua:N0}  |  " +
                $"Đang ở: {tongDangO:N0}  |  Phòng đã đầy: {soPhongDayHoanToan:N0}";
        }

        // ---------------------------------------------------------------
        // Tab 3: HỢP ĐỒNG & HÓA ĐƠN - toàn bộ hợp đồng, tóm tắt hóa đơn/thanh toán.
        // ---------------------------------------------------------------
        private void NapHopDongHoaDon(AppDbContext db)
        {
            var danhSachHopDong = db.HopDongs
                .Include(h => h.MaSvNavigation)
                .OrderByDescending(h => h.NgayLap)
                .ToList();

            dgvHopDong.DataSource = danhSachHopDong.Select(h => new
            {
                MaHopDong = h.MaHopDong,
                MaSv = h.MaSv,
                HoTen = h.MaSvNavigation?.HoTen ?? "",
                MaPhong = h.MaPhong,
                NgayLap = h.NgayLap.ToString("dd/MM/yyyy"),
                NgayHetHan = h.NgayHetHan.ToString("dd/MM/yyyy"),
                TrangThai = h.TrangThai
            }).ToList();

            int tongHopDong = danhSachHopDong.Count;
            int conHieuLuc = danhSachHopDong.Count(h => h.TrangThai == "Còn hiệu lực");

            int tongHoaDon = db.HoaDons.Count();
            int daThanhToan = db.HoaDons.Count(h => h.TrangThai == TrangThaiHoaDon.DaThanhToan);
            int choXacNhan = db.HoaDons.Count(h => h.TrangThai == TrangThaiHoaDon.ChoXacNhan);
            int chuaThanhToan = db.HoaDons.Count(h => h.TrangThai == TrangThaiHoaDon.ChuaThanhToan);

            int tongThanhToan = db.ThanhToans.Count();
            int ttChoXacNhan = db.ThanhToans.Count(t => t.TrangThai == TrangThaiThanhToan.ChoXacNhan);

            lblHopDong.Text = $"Hợp đồng: {tongHopDong:N0} (còn hiệu lực: {conHieuLuc:N0})  |  " +
                $"Hóa đơn: {tongHoaDon:N0} (đã TT: {daThanhToan:N0}, chờ xác nhận: {choXacNhan:N0}, chưa TT: {chuaThanhToan:N0})  |  " +
                $"Giao dịch thanh toán: {tongThanhToan:N0} (chờ xác nhận: {ttChoXacNhan:N0})";
        }

        // ---------------------------------------------------------------
        // Tab 4: DOANH THU (UC19) - toàn bộ hóa đơn nhóm theo tháng/năm.
        // ---------------------------------------------------------------
        private void NapDoanhThu(AppDbContext db)
        {
            var danhSachHoaDon = db.HoaDons.ToList();

            var theoThang = danhSachHoaDon
                .GroupBy(h => new { h.Nam, h.Thang })
                .OrderByDescending(g => g.Key.Nam).ThenByDescending(g => g.Key.Thang)
                .Select(g => new
                {
                    ThangNam = $"{g.Key.Thang:00}/{g.Key.Nam}",
                    SoHoaDon = g.Count(),
                    TongPhaiThu = g.Sum(h => h.TongTien),
                    DaThu = g.Where(h => h.TrangThai == TrangThaiHoaDon.DaThanhToan).Sum(h => h.TongTien),
                })
                .Select(x => new
                {
                    ThangNam = x.ThangNam,
                    SoHoaDon = x.SoHoaDon,
                    TongPhaiThu = x.TongPhaiThu.ToString("N0") + " đ",
                    DaThu = x.DaThu.ToString("N0") + " đ",
                    ChuaThu = (x.TongPhaiThu - x.DaThu).ToString("N0") + " đ"
                })
                .ToList();

            dgvDoanhThu.DataSource = theoThang;

            decimal tongPhaiThu = danhSachHoaDon.Sum(h => h.TongTien);
            decimal tongDaThu = danhSachHoaDon.Where(h => h.TrangThai == TrangThaiHoaDon.DaThanhToan).Sum(h => h.TongTien);

            lblDoanhThu.Text = $"Tổng số tiền phải thu (toàn hệ thống, mọi tháng): {tongPhaiThu.ToString("N0")} đ  |  " +
                $"Đã thu: {tongDaThu.ToString("N0")} đ  |  Còn phải thu: {(tongPhaiThu - tongDaThu).ToString("N0")} đ";

            NapBangDieuKhienDoanhThu(db, danhSachHoaDon);
        }

        /// <summary>
        /// Nạp dữ liệu cho khối "bảng điều khiển" trực quan ở đầu tab Doanh thu: 3 thẻ KPI,
        /// biểu đồ cột doanh thu 5 tháng gần nhất, và tình trạng lấp đầy theo khu nhà.
        /// "Doanh thu" ở đây là số tiền ĐÃ THU (hóa đơn đã thanh toán), thống nhất với cách
        /// tính "doanh thu đã thu" đang dùng ở các nơi khác trong màn hình báo cáo này.
        /// </summary>
        private void NapBangDieuKhienDoanhThu(AppDbContext db, System.Collections.Generic.List<HoaDon> danhSachHoaDon)
        {
            var homNay = DateTime.Today;
            int thangHt = homNay.Month, namHt = homNay.Year;
            int thangTruoc = thangHt == 1 ? 12 : thangHt - 1;
            int namThangTruoc = thangHt == 1 ? namHt - 1 : namHt;

            decimal DoanhThuThang(int thang, int nam) => danhSachHoaDon
                .Where(h => h.Thang == thang && h.Nam == nam && h.TrangThai == TrangThaiHoaDon.DaThanhToan)
                .Sum(h => (decimal?)h.TongTien) ?? 0;

            // ----- Thẻ 1: Doanh thu tháng này, so với tháng trước -----
            decimal dtThangNay = DoanhThuThang(thangHt, namHt);
            decimal dtThangTruoc = DoanhThuThang(thangTruoc, namThangTruoc);
            double phanTramDoi = dtThangTruoc > 0 ? (double)((dtThangNay - dtThangTruoc) / dtThangTruoc) * 100.0 : 0;

            cardDoanhThuThangNay.TieuDe = "Doanh thu tháng này";
            cardDoanhThuThangNay.GiaTri = dtThangNay.ToString("N0") + "đ";
            cardDoanhThuThangNay.MoTa = dtThangTruoc > 0
                ? $"{(phanTramDoi >= 0 ? "▲" : "▼")} {Math.Abs(phanTramDoi):N1}% so với tháng trước"
                : "Chưa có dữ liệu tháng trước để so sánh";
            cardDoanhThuThangNay.Invalidate();

            // ----- Thẻ 2: Hóa đơn chưa thanh toán, trong đó có bao nhiêu hóa đơn quá hạn -----
            // Quá hạn = hóa đơn của một kỳ (tháng/năm) đã qua nhưng vẫn chưa thanh toán xong.
            int hoaDonChuaTT = danhSachHoaDon.Count(h => h.TrangThai != TrangThaiHoaDon.DaThanhToan);
            int hoaDonQuaHan = danhSachHoaDon.Count(h => h.TrangThai != TrangThaiHoaDon.DaThanhToan &&
                (h.Nam < namHt || (h.Nam == namHt && h.Thang < thangHt)));

            cardHoaDonChuaTT.TieuDe = "Hóa đơn chưa thanh toán";
            cardHoaDonChuaTT.GiaTri = hoaDonChuaTT.ToString("N0");
            cardHoaDonChuaTT.MoTa = hoaDonQuaHan > 0
                ? $"▼ {hoaDonQuaHan:N0} hóa đơn quá hạn"
                : "Không có hóa đơn quá hạn";
            cardHoaDonChuaTT.Invalidate();

            // ----- Thẻ 3: Đăng ký chờ duyệt, trong đó bao nhiêu cần xử lý gấp trong tuần -----
            // "Cần xử lý trong tuần" = đăng ký chờ duyệt có ngày bắt đầu ở (dự kiến nhận phòng)
            // rơi vào 7 ngày tới, vì DangKyO không có sẵn cột ngày nộp đơn để lọc theo đó.
            int dangKyChoDuyet = db.DangKyOs.Count(d => d.TrangThai == TrangThaiDangKy.ChoDuyet);
            var moc7Ngay = DateOnly.FromDateTime(homNay.AddDays(7));
            int dangKyGapTrongTuan = db.DangKyOs.Count(d => d.TrangThai == TrangThaiDangKy.ChoDuyet &&
                d.NgayBatDau != null && d.NgayBatDau <= moc7Ngay);

            cardDangKyChoDuyetTab.TieuDe = "Đăng ký chờ duyệt";
            cardDangKyChoDuyetTab.GiaTri = dangKyChoDuyet.ToString("N0");
            cardDangKyChoDuyetTab.MoTa = $"{dangKyGapTrongTuan:N0} cần xử lý trong tuần";
            cardDangKyChoDuyetTab.Invalidate();

            // ----- Biểu đồ cột: doanh thu đã thu 5 tháng gần nhất, tháng hiện tại tô nổi bật -----
            var duLieuBieuDo = new System.Collections.Generic.List<(string Nhan, decimal GiaTri)>();
            for (int i = 4; i >= 0; i--)
            {
                var moc = homNay.AddMonths(-i);
                duLieuBieuDo.Add(($"T{moc.Month}", DoanhThuThang(moc.Month, moc.Year)));
            }
            bieuDoDoanhThu.SetDuLieu(duLieuBieuDo, duLieuBieuDo.Count - 1);

            // ----- Tình trạng lấp đầy theo từng khu nhà -----
            var lapDayTheoKhu = db.KhuNhas
                .Include(k => k.Phongs)
                .OrderBy(k => k.TenKhu)
                .ToList()
                .Select(k =>
                {
                    int sucChua = k.Phongs.Sum(p => p.SoLuongToiDa ?? 0);
                    int dangO = k.Phongs.Sum(p => p.SoLuongHienTai ?? 0);
                    double tyLe = sucChua > 0 ? (double)dangO / sucChua * 100.0 : 0;
                    return (TenKhu: k.TenKhu, TyLeLapDay: tyLe);
                })
                .ToList();

            dsLapDayKhu.SetDuLieu(lapDayTheoKhu);

            // Thẻ "Tình trạng lấp đầy theo khu nhà" có chiều cao CỐ ĐỊNH (Designer) đủ hiển
            // thị vài khu, không co giãn theo đúng số khu nhà thực tế trong CSDL nữa - vì số
            // khu nhà có thể lên tới hàng chục (CSDL mẫu hiện có 20 khu), nếu co giãn theo sẽ
            // đẩy bảng dgvDoanhThu bên dưới ra ngoài vùng nhìn thấy mà không cuộn tới được.
            // Danh sách vẫn hiển thị ĐẦY ĐỦ mọi khu nhà, chỉ là cuộn được bên trong thẻ
            // (xem pnlLapDayKhuScroll ở Designer). Vì vậy pnlDoanhThuDashboard cũng giữ
            // nguyên chiều cao cố định đã tính sẵn ở Designer, không cần tính lại ở đây.
        }

        // ---------------------------------------------------------------
        // Tab 5: NHÂN VIÊN - toàn bộ danh sách nhân viên ký túc xá.
        // ---------------------------------------------------------------
        private void NapNhanVien(AppDbContext db)
        {
            var danhSachNv = db.NhanViens.OrderBy(nv => nv.MaNv).ToList();

            dgvNhanVien.DataSource = danhSachNv.Select(nv => new
            {
                MaNv = nv.MaNv,
                HoTen = nv.HoTen,
                GioiTinh = nv.GioiTinh ?? "",
                SoDienThoai = nv.SoDienThoai ?? "",
                Email = nv.Email ?? "",
                ChucVu = nv.ChucVu ?? ""
            }).ToList();

            lblNhanVien.Text = $"Tổng số nhân viên: {danhSachNv.Count:N0}";
        }

        // ---------------------------------------------------------------
        // UC20 - Xuất báo cáo: chỉ xuất dữ liệu của TAB đang được chọn ra 1 file Excel
        // (VD: đang ở tab "Sinh viên" thì xuất đúng danh sách sinh viên, đang ở tab
        // "Tổng quan hệ thống" thì xuất đúng bảng số liệu tổng quan...).
        // ---------------------------------------------------------------
        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            var (tenSheet, dgv, moTaTrang) = LayThongTinTrangDangChon();

            using var hopThoaiLuu = new SaveFileDialog
            {
                Title = $"Xuất báo cáo {moTaTrang} ra Excel",
                Filter = "Excel Workbook (*.xlsx)|*.xlsx",
                FileName = $"BaoCao_{tenSheet.Replace(" ", "")}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
            };

            if (hopThoaiLuu.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                using var wb = new XLWorkbook();

                GhiSheet(wb, tenSheet, dgv);

                wb.SaveAs(hopThoaiLuu.FileName);

                GhiNhatKyBaoCao(moTaTrang);

                bool muonMo = ThongBao.InfoCoNutXem(this,
                    $"Đã xuất báo cáo {moTaTrang} ra file Excel thành công.",
                    "Xuất báo cáo", "Mở file");

                if (muonMo)
                {
                    Process.Start(new ProcessStartInfo(hopThoaiLuu.FileName) { UseShellExecute = true });
                }
            }
            catch (Exception ex)
            {
                ThongBao.Loi("Xuất Excel thất bại: " + ex.Message);
            }
        }

        /// <summary>Ứng với TAB đang được chọn trên tabBaoCao, trả về (tên sheet Excel, bảng
        /// dữ liệu cần xuất, mô tả ngắn dùng trong thông báo/tên file). Mặc định (không khớp
        /// tab nào, VD tab 0) là "Tổng quan hệ thống".</summary>
        private (string TenSheet, DataGridView Dgv, string MoTa) LayThongTinTrangDangChon() => tabBaoCao.SelectedIndex switch
        {
            1 => ("Sinh vien", dgvSinhVien, "sinh viên"),
            2 => ("Phong", dgvPhong, "phòng"),
            3 => ("Hop dong - Hoa don", dgvHopDong, "hợp đồng & hóa đơn"),
            4 => ("Doanh thu", dgvDoanhThu, "doanh thu"),
            5 => ("Nhan vien", dgvNhanVien, "nhân viên"),
            _ => ("Tong quan he thong", dgvTongQuan, "tổng quan hệ thống"),
        };

        /// <summary>Ghi toàn bộ dữ liệu đang hiển thị trên 1 DataGridView vào 1 sheet Excel mới.</summary>
        private static void GhiSheet(XLWorkbook wb, string tenSheet, DataGridView dgv)
        {
            var ws = wb.Worksheets.Add(tenSheet);

            for (int c = 0; c < dgv.Columns.Count; c++)
            {
                ws.Cell(1, c + 1).Value = dgv.Columns[c].HeaderText;
                ws.Cell(1, c + 1).Style.Font.Bold = true;
            }

            for (int r = 0; r < dgv.Rows.Count; r++)
            {
                for (int c = 0; c < dgv.Columns.Count; c++)
                {
                    ws.Cell(r + 2, c + 1).Value = dgv.Rows[r].Cells[c].Value?.ToString() ?? "";
                }
            }

            ws.Columns().AdjustToContents();
        }

        /// <summary>Ghi 1 dòng lịch sử vào bảng BaoCao (UC20), gắn theo đúng tên tab vừa xuất.
        /// NguoiLap để trống vì tài khoản Quản trị viên hiện chưa gắn với một hồ sơ Nhân
        /// viên (NhanVien.MaNV) cụ thể.</summary>
        private static void GhiNhatKyBaoCao(string moTaTrang)
        {
            try
            {
                using var db = new AppDbContext();
                db.BaoCaos.Add(new BaoCao
                {
                    TenBaoCao = $"Báo cáo thống kê - {char.ToUpperInvariant(moTaTrang[0])}{moTaTrang[1..]}",
                    NgayLap = DateOnly.FromDateTime(DateTime.Today),
                    NguoiLap = null
                });
                db.SaveChanges();
            }
            catch
            {
                // Ghi nhật ký báo cáo là phụ trợ - nếu thất bại (VD: CSDL cũ chưa kịp có bảng
                // BaoCao) thì KHÔNG chặn việc xuất Excel đã thành công ở trên.
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
