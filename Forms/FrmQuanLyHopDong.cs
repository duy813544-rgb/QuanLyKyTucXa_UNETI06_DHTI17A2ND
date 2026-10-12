using System;
using System.Linq;
using System.Windows.Forms;
using DoAnNhom06.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// UC06 - Quản lý hợp đồng: Nhân viên/Quản trị viên xem danh sách các đăng ký
    /// đã được duyệt (coi như "hợp đồng ở"), có thể gia hạn (UC07) hoặc chấm dứt sớm.
    /// </summary>
    public partial class FrmQuanLyHopDong : Form
    {
        public FrmQuanLyHopDong()
        {
            InitializeComponent();
        }

        private void FrmQuanLyHopDong_Load(object sender, EventArgs e)
        {
            TaiDanhSach();
        }

        private void TaiDanhSach(string? tuKhoa = null)
        {
            using var db = new AppDbContext();

            var query = db.DangKyOs
                .Include(d => d.MaSvNavigation)
                .Include(d => d.MaPhongNavigation)
                .Where(d => d.TrangThai == TrangThaiDangKy.DaDuyet)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                tuKhoa = tuKhoa.Trim();
                query = query.Where(d =>
                    (d.MaSv != null && d.MaSv.Contains(tuKhoa)) ||
                    (d.MaPhong != null && d.MaPhong.Contains(tuKhoa)) ||
                    (d.MaSvNavigation != null && d.MaSvNavigation.HoTen.Contains(tuKhoa)));
            }

            var danhSach = query.OrderByDescending(d => d.MaDangKy).ToList();
            var homNay = DateOnly.FromDateTime(DateTime.Today);

            dgvHopDong.DataSource = danhSach.Select(d => new
            {
                MaDangKy = d.MaDangKy,
                MaSv = d.MaSv,
                HoTen = d.MaSvNavigation != null ? d.MaSvNavigation.HoTen : "",
                MaPhong = d.MaPhong,
                NgayBatDau = d.NgayBatDau.HasValue ? d.NgayBatDau.Value.ToString("dd/MM/yyyy") : "",
                NgayKetThuc = d.NgayKetThuc.HasValue ? d.NgayKetThuc.Value.ToString("dd/MM/yyyy") : "Chưa xác định",
                TinhTrang = (d.NgayKetThuc == null || d.NgayKetThuc >= homNay) ? "Còn hiệu lực" : "Đã kết thúc"
            }).ToList();

            lblSoLuong.Text = $"Tổng số: {danhSach.Count} hợp đồng";
        }

        private int? LayMaDangKyDangChon()
        {
            if (dgvHopDong.CurrentRow == null)
            {
                return null;
            }
            return (int)dgvHopDong.CurrentRow.Cells["MaDangKy"].Value;
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            TaiDanhSach(txtTimKiem.Text);
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                TaiDanhSach(txtTimKiem.Text);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = string.Empty;
            TaiDanhSach();
        }

        private void btnGiaHan_Click(object sender, EventArgs e)
        {
            int? maDangKy = LayMaDangKyDangChon();
            if (maDangKy == null)
            {
                MessageBox.Show("Vui lòng chọn một hợp đồng trong danh sách.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var db = new AppDbContext();
            var dangKy = db.DangKyOs.Include(d => d.MaSvNavigation)
                .FirstOrDefault(d => d.MaDangKy == maDangKy);
            if (dangKy == null)
            {
                MessageBox.Show("Không tìm thấy hợp đồng (có thể vừa bị thay đổi).",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TaiDanhSach(txtTimKiem.Text);
                return;
            }

            string moTa = $"Hợp đồng của {dangKy.MaSvNavigation?.HoTen} (Mã SV: {dangKy.MaSv})\nPhòng: {dangKy.MaPhong}";
            using var frm = new FrmChonNgayGiaHan(moTa, dangKy.NgayKetThuc);
            if (frm.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            dangKy.NgayKetThuc = frm.NgayKetThucMoi;

            // Đồng bộ hạn hợp đồng (HopDong) tương ứng để tra cứu (UC10) và các
            // hóa đơn phát sinh sau này khớp với hạn mới của đăng ký ở.
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
            TaiDanhSach(txtTimKiem.Text);
        }

        private void btnChamDut_Click(object sender, EventArgs e)
        {
            int? maDangKy = LayMaDangKyDangChon();
            if (maDangKy == null)
            {
                MessageBox.Show("Vui lòng chọn một hợp đồng trong danh sách.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var db = new AppDbContext();
            var dangKy = db.DangKyOs.FirstOrDefault(d => d.MaDangKy == maDangKy);
            if (dangKy == null)
            {
                TaiDanhSach(txtTimKiem.Text);
                return;
            }

            var homNay = DateOnly.FromDateTime(DateTime.Today);
            if (dangKy.NgayKetThuc.HasValue && dangKy.NgayKetThuc < homNay)
            {
                MessageBox.Show("Hợp đồng này đã kết thúc trước đó rồi.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var xacNhan = MessageBox.Show(
                "Bạn có chắc chắn muốn chấm dứt hợp đồng này ngay hôm nay không?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (xacNhan != DialogResult.Yes)
            {
                return;
            }

            dangKy.NgayKetThuc = homNay;

            // Đồng bộ hợp đồng (HopDong) tương ứng: đánh dấu đã kết thúc để tra cứu
            // (UC10) không còn hiển thị hợp đồng này là "Còn hiệu lực" nữa.
            var hopDongLienQuan = db.HopDongs
                .FirstOrDefault(h => h.MaSv == dangKy.MaSv && h.MaPhong == dangKy.MaPhong);
            if (hopDongLienQuan != null)
            {
                hopDongLienQuan.NgayHetHan = homNay;
                hopDongLienQuan.TrangThai = "Đã kết thúc";
            }

            var phong = db.Phongs.FirstOrDefault(p => p.MaPhong == dangKy.MaPhong);
            if (phong != null)
            {
                phong.SoLuongHienTai = Math.Max(0, (phong.SoLuongHienTai ?? 0) - 1);
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chấm dứt hợp đồng thất bại: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Đã chấm dứt hợp đồng.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            TaiDanhSach(txtTimKiem.Text);
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnXemTatCa_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
            TaiDanhSach();
        }
    }
}
