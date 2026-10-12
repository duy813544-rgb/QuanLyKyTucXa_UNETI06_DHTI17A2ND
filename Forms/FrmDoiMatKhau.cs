using System;
using System.Linq;
using System.Windows.Forms;
using DoAnNhom06.Models;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// Cho phép tài khoản đang đăng nhập (Sinh viên, Nhân viên, Quản trị viên - vai trò nào
    /// cũng dùng được) tự đổi mật khẩu của chính mình.
    /// </summary>
    public partial class FrmDoiMatKhau : Form
    {
        private readonly TaiKhoan _taiKhoanDangNhap;

        public FrmDoiMatKhau(TaiKhoan taiKhoanDangNhap)
        {
            InitializeComponent();
            _taiKhoanDangNhap = taiKhoanDangNhap;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string matKhauCu = txtMatKhauCu.Text;
            string matKhauMoi = txtMatKhauMoi.Text;
            string xacNhan = txtXacNhanMatKhauMoi.Text;

            if (string.IsNullOrEmpty(matKhauCu) || string.IsNullOrEmpty(matKhauMoi))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ mật khẩu hiện tại và mật khẩu mới.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (matKhauMoi.Length < 6)
            {
                MessageBox.Show("Mật khẩu mới phải có ít nhất 6 ký tự.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (matKhauMoi != xacNhan)
            {
                MessageBox.Show("Mật khẩu mới xác nhận không khớp.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var db = new AppDbContext();

            // Lấy lại bản ghi mới nhất từ CSDL (không dùng thẳng _taiKhoanDangNhap để tránh
            // lệch dữ liệu nếu có thay đổi từ nơi khác), rồi so mật khẩu hiện tại.
            var taiKhoan = db.TaiKhoans.FirstOrDefault(tk => tk.MaTk == _taiKhoanDangNhap.MaTk);
            if (taiKhoan == null)
            {
                MessageBox.Show("Không tìm thấy tài khoản.", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (taiKhoan.MatKhau != matKhauCu)
            {
                MessageBox.Show("Mật khẩu hiện tại không đúng.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (matKhauMoi == matKhauCu)
            {
                MessageBox.Show("Mật khẩu mới phải khác mật khẩu hiện tại.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            taiKhoan.MatKhau = matKhauMoi;

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đổi mật khẩu thất bại: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Cập nhật luôn đối tượng tài khoản đang dùng trong phiên làm việc hiện tại,
            // để nếu có màn hình nào khác đang giữ tham chiếu này thì vẫn đồng bộ.
            _taiKhoanDangNhap.MatKhau = matKhauMoi;

            MessageBox.Show("Đổi mật khẩu thành công.", "Thành công",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            txtMatKhauCu.UseSystemPasswordChar = !hien;
            txtMatKhauMoi.UseSystemPasswordChar = !hien;
            txtXacNhanMatKhauMoi.UseSystemPasswordChar = !hien;
        }
    }
}
