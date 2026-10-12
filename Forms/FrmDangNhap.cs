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
    public partial class FrmDangNhap : Form
    {
        /// <summary>
        /// Tài khoản đăng nhập thành công (null nếu chưa đăng nhập / bị hủy).
        /// </summary>
        public TaiKhoan? TaiKhoanDangNhap { get; private set; }

        public FrmDangNhap()
        {
            InitializeComponent();
        }

        private void FrmDangNhap_Load(object sender, EventArgs e)
        {

        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text;

            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var db = new AppDbContext();
            var taiKhoan = db.TaiKhoans
                .FirstOrDefault(tk => tk.TenDangNhap == tenDangNhap && tk.MatKhau == matKhau);

            if (taiKhoan == null)
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu.",
                    "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.TaiKhoanDangNhap = taiKhoan;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
        }

        private void lnkDangKy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using var frmDangKy = new FrmDangKy();
            if (frmDangKy.ShowDialog(this) == DialogResult.OK)
            {
                MessageBox.Show("Đăng ký thành công. Bạn có thể đăng nhập ngay bây giờ.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtTenDangNhap.Text = frmDangKy.TenDangNhapDaDangKy;
                txtMatKhau.Text = string.Empty;
                txtMatKhau.Focus();
            }
        }
    }
}
