using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using DoAnNhom06.Helpers;
using DoAnNhom06.Models;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using System.IO;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// UC16 - In hóa đơn: Quản trị viên xem danh sách hóa đơn phí ký túc xá và in
    /// (hoặc xem trước khi in) hóa đơn của một sinh viên/tháng cụ thể.
    /// </summary>
    public partial class FrmInHoaDon : Form
    {
        // Nội dung hóa đơn đang được chọn để in - nạp 1 lần lúc bấm In/Xem trước,
        // dùng lại trong PrintDocument.PrintPage (có thể gọi lại nhiều lần khi in).
        private sealed class NoiDungHoaDon
        {
            public string MaHoaDon = "";
            public string MaSv = "";
            public string HoTen = "";
            public string MaPhong = "";
            public string ThangNam = "";
            public string TongTien = "";
            public string TrangThai = "";
            public string NgayLap = "";
        }

        public FrmInHoaDon()
        {
            InitializeComponent();
        }

        private void FrmInHoaDon_Load(object sender, EventArgs e)
        {
            TaiDanhSach();
        }

        private void TaiDanhSach(string? tuKhoa = null)
        {
            using var db = new AppDbContext();

            var query = db.HoaDons
                .Include(h => h.MaHopDongNavigation)
                    .ThenInclude(hd => hd!.MaSvNavigation)
                .Include(h => h.MaHopDongNavigation)
                    .ThenInclude(hd => hd!.MaPhongNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                tuKhoa = tuKhoa.Trim();
                query = query.Where(h =>
                    h.MaHoaDon.Contains(tuKhoa) ||
                    (h.MaHopDongNavigation != null && h.MaHopDongNavigation.MaSv.Contains(tuKhoa)) ||
                    (h.MaHopDongNavigation != null && h.MaHopDongNavigation.MaSvNavigation != null
                        && h.MaHopDongNavigation.MaSvNavigation.HoTen.Contains(tuKhoa)));
            }

            var danhSach = query.OrderByDescending(h => h.Nam).ThenByDescending(h => h.Thang).ToList();

            dgvHoaDon.DataSource = danhSach.Select(h => new
            {
                MaHoaDon = h.MaHoaDon,
                MaSv = h.MaHopDongNavigation?.MaSv ?? "",
                HoTen = h.MaHopDongNavigation?.MaSvNavigation?.HoTen ?? "",
                MaPhong = h.MaHopDongNavigation?.MaPhong ?? "",
                ThangNam = $"{h.Thang:00}/{h.Nam}",
                TongTien = h.TongTien.ToString("N0") + " đ",
                TrangThai = TrangThaiHoaDon.HienThi(h.TrangThai),
                NgayLap = h.NgayLap.ToString("dd/MM/yyyy")
            }).ToList();

            lblSoLuong.Text = $"Tổng số: {danhSach.Count} hóa đơn";
        }

        private void btnTimKiem_Click(object sender, EventArgs e) => TaiDanhSach(txtTimKiem.Text);

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                TaiDanhSach(txtTimKiem.Text);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = string.Empty;
            TaiDanhSach();
        }

        private void dgvHoaDon_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                MoXemTruoc();
            }
        }

        private NoiDungHoaDon? LayNoiDungHoaDonDangChon()
        {
            if (dgvHoaDon.CurrentRow == null)
            {
                return null;
            }

            var row = dgvHoaDon.CurrentRow;
            return new NoiDungHoaDon
            {
                MaHoaDon = row.Cells["MaHoaDon"].Value?.ToString() ?? "",
                MaSv = row.Cells["MaSv"].Value?.ToString() ?? "",
                HoTen = row.Cells["HoTen"].Value?.ToString() ?? "",
                MaPhong = row.Cells["MaPhong"].Value?.ToString() ?? "",
                ThangNam = row.Cells["ThangNam"].Value?.ToString() ?? "",
                TongTien = row.Cells["TongTien"].Value?.ToString() ?? "",
                TrangThai = row.Cells["TrangThai"].Value?.ToString() ?? "",
                NgayLap = row.Cells["NgayLap"].Value?.ToString() ?? ""
            };
        }

        /// <summary>Vẽ nội dung 1 hóa đơn lên trang in (dùng cho cả In trực tiếp và Xem trước).</summary>
        private static void VeHoaDon(PrintPageEventArgs e, NoiDungHoaDon hd)
        {
            var g = e.Graphics!;
            float x = 80, y = 60;

            using var fontTieuDe = new Font("Segoe UI", 18F, FontStyle.Bold);
            using var fontNhan = new Font("Segoe UI", 11F, FontStyle.Bold);
            using var fontGiaTri = new Font("Segoe UI", 11F);
            using var fontNho = new Font("Segoe UI", 9F, FontStyle.Italic);

            g.DrawString("KÝ TÚC XÁ - HÓA ĐƠN PHÍ", fontTieuDe, Brushes.Black, x, y);
            y += 45;
            g.DrawLine(Pens.Black, x, y, x + 560, y);
            y += 20;

            void DongThongTin(string nhan, string giaTri)
            {
                g.DrawString(nhan, fontNhan, Brushes.Black, x, y);
                g.DrawString(giaTri, fontGiaTri, Brushes.Black, x + 200, y);
                y += 32;
            }

            DongThongTin("Mã hóa đơn:", hd.MaHoaDon);
            DongThongTin("Mã sinh viên:", hd.MaSv);
            DongThongTin("Họ tên:", hd.HoTen);
            DongThongTin("Phòng:", hd.MaPhong);
            DongThongTin("Tháng/Năm:", hd.ThangNam);
            DongThongTin("Ngày lập:", hd.NgayLap);
            DongThongTin("Trạng thái:", hd.TrangThai);

            y += 10;
            g.DrawLine(Pens.Black, x, y, x + 560, y);
            y += 20;

            using var fontTongTien = new Font("Segoe UI", 14F, FontStyle.Bold);
            g.DrawString("TỔNG TIỀN PHẢI THU:", fontTongTien, Brushes.Black, x, y);
            g.DrawString(hd.TongTien, fontTongTien, Brushes.DarkRed, x + 260, y);

            y += 70;
            g.DrawString($"In lúc: {DateTime.Now:dd/MM/yyyy HH:mm}", fontNho, Brushes.Gray, x, y);
        }

        private PrintDocument TaoPrintDocument(NoiDungHoaDon hd)
        {
            var doc = new PrintDocument();
            doc.DocumentName = $"HoaDon_{hd.MaHoaDon}";
            doc.PrintPage += (s, e) => VeHoaDon(e, hd);
            return doc;
        }

        private void MoXemTruoc()
        {
            var hd = LayNoiDungHoaDonDangChon();
            if (hd == null)
            {
                ThongBao.CanhBao("Vui lòng chọn một hóa đơn trong danh sách.");
                return;
            }

            // Hiển thị form chi tiết hóa đơn (vừa double-click vào)
            using var frmChiTiet = new FrmChiTietHoaDon(hd.MaHoaDon);
            frmChiTiet.ShowDialog(this);
        }

        private void btnXemTruoc_Click(object sender, EventArgs e) => MoXemTruoc();

        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            XuatExcel();
        }
        private void XuatExcel()
{
    if (dgvHoaDon.Rows.Count == 0)
    {
        ThongBao.CanhBao("Không có dữ liệu để xuất.");
        return;
    }

    using SaveFileDialog save = new SaveFileDialog();
    save.Filter = "Excel Workbook|*.xlsx";
    save.FileName = "DanhSachHoaDon.xlsx";

    if (save.ShowDialog() != DialogResult.OK)
        return;

    try
    {
        using XLWorkbook wb = new XLWorkbook();

        var ws = wb.Worksheets.Add("HoaDon");

        //Tiêu đề
        ws.Cell(1, 1).Value = "DANH SÁCH HÓA ĐƠN";
        ws.Range(1, 1, 1, 8).Merge();

        ws.Cell(1, 1).Style.Font.Bold = true;
        ws.Cell(1, 1).Style.Font.FontSize = 18;
        ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        //Header
        for (int i = 0; i < dgvHoaDon.Columns.Count; i++)
        {
            ws.Cell(3, i + 1).Value = dgvHoaDon.Columns[i].HeaderText;
            ws.Cell(3, i + 1).Style.Font.Bold = true;
            ws.Cell(3, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
        }

        //Dữ liệu
        for (int i = 0; i < dgvHoaDon.Rows.Count; i++)
        {
            for (int j = 0; j < dgvHoaDon.Columns.Count; j++)
            {
                ws.Cell(i + 4, j + 1).Value =
                    dgvHoaDon.Rows[i].Cells[j].Value?.ToString();
            }
        }

        ws.Columns().AdjustToContents();

        wb.SaveAs(save.FileName);

        ThongBao.Info("Xuất Excel thành công!");
    }
    catch (Exception ex)
    {
        ThongBao.Loi(ex.Message);
    }
}

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
