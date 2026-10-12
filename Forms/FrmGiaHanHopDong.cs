using System;
using System.Linq;
using System.Windows.Forms;
using DoAnNhom06.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// UC07 - Gia hạn hợp đồng: dùng chung cho cả Sinh viên và Nhân viên, nhưng hành vi
    /// khác nhau theo vai trò:
    ///   - Sinh viên: KHÔNG được tự gõ Mã số sinh viên (tránh xem/gia hạn hộ người khác) -
    ///     hệ thống tự khóa theo đúng tài khoản đang đăng nhập và tự tải hợp đồng của mình.
    ///   - Nhân viên: vẫn tra cứu được hợp đồng của bất kỳ sinh viên nào (nhập Mã SV để tìm),
    ///     giống chức năng "Quản lý hợp đồng" nhưng ở dạng rút gọn.
    /// </summary>
    public partial class FrmGiaHanHopDong : Form
    {
        private readonly TaiKhoan _taiKhoanDangNhap;
        private bool LaSinhVien => _taiKhoanDangNhap.Quyen == VaiTro.SinhVien;

        public FrmGiaHanHopDong(TaiKhoan taiKhoanDangNhap)
        {
            InitializeComponent();
            _taiKhoanDangNhap = taiKhoanDangNhap;
        }

        private void FrmGiaHanHopDong_Load(object sender, EventArgs e)
        {
            // Nhân viên/Quản trị viên
            if (!LaSinhVien)
            {
                btnTatCa.Visible = true;
                return;
            }

            // Sinh viên
            btnTatCa.Visible = false;

            // Ẩn ô nhập mã SV và nút tìm kiếm
            lblMaSv.Text = "Hợp đồng của bạn:";
            txtMaSv.Visible = false;
            btnTimKiem.Visible = false;

            if (string.IsNullOrEmpty(_taiKhoanDangNhap.MaSv))
            {
                MessageBox.Show(
                    "Tài khoản của bạn chưa được liên kết với hồ sơ sinh viên nào.\n" +
                    "Vui lòng liên hệ nhân viên quản lý ký túc xá.",
                    "Chưa liên kết hồ sơ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            int soHopDong = TaiDanhSach(_taiKhoanDangNhap.MaSv);

            if (soHopDong == 0)
            {
                MessageBox.Show(
                    "Bạn chưa đăng ký ký túc xá nên không có hợp đồng nào để gia hạn.\n" +
                    "Vui lòng đăng ký ở trước khi sử dụng chức năng này.",
                    "Chưa đăng ký ký túc xá",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                Close();
            }
        }

        /// <summary>Tải danh sách hợp đồng còn hiệu lực của sinh viên; trả về số hợp đồng tìm được.</summary>
        private int TaiDanhSach(string maSv)
        {
            using var db = new AppDbContext();
            var homNay = DateOnly.FromDateTime(DateTime.Today);

            var danhSach = db.DangKyOs
                .Include(d => d.MaPhongNavigation)
                .Where(d => d.MaSv == maSv
                    && d.TrangThai == TrangThaiDangKy.DaDuyet
                    && (d.NgayKetThuc == null || d.NgayKetThuc >= homNay))
                .OrderByDescending(d => d.MaDangKy)
                .ToList();

            dgvHopDong.DataSource = danhSach.Select(d => new
            {
                MaDangKy = d.MaDangKy,
                MaPhong = d.MaPhong,
                NgayBatDau = d.NgayBatDau.HasValue ? d.NgayBatDau.Value.ToString("dd/MM/yyyy") : "",
                NgayKetThuc = d.NgayKetThuc.HasValue ? d.NgayKetThuc.Value.ToString("dd/MM/yyyy") : "Chưa xác định",
            }).ToList();

            lblGhiChu.Text = danhSach.Count > 0
                ? $"Tìm thấy {danhSach.Count} hợp đồng còn hiệu lực. Chọn 1 dòng rồi bấm \"Gia hạn\"."
                : "Không tìm thấy hợp đồng còn hiệu lực nào cho mã sinh viên này.";

            return danhSach.Count;
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string maSv = txtMaSv.Text.Trim();
            if (string.IsNullOrEmpty(maSv))
            {
                MessageBox.Show("Vui lòng nhập Mã số sinh viên.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TaiDanhSach(maSv);
        }

        private void txtMaSv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnTimKiem_Click(sender, e);
            }
        }

        private void btnGiaHan_Click(object sender, EventArgs e)
        {
            if (dgvHopDong.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một hợp đồng trong danh sách.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maDangKy = (int)dgvHopDong.CurrentRow.Cells["MaDangKy"].Value;

            using var db = new AppDbContext();
            var dangKy = db.DangKyOs.FirstOrDefault(d => d.MaDangKy == maDangKy);
            if (dangKy == null)
            {
                MessageBox.Show("Không tìm thấy hợp đồng (có thể vừa bị thay đổi).",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TaiDanhSach(LaSinhVien ? (_taiKhoanDangNhap.MaSv ?? "") : txtMaSv.Text.Trim());
                return;
            }

            // Phòng thủ thêm: sinh viên chỉ được gia hạn đúng hợp đồng của chính mình,
            // kể cả khi dữ liệu trên lưới vì lý do nào đó không còn khớp tài khoản.
            if (LaSinhVien && dangKy.MaSv != _taiKhoanDangNhap.MaSv)
            {
                MessageBox.Show("Bạn không có quyền gia hạn hợp đồng này.",
                    "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string moTa = $"Hợp đồng phòng {dangKy.MaPhong}";
            using var frm = new FrmChonNgayGiaHan(moTa, dangKy.NgayKetThuc);
            if (frm.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            dangKy.NgayKetThuc = frm.NgayKetThucMoi;

            // Đồng bộ hạn hợp đồng (HopDong) tương ứng, tránh lệch dữ liệu giữa
            // đăng ký ở (DangKyO) và hợp đồng dùng cho tra cứu/thanh toán.
            var hopDongLienQuan = db.HopDongs
                .FirstOrDefault(h => h.MaSv == dangKy.MaSv && h.MaPhong == dangKy.MaPhong);
            if (hopDongLienQuan != null)
            {
                hopDongLienQuan.NgayHetHan = frm.NgayKetThucMoi;
                hopDongLienQuan.TrangThai = "Còn hiệu lực";
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gia hạn thất bại: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Đã gia hạn hợp đồng thành công.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            TaiDanhSach(LaSinhVien ? (_taiKhoanDangNhap.MaSv ?? "") : txtMaSv.Text.Trim());
        }
        private void TaiTatCaHopDong()
        {
            using var db = new AppDbContext();

            var homNay = DateOnly.FromDateTime(DateTime.Today);

            var danhSach = db.DangKyOs
                .Include(d => d.MaSvNavigation)
                .Include(d => d.MaPhongNavigation)
                .Where(d => d.TrangThai == TrangThaiDangKy.DaDuyet
                         && (d.NgayKetThuc == null || d.NgayKetThuc >= homNay))
                .OrderBy(d => d.MaPhong)
                .ToList();

            dgvHopDong.DataSource = danhSach.Select(d => new
            {
                MaDangKy = d.MaDangKy,
                MaSV = d.MaSv,
                HoTen = d.MaSvNavigation?.HoTen,
                MaPhong = d.MaPhong,
                NgayBatDau = d.NgayBatDau.HasValue
                    ? d.NgayBatDau.Value.ToString("dd/MM/yyyy")
                    : "",

                NgayKetThuc = d.NgayKetThuc.HasValue
                    ? d.NgayKetThuc.Value.ToString("dd/MM/yyyy")
                    : "Chưa xác định",

                TrangThai = "Còn hiệu lực"
            }).ToList();

            lblGhiChu.Text = $"Có {danhSach.Count} hợp đồng còn hiệu lực.";
        }
        private void btnTatCa_Click(object sender, EventArgs e)
        {
            using var db = new AppDbContext();

            var homNay = DateOnly.FromDateTime(DateTime.Today);

            var danhSach = db.DangKyOs
                .Include(d => d.MaSvNavigation)
                .Include(d => d.MaPhongNavigation)
                .Where(d => d.TrangThai == TrangThaiDangKy.DaDuyet
                         && (d.NgayKetThuc == null || d.NgayKetThuc >= homNay))
                .OrderBy(d => d.MaPhong)
                .ToList();

            dgvHopDong.DataSource = danhSach.Select(d => new
            {
                MaDangKy = d.MaDangKy,
                MaSV = d.MaSv,
                HoTen = d.MaSvNavigation!.HoTen,
                MaPhong = d.MaPhong,
                NgayBatDau = d.NgayBatDau?.ToString("dd/MM/yyyy"),
                NgayKetThuc = d.NgayKetThuc?.ToString("dd/MM/yyyy"),
                TrangThai = "Còn hiệu lực"
            }).ToList();

            lblGhiChu.Text = $"Có {danhSach.Count} hợp đồng còn hiệu lực.";
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblMaSv_Click(object sender, EventArgs e)
        {

        }
    }
}
