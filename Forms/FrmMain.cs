using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DoAnNhom06.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Forms
{
    public partial class FrmMain : Form
    {
        private readonly TaiKhoan _taiKhoanDangNhap;

        /// <summary>
        /// True nếu người dùng chọn Đăng xuất (Program.cs sẽ mở lại màn hình đăng nhập).
        /// False nếu người dùng Thoát hẳn ứng dụng.
        /// </summary>
        public bool YeuCauDangXuat { get; private set; }

        public FrmMain(TaiKhoan taiKhoanDangNhap)
        {
            InitializeComponent();

            _taiKhoanDangNhap = taiKhoanDangNhap;

            lblTaiKhoan.Text = $"Bạn đăng nhập: {_taiKhoanDangNhap.TenDangNhap}{Environment.NewLine}({_taiKhoanDangNhap.Quyen})";

            string? quyen = _taiKhoanDangNhap.Quyen;

            bool laAdmin = quyen == "Quản trị viên";
            bool laNhanVien = quyen == "Nhân viên";
            bool laSinhVien = quyen == "Sinh viên";

            // Nhân viên
            btnQuanLySinhVien.Visible = laNhanVien;
            btnQuanLyPhong.Visible = laNhanVien;
            btnDuyetDangKy.Visible = laNhanVien;
            btnQuanLyHopDong.Visible = laNhanVien;
            btnXacNhanThanhToan.Visible = laNhanVien;

            // Sinh viên
            btnDangKyO.Visible = laSinhVien;
            btnThanhToanPhi.Visible = laSinhVien;
            btnPhongCuaToi.Visible = laSinhVien;
            btnBaoHong.Visible = laSinhVien;
            btnXuLyBaoHong.Visible = laNhanVien;
            btnXinChuyenPhong.Visible = laSinhVien;
            btnDuyetChuyenPhong.Visible = laNhanVien;

            // Sinh viên + Nhân viên
            btnGiaHanHopDong.Visible = laSinhVien || laNhanVien;
            btnTraCuuThongTin.Visible = laSinhVien || laNhanVien;

            bool coTraCuu = laSinhVien || laNhanVien;

            pnlTraCuu.Visible = coTraCuu;
            tlpBody.RowStyles[0] =
                new RowStyle(SizeType.Absolute, coTraCuu ? 78F : 0F);

            // Chức năng riêng của Quản trị viên
            btnQuanLyTaiKhoan.Visible = laAdmin;
            btnQuanLyTaiKhoan.Visible = quyen == "Quản trị viên";
            btnBaoCaoThongKe.Visible = laAdmin;

            // Phân phòng, In hóa đơn: dùng chung cho Quản trị viên VÀ Nhân viên.
            btnPhanPhongAdmin.Visible = laAdmin || laNhanVien;
            btnInHoaDon.Visible = laAdmin || laNhanVien;

            flpThongKe.Visible = laAdmin;

            tlpBody.RowStyles[1] =
                new RowStyle(SizeType.Absolute, laAdmin ? 116F : 0F);

            if (laAdmin)
            {
                CapNhatChiSoTongQuan();
            }

            // Menu
            danhMucToolStripMenuItem.Visible = laAdmin;
            sinhVienToolStripMenuItem.Visible = laNhanVien;
            phongToolStripMenuItem.Visible = laNhanVien;

            // Thông tin bản thân chỉ dành cho sinh viên
            mnuThongTinBanThan.Visible = laSinhVien;
        }

        /// <summary>
        /// Tính lại 4 số liệu tổng quan trên Trang chủ Quản trị viên: tổng số phòng, số phòng
        /// còn trống, số sinh viên đang ở (= tổng chỗ đang có người ở) và tỷ lệ lấp đầy (%).
        /// Gọi lại mỗi khi số liệu phòng có thể đã thay đổi (VD: sau khi Phân phòng - UC15).
        /// </summary>
        private void CapNhatChiSoTongQuan()
        {
            using var db = new AppDbContext();

            int tongSoPhong = db.Phongs.Count();
            int tongSucChua = db.Phongs.Sum(p => (int?)p.SoLuongToiDa) ?? 0;
            int tongDangO = db.Phongs.Sum(p => (int?)p.SoLuongHienTai) ?? 0;
            int phongConTrong = db.Phongs.Count(p => (p.SoLuongHienTai ?? 0) < (p.SoLuongToiDa ?? 0));
            double tyLeLapDay = tongSucChua > 0 ? (double)tongDangO / tongSucChua * 100.0 : 0;

            cardTongPhong.GiaTri = tongSoPhong.ToString("N0");
            cardTongPhong.MoTa = "Tổng số phòng";

            cardPhongTrong.GiaTri = phongConTrong.ToString("N0");
            cardPhongTrong.MoTa = "Phòng còn trống";

            cardSinhVienDangO.GiaTri = tongDangO.ToString("N0");
            cardSinhVienDangO.MoTa = "Sinh viên đang ở";

            cardTyLeLapDay.GiaTri = tyLeLapDay.ToString("N0") + "%";
            cardTyLeLapDay.MoTa = "Tỷ lệ lấp đầy";

            cardTongPhong.Invalidate();
            cardPhongTrong.Invalidate();
            cardSinhVienDangO.Invalidate();
            cardTyLeLapDay.Invalidate();
        }

        private void lblTaiKhoan_Click(object sender, EventArgs e)
        {
            cmsTaiKhoan.Show(lblTaiKhoan, new Point(0, lblTaiKhoan.Height));
        }

        private void mnuThongTinBanThan_Click(object sender, EventArgs e)
        {
            using var frm = new FrmThongTinCaNhan(_taiKhoanDangNhap);
            frm.ShowDialog(this);
        }

        private void mnuDoiMatKhau_Click(object sender, EventArgs e)
        {
            using var frm = new FrmDoiMatKhau(_taiKhoanDangNhap);
            frm.ShowDialog(this);
        }

        private void mnuDangXuat_Click(object sender, EventArgs e)
        {
            YeuCauDangXuat = true;
            this.Close();
        }

        private void sinhVienToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_taiKhoanDangNhap.Quyen != "Nhân viên")
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.",
                    "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var frm = new FrmDanhSachSinhVien();
            frm.ShowDialog(this);
        }

        private void phongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!VaiTro.CoQuyenQuanLyPhong(_taiKhoanDangNhap.Quyen))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.",
                    "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var frm = new FrmDanhSachPhong();
            frm.ShowDialog(this);
        }

        private void btnDuyetDangKy_Click(object sender, EventArgs e)
        {
            if (!VaiTro.CoQuyenDuyetDangKy(_taiKhoanDangNhap.Quyen))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.",
                    "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var frm = new FrmDuyetDangKy();
            frm.ShowDialog(this);
        }

        private void btnQuanLyHopDong_Click(object sender, EventArgs e)
        {
            if (!VaiTro.CoQuyenQuanLyHopDong(_taiKhoanDangNhap.Quyen))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.",
                    "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var frm = new FrmQuanLyHopDong();
            frm.ShowDialog(this);
        }

        private void btnDangKyO_Click(object sender, EventArgs e)
        {
            if (!VaiTro.CoQuyenDangKyO(_taiKhoanDangNhap.Quyen))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.",
                    "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var frm = new FrmDangKyO(_taiKhoanDangNhap);
            frm.ShowDialog(this);
        }

        private void btnGiaHanHopDong_Click(object sender, EventArgs e)
        {
            if (!VaiTro.CoQuyenGiaHanHopDong(_taiKhoanDangNhap.Quyen))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.",
                    "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var frm = new FrmGiaHanHopDong(_taiKhoanDangNhap);
            frm.ShowDialog(this);
        }

        private void btnThanhToanPhi_Click(object sender, EventArgs e)
        {
            if (!VaiTro.CoQuyenThanhToanPhi(_taiKhoanDangNhap.Quyen))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.",
                    "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Nếu sinh viên không còn hóa đơn nào ở trạng thái "Chưa thanh toán" (đã nộp hết,
            // hoặc đang chờ nhân viên xác nhận), báo ngay "đã nộp xong" thay vì mở một màn
            // hình thanh toán trống rỗng.
            //
            // LƯU Ý: "không còn hóa đơn CHƯA thanh toán" KHÔNG đồng nghĩa với "đã nộp xong
            // hết phí" - sinh viên có thể đơn giản là CHƯA TỪNG có hóa đơn nào (ví dụ đăng ký
            // ở còn đang chờ duyệt, hoặc chưa đăng ký ở lần nào), lúc đó nói "đã nộp xong" là
            // sai và gây hiểu nhầm. Phải phân biệt rõ 2 trường hợp này.
            if (!string.IsNullOrEmpty(_taiKhoanDangNhap.MaSv))
            {
                using var db = new AppDbContext();

                bool coHoaDonNaoKhong = db.HoaDons.Any(h =>
                    h.MaHopDongNavigation != null &&
                    h.MaHopDongNavigation.MaSv == _taiKhoanDangNhap.MaSv);

                if (!coHoaDonNaoKhong)
                {
                    // Chưa từng có hóa đơn nào - không phải đã nộp xong, mà là chưa có gì để nộp.
                    MessageBox.Show(
    "Bạn hiện chưa có hóa đơn nào trong hệ thống.\n" +
    "Hóa đơn sẽ được lập sau khi đăng ký ở được duyệt và có hợp đồng.",
    "Thanh toán phí",
    MessageBoxButtons.OK,
    MessageBoxIcon.Information);
                    return;
                }

                bool conNoPhi = db.HoaDons.Any(h =>
                    h.TrangThai == TrangThaiHoaDon.ChuaThanhToan &&
                    h.MaHopDongNavigation != null &&
                    h.MaHopDongNavigation.MaSv == _taiKhoanDangNhap.MaSv);

                if (!conNoPhi)
                {
                    DialogResult ketQua = MessageBox.Show(
    "🎉 Bạn đã nộp xong tất cả các khoản phí!\n" +
    "Hiện không có hóa đơn nào cần thanh toán.\n\n" +
    "Bạn có muốn xem các hóa đơn đã thanh toán không?",
    "Thanh toán phí",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Information);

                    if (ketQua == DialogResult.Yes)
                    {
                        HienThiHoaDonDaThanhToan(_taiKhoanDangNhap.MaSv!);
                    }
                    return;
                }
            }

            using var frm = new FrmThanhToanPhi(_taiKhoanDangNhap);
            frm.ShowDialog(this);
        }

        /// <summary>
        /// Bấm nút "Xem" trên hộp thoại "Đã nộp xong tất cả các khoản phí" -> mở danh sách
        /// các hóa đơn đã thanh toán / đang chờ xác nhận của chính sinh viên đang đăng nhập.
        /// </summary>
        private void HienThiHoaDonDaThanhToan(string maSv)
        {
            using var db = new AppDbContext();

            var sv = db.SinhViens.FirstOrDefault(s => s.MaSv == maSv);

            var dsHoaDon = db.HoaDons
                .Include(h => h.MaHopDongNavigation)
                .Where(h => h.MaHopDongNavigation != null
                    && h.MaHopDongNavigation.MaSv == maSv
                    && h.TrangThai != TrangThaiHoaDon.ChuaThanhToan)
                .OrderByDescending(h => h.Nam).ThenByDescending(h => h.Thang)
                .Select(h => new
                {
                    MaHoaDon = h.MaHoaDon,
                    ThangNam = $"{h.Thang:00}/{h.Nam}",
                    TongTien = h.TongTien.ToString("N0") + " đ",
                    NgayLap = h.NgayLap.ToString("dd/MM/yyyy"),
                    TrangThai = TrangThaiHoaDon.HienThi(h.TrangThai)
                })
                .ToList()
                .Cast<object>()
                .ToList();

            var thongTin = new List<(string, string)>
            {
                ("Mã sinh viên", maSv),
                ("Họ tên", sv?.HoTen ?? "")
            };

            using var frmChiTiet = new FrmChiTietTraCuu($"Hóa đơn đã thanh toán - {maSv}",
                thongTin, "Danh sách hóa đơn", dsHoaDon, coNutXemHoaDon: true);
            frmChiTiet.ShowDialog(this);
        }

        private void btnXacNhanThanhToan_Click(object sender, EventArgs e)
        {
            if (!VaiTro.CoQuyenXacNhanThanhToan(_taiKhoanDangNhap.Quyen))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.",
                    "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var frm = new FrmXacNhanThanhToan();
            frm.ShowDialog(this);
        }

        private void btnTraCuuThongTin_Click(object sender, EventArgs e)
        {
            if (!VaiTro.CoQuyenTraCuuThongTin(_taiKhoanDangNhap.Quyen))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.",
                    "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var frm = new FrmTraCuuThongTin(_taiKhoanDangNhap);
            frm.ShowDialog(this);
        }

        // ---------------------------------------------------------------
        // Chức năng RIÊNG của Sinh viên: xem phòng của mình (bản rút gọn,
        // chỉ đọc, của UC15 - chỉ xem, không chuyển). Khác với màn hình Quản
        // trị viên/Nhân viên, chức năng này CHỈ hiển thị đúng dữ liệu của
        // tài khoản đang đăng nhập - không cho xem/thao tác hộ người khác.
        // ---------------------------------------------------------------

        private void btnPhongCuaToi_Click(object sender, EventArgs e)
        {
            if (!VaiTro.CoQuyenXemPhongCuaMinh(_taiKhoanDangNhap.Quyen))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.",
                    "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(_taiKhoanDangNhap.MaSv))
            {
                MessageBox.Show(
                    "Tài khoản của bạn chưa được liên kết với hồ sơ sinh viên nào.\n" +
                    "Vui lòng liên hệ nhân viên quản lý ký túc xá.",
                    "Chưa liên kết hồ sơ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var db = new AppDbContext();

            // Chỉ lấy đăng ký ở ĐÃ ĐƯỢC DUYỆT của chính tài khoản đang đăng nhập.
            var dangKy = db.DangKyOs
                .Include(d => d.MaPhongNavigation)
                    .ThenInclude(p => p!.MaKhuNavigation)
                .Include(d => d.MaPhongNavigation)
                    .ThenInclude(p => p!.MaLoaiNavigation)
                .Where(d => d.MaSv == _taiKhoanDangNhap.MaSv && d.TrangThai == TrangThaiDangKy.DaDuyet)
                .OrderByDescending(d => d.MaDangKy)
                .FirstOrDefault();

            if (dangKy == null)
            {
                MessageBox.Show(
                    "Bạn hiện chưa được phân phòng nào.\n" +
                    "Phòng sẽ được phân sau khi đăng ký ở của bạn được duyệt.",
                    "Phòng của tôi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var phong = dangKy.MaPhongNavigation;
            decimal? donGia = phong?.MaLoaiNavigation?.DonGia;

            var thongTin = new List<(string, string)>
            {
                ("Mã số sinh viên", _taiKhoanDangNhap.MaSv ?? ""),
                ("Mã phòng", dangKy.MaPhong ?? ""),
                ("Khu nhà", phong?.MaKhuNavigation?.TenKhu ?? ""),
                ("Loại phòng", phong?.MaLoaiNavigation?.TenLoai ?? ""),
                ("Đơn giá", donGia.HasValue ? donGia.Value.ToString("N0") + " đ" : ""),
                ("Sĩ số phòng", $"{phong?.SoLuongHienTai ?? 0}/{phong?.SoLuongToiDa ?? 0}"),
                ("Ngày bắt đầu ở", dangKy.NgayBatDau?.ToString("dd/MM/yyyy") ?? "")
            };

            // Lịch sử phân phòng (bảng PhanPhong) ứng với đăng ký ở hiện tại của sinh viên.
            var lichSuPhanPhong = db.PhanPhongs
                .Where(p => p.MaDangKy == dangKy.MaDangKy)
                .OrderByDescending(p => p.NgayPhan)
                .ToList()
                .Select(p => new
                {
                    NgayPhan = p.NgayPhan.HasValue ? p.NgayPhan.Value.ToString("dd/MM/yyyy") : "",
                    Phong = dangKy.MaPhong ?? ""
                })
                .Cast<object>()
                .ToList();

            using var frmChiTiet = new FrmChiTietTraCuu($"Phòng của tôi - {_taiKhoanDangNhap.MaSv}",
                thongTin, "Lịch sử phân phòng", lichSuPhanPhong);
            frmChiTiet.ShowDialog(this);
        }

        // --- Báo hỏng cơ sở vật chất: Sinh viên gửi yêu cầu, Nhân viên xử lý ---
        private void btnBaoHong_Click(object sender, EventArgs e)
        {
            if (!VaiTro.CoQuyenBaoHong(_taiKhoanDangNhap.Quyen))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.",
                    "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var frm = new FrmBaoHong(_taiKhoanDangNhap);
            frm.ShowDialog(this);
        }

        private void btnXuLyBaoHong_Click(object sender, EventArgs e)
        {
            if (!VaiTro.CoQuyenXuLyBaoHong(_taiKhoanDangNhap.Quyen))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.",
                    "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var frm = new FrmXuLyBaoHong();
            frm.ShowDialog(this);
        }

        // --- Xin chuyển phòng: Sinh viên gửi yêu cầu, Nhân viên duyệt ---
        private void btnXinChuyenPhong_Click(object sender, EventArgs e)
        {
            if (!VaiTro.CoQuyenXinChuyenPhong(_taiKhoanDangNhap.Quyen))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.",
                    "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var frm = new FrmXinChuyenPhong(_taiKhoanDangNhap);
            frm.ShowDialog(this);
        }

        private void btnDuyetChuyenPhong_Click(object sender, EventArgs e)
        {
            if (!VaiTro.CoQuyenDuyetChuyenPhong(_taiKhoanDangNhap.Quyen))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.",
                    "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var frm = new FrmDuyetChuyenPhong();
            frm.ShowDialog(this);
        }



        // ---------------------------------------------------------------
        // UC11 (Báo cáo thống kê), UC12 (Quản lý tài khoản): riêng Quản trị viên.
        // UC15 (Phân phòng), UC16 (In hóa đơn): dùng chung Quản trị viên + Nhân viên
        // (xem VaiTro.CoQuyenPhanPhong / VaiTro.CoQuyenInHoaDon).
        // ---------------------------------------------------------------

        private void btnQuanLyTaiKhoan_Click(object sender, EventArgs e)
        {
            if (!VaiTro.CoQuyenQuanLyTaiKhoan(_taiKhoanDangNhap.Quyen))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.",
                    "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var frm = new FrmQuanLyTaiKhoan(_taiKhoanDangNhap);
            frm.ShowDialog(this);
        }

        private void btnBaoCaoThongKe_Click(object sender, EventArgs e)
        {
            if (!VaiTro.CoQuyenBaoCaoThongKe(_taiKhoanDangNhap.Quyen))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.",
                    "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var frm = new FrmBaoCaoThongKe();
            frm.ShowDialog(this);
        }

        private void btnPhanPhongAdmin_Click(object sender, EventArgs e)
        {
            if (!VaiTro.CoQuyenPhanPhong(_taiKhoanDangNhap.Quyen))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.",
                    "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var frm = new FrmPhanPhong(_taiKhoanDangNhap);
            frm.ShowDialog(this);

            // Số phòng còn trống / tỷ lệ lấp đầy có thể đã thay đổi sau khi chuyển phòng.
            CapNhatChiSoTongQuan();
        }

        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            if (!VaiTro.CoQuyenInHoaDon(_taiKhoanDangNhap.Quyen))
            {
                MessageBox.Show("Bạn không có quyền sử dụng chức năng này.",
                    "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var frm = new FrmInHoaDon();
            frm.ShowDialog(this);
        }

        private void dangXuatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YeuCauDangXuat = true;
            this.Close();
        }

        private void thoatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YeuCauDangXuat = false;
            this.Close();
        }
    }
}
