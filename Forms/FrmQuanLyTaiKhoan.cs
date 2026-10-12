using System;
using System.Linq;
using System.Windows.Forms;
using DoAnNhom06.Helpers;
using DoAnNhom06.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// UC12 - Quản lý tài khoản: Quản trị viên xem, thêm, sửa, xóa các tài khoản
    /// đăng nhập hệ thống (Sinh viên, Nhân viên, Quản trị viên).
    /// </summary>
    public partial class FrmQuanLyTaiKhoan : Form
    {
        private readonly TaiKhoan _taiKhoanDangNhap;

        public FrmQuanLyTaiKhoan(TaiKhoan taiKhoanDangNhap)
        {
            InitializeComponent();
            _taiKhoanDangNhap = taiKhoanDangNhap;
        }

        private void FrmQuanLyTaiKhoan_Load(object sender, EventArgs e)
        {
            cboLocQuyen.Items.Clear();
            cboLocQuyen.Items.Add("Tất cả quyền");
            cboLocQuyen.Items.Add(VaiTro.SinhVien);
            cboLocQuyen.Items.Add(VaiTro.NhanVien);
            cboLocQuyen.Items.Add(VaiTro.Admin);
            cboLocQuyen.SelectedIndex = 0;

            TaiDanhSach();
        }

        /// <summary>Tải danh sách tài khoản, có thể lọc theo từ khóa và/hoặc theo quyền.</summary>
        private void TaiDanhSach(string? tuKhoa = null)
        {
            using var db = new AppDbContext();
            var query = db.TaiKhoans.Include(tk => tk.MaSvNavigation).AsQueryable();

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                tuKhoa = tuKhoa.Trim();
                query = query.Where(tk =>
                    tk.TenDangNhap.Contains(tuKhoa) ||
                    (tk.MaSv != null && tk.MaSv.Contains(tuKhoa)));
            }

            string? quyenLoc = cboLocQuyen.SelectedIndex > 0 ? cboLocQuyen.SelectedItem?.ToString() : null;
            if (!string.IsNullOrEmpty(quyenLoc))
            {
                query = query.Where(tk => tk.Quyen == quyenLoc);
            }

            var danhSach = query.OrderBy(tk => tk.MaTk).ToList();

            dgvTaiKhoan.DataSource = danhSach.Select(tk => new
            {
                MaTk = tk.MaTk,
                TenDangNhap = tk.TenDangNhap,
                Quyen = tk.Quyen,
                MaSv = tk.MaSv ?? "",
                HoTenSinhVien = tk.MaSvNavigation?.HoTen ?? ""
            }).ToList();

            lblSoLuong.Text = $"Tổng số: {danhSach.Count} tài khoản";
        }

        private void cboLocQuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            TaiDanhSach(txtTimKiem.Text);
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
            cboLocQuyen.SelectedIndex = 0;
            TaiDanhSach();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            using var frm = new FrmThemSuaTaiKhoan();
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                TaiDanhSach(txtTimKiem.Text);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            int? maTk = LayMaTkDangChon();
            if (maTk == null)
            {
                ThongBao.CanhBao("Vui lòng chọn một tài khoản trong danh sách.");
                return;
            }

            using var db = new AppDbContext();
            var taiKhoan = db.TaiKhoans.FirstOrDefault(tk => tk.MaTk == maTk);
            if (taiKhoan == null)
            {
                ThongBao.Loi("Không tìm thấy tài khoản (có thể vừa bị xóa).");
                TaiDanhSach(txtTimKiem.Text);
                return;
            }

            using var frm = new FrmThemSuaTaiKhoan(taiKhoan);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                TaiDanhSach(txtTimKiem.Text);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            int? maTk = LayMaTkDangChon();
            if (maTk == null)
            {
                ThongBao.CanhBao("Vui lòng chọn một tài khoản trong danh sách.");
                return;
            }

            if (maTk == _taiKhoanDangNhap.MaTk)
            {
                ThongBao.CanhBao("Không thể xóa chính tài khoản đang đăng nhập.");
                return;
            }

            using var db = new AppDbContext();
            var taiKhoan = db.TaiKhoans.FirstOrDefault(tk => tk.MaTk == maTk);
            if (taiKhoan == null)
            {
                TaiDanhSach(txtTimKiem.Text);
                return;
            }

            // Không cho xóa Quản trị viên cuối cùng - tránh trường hợp hệ thống mất hết
            // tài khoản có quyền Quản lý tài khoản, hóa đơn, báo cáo...
            if (VaiTro.LaQuanTriVien(taiKhoan.Quyen))
            {
                int soAdminConLai = db.TaiKhoans.Count(tk => tk.Quyen == VaiTro.Admin);
                if (soAdminConLai <= 1)
                {
                    ThongBao.CanhBao("Không thể xóa Quản trị viên cuối cùng của hệ thống.");
                    return;
                }
            }

            bool xacNhan = ThongBao.Hoi(
                $"Bạn có chắc chắn muốn xóa tài khoản '{taiKhoan.TenDangNhap}' không?");
            if (!xacNhan)
            {
                return;
            }

            try
            {
                db.TaiKhoans.Remove(taiKhoan);
                db.SaveChanges();
            }
            catch (Exception)
            {
                ThongBao.Loi(
                    "Không thể xóa tài khoản này (có thể đang có dữ liệu liên quan).");
                return;
            }

            ThongBao.Info("Đã xóa tài khoản thành công.");
            TaiDanhSach(txtTimKiem.Text);
        }

        private int? LayMaTkDangChon()
        {
            if (dgvTaiKhoan.CurrentRow == null)
            {
                return null;
            }
            return (int)dgvTaiKhoan.CurrentRow.Cells["MaTk"].Value;
        }

        private void dgvTaiKhoan_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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
