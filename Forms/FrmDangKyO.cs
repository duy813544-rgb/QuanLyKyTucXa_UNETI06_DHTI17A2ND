using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DoAnNhom06.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// UC04 (bản tối giản) - Sinh viên đăng ký ở ký túc xá: tạo một bản ghi DangKyO
    /// ở trạng thái "Chờ duyệt" để Nhân viên xử lý ở màn hình Duyệt đăng ký (UC05).
    /// </summary>
    public partial class FrmDangKyO : Form
    {
        /// <summary>Một dòng trong danh sách phòng còn trống, tự hiển thị "MaPhong (còn trống x/y chỗ)".</summary>
        private sealed class PhongConTrongMuc
        {
            public string MaPhong { get; init; } = "";
            public int SoLuongHienTai { get; init; }
            public int SoLuongToiDa { get; init; }
            public override string ToString()
                => $"{MaPhong}  —  còn trống {SoLuongToiDa - SoLuongHienTai}/{SoLuongToiDa} chỗ";
        }

        private readonly TaiKhoan _taiKhoanDangNhap;

        public FrmDangKyO(TaiKhoan taiKhoanDangNhap)
        {
            InitializeComponent();
            _taiKhoanDangNhap = taiKhoanDangNhap;
        }

        private void FrmDangKyO_Load(object sender, EventArgs e)
        {
            // Sinh viên không được tự gõ Mã số sinh viên: khóa cứng theo đúng tài khoản
            // đang đăng nhập, để không thể đăng ký hộ/đăng ký nhầm mã người khác.
            txtMaSv.Text = _taiKhoanDangNhap.MaSv ?? "";
            txtMaSv.ReadOnly = true;
            txtMaSv.BackColor = Color.Gainsboro;
            txtMaSv.TabStop = false;

            if (string.IsNullOrEmpty(_taiKhoanDangNhap.MaSv))
            {
                MessageBox.Show(
                    "Tài khoản của bạn chưa được liên kết với hồ sơ sinh viên nào.\n" +
                    "Vui lòng liên hệ nhân viên quản lý ký túc xá để được liên kết trước khi đăng ký ở.",
                    "Chưa liên kết hồ sơ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnDangKy.Enabled = false;
                return;
            }

            // Nếu sinh viên đã có yêu cầu đăng ký đang chờ duyệt hoặc đã được duyệt rồi,
            // hiện tình trạng thay vì cho đăng ký thêm (tránh đăng ký trùng - xem thêm kiểm
            // tra "daCoYeuCau" ở btnDangKy_Click, vốn chỉ chặn SAU khi đã chọn phòng xong).
            if (HienTinhTrangNeuDaCoYeuCau())
            {
                return;
            }

            using var db = new AppDbContext();

            // Chỉ liệt kê phòng CÒN CHỖ TRỐNG (phòng đã đầy/"kín" sẽ không xuất hiện ở đây),
            // và hiện rõ "còn trống x/y chỗ" để sinh viên biết phòng còn rộng hay sắp đầy.
            var phongConTrong = db.Phongs
                .Where(p => (p.SoLuongHienTai ?? 0) < (p.SoLuongToiDa ?? 0))
                .OrderBy(p => p.MaPhong)
                .Select(p => new PhongConTrongMuc
                {
                    MaPhong = p.MaPhong,
                    SoLuongHienTai = p.SoLuongHienTai ?? 0,
                    SoLuongToiDa = p.SoLuongToiDa ?? 0
                })
                .ToList();

            cboPhong.Items.Clear();
            cboPhong.Items.AddRange(phongConTrong.ToArray());

            lblGhiChuPhong.Text = phongConTrong.Count > 0
                ? "Danh sách trên chỉ gồm phòng còn chỗ trống; phòng đã đầy (\"kín\") sẽ không hiển thị."
                : "Hiện không còn phòng nào trống. Vui lòng quay lại sau hoặc liên hệ nhân viên.";

            dtpNgayBatDau.MinDate = DateTime.Today;
            dtpNgayBatDau.Value = DateTime.Today;
        }

        /// <summary>
        /// Kiểm tra xem sinh viên đã có yêu cầu đăng ký nào đang "Chờ duyệt" hoặc "Đã duyệt"
        /// chưa. Nếu có, ẩn toàn bộ form đăng ký và hiện tình trạng (phòng, loại phòng,
        /// ngày phân...) thay vào đó, rồi trả về true. Trả về false nếu chưa có yêu cầu nào
        /// (cho phép đăng ký như bình thường).
        /// </summary>
        private bool HienTinhTrangNeuDaCoYeuCau()
        {
            using var db = new AppDbContext();

            var dangKy = db.DangKyOs
                .Include(d => d.MaPhongNavigation).ThenInclude(p => p!.MaKhuNavigation)
                .Include(d => d.MaPhongNavigation).ThenInclude(p => p!.MaLoaiNavigation)
                .Include(d => d.PhanPhongs)
                .Where(d => d.MaSv == _taiKhoanDangNhap.MaSv &&
                    (d.TrangThai == TrangThaiDangKy.ChoDuyet || d.TrangThai == TrangThaiDangKy.DaDuyet))
                .OrderByDescending(d => d.MaDangKy)
                .FirstOrDefault();

            if (dangKy == null)
            {
                return false;
            }

            // Chuyển giao diện sang chế độ "chỉ xem tình trạng".
            lblMaSv.Visible = false;
            txtMaSv.Visible = false;
            lblPhong.Visible = false;
            cboPhong.Visible = false;
            lblGhiChuPhong.Visible = false;
            lblNgayBatDau.Visible = false;
            dtpNgayBatDau.Visible = false;
            btnDangKy.Visible = false;

            btnHuy.Text = "Đóng";
            btnHuy.Location = new Point((ClientSize.Width - btnHuy.Width) / 2, 265);

            lblTrangThaiHienTai.Visible = true;

            var phong = dangKy.MaPhongNavigation;
            string tenKhu = phong?.MaKhuNavigation?.TenKhu ?? "(không rõ)";
            string tenLoai = phong?.MaLoaiNavigation?.TenLoai ?? "(chưa phân loại)";
            string donGia = phong?.MaLoaiNavigation?.DonGia.HasValue == true
                ? phong.MaLoaiNavigation.DonGia.Value.ToString("N0") + " đ/tháng"
                : "";
            string ngayPhan = dangKy.PhanPhongs
                .OrderByDescending(pp => pp.NgayPhan)
                .Select(pp => pp.NgayPhan)
                .FirstOrDefault()?.ToString("dd/MM/yyyy") ?? "";

            if (dangKy.TrangThai == TrangThaiDangKy.DaDuyet)
            {
                lblTitle.Text = "TÌNH TRẠNG ĐĂNG KÝ Ở KÝ TÚC XÁ";
                lblTrangThaiHienTai.Text =
                    "✅ Yêu cầu đăng ký của bạn đã được DUYỆT.\n\n" +
                    $"Phòng:          {dangKy.MaPhong}  (Khu {tenKhu})\n" +
                    $"Loại phòng:     {tenLoai}" + (string.IsNullOrEmpty(donGia) ? "" : $"  -  {donGia}") + "\n" +
                    $"Ngày bắt đầu ở: {(dangKy.NgayBatDau.HasValue ? dangKy.NgayBatDau.Value.ToString("dd/MM/yyyy") : "(chưa xác định)")}\n" +
                    $"Ngày phân phòng: {(string.IsNullOrEmpty(ngayPhan) ? "(chưa có dữ liệu)" : ngayPhan)}\n\n" +
                    "Chúc bạn có một kỳ ở ký túc xá vui vẻ!";
            }
            else
            {
                lblTitle.Text = "ĐANG CHỜ DUYỆT ĐĂNG KÝ";
                lblTrangThaiHienTai.Text =
                    "⏳ Yêu cầu đăng ký của bạn đang CHỜ NHÂN VIÊN DUYỆT.\n\n" +
                    $"Phòng đã chọn:  {dangKy.MaPhong}  (Khu {tenKhu})\n" +
                    $"Loại phòng:     {tenLoai}" + (string.IsNullOrEmpty(donGia) ? "" : $"  -  {donGia}") + "\n" +
                    $"Ngày mong muốn: {(dangKy.NgayBatDau.HasValue ? dangKy.NgayBatDau.Value.ToString("dd/MM/yyyy") : "(chưa xác định)")}\n\n" +
                    "Vui lòng quay lại sau hoặc liên hệ nhân viên quản lý ký túc xá nếu cần hỗ trợ.";
            }

            return true;
        }

        private void btnDangKy_Click(object sender, EventArgs e)
        {
            string maSv = txtMaSv.Text.Trim();
            if (string.IsNullOrEmpty(maSv))
            {
                MessageBox.Show(
                    "Tài khoản của bạn chưa được liên kết với hồ sơ sinh viên.\n" +
                    "Vui lòng liên hệ nhân viên quản lý ký túc xá.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboPhong.SelectedItem is not PhongConTrongMuc phongDaChon)
            {
                MessageBox.Show("Vui lòng chọn phòng.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maPhong = phongDaChon.MaPhong;

            using var db = new AppDbContext();

            bool coHoSo = db.SinhViens.Any(sv => sv.MaSv == maSv);
            if (!coHoSo)
            {
                MessageBox.Show(
                    "Không tìm thấy hồ sơ sinh viên với mã này.\n" +
                    "Vui lòng liên hệ nhân viên quản lý ký túc xá để được tạo hồ sơ trước.",
                    "Không tìm thấy hồ sơ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool daCoYeuCau = db.DangKyOs.Any(d => d.MaSv == maSv &&
                (d.TrangThai == TrangThaiDangKy.ChoDuyet || d.TrangThai == TrangThaiDangKy.DaDuyet));
            if (daCoYeuCau)
            {
                MessageBox.Show(
                    "Bạn đã có một yêu cầu đăng ký đang chờ duyệt hoặc đã có hợp đồng còn hiệu lực rồi.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dangKyMoi = new DangKyO
            {
                MaSv = maSv,
                MaPhong = maPhong,
                NgayBatDau = DateOnly.FromDateTime(dtpNgayBatDau.Value),
                TrangThai = TrangThaiDangKy.ChoDuyet
            };

            db.DangKyOs.Add(dangKyMoi);

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

            MessageBox.Show("Đăng ký thành công! Vui lòng chờ nhân viên duyệt.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
