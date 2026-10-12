using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DoAnNhom06.Models;

namespace DoAnNhom06.Forms
{
    public partial class FrmDanhSachSinhVien : Form
    {
        public FrmDanhSachSinhVien()
        {
            InitializeComponent();
        }

        private void FrmDanhSachSinhVien_Load(object sender, EventArgs e)
        {
            cboGioiTinh.Items.Add("Giới tính");
            cboGioiTinh.Items.Add("Nam");
            cboGioiTinh.Items.Add("Nữ");

            cboGioiTinh.SelectedIndex = 0;

            TaiDanhSach();
        }

        /// <summary>
        /// Tải danh sách sinh viên từ database, có thể lọc theo từ khóa
        /// (tìm theo Mã sinh viên, Họ tên hoặc Lớp).
        /// </summary>
        private void TaiDanhSach(string? tuKhoa = null)
        {
            using var db = new AppDbContext();

            var query = db.SinhViens.AsQueryable();

            // Lọc theo từ khóa
            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                tuKhoa = tuKhoa.Trim();

                query = query.Where(sv =>
                    sv.MaSv.Contains(tuKhoa) ||
                    sv.HoTen.Contains(tuKhoa) ||
                    (sv.Lop != null && sv.Lop.Contains(tuKhoa)));
            }

            // Lọc theo giới tính
            if (cboGioiTinh.Text == "Nam")
            {
                query = query.Where(sv => sv.GioiTinh == "Nam");
            }
            else if (cboGioiTinh.Text == "Nữ")
            {
                query = query.Where(sv => sv.GioiTinh == "Nữ");
            }

            var danhSach = query.OrderBy(sv => sv.MaSv).ToList();

            dgvSinhVien.DataSource = danhSach.Select(sv => new
            {
                MaSv = sv.MaSv,
                HoTen = sv.HoTen,
                NgaySinh = sv.NgaySinh.HasValue ? sv.NgaySinh.Value.ToString("dd/MM/yyyy") : "",
                GioiTinh = sv.GioiTinh,
                Lop = sv.Lop,
                SoDienThoai = sv.SoDienThoai,
                QueQuan = sv.QueQuan
            }).ToList();

            lblSoLuong.Text = $"Tổng số: {danhSach.Count} sinh viên";
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
        private void cboGioiTinh_SelectedIndexChanged(object sender, EventArgs e)
        {
            TaiDanhSach(txtTimKiem.Text);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = string.Empty;
            TaiDanhSach();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            using var frm = new FrmThemSuaSinhVien();
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                TaiDanhSach(txtTimKiem.Text);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string? maSv = LayMaSvDangChon();
            if (maSv == null)
            {
                MessageBox.Show("Vui lòng chọn một sinh viên trong danh sách.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var db = new AppDbContext();
            var sv = db.SinhViens.FirstOrDefault(s => s.MaSv == maSv);
            if (sv == null)
            {
                MessageBox.Show("Không tìm thấy sinh viên (có thể vừa bị xóa).",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TaiDanhSach(txtTimKiem.Text);
                return;
            }

            using var frm = new FrmThemSuaSinhVien(sv);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                TaiDanhSach(txtTimKiem.Text);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string? maSv = LayMaSvDangChon();
            if (maSv == null)
            {
                MessageBox.Show("Vui lòng chọn một sinh viên trong danh sách.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var xacNhan = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa sinh viên có mã '{maSv}' không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (xacNhan != DialogResult.Yes)
            {
                return;
            }

            using var db = new AppDbContext();
            var sv = db.SinhViens.FirstOrDefault(s => s.MaSv == maSv);
            if (sv == null)
            {
                TaiDanhSach(txtTimKiem.Text);
                return;
            }

            try
            {
                db.SinhViens.Remove(sv);
                db.SaveChanges();
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Không thể xóa sinh viên này vì đang có dữ liệu đăng ký ở ký túc xá liên quan.\n" +
                    "Vui lòng xóa các đăng ký liên quan trước.",
                    "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Đã xóa sinh viên thành công.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            TaiDanhSach(txtTimKiem.Text);
        }


        private string? LayMaSvDangChon()
        {
            if (dgvSinhVien.CurrentRow == null)
            {
                return null;
            }
            return dgvSinhVien.CurrentRow.Cells["MaSv"].Value?.ToString();
        }

        private void dgvSinhVien_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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
