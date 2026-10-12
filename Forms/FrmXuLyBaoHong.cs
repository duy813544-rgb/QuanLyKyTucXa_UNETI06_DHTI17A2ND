using System;
using System.Linq;
using System.Windows.Forms;
using DoAnNhom06.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// Xử lý báo hỏng cơ sở vật chất (Nhân viên): xem danh sách yêu cầu báo hỏng của
    /// sinh viên, tiếp nhận (chuyển "Đang sửa") và đánh dấu hoàn thành, kèm ghi chú xử lý.
    /// </summary>
    public partial class FrmXuLyBaoHong : Form
    {
        public FrmXuLyBaoHong()
        {
            InitializeComponent();
        }

        private void FrmXuLyBaoHong_Load(object sender, EventArgs e)
        {
            cboTrangThai.Items.Add("Tất cả");
            cboTrangThai.Items.Add("Chờ xử lý");
            cboTrangThai.Items.Add("Đang sửa");
            cboTrangThai.Items.Add("Hoàn thành");
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

            var query = db.BaoHongs
                .Include(b => b.MaSvNavigation)
                .AsQueryable();

            switch (cboTrangThai.Text)
            {
                case "Chờ xử lý":
                    query = query.Where(b => b.TrangThai == TrangThaiBaoHong.ChoXuLy);
                    break;
                case "Đang sửa":
                    query = query.Where(b => b.TrangThai == TrangThaiBaoHong.DangSua);
                    break;
                case "Hoàn thành":
                    query = query.Where(b => b.TrangThai == TrangThaiBaoHong.HoanThanh);
                    break;
                default:
                    // Tất cả
                    break;
            }

            var danhSach = query
                .OrderBy(b => b.TrangThai == TrangThaiBaoHong.HoanThanh ? 1 : 0)
                .ThenByDescending(b => b.MaBaoHong)
                .ToList();

            dgvBaoHong.DataSource = danhSach.Select(b => new
            {
                b.MaBaoHong,
                MaSv = b.MaSv,
                HoTen = b.MaSvNavigation?.HoTen ?? "",
                b.MaPhong,
                LoaiSuCo = b.LoaiSuCo,
                MoTa = b.MoTa ?? "",
                NgayBaoCao = b.NgayBaoCao.ToString("dd/MM/yyyy"),
                TrangThai = TrangThaiBaoHong.HienThi(b.TrangThai)
            }).ToList();

            lblSoLuong.Text = $"Tổng số: {danhSach.Count} yêu cầu";
        }

        private int? LayMaBaoHongDangChon()
        {
            if (dgvBaoHong.CurrentRow == null)
            {
                return null;
            }
            return (int)dgvBaoHong.CurrentRow.Cells["MaBaoHong"].Value;
        }

        private void btnTiepNhan_Click(object sender, EventArgs e)
        {
            int? maBaoHong = LayMaBaoHongDangChon();
            if (maBaoHong == null)
            {
                MessageBox.Show("Vui lòng chọn một yêu cầu trong danh sách.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var db = new AppDbContext();
            var baoHong = db.BaoHongs.FirstOrDefault(b => b.MaBaoHong == maBaoHong);
            if (baoHong == null)
            {
                TaiDanhSach();
                return;
            }

            if (baoHong.TrangThai != TrangThaiBaoHong.ChoXuLy)
            {
                MessageBox.Show("Chỉ có thể tiếp nhận các yêu cầu đang \"Chờ xử lý\".",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            baoHong.TrangThai = TrangThaiBaoHong.DangSua;
            baoHong.GhiChuXuLy = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? baoHong.GhiChuXuLy : txtGhiChu.Text.Trim();

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cập nhật thất bại: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Đã tiếp nhận yêu cầu, chuyển sang trạng thái \"Đang sửa\".",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtGhiChu.Clear();
            TaiDanhSach();
        }

        private void btnHoanThanh_Click(object sender, EventArgs e)
        {
            int? maBaoHong = LayMaBaoHongDangChon();
            if (maBaoHong == null)
            {
                MessageBox.Show("Vui lòng chọn một yêu cầu trong danh sách.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var db = new AppDbContext();
            var baoHong = db.BaoHongs.FirstOrDefault(b => b.MaBaoHong == maBaoHong);
            if (baoHong == null)
            {
                TaiDanhSach();
                return;
            }

            if (baoHong.TrangThai == TrangThaiBaoHong.HoanThanh)
            {
                MessageBox.Show("Yêu cầu này đã hoàn thành.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            baoHong.TrangThai = TrangThaiBaoHong.HoanThanh;
            baoHong.NgayXuLy = DateOnly.FromDateTime(DateTime.Today);
            baoHong.GhiChuXuLy = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? baoHong.GhiChuXuLy : txtGhiChu.Text.Trim();

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cập nhật thất bại: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Đã đánh dấu hoàn thành.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtGhiChu.Clear();
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
