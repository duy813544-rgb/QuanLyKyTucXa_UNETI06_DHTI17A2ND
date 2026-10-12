using System;
using System.Linq;
using System.Windows.Forms;
using DoAnNhom06.Helpers;
using DoAnNhom06.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// UC05 - Duyệt đăng ký: Nhân viên/Quản trị viên duyệt hoặc từ chối các
    /// yêu cầu đăng ký ở ký túc xá đang ở trạng thái "Chờ duyệt".
    /// </summary>
    public partial class FrmDuyetDangKy : Form
    {
        public FrmDuyetDangKy()
        {
            InitializeComponent();
        }

        private void FrmDuyetDangKy_Load(object sender, EventArgs e)
        {
            cboTrangThai.Items.Add("Tất cả");
            cboTrangThai.Items.Add("Chờ duyệt");
            cboTrangThai.Items.Add("Đã duyệt");

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

            var query = db.DangKyOs
                .Include(d => d.MaSvNavigation)
                .Include(d => d.MaPhongNavigation)
                .AsQueryable();

            // Lọc theo ComboBox
            switch (cboTrangThai.Text)
            {
                case "Chờ duyệt":
                    query = query.Where(d => d.TrangThai == TrangThaiDangKy.ChoDuyet);
                    break;

                case "Đã duyệt":
                    query = query.Where(d => d.TrangThai == TrangThaiDangKy.DaDuyet);
                    break;

                case "Từ chối":
                    query = query.Where(d => d.TrangThai == TrangThaiDangKy.TuChoi);
                    break;

                default:
                    // Tất cả
                    break;
            }

            var danhSach = query
                .OrderBy(d => d.MaDangKy)
                .ToList();

            dgvChoDuyet.DataSource = danhSach.Select(d => new
            {
                MaDangKy = d.MaDangKy,
                MaSv = d.MaSv,
                HoTen = d.MaSvNavigation?.HoTen ?? "",
                MaPhong = d.MaPhong,
                TrangThai = TrangThaiDangKy.HienThi(d.TrangThai),
                NgayBatDauMongMuon = d.NgayBatDau.HasValue
                    ? d.NgayBatDau.Value.ToString("dd/MM/yyyy")
                    : ""
            }).ToList();

            lblSoLuong.Text = $"Tổng số: {danhSach.Count} yêu cầu";
        }

        private int? LayMaDangKyDangChon()
        {
            if (dgvChoDuyet.CurrentRow == null)
            {
                return null;
            }
            return (int)dgvChoDuyet.CurrentRow.Cells["MaDangKy"].Value;
        }

        private void btnDuyet_Click(object sender, EventArgs e)
        {
            int? maDangKy = LayMaDangKyDangChon();
            if (maDangKy == null)
            {
                MessageBox.Show("Vui lòng chọn một yêu cầu trong danh sách.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var db = new AppDbContext();
            var dangKy = db.DangKyOs.FirstOrDefault(d => d.MaDangKy == maDangKy);
            if (dangKy == null)
            {
                MessageBox.Show("Không tìm thấy yêu cầu (có thể vừa được xử lý).",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TaiDanhSach();
                return;
            }

            var phong = db.Phongs.FirstOrDefault(p => p.MaPhong == dangKy.MaPhong);
            if (phong == null)
            {
                MessageBox.Show("Phòng đăng ký không còn tồn tại.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TaiDanhSach();
                return;
            }

            int hienTai = phong.SoLuongHienTai ?? 0;
            int toiDa = phong.SoLuongToiDa ?? 0;
            if (hienTai >= toiDa)
            {
                MessageBox.Show("Phòng này đã đủ số lượng, không thể duyệt thêm.\n" +
                    "Vui lòng từ chối hoặc chuyển sinh viên sang phòng khác.",
                    "Không thể duyệt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            dangKy.TrangThai = TrangThaiDangKy.DaDuyet;
            dangKy.NgayBatDau ??= DateOnly.FromDateTime(DateTime.Today);
            phong.SoLuongHienTai = hienTai + 1;

            // Ghi nhận "ngày phân" để sinh viên xem được sau này (UC04 - xem tình trạng đăng ký).
            db.PhanPhongs.Add(new PhanPhong
            {
                MaDangKy = dangKy.MaDangKy,
                NgayPhan = DateOnly.FromDateTime(DateTime.Today)
            });

            // UC06/UC08/UC10: Hợp đồng (HopDong) chỉ được sinh ra khi đăng ký ở đã
            // được duyệt - tránh trường hợp sinh viên chưa đăng ký (hoặc đang chờ
            // duyệt) mà vẫn có hợp đồng "Còn hiệu lực" khi tra cứu.
            bool daCoHopDong = db.HopDongs.Any(h => h.MaSv == dangKy.MaSv && h.MaPhong == dangKy.MaPhong);
            if (!daCoHopDong)
            {
                string maHopDongMoi = MaSoHelper.SinhMaMoi(db.HopDongs.Select(h => h.MaHopDong).ToList(), "HD");
                db.HopDongs.Add(new HopDong
                {
                    MaHopDong = maHopDongMoi,
                    MaSv = dangKy.MaSv!,
                    MaPhong = dangKy.MaPhong!,
                    NgayLap = DateOnly.FromDateTime(DateTime.Today),
                    NgayHetHan = dangKy.NgayKetThuc ?? DateOnly.FromDateTime(DateTime.Today.AddYears(1)),
                    TrangThai = "Còn hiệu lực"
                });
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Duyệt thất bại: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Đã duyệt đăng ký thành công.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            TaiDanhSach();
        }

        private void btnTuChoi_Click(object sender, EventArgs e)
        {
            int? maDangKy = LayMaDangKyDangChon();
            if (maDangKy == null)
            {
                MessageBox.Show("Vui lòng chọn một yêu cầu trong danh sách.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var xacNhan = MessageBox.Show("Bạn có chắc chắn muốn từ chối yêu cầu này không?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (xacNhan != DialogResult.Yes)
            {
                return;
            }

            using var db = new AppDbContext();
            var dangKy = db.DangKyOs.FirstOrDefault(d => d.MaDangKy == maDangKy);
            if (dangKy == null)
            {
                TaiDanhSach();
                return;
            }

            dangKy.TrangThai = TrangThaiDangKy.TuChoi;

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Từ chối thất bại: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Đã từ chối yêu cầu.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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
