using System;
using System.Linq;
using System.Windows.Forms;
using DoAnNhom06.Helpers;
using DoAnNhom06.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// UC08 - Thanh toán phí: chỉ Sinh viên dùng màn hình này (xem VaiTro.CoQuyenThanhToanPhi).
    /// Hệ thống tự khóa theo đúng tài khoản đang đăng nhập để hiện các hóa đơn chưa thanh toán
    /// của CHÍNH sinh viên đó - không cho gõ tay Mã số sinh viên để xem/thanh toán hộ người khác.
    ///
    /// Giao dịch thanh toán được ghi nhận ở trạng thái "Chờ xác nhận"; hóa đơn cũng
    /// chuyển sang "Chờ xác nhận" và chỉ được đánh dấu "Đã thanh toán" sau khi Nhân
    /// viên xác nhận ở màn hình Xác nhận thanh toán (UC09).
    /// </summary>
    public partial class FrmThanhToanPhi : BaseAppForm
    {
        private const string TienToMaThanhToan = "TT";
        private const string ColXemChiTiet = "colXemChiTiet";
        private readonly TaiKhoan _taiKhoanDangNhap;

        public FrmThanhToanPhi(TaiKhoan taiKhoanDangNhap)
        {
            InitializeComponent();
            _taiKhoanDangNhap = taiKhoanDangNhap;
        }

        private void FrmThanhToanPhi_Load(object sender, EventArgs e)
        {
            // Ẩn hẳn ô nhập Mã số sinh viên: sinh viên chỉ được xem/thanh toán hóa đơn của chính mình.
            lblMaSv.Text = "Hóa đơn của bạn:";
            txtMaSv.Visible = false;
            btnTimKiem.Visible = false;

            if (string.IsNullOrEmpty(_taiKhoanDangNhap.MaSv))
            {
                ThongBao.CanhBao(
                    "Tài khoản của bạn chưa được liên kết với hồ sơ sinh viên nào.\n" +
                    "Vui lòng liên hệ nhân viên quản lý ký túc xá.",
                    "Chưa liên kết hồ sơ");
                return;
            }

            txtMaSv.Text = _taiKhoanDangNhap.MaSv;
            TaiDanhSach(_taiKhoanDangNhap.MaSv);
        }

        private void TaiDanhSach(string maSv)
        {
            using var db = new AppDbContext();

            var danhSach = db.HoaDons
                .Include(h => h.MaHopDongNavigation)
                .Where(h => h.TrangThai == TrangThaiHoaDon.ChuaThanhToan
                    && h.MaHopDongNavigation != null
                    && h.MaHopDongNavigation.MaSv == maSv)
                .OrderBy(h => h.NgayLap)
                .ToList();

            dgvHoaDon.DataSource = danhSach.Select(h => new
            {
                MaHoaDon = h.MaHoaDon,
                MaHopDong = h.MaHopDong,
                MaPhong = h.MaHopDongNavigation?.MaPhong,
                Thang = h.Thang,
                Nam = h.Nam,
                TongTien = h.TongTien.ToString("N0") + " đ",
                NgayLap = h.NgayLap.ToString("dd/MM/yyyy"),
                TrangThai = TrangThaiHoaDon.HienThi(h.TrangThai)
            }).ToList();

            lblGhiChu.Text = danhSach.Count > 0
                ? $"Tìm thấy {danhSach.Count} hóa đơn chưa thanh toán. Chọn 1 dòng, chọn phương thức rồi bấm \"Thanh toán\"."
                : "Không tìm thấy hóa đơn chưa thanh toán nào cho mã sinh viên này.";
        }

        /// <summary>Sau khi bảng hóa đơn tải/lọc lại dữ liệu xong, thêm cột nút "Xem chi tiết"
        /// vào cuối bảng để sinh viên xem chi tiết từng dòng phí của hóa đơn (kể cả khi
        /// hóa đơn CHƯA thanh toán) mà không cần thanh toán trước.</summary>
        private void dgvHoaDon_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgvHoaDon.Columns.Count == 0) return; // chưa có dữ liệu/cột nào thì chưa thêm nút

            if (dgvHoaDon.Columns.Contains(ColXemChiTiet))
            {
                dgvHoaDon.Columns[ColXemChiTiet].DisplayIndex = dgvHoaDon.Columns.Count - 1;
                return;
            }

            var cot = new DataGridViewButtonColumn
            {
                Name = ColXemChiTiet,
                HeaderText = "",
                Text = "Xem chi tiết",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat,
                Width = 110,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            dgvHoaDon.Columns.Add(cot);
            cot.DisplayIndex = dgvHoaDon.Columns.Count - 1;
        }

        /// <summary>Bấm nút "Xem chi tiết" trên một dòng hóa đơn -> mở hộp thoại chi tiết hóa đơn
        /// (đầy đủ các dòng phí điện, nước, internet...), không phụ thuộc hóa đơn đã thanh toán hay chưa.</summary>
        private void dgvHoaDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvHoaDon.Columns[e.ColumnIndex].Name != ColXemChiTiet) return;

            var maHoaDon = dgvHoaDon.Rows[e.RowIndex].Cells["MaHoaDon"].Value?.ToString();
            if (string.IsNullOrEmpty(maHoaDon)) return;

            using var frmChiTiet = new FrmChiTietHoaDon(maHoaDon);
            frmChiTiet.ShowDialog(this);
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string maSv = txtMaSv.Text.Trim();
            if (string.IsNullOrEmpty(maSv))
            {
                ThongBao.CanhBao("Vui lòng nhập Mã số sinh viên.");
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

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow == null)
            {
                ThongBao.CanhBao("Vui lòng chọn một hóa đơn trong danh sách.");
                return;
            }

            if (cboPhuongThuc.SelectedItem == null)
            {
                ThongBao.CanhBao("Vui lòng chọn phương thức thanh toán.");
                return;
            }

            string maHoaDon = dgvHoaDon.CurrentRow.Cells["MaHoaDon"].Value.ToString()!;
            string phuongThuc = cboPhuongThuc.SelectedItem.ToString()!;

            using var db = new AppDbContext();
            var hoaDon = db.HoaDons.Include(h => h.MaHopDongNavigation).FirstOrDefault(h => h.MaHoaDon == maHoaDon);
            if (hoaDon == null)
            {
                ThongBao.Loi("Không tìm thấy hóa đơn (có thể vừa bị thay đổi).");
                TaiDanhSach(txtMaSv.Text.Trim());
                return;
            }

            // Phòng thủ thêm: chỉ được thanh toán hóa đơn thuộc đúng hợp đồng của chính mình.
            if (hoaDon.MaHopDongNavigation?.MaSv != _taiKhoanDangNhap.MaSv)
            {
                ThongBao.CanhBao("Bạn không có quyền thanh toán hóa đơn này.");
                return;
            }

            // Luồng ngoại lệ: hóa đơn đã được thanh toán (hoặc đang chờ xác nhận) từ trước.
            if (hoaDon.TrangThai != TrangThaiHoaDon.ChuaThanhToan)
            {
                ThongBao.Info("Hóa đơn này đã được thanh toán hoặc đang chờ xác nhận rồi.");
                TaiDanhSach(txtMaSv.Text.Trim());
                return;
            }

            var giaoDich = new ThanhToan
            {
                MaThanhToan = MaSoHelper.SinhMaMoi(db.ThanhToans.Select(t => t.MaThanhToan), TienToMaThanhToan),
                MaHoaDon = hoaDon.MaHoaDon,
                NgayThanhToan = DateOnly.FromDateTime(DateTime.Today),
                SoTien = hoaDon.TongTien,
                PhuongThuc = phuongThuc,
                TrangThai = TrangThaiThanhToan.ChoXacNhan
            };

            hoaDon.TrangThai = TrangThaiHoaDon.ChoXacNhan;
            db.ThanhToans.Add(giaoDich);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Luồng ngoại lệ: thanh toán thất bại.
                ThongBao.Loi("Thanh toán thất bại: " + ex.Message);
                return;
            }

            ThongBao.Info(
                $"Đã ghi nhận thanh toán {hoaDon.TongTien:N0} đ cho hóa đơn {hoaDon.MaHoaDon}.\n" +
                "Vui lòng chờ nhân viên xác nhận giao dịch.");
            TaiDanhSach(txtMaSv.Text.Trim());
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
