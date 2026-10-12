using System;
using System.Linq;
using System.Windows.Forms;
using DoAnNhom06.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// Báo hỏng cơ sở vật chất (Sinh viên): gửi yêu cầu báo hỏng (bóng đèn, quạt,
    /// điều hòa, vòi nước rò rỉ...) cho phòng mình đang ở, và xem lại trạng thái xử lý
    /// của các yêu cầu đã gửi (Chờ xử lý / Đang sửa / Hoàn thành).
    /// </summary>
    public partial class FrmBaoHong : Form
    {
        private readonly TaiKhoan _taiKhoanDangNhap;
        private string? _maPhongHienTai;

        public FrmBaoHong(TaiKhoan taiKhoanDangNhap)
        {
            InitializeComponent();
            _taiKhoanDangNhap = taiKhoanDangNhap;
        }

        private void FrmBaoHong_Load(object sender, EventArgs e)
        {
            cboLoaiSuCo.Items.AddRange(LoaiSuCoCoSoVatChat.DanhSach);
            cboLoaiSuCo.SelectedIndex = 0;

            if (string.IsNullOrEmpty(_taiKhoanDangNhap.MaSv))
            {
                MessageBox.Show(
                    "Tài khoản của bạn chưa được liên kết với hồ sơ sinh viên nào.\n" +
                    "Vui lòng liên hệ nhân viên quản lý ký túc xá.",
                    "Chưa liên kết hồ sơ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnGuiBaoHong.Enabled = false;
                return;
            }

            using var db = new AppDbContext();

            // Phòng hiện tại của sinh viên: lấy từ đăng ký ở đã được duyệt gần nhất.
            _maPhongHienTai = db.DangKyOs
                .Where(d => d.MaSv == _taiKhoanDangNhap.MaSv && d.TrangThai == TrangThaiDangKy.DaDuyet)
                .OrderByDescending(d => d.MaDangKy)
                .Select(d => d.MaPhong)
                .FirstOrDefault();

            lblPhongHienTai.Text = string.IsNullOrEmpty(_maPhongHienTai)
                ? "Bạn hiện chưa được phân phòng nên chưa thể gửi báo hỏng."
                : $"Phòng hiện tại: {_maPhongHienTai}";

            btnGuiBaoHong.Enabled = !string.IsNullOrEmpty(_maPhongHienTai);

            TaiDanhSach();
        }

        private void TaiDanhSach()
        {
            if (string.IsNullOrEmpty(_taiKhoanDangNhap.MaSv))
            {
                return;
            }

            using var db = new AppDbContext();

            var danhSach = db.BaoHongs
                .Where(b => b.MaSv == _taiKhoanDangNhap.MaSv)
                .OrderByDescending(b => b.MaBaoHong)
                .ToList();

            dgvBaoHong.DataSource = danhSach.Select(b => new
            {
                b.MaBaoHong,
                b.MaPhong,
                LoaiSuCo = b.LoaiSuCo,
                MoTa = b.MoTa ?? "",
                NgayBaoCao = b.NgayBaoCao.ToString("dd/MM/yyyy"),
                TrangThai = TrangThaiBaoHong.HienThi(b.TrangThai),
                GhiChuXuLy = b.GhiChuXuLy ?? ""
            }).ToList();

            lblSoLuong.Text = $"Tổng số: {danhSach.Count} yêu cầu đã gửi";
        }

        private void btnGuiBaoHong_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_maPhongHienTai))
            {
                MessageBox.Show("Bạn chưa được phân phòng nên chưa thể gửi báo hỏng.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboLoaiSuCo.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn loại sự cố.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var db = new AppDbContext();

            var baoHongMoi = new BaoHong
            {
                MaSv = _taiKhoanDangNhap.MaSv!,
                MaPhong = _maPhongHienTai,
                LoaiSuCo = cboLoaiSuCo.SelectedItem.ToString()!,
                MoTa = string.IsNullOrWhiteSpace(txtMoTa.Text) ? null : txtMoTa.Text.Trim(),
                NgayBaoCao = DateOnly.FromDateTime(DateTime.Today),
                TrangThai = TrangThaiBaoHong.ChoXuLy
            };

            db.BaoHongs.Add(baoHongMoi);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gửi báo hỏng thất bại: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Đã gửi yêu cầu báo hỏng. Vui lòng chờ nhân viên xử lý.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txtMoTa.Clear();
            cboLoaiSuCo.SelectedIndex = 0;
            TaiDanhSach();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            TaiDanhSach();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
