using System;
using System.Linq;
using System.Windows.Forms;
using DoAnNhom06.Helpers;
using DoAnNhom06.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// Xin chuyển phòng (Sinh viên): chọn phòng muốn chuyển đến trong số các phòng còn
    /// chỗ trống, gửi kèm lý do, và chờ Nhân viên duyệt. Sinh viên cũng xem lại được
    /// trạng thái các yêu cầu đã gửi trước đó.
    /// </summary>
    public partial class FrmXinChuyenPhong : Form
    {
        private sealed class PhongConTrongMuc
        {
            public string MaPhong { get; init; } = "";
            public int SoLuongHienTai { get; init; }
            public int SoLuongToiDa { get; init; }
            public override string ToString()
                => $"{MaPhong}  —  còn trống {SoLuongToiDa - SoLuongHienTai}/{SoLuongToiDa} chỗ";
        }

        private readonly TaiKhoan _taiKhoanDangNhap;
        private string? _maPhongHienTai;

        public FrmXinChuyenPhong(TaiKhoan taiKhoanDangNhap)
        {
            InitializeComponent();
            _taiKhoanDangNhap = taiKhoanDangNhap;
        }

        private void FrmXinChuyenPhong_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_taiKhoanDangNhap.MaSv))
            {
                ThongBao.CanhBao(
                    "Tài khoản của bạn chưa được liên kết với hồ sơ sinh viên nào.\n" +
                    "Vui lòng liên hệ nhân viên quản lý ký túc xá.",
                    "Chưa liên kết hồ sơ");
                btnGuiYeuCau.Enabled = false;
                return;
            }

            using var db = new AppDbContext();

            _maPhongHienTai = db.DangKyOs
                .Where(d => d.MaSv == _taiKhoanDangNhap.MaSv && d.TrangThai == TrangThaiDangKy.DaDuyet)
                .OrderByDescending(d => d.MaDangKy)
                .Select(d => d.MaPhong)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(_maPhongHienTai))
            {
                lblPhongHienTai.Text = "Bạn hiện chưa được phân phòng nên chưa thể xin chuyển phòng.";
                btnGuiYeuCau.Enabled = false;
                return;
            }

            lblPhongHienTai.Text = $"Phòng hiện tại: {_maPhongHienTai}";

            bool daCoYeuCauChoDuyet = db.YeuCauChuyenPhongs.Any(y =>
                y.MaSv == _taiKhoanDangNhap.MaSv && y.TrangThai == TrangThaiYeuCauChuyenPhong.ChoDuyet);
            if (daCoYeuCauChoDuyet)
            {
                lblPhongHienTai.Text += "\nBạn đang có một yêu cầu chuyển phòng chờ duyệt, vui lòng chờ xử lý xong.";
                btnGuiYeuCau.Enabled = false;
            }
            else
            {
                var phongConTrong = db.Phongs
                    .Where(p => p.MaPhong != _maPhongHienTai && (p.SoLuongHienTai ?? 0) < (p.SoLuongToiDa ?? 0))
                    .OrderBy(p => p.MaPhong)
                    .Select(p => new PhongConTrongMuc
                    {
                        MaPhong = p.MaPhong,
                        SoLuongHienTai = p.SoLuongHienTai ?? 0,
                        SoLuongToiDa = p.SoLuongToiDa ?? 0
                    })
                    .ToList();

                cboPhongMoi.Items.Clear();
                cboPhongMoi.Items.AddRange(phongConTrong.ToArray());

                if (phongConTrong.Count == 0)
                {
                    btnGuiYeuCau.Enabled = false;
                }
            }

            TaiDanhSach();
        }

        private void TaiDanhSach()
        {
            using var db = new AppDbContext();

            var danhSach = db.YeuCauChuyenPhongs
                .Where(y => y.MaSv == _taiKhoanDangNhap.MaSv)
                .OrderByDescending(y => y.MaYeuCau)
                .ToList();

            dgvYeuCau.DataSource = danhSach.Select(y => new
            {
                y.MaYeuCau,
                PhongHienTai = y.MaPhongHienTai,
                PhongMoi = y.MaPhongMoi,
                LyDo = y.LyDo ?? "",
                NgayGui = y.NgayGui.ToString("dd/MM/yyyy"),
                TrangThai = TrangThaiYeuCauChuyenPhong.HienThi(y.TrangThai),
                GhiChu = y.GhiChu ?? ""
            }).ToList();

            lblSoLuong.Text = $"Tổng số: {danhSach.Count} yêu cầu đã gửi";
        }

        private void btnGuiYeuCau_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_maPhongHienTai))
            {
                ThongBao.CanhBao("Bạn chưa được phân phòng nên chưa thể xin chuyển phòng.");
                return;
            }

            if (cboPhongMoi.SelectedItem is not PhongConTrongMuc phongDaChon)
            {
                ThongBao.CanhBao("Vui lòng chọn phòng muốn chuyển đến.");
                return;
            }

            using var db = new AppDbContext();

            bool daCoYeuCauChoDuyet = db.YeuCauChuyenPhongs.Any(y =>
                y.MaSv == _taiKhoanDangNhap.MaSv && y.TrangThai == TrangThaiYeuCauChuyenPhong.ChoDuyet);
            if (daCoYeuCauChoDuyet)
            {
                ThongBao.CanhBao("Bạn đang có một yêu cầu chuyển phòng chờ duyệt rồi.");
                return;
            }

            var yeuCauMoi = new YeuCauChuyenPhong
            {
                MaSv = _taiKhoanDangNhap.MaSv!,
                MaPhongHienTai = _maPhongHienTai,
                MaPhongMoi = phongDaChon.MaPhong,
                LyDo = string.IsNullOrWhiteSpace(txtLyDo.Text) ? null : txtLyDo.Text.Trim(),
                NgayGui = DateOnly.FromDateTime(DateTime.Today),
                TrangThai = TrangThaiYeuCauChuyenPhong.ChoDuyet
            };

            db.YeuCauChuyenPhongs.Add(yeuCauMoi);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ThongBao.Loi("Gửi yêu cầu thất bại: " + ex.Message);
                return;
            }

            ThongBao.Info("Đã gửi yêu cầu chuyển phòng. Vui lòng chờ nhân viên duyệt.");

            btnGuiYeuCau.Enabled = false;
            cboPhongMoi.Enabled = false;
            txtLyDo.Clear();
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
