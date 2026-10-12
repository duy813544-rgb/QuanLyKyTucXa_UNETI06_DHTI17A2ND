using System;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using DoAnNhom06.Models;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// Trang "Thông tin cá nhân" của Sinh viên - trình bày dạng thẻ hồ sơ (ảnh đại diện +
    /// từng nhóm thông tin) thay vì bảng lưới như màn hình Tra cứu thông tin.
    /// Chỉ hiển thị các trường hiện có trong bảng SinhVien (Mã SV, Họ tên, Ngày sinh,
    /// Giới tính, Lớp, Số điện thoại, Quê quán) - CSDL hiện chưa lưu thêm Khoa/Ngành/
    /// Khóa học/Trạng thái... như một cổng thông tin sinh viên đầy đủ.
    /// </summary>
    public partial class FrmThongTinCaNhan : Form
    {
        private readonly TaiKhoan _taiKhoanDangNhap;

        public FrmThongTinCaNhan(TaiKhoan taiKhoanDangNhap)
        {
            InitializeComponent();
            _taiKhoanDangNhap = taiKhoanDangNhap;
        }

        private void FrmThongTinCaNhan_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_taiKhoanDangNhap.MaSv))
            {
                MessageBox.Show(
                    "Tài khoản của bạn chưa được liên kết với hồ sơ sinh viên nào.\n" +
                    "Vui lòng liên hệ nhân viên quản lý ký túc xá.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            using var db = new AppDbContext();
            var sv = db.SinhViens.FirstOrDefault(x => x.MaSv == _taiKhoanDangNhap.MaSv);

            if (sv == null)
            {
                MessageBox.Show("Không tìm thấy hồ sơ sinh viên tương ứng.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            lblHoTenHeader.Text = sv.HoTen;
            lblMaSvHeader.Text = $"MSSV: {sv.MaSv}";

            lblGtMaSv.Text = sv.MaSv;
            lblGtLop.Text = string.IsNullOrEmpty(sv.Lop) ? "(chưa cập nhật)" : sv.Lop;

            lblGtNgaySinh.Text = sv.NgaySinh.HasValue
                ? sv.NgaySinh.Value.ToString("dd/MM/yyyy")
                : "(chưa cập nhật)";
            lblGtGioiTinh.Text = string.IsNullOrEmpty(sv.GioiTinh) ? "(chưa cập nhật)" : sv.GioiTinh;
            lblGtSdt.Text = string.IsNullOrEmpty(sv.SoDienThoai) ? "(chưa cập nhật)" : sv.SoDienThoai;
            lblGtQueQuan.Text = string.IsNullOrEmpty(sv.QueQuan) ? "(chưa cập nhật)" : sv.QueQuan;
        }

        /// <summary>Vẽ icon đầu người (không dùng ảnh thật) làm ảnh đại diện.</summary>
        private void pnlAvatar_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new RectangleF(0, 0, pnlAvatar.Width - 1, pnlAvatar.Height - 1);

            using (var brNen = new SolidBrush(Color.FromArgb(70, 100, 165)))
                g.FillEllipse(brNen, rect);

            using var brIcon = new SolidBrush(Color.White);
            float cx = rect.X + rect.Width / 2f;
            float cy = rect.Y + rect.Height / 2f;

            // Đầu
            g.FillEllipse(brIcon, cx - 12, cy - 22, 24, 24);
            // Vai (nửa hình tròn lớn hơn, cắt bởi mép panel)
            g.FillPie(brIcon, cx - 22, cy - 2, 44, 40, 180, 180);
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
