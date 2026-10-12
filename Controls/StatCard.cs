using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using DoAnNhom06.Models;

namespace DoAnNhom06.Controls
{
    public class StatCard : Control
    {
        private bool _hover;

        public Color AccentColor { get; set; } = Color.RoyalBlue;

        public string TieuDe { get; set; } = "";

        public string GiaTri { get; set; } = "0";

        public string MoTa { get; set; } = "";

        public StatCard()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.ResizeRedraw,
                true);

            BackColor = Color.White;
            ForeColor = Color.FromArgb(35, 35, 40);

            Size = new Size(210, 100);
            Margin = new Padding(10);

            Cursor = Cursors.Default;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _hover = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _hover = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            // Tự lấy dữ liệu khi StatCard được tạo trên FrmMain
            TaiDuLieu();
        }

        private void TaiDuLieu()
        {
            try
            {
                using var db = new AppDbContext();

                if (Name == "cardTongPhong")
                {
                    int tongPhong = db.Phongs.Count();

                    TieuDe = "Tổng số phòng";
                    GiaTri = tongPhong.ToString("N0");
                    MoTa = "Tất cả phòng trong KTX";
                }
                else if (Name == "cardPhongTrong")
                {
                    int phongTrong = db.Phongs.Count(p =>
                        (p.SoLuongHienTai ?? 0) <
                        (p.SoLuongToiDa ?? 0));

                    TieuDe = "Phòng còn trống";
                    GiaTri = phongTrong.ToString("N0");
                    MoTa = "Phòng chưa đủ số người";
                }
                else if (Name == "cardSinhVienDangO")
                {
                    int sinhVienDangO = db.Phongs.Sum(p =>
                        p.SoLuongHienTai ?? 0);

                    TieuDe = "Sinh viên đang ở";
                    GiaTri = sinhVienDangO.ToString("N0");
                    MoTa = "Số sinh viên hiện tại";
                }
                else if (Name == "cardTyLeLapDay")
                {
                    int sucChua = db.Phongs.Sum(p =>
                        p.SoLuongToiDa ?? 0);

                    int dangO = db.Phongs.Sum(p =>
                        p.SoLuongHienTai ?? 0);

                    double tyLe = sucChua > 0
                        ? (double)dangO / sucChua * 100
                        : 0;

                    TieuDe = "Tỷ lệ lấp đầy";
                    GiaTri = $"{tyLe:N1}%";
                    MoTa = $"{dangO:N0}/{sucChua:N0} chỗ đang sử dụng";
                }

                Invalidate();
            }
            catch
            {
                TieuDe = "";
                GiaTri = "0";
                MoTa = "Không thể tải dữ liệu";
            }
        }

        /// <summary>
        /// Cho phép FrmMain gọi lại để cập nhật số liệu.
        /// </summary>
        public void LamMoiDuLieu()
        {
            TaiDuLieu();
        }

        private static GraphicsPath TaoVienBoTron(
            RectangleF rect,
            float radius)
        {
            float d = radius * 2;

            var path = new GraphicsPath();

            path.AddArc(
                rect.X,
                rect.Y,
                d,
                d,
                180,
                90);

            path.AddArc(
                rect.Right - d,
                rect.Y,
                d,
                d,
                270,
                90);

            path.AddArc(
                rect.Right - d,
                rect.Bottom - d,
                d,
                d,
                0,
                90);

            path.AddArc(
                rect.X,
                rect.Bottom - d,
                d,
                d,
                90,
                90);

            path.CloseFigure();

            return path;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            float w = Width - 3;
            float h = Height - 3;

            if (w <= 0 || h <= 0)
                return;

            var rect = new RectangleF(
                1,
                1,
                w,
                h);

            using var path = TaoVienBoTron(rect, 12);

            // Nền
            Color nen = _hover
                ? Color.FromArgb(250, 251, 253)
                : Color.White;

            using (var brushNen = new SolidBrush(nen))
            {
                g.FillPath(brushNen, path);
            }

            // Viền
            using (var pen = new Pen(
                _hover
                    ? AccentColor
                    : Color.FromArgb(225, 228, 233),
                _hover ? 1.5f : 1f))
            {
                g.DrawPath(pen, path);
            }

            // Thanh màu bên trái
            using (var brushAccent = new SolidBrush(AccentColor))
            {
                g.FillRectangle(
                    brushAccent,
                    1,
                    12,
                    5,
                    Height - 24);
            }

            // Tiêu đề
            using var fontTitle =
                new Font(
                    "Segoe UI",
                    9.5F,
                    FontStyle.Regular);

            using var brushTitle =
                new SolidBrush(
                    Color.FromArgb(105, 110, 120));

            var titleRect =
                new RectangleF(
                    20,
                    13,
                    Width - 30,
                    22);

            using var fmtTitle =
                new StringFormat
                {
                    LineAlignment =
                        StringAlignment.Center
                };

            g.DrawString(
                TieuDe,
                fontTitle,
                brushTitle,
                titleRect,
                fmtTitle);

            // Giá trị lớn
            using var fontValue =
                new Font(
                    "Segoe UI",
                    22F,
                    FontStyle.Bold);

            using var brushValue =
                new SolidBrush(AccentColor);

            var valueRect =
                new RectangleF(
                    20,
                    31,
                    Width - 30,
                    38);

            using var fmtValue =
                new StringFormat
                {
                    LineAlignment =
                        StringAlignment.Center
                };

            g.DrawString(
                GiaTri,
                fontValue,
                brushValue,
                valueRect,
                fmtValue);

            // Mô tả
            using var fontDesc =
                new Font(
                    "Segoe UI",
                    7.8F,
                    FontStyle.Regular);

            // "▲" (tăng) hiển thị màu xanh lá, "▼" (giảm / cần chú ý) hiển thị màu đỏ đậm -
            // dùng cho các dòng mô tả kiểu "▲ 8.2% so với tháng trước" hoặc
            // "▼ 5 hóa đơn quá hạn"; mô tả thường vẫn giữ màu xám như trước.
            Color mauMoTa = MoTa.StartsWith("▲")
                ? Color.FromArgb(30, 130, 65)
                : MoTa.StartsWith("▼")
                    ? Color.FromArgb(160, 35, 35)
                    : Color.FromArgb(140, 144, 150);

            using var brushDesc =
                new SolidBrush(mauMoTa);

            var descRect =
                new RectangleF(
                    20,
                    70,
                    Width - 30,
                    Height - 75);

            using var fmtDesc =
                new StringFormat
                {
                    LineAlignment =
                        StringAlignment.Center,
                    Trimming =
                        StringTrimming.EllipsisCharacter
                };

            g.DrawString(
                MoTa,
                fontDesc,
                brushDesc,
                descRect,
                fmtDesc);
        }
    }
}