using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DoAnNhom06.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Forms
{
    public partial class FrmDanhSachPhong : Form
    {
        public FrmDanhSachPhong()
        {
            InitializeComponent();
        }

        private void FrmDanhSachPhong_Load(object sender, EventArgs e)
        {
            TaiDanhSach();
        }

        /// <summary>
        /// Tải danh sách phòng từ database, có thể lọc theo từ khóa
        /// (tìm theo Mã phòng hoặc Tên khu nhà).
        /// </summary>
        private void TaiDanhSach(string? tuKhoa = null)
        {
            using var db = new AppDbContext();
            var query = db.Phongs.Include(p => p.MaKhuNavigation).AsQueryable();

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                tuKhoa = tuKhoa.Trim();
                query = query.Where(p =>
                    p.MaPhong.Contains(tuKhoa) ||
                    (p.MaKhuNavigation != null && p.MaKhuNavigation.TenKhu.Contains(tuKhoa)));
            }

            var danhSach = query.OrderBy(p => p.MaPhong).ToList();

            dgvPhong.DataSource = danhSach.Select(p => new
            {
                MaPhong = p.MaPhong,
                KhuNha = p.MaKhuNavigation != null ? $"{p.MaKhuNavigation.MaKhu} - {p.MaKhuNavigation.TenKhu}" : "",
                SoLuongToiDa = p.SoLuongToiDa ?? 0,
                SoLuongHienTai = p.SoLuongHienTai ?? 0,
                ConTrong = (p.SoLuongToiDa ?? 0) - (p.SoLuongHienTai ?? 0)
            }).ToList();

            lblSoLuong.Text = $"Tổng số: {danhSach.Count} phòng";
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

        private void btnThem_Click(object sender, EventArgs e)
        {
            using var frm = new FrmThemSuaPhong();
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                TaiDanhSach(txtTimKiem.Text);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string? maPhong = LayMaPhongDangChon();
            if (maPhong == null)
            {
                MessageBox.Show("Vui lòng chọn một phòng trong danh sách.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var db = new AppDbContext();
            var phong = db.Phongs.FirstOrDefault(p => p.MaPhong == maPhong);
            if (phong == null)
            {
                MessageBox.Show("Không tìm thấy phòng (có thể vừa bị xóa).",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TaiDanhSach(txtTimKiem.Text);
                return;
            }

            using var frm = new FrmThemSuaPhong(phong);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                TaiDanhSach(txtTimKiem.Text);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string? maPhong = LayMaPhongDangChon();
            if (maPhong == null)
            {
                MessageBox.Show("Vui lòng chọn một phòng trong danh sách.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var xacNhan = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa phòng '{maPhong}' không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (xacNhan != DialogResult.Yes)
            {
                return;
            }

            using var db = new AppDbContext();
            var phong = db.Phongs.FirstOrDefault(p => p.MaPhong == maPhong);
            if (phong == null)
            {
                TaiDanhSach(txtTimKiem.Text);
                return;
            }

            try
            {
                db.Phongs.Remove(phong);
                db.SaveChanges();
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Không thể xóa phòng này vì đang có sinh viên đăng ký ở liên quan.\n" +
                    "Vui lòng xóa các đăng ký liên quan trước.",
                    "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Đã xóa phòng thành công.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            TaiDanhSach(txtTimKiem.Text);
        }

        private string? LayMaPhongDangChon()
        {
            if (dgvPhong.CurrentRow == null)
            {
                return null;
            }
            return dgvPhong.CurrentRow.Cells["MaPhong"].Value?.ToString();
        }

        private void dgvPhong_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnSua_Click(sender, e);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
