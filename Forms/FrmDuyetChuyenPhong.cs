using System;
using System.Linq;
using System.Windows.Forms;
using DoAnNhom06.Helpers;
using DoAnNhom06.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// Duyệt yêu cầu chuyển phòng (Nhân viên): xem các yêu cầu chuyển phòng của sinh
    /// viên, duyệt (thực hiện chuyển phòng thật sự, giống UC15 - Phân phòng) hoặc từ chối.
    /// </summary>
    public partial class FrmDuyetChuyenPhong : Form
    {
        public FrmDuyetChuyenPhong()
        {
            InitializeComponent();
        }

        private void FrmDuyetChuyenPhong_Load(object sender, EventArgs e)
        {
            cboTrangThai.Items.Add("Tất cả");
            cboTrangThai.Items.Add("Chờ duyệt");
            cboTrangThai.Items.Add("Đã duyệt");
            cboTrangThai.Items.Add("Từ chối");
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

            var query = db.YeuCauChuyenPhongs
                .Include(y => y.MaSvNavigation)
                .AsQueryable();

            switch (cboTrangThai.Text)
            {
                case "Chờ duyệt":
                    query = query.Where(y => y.TrangThai == TrangThaiYeuCauChuyenPhong.ChoDuyet);
                    break;
                case "Đã duyệt":
                    query = query.Where(y => y.TrangThai == TrangThaiYeuCauChuyenPhong.DaDuyet);
                    break;
                case "Từ chối":
                    query = query.Where(y => y.TrangThai == TrangThaiYeuCauChuyenPhong.TuChoi);
                    break;
                default:
                    // Tất cả
                    break;
            }

            var danhSach = query
                .OrderBy(y => y.TrangThai == TrangThaiYeuCauChuyenPhong.ChoDuyet ? 0 : 1)
                .ThenByDescending(y => y.MaYeuCau)
                .ToList();

            dgvYeuCau.DataSource = danhSach.Select(y => new
            {
                y.MaYeuCau,
                MaSv = y.MaSv,
                HoTen = y.MaSvNavigation?.HoTen ?? "",
                PhongHienTai = y.MaPhongHienTai,
                PhongMoi = y.MaPhongMoi,
                LyDo = y.LyDo ?? "",
                NgayGui = y.NgayGui.ToString("dd/MM/yyyy"),
                TrangThai = TrangThaiYeuCauChuyenPhong.HienThi(y.TrangThai)
            }).ToList();

            lblSoLuong.Text = $"Tổng số: {danhSach.Count} yêu cầu";
        }

        private int? LayMaYeuCauDangChon()
        {
            if (dgvYeuCau.CurrentRow == null)
            {
                return null;
            }
            return (int)dgvYeuCau.CurrentRow.Cells["MaYeuCau"].Value;
        }

        private void btnDuyet_Click(object sender, EventArgs e)
        {
            int? maYeuCau = LayMaYeuCauDangChon();
            if (maYeuCau == null)
            {
                ThongBao.CanhBao("Vui lòng chọn một yêu cầu trong danh sách.");
                return;
            }

            using var db = new AppDbContext();

            var yeuCau = db.YeuCauChuyenPhongs.FirstOrDefault(y => y.MaYeuCau == maYeuCau);
            if (yeuCau == null || yeuCau.TrangThai != TrangThaiYeuCauChuyenPhong.ChoDuyet)
            {
                ThongBao.Loi("Không tìm thấy yêu cầu đang chờ duyệt (có thể vừa được xử lý).");
                TaiDanhSach();
                return;
            }

            // Lấy đăng ký ở hiện tại (đã duyệt) của sinh viên để xác định phòng thật sự
            // đang ở - đề phòng phòng đã đổi khác so với lúc gửi yêu cầu.
            var dangKy = db.DangKyOs
                .Where(d => d.MaSv == yeuCau.MaSv && d.TrangThai == TrangThaiDangKy.DaDuyet)
                .OrderByDescending(d => d.MaDangKy)
                .FirstOrDefault();

            if (dangKy == null)
            {
                ThongBao.Loi("Sinh viên hiện không có đăng ký ở nào đang hiệu lực, không thể duyệt.");
                return;
            }

            string phongCu = dangKy.MaPhong ?? "";
            string phongMoi = yeuCau.MaPhongMoi;

            if (phongCu == phongMoi)
            {
                ThongBao.CanhBao("Sinh viên đã ở phòng này rồi, không cần chuyển.");
                return;
            }

            var phongCuEntity = db.Phongs.FirstOrDefault(p => p.MaPhong == phongCu);
            var phongMoiEntity = db.Phongs.FirstOrDefault(p => p.MaPhong == phongMoi);

            if (phongMoiEntity == null)
            {
                ThongBao.Loi("Không tìm thấy phòng muốn chuyển đến (có thể vừa bị xóa).");
                TaiDanhSach();
                return;
            }

            if ((phongMoiEntity.SoLuongHienTai ?? 0) >= (phongMoiEntity.SoLuongToiDa ?? 0))
            {
                ThongBao.CanhBao("Phòng muốn chuyển đến hiện đã hết chỗ trống. Vui lòng từ chối hoặc để sinh viên chọn phòng khác.");
                return;
            }

            // Cập nhật số lượng 2 phòng.
            if (phongCuEntity != null)
            {
                phongCuEntity.SoLuongHienTai = Math.Max(0, (phongCuEntity.SoLuongHienTai ?? 0) - 1);
            }
            phongMoiEntity.SoLuongHienTai = (phongMoiEntity.SoLuongHienTai ?? 0) + 1;

            // Cập nhật đăng ký ở sang phòng mới.
            dangKy.MaPhong = phongMoi;

            // Đồng bộ hợp đồng đang hiệu lực (nếu có) sang phòng mới.
            var hopDong = db.HopDongs.FirstOrDefault(h =>
                h.MaSv == yeuCau.MaSv && h.MaPhong == phongCu && h.TrangThai == "Còn hiệu lực");
            if (hopDong != null)
            {
                hopDong.MaPhong = phongMoi;
            }

            // Ghi nhận lịch sử phân phòng.
            db.PhanPhongs.Add(new PhanPhong
            {
                MaDangKy = dangKy.MaDangKy,
                NgayPhan = DateOnly.FromDateTime(DateTime.Today)
            });

            yeuCau.TrangThai = TrangThaiYeuCauChuyenPhong.DaDuyet;
            yeuCau.NgayXuLy = DateOnly.FromDateTime(DateTime.Today);
            yeuCau.GhiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? yeuCau.GhiChu : txtGhiChu.Text.Trim();

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ThongBao.Loi("Duyệt thất bại: " + ex.Message);
                return;
            }

            ThongBao.Info($"Đã duyệt: chuyển sinh viên {yeuCau.MaSv} từ phòng {phongCu} sang phòng {phongMoi}.");
            txtGhiChu.Clear();
            TaiDanhSach();
        }

        private void btnTuChoi_Click(object sender, EventArgs e)
        {
            int? maYeuCau = LayMaYeuCauDangChon();
            if (maYeuCau == null)
            {
                ThongBao.CanhBao("Vui lòng chọn một yêu cầu trong danh sách.");
                return;
            }

            if (!ThongBao.Hoi("Bạn có chắc chắn muốn từ chối yêu cầu này không?"))
            {
                return;
            }

            using var db = new AppDbContext();
            var yeuCau = db.YeuCauChuyenPhongs.FirstOrDefault(y => y.MaYeuCau == maYeuCau);
            if (yeuCau == null || yeuCau.TrangThai != TrangThaiYeuCauChuyenPhong.ChoDuyet)
            {
                TaiDanhSach();
                return;
            }

            yeuCau.TrangThai = TrangThaiYeuCauChuyenPhong.TuChoi;
            yeuCau.NgayXuLy = DateOnly.FromDateTime(DateTime.Today);
            yeuCau.GhiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? yeuCau.GhiChu : txtGhiChu.Text.Trim();

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ThongBao.Loi("Từ chối thất bại: " + ex.Message);
                return;
            }

            ThongBao.Info("Đã từ chối yêu cầu.");
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
