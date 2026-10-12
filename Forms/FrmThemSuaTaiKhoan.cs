using System;
using System.Linq;
using System.Windows.Forms;
using DoAnNhom06.Helpers;
using DoAnNhom06.Models;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// Dialog thêm/sửa 1 tài khoản, dùng trong UC12 - Quản lý tài khoản.
    /// </summary>
    public partial class FrmThemSuaTaiKhoan : Form
    {
        // Item hiển thị trong ComboBox chọn sinh viên liên kết, ghi đè ToString()
        // để hiện "Mã SV - Họ tên". null MaSv = "(Không liên kết)".
        private sealed class SinhVienItem
        {
            public string? MaSv { get; }
            public string HoTen { get; }

            public SinhVienItem(string? maSv, string hoTen)
            {
                MaSv = maSv;
                HoTen = hoTen;
            }

            public override string ToString() => MaSv == null ? HoTen : $"{MaSv} - {HoTen}";
        }

        // null = đang thêm mới, khác null = đang sửa (giữ mã tài khoản đang sửa)
        private readonly int? _maTkDangSua;

        /// <summary>Chế độ thêm mới.</summary>
        public FrmThemSuaTaiKhoan()
        {
            InitializeComponent();
            _maTkDangSua = null;
            Text = "Thêm tài khoản";

            TaiDanhSachQuyen();
            TaiDanhSachSinhVien(null);
        }

        /// <summary>Chế độ sửa, truyền vào tài khoản cần sửa.</summary>
        public FrmThemSuaTaiKhoan(TaiKhoan taiKhoan)
        {
            InitializeComponent();
            _maTkDangSua = taiKhoan.MaTk;
            Text = "Sửa tài khoản";

            TaiDanhSachQuyen();

            txtTenDangNhap.Text = taiKhoan.TenDangNhap;
            txtTenDangNhap.Enabled = false; // không cho đổi tên đăng nhập khi sửa

            cboQuyen.SelectedItem = taiKhoan.Quyen;

            TaiDanhSachSinhVien(taiKhoan.MaSv);

            if (taiKhoan.MaSv != null)
            {
                foreach (var obj in cboSinhVien.Items)
                {
                    if (obj is SinhVienItem item && item.MaSv == taiKhoan.MaSv)
                    {
                        cboSinhVien.SelectedItem = obj;
                        break;
                    }
                }
            }
        }

        private void TaiDanhSachQuyen()
        {
            cboQuyen.Items.Clear();
            cboQuyen.Items.Add(VaiTro.SinhVien);
            cboQuyen.Items.Add(VaiTro.NhanVien);
            cboQuyen.Items.Add(VaiTro.Admin);

            if (cboQuyen.SelectedIndex < 0)
            {
                cboQuyen.SelectedIndex = 0;
            }

            CapNhatTrangThaiComboSinhVien();
        }

        /// <summary>
        /// Nạp danh sách sinh viên có thể liên kết: sinh viên CHƯA có tài khoản nào khác,
        /// cộng thêm chính sinh viên đang được gán cho tài khoản này (nếu đang sửa).
        /// </summary>
        private void TaiDanhSachSinhVien(string? maSvDangGan)
        {
            using var db = new AppDbContext();

            var maSvDaCoTaiKhoan = db.TaiKhoans
                .Where(tk => tk.MaSv != null && tk.MaTk != (_maTkDangSua ?? -1))
                .Select(tk => tk.MaSv!)
                .ToHashSet();

            var danhSachSv = db.SinhViens
                .Where(sv => !maSvDaCoTaiKhoan.Contains(sv.MaSv) || sv.MaSv == maSvDangGan)
                .OrderBy(sv => sv.MaSv)
                .ToList();

            cboSinhVien.Items.Clear();
            cboSinhVien.Items.Add(new SinhVienItem(null, "(Không liên kết)"));
            foreach (var sv in danhSachSv)
            {
                cboSinhVien.Items.Add(new SinhVienItem(sv.MaSv, sv.HoTen));
            }

            cboSinhVien.SelectedIndex = 0;
        }

        /// <summary>Chỉ cho chọn Sinh viên liên kết khi Quyền đang chọn là "Sinh viên".</summary>
        private void CapNhatTrangThaiComboSinhVien()
        {
            bool laSinhVien = cboQuyen.SelectedItem?.ToString() == VaiTro.SinhVien;
            cboSinhVien.Enabled = laSinhVien;
            if (!laSinhVien)
            {
                cboSinhVien.SelectedIndex = 0; // (Không liên kết)
            }
        }

        private void cboQuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            CapNhatTrangThaiComboSinhVien();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text;
            string? quyen = cboQuyen.SelectedItem?.ToString();
            string? maSv = (cboSinhVien.SelectedItem as SinhVienItem)?.MaSv;

            if (string.IsNullOrEmpty(tenDangNhap))
            {
                ThongBao.CanhBao("Vui lòng nhập Tên đăng nhập.");
                return;
            }

            if (tenDangNhap.Length < 4)
            {
                ThongBao.CanhBao("Tên đăng nhập phải có ít nhất 4 ký tự.");
                return;
            }

            if (string.IsNullOrEmpty(quyen))
            {
                ThongBao.CanhBao("Vui lòng chọn Quyền.");
                return;
            }

            // Thêm mới bắt buộc phải có mật khẩu; sửa thì cho phép để trống (giữ nguyên).
            if (_maTkDangSua == null && string.IsNullOrEmpty(matKhau))
            {
                ThongBao.CanhBao("Vui lòng nhập Mật khẩu.");
                return;
            }

            if (!string.IsNullOrEmpty(matKhau) && matKhau.Length < 6)
            {
                ThongBao.CanhBao("Mật khẩu phải có ít nhất 6 ký tự.");
                return;
            }

            if (quyen == VaiTro.SinhVien && maSv == null)
            {
                ThongBao.CanhBao("Vui lòng chọn Sinh viên liên kết với tài khoản Sinh viên này.");
                return;
            }

            using var db = new AppDbContext();

            if (_maTkDangSua == null)
            {
                // Thêm mới -> kiểm tra trùng tên đăng nhập
                bool daTonTai = db.TaiKhoans.Any(tk => tk.TenDangNhap == tenDangNhap);
                if (daTonTai)
                {
                    ThongBao.CanhBao("Tên đăng nhập đã tồn tại, vui lòng chọn tên khác.");
                    return;
                }

                var taiKhoanMoi = new TaiKhoan
                {
                    TenDangNhap = tenDangNhap,
                    MatKhau = matKhau,
                    Quyen = quyen,
                    MaSv = quyen == VaiTro.SinhVien ? maSv : null
                };

                db.TaiKhoans.Add(taiKhoanMoi);
            }
            else
            {
                var taiKhoan = db.TaiKhoans.FirstOrDefault(tk => tk.MaTk == _maTkDangSua);
                if (taiKhoan == null)
                {
                    ThongBao.Loi("Không tìm thấy tài khoản để cập nhật (có thể đã bị xóa).");
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }

                taiKhoan.Quyen = quyen;
                taiKhoan.MaSv = quyen == VaiTro.SinhVien ? maSv : null;

                if (!string.IsNullOrEmpty(matKhau))
                {
                    taiKhoan.MatKhau = matKhau;
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ThongBao.Loi("Lưu thất bại: " + ex.Message);
                return;
            }

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
            txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;
        }
    }
}
