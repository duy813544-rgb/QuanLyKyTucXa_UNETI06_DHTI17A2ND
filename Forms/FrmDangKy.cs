using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DoAnNhom06.Helpers;
using DoAnNhom06.Models;

namespace DoAnNhom06.Forms
{
    public partial class FrmDangKy : Form
    {
        // Tiền tố mã sinh viên tự sinh, ví dụ SV001, SV002... (đồng bộ với FrmThemSuaSinhVien).
        private const string TienToMaSv = "SV";

        /// <summary>
        /// Tên đăng nhập vừa đăng ký thành công, để FrmDangNhap điền sẵn lại.
        /// </summary>
        public string TenDangNhapDaDangKy { get; private set; } = string.Empty;

        public FrmDangKy()
        {
            InitializeComponent();
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text;
            string xacNhan = txtXacNhanMatKhau.Text;

            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (tenDangNhap.Length < 4)
            {
                MessageBox.Show("Tên đăng nhập phải có ít nhất 4 ký tự.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (matKhau.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (matKhau != xacNhan)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string hoTen = txtHoTen.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();
            string? gioiTinh = cboGioiTinh.SelectedItem?.ToString();
            string queQuan = txtQueQuan.Text.Trim();

            if (string.IsNullOrEmpty(hoTen) || string.IsNullOrEmpty(soDienThoai))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Họ tên và Số điện thoại.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(gioiTinh))
            {
                MessageBox.Show("Vui lòng chọn Giới tính.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(queQuan))
            {
                MessageBox.Show("Vui lòng nhập Quê quán.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateOnly ngaySinh = DateOnly.FromDateTime(dtpNgaySinh.Value);

            using var db = new AppDbContext();

            bool tenDaTonTai = db.TaiKhoans.Any(tk => tk.TenDangNhap == tenDangNhap);
            if (tenDaTonTai)
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại, vui lòng chọn tên khác.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Không cần sinh viên nhập Mã SV. Kiểm tra xem đã có hồ sơ nào trùng
            // Số điện thoại chưa (tránh 1 người bấm đăng ký nhiều lần tạo trùng hồ sơ).
            var hoSoTrungSdt = db.SinhViens
                .FirstOrDefault(sv => sv.SoDienThoai != null && sv.SoDienThoai.Trim() == soDienThoai);

            string maSv;

            if (hoSoTrungSdt != null)
            {
                bool sdtDaCoTaiKhoan = db.TaiKhoans.Any(tk => tk.MaSv == hoSoTrungSdt.MaSv);
                if (sdtDaCoTaiKhoan)
                {
                    MessageBox.Show(
                        "Số điện thoại này đã được dùng để đăng ký một tài khoản khác rồi.\n" +
                        "Nếu đây là tài khoản của bạn, vui lòng đăng nhập thay vì đăng ký mới.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Hồ sơ đã có sẵn (do trước đó đăng ký dở, hoặc nhân viên tạo sẵn) nhưng
                // chưa có tài khoản -> dùng lại đúng hồ sơ này, không tạo mã mới.
                maSv = hoSoTrungSdt.MaSv;
            }
            else
            {
                // Tự sinh Mã số sinh viên mới, không trùng, và tự tạo hồ sơ sinh viên.
                maSv = MaSoHelper.SinhMaMoi(db.SinhViens.Select(sv => sv.MaSv), TienToMaSv);

                var sinhVienMoi = new SinhVien
                {
                    MaSv = maSv,
                    HoTen = hoTen,
                    NgaySinh = ngaySinh,
                    GioiTinh = gioiTinh,
                    SoDienThoai = soDienThoai,
                    QueQuan = queQuan
                };
                db.SinhViens.Add(sinhVienMoi);
            }

            var taiKhoanMoi = new TaiKhoan
            {
                TenDangNhap = tenDangNhap,
                MatKhau = matKhau,
                // Tài khoản tự đăng ký mặc định là Sinh viên.
                Quyen = VaiTro.SinhVien,
                MaSv = maSv
            };
            db.TaiKhoans.Add(taiKhoanMoi);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đăng ký thất bại: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            TenDangNhapDaDangKy = tenDangNhap;

            MessageBox.Show(
                $"Đăng ký thành công!\nMã số sinh viên của bạn là: {maSv}\n" +
                "Vui lòng ghi nhớ mã này để tra cứu thông tin sau này.",
                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            bool hien = chkHienMatKhau.Checked;
            txtMatKhau.UseSystemPasswordChar = !hien;
            txtXacNhanMatKhau.UseSystemPasswordChar = !hien;
        }
    }
}
