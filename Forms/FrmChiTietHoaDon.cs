using ClosedXML.Excel;
using DoAnNhom06.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows.Forms;

namespace DoAnNhom06.Forms
{
    /// <summary>
    /// Form hiển thị chi tiết hóa đơn với các dòng phí (điện, nước, internet, v.v.)
    /// </summary>
    public partial class FrmChiTietHoaDon : Form
    {
        private string _maHoaDon;

        public FrmChiTietHoaDon(string maHoaDon)
        {
            InitializeComponent();
            _maHoaDon = maHoaDon;
        }

        private void FrmChiTietHoaDon_Load(object sender, EventArgs e)
        {
            TaiChiTiet();
        }

        private void TaiChiTiet()
        {
            using var db = new AppDbContext();

            // Lấy thông tin hóa đơn
            var hoaDon = db.HoaDons
                .Include(h => h.MaHopDongNavigation)
                    .ThenInclude(hd => hd!.MaSvNavigation)
                .Include(h => h.MaHopDongNavigation)
                    .ThenInclude(hd => hd!.MaPhongNavigation)
                .Include(h => h.ChiTietHoaDons)
                .FirstOrDefault(h => h.MaHoaDon == _maHoaDon);

            if (hoaDon == null)
            {
                MessageBox.Show("Không tìm thấy hóa đơn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            // Hiển thị thông tin chung
            lblMaHoaDon.Text = $"Mã hóa đơn: {hoaDon.MaHoaDon}";
            lblMaSv.Text = $"Mã SV: {hoaDon.MaHopDongNavigation?.MaSv ?? "N/A"}";
            lblHoTen.Text = $"Họ tên: {hoaDon.MaHopDongNavigation?.MaSvNavigation?.HoTen ?? "N/A"}";
            lblPhong.Text = $"Phòng: {hoaDon.MaHopDongNavigation?.MaPhong ?? "N/A"}";
            lblThangNam.Text = $"Tháng/Năm: {hoaDon.Thang:00}/{hoaDon.Nam}";
            lblNgayLap.Text = $"Ngày lập: {hoaDon.NgayLap:dd/MM/yyyy}";
            lblTrangThai.Text = $"Trạng thái: {TrangThaiHoaDon.HienThi(hoaDon.TrangThai)}";

            // Hiển thị chi tiết hóa đơn
            var chiTiet = hoaDon.ChiTietHoaDons
                .OrderBy(c => c.MaCthd)
                .Select(c => new
                {
                    c.MaCthd,
                    LoaiPhi = c.LoaiPhi,
                    SoTien = c.SoTien.ToString("N0") + " đ",
                    GhiChu = c.GhiChu ?? ""
                }).ToList();

            dgvChiTiet.DataSource = chiTiet;

            // Tính tổng tiền từ chi tiết
            decimal tongTienChiTiet = hoaDon.ChiTietHoaDons.Sum(c => c.SoTien);

            // Hiển thị tổng tiền
            lblTongTien.Text = $"Tổng tiền: {hoaDon.TongTien:N0} đ";
            lblTongChiTiet.Text = $"(Tính từ chi tiết: {tongTienChiTiet:N0} đ)";

            // Kiểm tra nếu có sai lệch
            if (tongTienChiTiet != hoaDon.TongTien)
            {
                lblTongChiTiet.ForeColor = System.Drawing.Color.Red;
                lblTongChiTiet.Text += " ⚠ [Có sai lệch]";
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {
                using SaveFileDialog save = new SaveFileDialog();

                save.Filter = "Excel Workbook|*.xlsx";
                save.FileName = $"HoaDon_{_maHoaDon}.xlsx";

                if (save.ShowDialog() != DialogResult.OK)
                    return;

                using var db = new AppDbContext();

                var hoaDon = db.HoaDons
                    .Include(h => h.MaHopDongNavigation)
                        .ThenInclude(hd => hd!.MaSvNavigation)
                    .Include(h => h.ChiTietHoaDons)
                    .FirstOrDefault(h => h.MaHoaDon == _maHoaDon);

                if (hoaDon == null)
                    return;

                using XLWorkbook wb = new XLWorkbook();

                var ws = wb.Worksheets.Add("ChiTietHoaDon");

                //--------------------------------------------------
                // TIÊU ĐỀ
                //--------------------------------------------------

                ws.Cell("A1").Value = "CHI TIẾT HÓA ĐƠN";
                ws.Range("A1:D1").Merge();

                ws.Cell("A1").Style.Font.Bold = true;
                ws.Cell("A1").Style.Font.FontSize = 18;
                ws.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                //--------------------------------------------------
                // THÔNG TIN
                //--------------------------------------------------

                ws.Cell("A3").Value = "Mã hóa đơn";
                ws.Cell("B3").Value = hoaDon.MaHoaDon;

                ws.Cell("A4").Value = "Mã sinh viên";
                ws.Cell("B4").Value = hoaDon.MaHopDongNavigation?.MaSv;

                ws.Cell("A5").Value = "Họ tên";
                ws.Cell("B5").Value = hoaDon.MaHopDongNavigation?.MaSvNavigation?.HoTen;

                ws.Cell("A6").Value = "Phòng";
                ws.Cell("B6").Value = hoaDon.MaHopDongNavigation?.MaPhong;

                ws.Cell("A7").Value = "Tháng/Năm";
                ws.Cell("B7").Value = $"{hoaDon.Thang:00}/{hoaDon.Nam}";

                ws.Cell("A8").Value = "Ngày lập";
                ws.Cell("B8").Value = hoaDon.NgayLap.ToString("dd/MM/yyyy");

                ws.Cell("A9").Value = "Trạng thái";
                ws.Cell("B9").Value = TrangThaiHoaDon.HienThi(hoaDon.TrangThai);

                //--------------------------------------------------
                // CHI TIẾT
                //--------------------------------------------------

                ws.Cell("A11").Value = "STT";
                ws.Cell("B11").Value = "Loại phí";
                ws.Cell("C11").Value = "Số tiền";
                ws.Cell("D11").Value = "Ghi chú";

                ws.Range("A11:D11").Style.Font.Bold = true;
                ws.Range("A11:D11").Style.Fill.BackgroundColor = XLColor.LightBlue;

                int row = 12;

                int stt = 1;

                foreach (var item in hoaDon.ChiTietHoaDons.OrderBy(x => x.MaCthd))
                {
                    ws.Cell(row, 1).Value = stt++;

                    ws.Cell(row, 2).Value = item.LoaiPhi;

                    ws.Cell(row, 3).Value = item.SoTien;

                    ws.Cell(row, 3).Style.NumberFormat.Format = "#,##0";

                    ws.Cell(row, 4).Value = item.GhiChu;

                    row++;
                }

                //--------------------------------------------------
                // TỔNG
                //--------------------------------------------------

                ws.Cell(row + 1, 2).Value = "TỔNG TIỀN";

                ws.Cell(row + 1, 3).Value = hoaDon.TongTien;

                ws.Cell(row + 1, 3).Style.NumberFormat.Format = "#,##0";

                ws.Range(row + 1, 2, row + 1, 3).Style.Font.Bold = true;

                //--------------------------------------------------
                // KẺ KHUNG
                //--------------------------------------------------

                ws.Range(11, 1, row - 1, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ws.Range(11, 1, row - 1, 4).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                ws.Columns().AdjustToContents();

                wb.SaveAs(save.FileName);

                MessageBox.Show(
                    "Xuất Excel thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}