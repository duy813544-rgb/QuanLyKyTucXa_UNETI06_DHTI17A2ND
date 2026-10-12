using System;
using System.Linq;
using System.Windows.Forms;
using DoAnNhom06.Helpers;
using DoAnNhom06.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// UC09 - Xác nhận thanh toán
    /// </summary>
    public partial class FrmXacNhanThanhToan : BaseAppForm
    {
        public FrmXacNhanThanhToan()
        {
            InitializeComponent();
        }

        private void FrmXacNhanThanhToan_Load(object sender, EventArgs e)
        {
            cboTrangThai.Items.Clear();

            cboTrangThai.Items.Add("Tất cả");
            cboTrangThai.Items.Add("Chờ xác nhận");
            cboTrangThai.Items.Add("Đã thanh toán");
            cboTrangThai.Items.Add("Chưa thanh toán");

            cboTrangThai.SelectedIndex = 0;

            TaiDanhSach();
        }
        private void cboTrangThai_SelectedIndexChanged(object sender, EventArgs e)
        {
            TaiDanhSach();
        }
        private void TaiDanhSach()
        {
            using var db = new AppDbContext();

            var query = db.ThanhToans
                .Include(t => t.MaHoaDonNavigation)
                    .ThenInclude(h => h!.MaHopDongNavigation)
                        .ThenInclude(hd => hd!.MaSvNavigation)
                .AsQueryable();

            switch (cboTrangThai.Text)
            {
                case "Chờ xác nhận":
                    query = query.Where(t => t.TrangThai == TrangThaiThanhToan.ChoXacNhan);
                    break;

                case "Đã thanh toán":
                    query = query.Where(t => t.TrangThai == TrangThaiThanhToan.DaThanhToan);
                    break;

                case "Chưa thanh toán":
                    query = query.Where(t => t.TrangThai == TrangThaiThanhToan.ChuaThanhToan);
                    break;

                case "Tất cả":
                default:
                    break;
            }

            var danhSach = query
                .OrderBy(t => t.NgayThanhToan)
                .ToList();

            dgvChoXacNhan.DataSource = danhSach.Select(t => new
            {
                MaThanhToan = t.MaThanhToan,
                MaHoaDon = t.MaHoaDon,
                MaSv = t.MaHoaDonNavigation?.MaHopDongNavigation?.MaSv,
                HoTen = t.MaHoaDonNavigation?.MaHopDongNavigation?.MaSvNavigation?.HoTen,
                SoTien = t.SoTien.ToString("N0") + " đ",
                PhuongThuc = t.PhuongThuc,

                TrangThai = TrangThaiThanhToan.HienThi(t.TrangThai),

                NgayThanhToan = t.NgayThanhToan.ToString("dd/MM/yyyy"),
            }).ToList();

            lblSoLuong.Text = $"Tổng số: {danhSach.Count} giao dịch";
        }


        private string? LayMaThanhToanDangChon()
        {
            if (dgvChoXacNhan.CurrentRow == null)
                return null;

            return dgvChoXacNhan.CurrentRow.Cells["MaThanhToan"].Value?.ToString();
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            string? maThanhToan = LayMaThanhToanDangChon();

            if (maThanhToan == null)
            {
                ThongBao.CanhBao("Vui lòng chọn một giao dịch.");
                return;
            }

            using var db = new AppDbContext();

            var giaoDich = db.ThanhToans.FirstOrDefault(t => t.MaThanhToan == maThanhToan);

            if (giaoDich == null)
            {
                ThongBao.Loi("Không tìm thấy giao dịch.");
                TaiDanhSach();
                return;
            }

            var hoaDon = db.HoaDons.FirstOrDefault(h => h.MaHoaDon == giaoDich.MaHoaDon);

            giaoDich.TrangThai = TrangThaiThanhToan.DaThanhToan;

            if (hoaDon != null)
            {
                hoaDon.TrangThai = TrangThaiHoaDon.DaThanhToan;
            }

            try
            {
                db.SaveChanges();
                ThongBao.Info("Đã xác nhận thanh toán.");
            }
            catch (Exception ex)
            {
                ThongBao.Loi(ex.Message);
            }

            TaiDanhSach();
        }

        private void btnTuChoi_Click(object sender, EventArgs e)
        {
            string? maThanhToan = LayMaThanhToanDangChon();

            if (maThanhToan == null)
            {
                ThongBao.CanhBao("Vui lòng chọn một giao dịch.");
                return;
            }

            if (!ThongBao.Hoi("Bạn có chắc muốn từ chối giao dịch này?"))
                return;

            using var db = new AppDbContext();

            var giaoDich = db.ThanhToans.FirstOrDefault(t => t.MaThanhToan == maThanhToan);

            if (giaoDich == null)
            {
                TaiDanhSach();
                return;
            }

            var hoaDon = db.HoaDons.FirstOrDefault(h => h.MaHoaDon == giaoDich.MaHoaDon);

            giaoDich.TrangThai = TrangThaiThanhToan.TuChoi;

            if (hoaDon != null)
            {
                hoaDon.TrangThai = TrangThaiHoaDon.ChuaThanhToan;
            }

            try
            {
                db.SaveChanges();
                ThongBao.Info("Đã từ chối giao dịch.");
            }
            catch (Exception ex)
            {
                ThongBao.Loi(ex.Message);
            }

            TaiDanhSach();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            cboTrangThai.SelectedIndex = 0;
            TaiDanhSach();
        }

  

        private void btnDong_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}