using System;
using System.Linq;
using System.Windows.Forms;
using DoAnNhom06.Helpers;
using DoAnNhom06.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// UC15 - Phân phòng: Quản trị viên xem danh sách sinh viên đang ở (đăng ký đã được
    /// duyệt) và có thể chuyển một sinh viên sang phòng khác còn chỗ trống (VD: khi cần
    /// tách phòng quá tải, hoặc theo yêu cầu đổi phòng).
    /// </summary>
    public partial class FrmPhanPhong : Form
    {
        private readonly TaiKhoan _taiKhoanDangNhap;

        public FrmPhanPhong(TaiKhoan taiKhoanDangNhap)
        {
            InitializeComponent();
            _taiKhoanDangNhap = taiKhoanDangNhap;
        }

        private void FrmPhanPhong_Load(object sender, EventArgs e)
        {
            TaiDanhSach();
        }

        private void TaiDanhSach(string? tuKhoa = null)
        {
            using var db = new AppDbContext();

            var query = db.DangKyOs
                .Include(d => d.MaSvNavigation)
                .Include(d => d.MaPhongNavigation)
                    .ThenInclude(p => p!.MaKhuNavigation)
                .Include(d => d.MaPhongNavigation)
                    .ThenInclude(p => p!.MaLoaiNavigation)
                .Where(d => d.TrangThai == TrangThaiDangKy.DaDuyet)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                tuKhoa = tuKhoa.Trim();
                query = query.Where(d =>
                    (d.MaSv != null && d.MaSv.Contains(tuKhoa)) ||
                    (d.MaSvNavigation != null && d.MaSvNavigation.HoTen.Contains(tuKhoa)) ||
                    (d.MaPhong != null && d.MaPhong.Contains(tuKhoa)));
            }

            var danhSach = query.OrderBy(d => d.MaSv).ToList();

            dgvDangO.DataSource = danhSach.Select(d => new
            {
                MaDangKy = d.MaDangKy,
                MaSv = d.MaSv,
                HoTen = d.MaSvNavigation?.HoTen ?? "",
                PhongHienTai = d.MaPhong,
                KhuNha = d.MaPhongNavigation?.MaKhuNavigation?.TenKhu ?? "",
                LoaiPhong = d.MaPhongNavigation?.MaLoaiNavigation?.TenLoai ?? "",
                SoLuong = $"{d.MaPhongNavigation?.SoLuongHienTai ?? 0}/{d.MaPhongNavigation?.SoLuongToiDa ?? 0}"
            }).ToList();

            lblSoLuong.Text = $"Tổng số: {danhSach.Count} sinh viên đang ở";
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

        private (int MaDangKy, string MaSv, string? PhongHienTai)? LayDongDangChon()
        {
            if (dgvDangO.CurrentRow == null)
            {
                return null;
            }

            int maDangKy = (int)dgvDangO.CurrentRow.Cells["MaDangKy"].Value;
            string maSv = dgvDangO.CurrentRow.Cells["MaSv"].Value?.ToString() ?? "";
            string? phongHienTai = dgvDangO.CurrentRow.Cells["PhongHienTai"].Value?.ToString();
            return (maDangKy, maSv, phongHienTai);
        }

        private void btnChuyenPhong_Click(object sender, EventArgs e)
        {
            var dong = LayDongDangChon();
            if (dong == null)
            {
                ThongBao.CanhBao("Vui lòng chọn một sinh viên trong danh sách.");
                return;
            }

            using var db = new AppDbContext();

            var dangKy = db.DangKyOs.FirstOrDefault(d => d.MaDangKy == dong.Value.MaDangKy);
            if (dangKy == null || dangKy.TrangThai != TrangThaiDangKy.DaDuyet)
            {
                ThongBao.Loi("Không tìm thấy đăng ký đang ở (có thể vừa bị thay đổi).");
                TaiDanhSach(txtTimKiem.Text);
                return;
            }

            string phongCu = dangKy.MaPhong ?? "";

            // Danh sách phòng có thể chuyển đến: còn chỗ trống và khác phòng hiện tại.
            var danhSachPhongCoTheChon = db.Phongs
                .Include(p => p.MaKhuNavigation)
                .Where(p => p.MaPhong != phongCu && (p.SoLuongHienTai ?? 0) < (p.SoLuongToiDa ?? 0))
                .OrderBy(p => p.MaPhong)
                .ToList();

            if (danhSachPhongCoTheChon.Count == 0)
            {
                ThongBao.CanhBao("Hiện không còn phòng nào có chỗ trống để chuyển đến.");
                return;
            }

            using var frmChon = new FrmChonPhongMoi(
                $"Chuyển sinh viên {dong.Value.MaSv} từ phòng {phongCu} sang phòng khác:",
                danhSachPhongCoTheChon);

            if (frmChon.ShowDialog(this) != DialogResult.OK || frmChon.MaPhongMoiDaChon == null)
            {
                return;
            }

            string phongMoi = frmChon.MaPhongMoiDaChon;

            var phongCuEntity = db.Phongs.FirstOrDefault(p => p.MaPhong == phongCu);
            var phongMoiEntity = db.Phongs.FirstOrDefault(p => p.MaPhong == phongMoi);

            if (phongMoiEntity == null)
            {
                ThongBao.Loi("Không tìm thấy phòng mới (có thể vừa bị xóa).");
                TaiDanhSach(txtTimKiem.Text);
                return;
            }

            if ((phongMoiEntity.SoLuongHienTai ?? 0) >= (phongMoiEntity.SoLuongToiDa ?? 0))
            {
                ThongBao.CanhBao("Phòng mới vừa hết chỗ trống, vui lòng chọn phòng khác.");
                return;
            }

            // Cập nhật số lượng 2 phòng.
            if (phongCuEntity != null)
            {
                phongCuEntity.SoLuongHienTai = Math.Max(0, (phongCuEntity.SoLuongHienTai ?? 0) - 1);
            }
            phongMoiEntity.SoLuongHienTai = (phongMoiEntity.SoLuongHienTai ?? 0) + 1;

            // Cập nhật đăng ký ở sang phòng mới.
            dangKy.MaPhong = phongMoi;

            // Đồng bộ hợp đồng đang hiệu lực của sinh viên này (nếu có) sang phòng mới.
            var hopDong = db.HopDongs.FirstOrDefault(h =>
                h.MaSv == dong.Value.MaSv && h.MaPhong == phongCu && h.TrangThai == "Còn hiệu lực");
            if (hopDong != null)
            {
                hopDong.MaPhong = phongMoi;
            }

            // Ghi nhận lịch sử phân phòng. MaNV để trống vì tài khoản Quản trị viên hiện
            // chưa gắn với một hồ sơ Nhân viên (NhanVien.MaNV) cụ thể nào trong CSDL.
            db.PhanPhongs.Add(new PhanPhong
            {
                MaNv = null,
                MaDangKy = dangKy.MaDangKy,
                NgayPhan = DateOnly.FromDateTime(DateTime.Today)
            });

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ThongBao.Loi("Chuyển phòng thất bại: " + ex.Message);
                return;
            }

            ThongBao.Info($"Đã chuyển sinh viên {dong.Value.MaSv} từ phòng {phongCu} sang phòng {phongMoi}.");
            TaiDanhSach(txtTimKiem.Text);
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
